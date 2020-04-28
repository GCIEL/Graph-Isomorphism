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
        float posH = Input.GetAxis("HorizontalR");
        directionToVertex = vertex.transform.position - this.transform.position;
        directionToVertex = Vector3.ClampMagnitude(directionToVertex, 10f);

        // If the user has given a forward or backward input, move the vertex in the direction that the user commanded
        if (posV > 0f)
        {
            vertex.transform.position = vertex.transform.position + Vector3.Scale(directionToVertex, new Vector3(0.015f, 0.015f, 0.015f));
            initVertexPos = initVertexPos + Vector3.Scale(directionToVertex, new Vector3(0.01f, 0.01f, 0.01f));
        }

        if (posV < 0f)
        {
            vertex.transform.position = vertex.transform.position - Vector3.Scale(directionToVertex, new Vector3(0.015f, 0.015f, 0.015f));
            initVertexPos = initVertexPos - Vector3.Scale(directionToVertex, new Vector3(0.01f, 0.01f, 0.01f));
        }

        // The code below will make it so that the joystick will be able to move the vertex left and right, in addition to forward and backward when selected. 
        // We choose not to use this code for simplicity


        /*
        if (posH > 0f)
        {
            Vector3 verticalVec= new Vector3(0f, 0.1f, 0f);
            Vector3 othogonalVec = Vector3.Cross(directionToVertex, verticalVec);
            othogonalVec = Vector3.Scale(othogonalVec, new Vector3(directionToVertex.magnitude/othogonalVec.magnitude, directionToVertex.magnitude / othogonalVec.magnitude, directionToVertex.magnitude / othogonalVec.magnitude));
            //Debug.Log("ortho" + othogonalVec.magnitude);
            vertex.transform.position = vertex.transform.position - Vector3.Scale(othogonalVec, new Vector3(0.01f, 0.01f, 0.01f));
            initVertexPos = initVertexPos - Vector3.Scale(othogonalVec, new Vector3(0.01f, 0.01f, 0.01f));
        }

        if (posH < 0f)
        {
            Vector3 verticalVec = new Vector3(0f, 0.1f, 0f);
            Vector3 othogonalVec = Vector3.Cross(directionToVertex, verticalVec);
            othogonalVec = Vector3.Scale(othogonalVec, new Vector3(directionToVertex.magnitude / othogonalVec.magnitude, directionToVertex.magnitude / othogonalVec.magnitude, directionToVertex.magnitude / othogonalVec.magnitude));
            vertex.transform.position = vertex.transform.position + Vector3.Scale(othogonalVec, new Vector3(0.01f, 0.01f, 0.01f));
            initVertexPos = initVertexPos + Vector3.Scale(othogonalVec, new Vector3(0.01f, 0.01f, 0.01f));
        }*/

        // Make it so that the edges will follow the vertex as it moves
        GameManager.Instance.moveEdges(vertex.GetComponent<Vertex>());
    }
}