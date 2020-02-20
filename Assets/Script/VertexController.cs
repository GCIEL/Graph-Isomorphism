using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;

public class VertexController : MonoBehaviour
{
    public GameObject cameraEye;
    public GameObject vertex;

    public Edge edgePrefab;

    public Vector3 initVertexPos;
    public Vector3 initControllerPos;
    public Vector3 directionToVertex;

    // calculates the relative x and z values from the current camera angle 
    (double, double) calculateNewPosition(double rotation)
    {
        double x = 0f;
        double z = 0f;

        z = Math.Cos(rotation) * 0.2;
        x = Math.Sin(rotation) * 0.2;

        return (x, z);
    }

    void Update()
    {
        Debug.Log(GameManager.Instance.selectedVertex);
        // Get the vertex (if selected at all) 
        vertex = GameManager.Instance.selectedVertex;

        // Locate the laser pointer handler to toggle laser
        GameObject rightController = GameObject.Find("Controller (right)");
        LaserPointerHandler controller = rightController.GetComponent<LaserPointerHandler>();

        // If selectVertex toggle isn't held or we have no selected vertex, return
        if (Input.GetAxis("SelectVertex") <= 0 || vertex == null)
        {
            
            initVertexPos = Vector3.zero;
            initControllerPos = Vector3.zero;

            controller.laserPointer.active = true;
            return;
        }

        // Disable laser pointer
        controller.laserPointer.active = false;

        // Save initial vertex/controller positions
        if (initVertexPos == Vector3.zero) initVertexPos = vertex.transform.position;
        if (initControllerPos == Vector3.zero) initControllerPos = this.transform.position;
        
        // Save `heading` vector that stores the direction that we want to move our vertex, and scale it and move
        Vector3 heading = this.transform.position - initControllerPos;
        vertex.transform.position = initVertexPos + Vector3.Scale(heading, new Vector3(25f, 25f, 25f));

        // posV finds if user inputs forward or backward
        float posV = Input.GetAxis("VerticalR");
        directionToVertex = vertex.transform.position - this.transform.position;
        Debug.Log(posV);
        if (posV > 0f)
        {
            vertex.transform.position = vertex.transform.position + Vector3.Scale(directionToVertex, new Vector3(0.01f,0.01f, 0.01f));
            initVertexPos = initVertexPos + Vector3.Scale(directionToVertex, new Vector3(0.01f, 0.01f, 0.01f));
        } 

        if (posV < 0f)
        {
            vertex.transform.position = vertex.transform.position - Vector3.Scale(directionToVertex, new Vector3(0.01f, 0.01f, 0.01f));
            initVertexPos = initVertexPos - Vector3.Scale(directionToVertex, new Vector3(0.01f, 0.01f, 0.01f));
        }


        Vertex v = vertex.GetComponent<Vertex>();
        HashSet<Edge> adjEdges = v.adjacentEdges;
        foreach (Edge edge in adjEdges)
        {
            Vertex other = null;
            foreach (Vertex o in edge.adjacentVertices)
            {
                if (o != v) other = o;
            }
            edge.adjacentVertices.ToList<Vertex>();

            Vector3 curr_pos = vertex.transform.position;
            Vector3 other_pos = other.gameObject.transform.position;

            Vector3 edge_pos = Vector3.Lerp(curr_pos, other_pos, 0.5f);
            edge.transform.position = edge_pos;
            
            var offset = other_pos - curr_pos;
            var scale = new Vector3(0.5f, offset.magnitude / 2, 0.5f);
            edge.transform.up = offset;
            edge.transform.localScale = scale;
        }
        //Debug.Log(v.adjacentEdges.Count);
        
    }
}