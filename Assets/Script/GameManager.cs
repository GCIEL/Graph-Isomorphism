using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Collections.ObjectModel;
using UnityEngine;
using UnityEngine.UI;

// GameManager class that utilizes a singleton pattern

public class GameManager : Singleton<GameManager> {

    bool isCube;

    // Prefabs for the edges and vertices
    public Vertex vertexPrefab;
    public Edge edgePrefab;
    
    // List of edges and vertices
    public List<Vertex> vertex_list = new List<Vertex>();
    public Edge[,] edges_list;

    // Chromatic number for this grapsh
    public int Answer { get; set; }

    string[] adjMatrixLines;
    string[] posLines;

    // Use this for initialization 
    void Start() {
        isCube = false;
    }

    // Update is called once per frame
    void Update() {
        CheckAll();
    }

    // changes the graph
    public void changeGraph()
    {   
        // graphComponenets stores all gameobjects rendered until now
        GameObject[] graphComponents = UnityEngine.Object.FindObjectsOfType<GameObject>();

        // we find the gameobjects that are either vertices or edges, and destroy them
        for (int i = 0; i < graphComponents.Length; i++)
        {
            if (graphComponents[i].name == "Sphere(Clone)" || graphComponents[i].name == "Cylinder(Clone)") Destroy(graphComponents[i]);
        }

        // according to the current graph, we instantiate a new graph
        if (isCube)
        {
            GameManager.Instance.buildGraph(Resources.Load("PyramidAdjMatrix", typeof(TextAsset)) as TextAsset, Resources.Load("PyramidPos", typeof(TextAsset)) as TextAsset, Resources.Load("Sphere", typeof(Vertex)) as Vertex, Resources.Load("Cylinder", typeof(Edge)) as Edge);
            isCube = false;
        }
        else
        {
            GameManager.Instance.buildGraph(Resources.Load("CubeAdjMatrix", typeof(TextAsset)) as TextAsset, Resources.Load("CubePos", typeof(TextAsset)) as TextAsset, Resources.Load("Sphere", typeof(Vertex)) as Vertex, Resources.Load("Cylinder", typeof(Edge)) as Edge);
            isCube = true;
        }
    }

    // Build graph from given textfile, vertex/edge Prefabs
    public void buildGraph(TextAsset adjMatrix, TextAsset pos, Vertex vertexPrefab, Edge edgePrefab)
    {
        vertex_list = new List<Vertex>();
        // Initialize the vertex_list for new graph

        this.vertexPrefab = vertexPrefab;
        this.edgePrefab = edgePrefab;

        // Parse the textfiles
        adjMatrixLines = Regex.Split(adjMatrix.text, "\n");
        posLines = Regex.Split(pos.text, "\n");

        // Initialize e
        edges_list = new Edge[adjMatrixLines.Length - 1, adjMatrixLines.Length - 1];

        // instantiate vertices and edges based on csv file.
        for (int i = 0; i < adjMatrixLines.Length - 1; i++)
        {
            string valueLineAdj = adjMatrixLines[i];
            string valueLinePos = posLines[i];

            string[] valuesAdj = Regex.Split(valueLineAdj, ",");
            string[] valuesPos = Regex.Split(valueLinePos, ",");
            

            // Get the x-y-z coordinate of each vertex
            int x_coord = Int32.Parse(valuesPos[0]);
            int y_coord = Int32.Parse(valuesPos[1]);
            int z_coord = Int32.Parse(valuesPos[2]);
            Vector3 position = new Vector3(x_coord, y_coord, z_coord);

            // instantiate vertices
            Vertex obj = Instantiate(vertexPrefab, position, Quaternion.identity);
            vertex_list.Add(obj);

            if (i != 0)
            {
                for (int j = 0; j < i; j++)
                {
                    int test = Int32.Parse(valuesAdj[j]);
                    if (test == 1)
                    {
                        Vector3 other_pos = vertex_list[j].transform.position;
                        Vector3 edge_pos = Vector3.Lerp(position, other_pos, 0.5f);

                        // Instantiate edge
                        Edge e = Instantiate(edgePrefab, edge_pos, Quaternion.identity);
                        var offset = other_pos - position;
                        var scale = new Vector3(0.5f, offset.magnitude / 2, 0.5f);
                        e.transform.up = offset;
                        e.transform.localScale = scale;
                        edges_list[i, j] = e;
                    }
                }
            }
        }
        Answer = Int32.Parse(adjMatrixLines[adjMatrixLines.Length - 1]);
    }

    public void CheckAll()
    {
        Boolean valid_coloring = true;
        Boolean is_finished = true;
        int color_used = 0;
        List<Color> color_list = new List<Color>();

        for (int i = 0; i < adjMatrixLines.Length - 1; i++)
        {
            string valueLine = adjMatrixLines[i];
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
                    int test = Int32.Parse(values[j]);
                    if (test == 1)
                    {
                        Color other_color = vertex_list[j].rend.material.color;
                        if (this_color != Color.white && this_color != Color.white && this_color == other_color)
                        {
                            valid_coloring = false;
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
    }

}

