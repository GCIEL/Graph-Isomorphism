﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Valve.VR.InteractionSystem;
using System.Collections.ObjectModel;
using UnityEngine;
using UnityEngine.UI;

public class Construction : MonoBehaviour {

    public TextAsset textfile;
    public Vertex vertexPrefab;
    public Edge edgePrefab;
    public List<Vertex> vertex_list = new List<Vertex>();
    public Edge[,] edges_list;
    private int answer;
    public Text monitor;
    private bool if_clicked = false;


    string fs;
    string[] fLines;


    // Use this for initialization
    void Start () {

        // Parse csv and store adj list into hash map.
        fs = textfile.text;
        fLines = Regex.Split(fs, "\n");

        edges_list = new Edge[fLines.Length-1, fLines.Length-1];

        // instantiate vertices and edges based on csv file.
        for (int i = 0; i< fLines.Length-1; i++)
        {
            string valueLine = fLines[i];
            string[] values = Regex.Split(valueLine, ",");

            // Get the x-y-z coordinate of each vertex
            int x_coord = Int32.Parse(values[0]);
            int y_coord = Int32.Parse(values[1]);
            int z_coord = Int32.Parse(values[2]);
            Vector3 pos = new Vector3(x_coord, y_coord, z_coord);

            // instantiate vertices
            Vertex obj = Instantiate(vertexPrefab, pos, Quaternion.identity);

            vertex_list.Add(obj);

            if(i != 0)
            {
                //Debug.Log("ah");
                for(int j = 0; j < i; j++)
                {
                    //Debug.Log("bro");
                    int test = Int32.Parse(values[3 + j]);
                    //Debug.Log(test);
                    if (test == 1)
                    {
                        //Debug.Log("cat");
                        Vector3 other_pos = vertex_list[j].transform.position;
                        Vector3 edge_pos = Vector3.Lerp(pos, other_pos, 0.5f);
                        Edge e = Instantiate(edgePrefab, edge_pos, Quaternion.identity);
                        var offset = other_pos - pos;
                        var scale = new Vector3(0.5f, offset.magnitude / 2, 0.5f);
                        e.transform.up = offset;
                        e.transform.localScale = scale;
                        edges_list[i, j] = e;
                    }
                }
            }

        }
        answer = Int32.Parse(fLines[fLines.Length - 1]);
       
	}
    public void trigger_pressed()
    {
        Debug.Log("trigger pressed");
        CheckAll();

    }
    public void CheckAll()
    {
        //if (Input.GetKeyDown("space")) {
            Boolean valid_coloring = true;
            Boolean is_finished = true;
            int color_used = 0;
            List<Color> color_list = new List<Color>();

            for (int i = 0; i < fLines.Length - 1; i++)
            {
                string valueLine = fLines[i];
                string[] values = Regex.Split(valueLine, ",");
                Color this_color = vertex_list[i].rend.material.color;
                if (this_color == Color.white)
                {
                    is_finished = false;
                }
                else
                {
                    if (!color_list.Contains(this_color))
                    {
                        color_used++;
                        color_list.Add(this_color);
                    }
                }

                if (i != 0)
                {
                    for (int j = 0; j < i; j++)
                    {
                        int test = Int32.Parse(values[3 + j]);
                        if (test == 1)
                        {
                            Color other_color = vertex_list[j].rend.material.color;
                            if (this_color != Color.white && this_color != Color.white && this_color == other_color)
                            {
                                valid_coloring = false;
                                //I wish to highlight this edge
                                edges_list[i, j].rend.material.color = Color.black;
                            }
                            else
                            {
                                edges_list[i, j].rend.material.color = Color.white;
                            }
                        }
                    }
                }
            }
            if (!valid_coloring)
            {
                Debug.Log("Stupid!");
                monitor.text = "Wrong Coloring!";
            }
            else
            {
                if (is_finished)
                {
                    if (color_used > answer)
                    {
                        Debug.Log("Nice! But could you do it with fewer colors?");
                        monitor.text = "Nice! But could you do it with fewer colors?";
                    }
                    else
                    {
                        Debug.Log("Smart!");
                        monitor.text = "Smart!";
                    }
                }
                else
                {
                    Debug.Log("Good job so far, finish it!");
                    monitor.text = "Good job so far, finish it!";
                }
            }
       // }
    }

	// Update is called once per frame
	void Update () {
       //CheckAll();
    }
}