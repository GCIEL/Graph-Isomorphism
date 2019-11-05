using UnityEngine;
using System.Collections;
using System;

public class VertexController : MonoBehaviour
{
    public GameObject cameraEye;
    public GameObject vertex;

  
    // calculates the relative x and z values from the current camera angle 
    (double, double) calculateNewPosition(double rotation)
    {
        double x = 0f;
        double z = 0f;

        z = Math.Cos(rotation) * 0.2;
        x = Math.Sin(rotation) * 0.2;

        return (x, z);
    }

    void LateUpdate()
    {
        vertex = GameManager.Instance.selectedVertex;
        
        if (Input.GetAxis("SelectVertex") <= 0 || vertex == null)
        {
            GameManager.Instance.selectedVertex = null;
            return;
        }

        // posV finds if user inputs forward or backward
        float posV = Input.GetAxis("VerticalR");

        // posH finds if user inputs left and right
        float posH = Input.GetAxis("HorizontalR");

        // angle to calculate positions forward and backward
        double angle_vertical = cameraEye.transform.localEulerAngles.y * Math.PI / 180;
        double x_vertical = calculateNewPosition(angle_vertical).Item1;
        double z_vertical = calculateNewPosition(angle_vertical).Item2;

        // angle to calculate positions left and right
        double angle_horizontal = (angle_vertical + (Math.PI / 2)) % (2 * Math.PI);
        double x_horizontal = calculateNewPosition(angle_horizontal).Item1;
        double z_horizontal = calculateNewPosition(angle_horizontal).Item2;

        // calculate new vertex positions according to input
        if (posV > 0f) vertex.transform.position = vertex.transform.position + new Vector3((float)x_vertical, 0f, (float)z_vertical);
        if (posV < 0f) vertex.transform.position = vertex.transform.position - new Vector3((float)x_vertical, 0f, (float)z_vertical);
        if (posH > 0f) vertex.transform.position = vertex.transform.position + new Vector3((float)x_horizontal, 0f, (float)z_horizontal);
        if (posH < 0f) vertex.transform.position = vertex.transform.position - new Vector3((float)x_horizontal, 0f, (float)z_horizontal);

        Vertex v = vertex.GetComponent<Vertex>();
        foreach (Edge e in v.adjacentEdges)
        {
            e.rend.material.color = Color.green;
        }
        Debug.Log(v.adjacentEdges.Count);
        //ArrayList Edges = vertex.adjacentEdges;
    }
}