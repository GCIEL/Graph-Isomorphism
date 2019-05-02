using UnityEngine;
using System.Collections;
using System;

public class CameraController : MonoBehaviour
{
    public GameObject camera;
    public GameObject cameraEye;
    
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
        float posV = Input.GetAxis("Vertical");
        float posH = Input.GetAxis("Horizontal");
        double rotation = cameraEye.transform.localEulerAngles.y * Math.PI / 180;
        double x = calculateNewPosition(rotation).Item1;
        double z = calculateNewPosition(rotation).Item2;
        if (posV > 0f)
        {
            camera.transform.position = camera.transform.position + new Vector3((float)x, 0f, (float)z);
        }
        if (posV < 0f)
        {
            camera.transform.position = camera.transform.position - new Vector3((float)x, 0f, (float)z);
        }
        if (posH > 0f)
        {
            /*
            double rotation = cameraEye.transform.localEulerAngles.y * Math.PI / 180;
            if (rotation < (Math.PI / 2))
            {
                rotation = (3 * Math.PI / 2) + rotation;
            }
            double x = calculateNewPosition(rotation).Item1;
            double z = calculateNewPosition(rotation).Item2;
            camera.transform.position = camera.transform.position - new Vector3((float)x, 0f, (float)z);*/
        }
        if (posH < 0f)
        {
            //camera.transform.position = camera.transform.position - new Vector3(0.5f, 0f, 0f);
        }
    }
}