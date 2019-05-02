using UnityEngine;
using System.Collections;
using System;

public class CameraController : MonoBehaviour
{
    public GameObject camera;
    public GameObject cameraEye;
    
	// calculates the relative x and z values from the current camera angle 
    (double, double) calculateNewPosition(double rotation)
    {
        double x = 0f;
        double z = 0f;
        if (rotation < (Math.PI / 2))
        {
            x = 0.01 / Math.Cos(rotation);
            z = 0.01 / Math.Sin(rotation);
        }
        else if (rotation > (Math.PI / 2) && rotation < Math.PI)
        {
            x = 0.01 / Math.Sin(rotation - (Math.PI / 2));
            z = 0.01 / -Math.Cos(rotation - (Math.PI / 2));

        }
        else if (rotation > Math.PI && rotation < 3 * (Math.PI / 2))
        {
            x = 0.01 / -Math.Sin(3 * (Math.PI / 2) - rotation);
            z = 0.01 / -Math.Cos(3 * (Math.PI / 2) - rotation);
        }
        else if (rotation > 3 * (Math.PI / 2))
        {
            x = 0.01 / -Math.Cos(2 * Math.PI - rotation);
            z = 0.01 / Math.Sin(2 * Math.PI - rotation);
        }
        return (x, z);
    }

    void LateUpdate()
    {
		// posV finds if user inputs forward or backward
        float posV = Input.GetAxis("Vertical");
		
		// posH finds if user inputs left and right
        float posH = Input.GetAxis("Horizontal");
		
		// angle to calculate positions forward and backward
        double angle_vertical = cameraEye.transform.localEulerAngles.y * Math.PI / 180;
        double x_vertical = calculateNewPosition(angle_vertical).Item1;
        double z_vertical = calculateNewPosition(angle_vertical).Item2;
		
		// angle to calculate positions left and right
		double angle_horizontal = (angle_vertical + (Math.PI / 2)) % (2 * Math.PI);
		double x_horizontal = calculateNewPosition(angle_horizontal).Item1;
        double z_horizontal = calculateNewPosition(angle_horizontal).Item2;
		
		// calculate new camera positions according to input
        if (posV > 0f) camera.transform.position = camera.transform.position + new Vector3((float) x_vertical, 0f, (float) z_vertical);
        if (posV < 0f) camera.transform.position = camera.transform.position - new Vector3((float) x_vertical, 0f, (float) z_vertical);
        if (posH > 0f) camera.transform.position = camera.transform.position + new Vector3((float) x_horizontal, 0f, (float) z_horizontal);
        if (posH < 0f) camera.transform.position = camera.transform.position - new Vector3((float) x_horizontal, 0f, (float) z_horizontal);
    }
}