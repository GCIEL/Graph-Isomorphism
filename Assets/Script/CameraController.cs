
using UnityEngine;
using System.Collections;
using System;

public class CameraController : MonoBehaviour
{
    public GameObject camera;
    public GameObject cameraEye;
    double angle_vertical;

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
		// posV finds if user inputs forward or backward
        float posV = Input.GetAxis("VerticalL");
		
		// posH finds if user inputs left and right
        float posH = Input.GetAxis("HorizontalL");

        Debug.Log(angle_vertical);

        if ((posV == 0 && posH == 0) || angle_vertical == 0.0) angle_vertical = cameraEye.transform.localEulerAngles.y * Math.PI / 180;
        double x_vertical = calculateNewPosition(angle_vertical).Item1;
        double z_vertical = calculateNewPosition(angle_vertical).Item2;

       

            // angle to calculate positions left and right
        double angle_horizontal = (angle_vertical + (Math.PI / 2)) % (2 * Math.PI);
		double x_horizontal = calculateNewPosition(angle_horizontal).Item1;
        double z_horizontal = calculateNewPosition(angle_horizontal).Item2;
		
		// calculate new camera positions according to input
        if (posV > 0f) camera.transform.position = camera.transform.position + new Vector3((float) x_vertical * 0.3f , 0f, (float) z_vertical * 0.3f);
        if (posV < 0f) camera.transform.position = camera.transform.position - new Vector3((float) x_vertical * 0.3f, 0f, (float) z_vertical * 0.3f);
        if (posH > 0f) camera.transform.position = camera.transform.position + new Vector3((float) x_horizontal * 0.3f, 0f, (float) z_horizontal * 0.3f);
        if (posH < 0f) camera.transform.position = camera.transform.position - new Vector3((float) x_horizontal * 0.3f, 0f, (float) z_horizontal * 0.3f);
    }
}