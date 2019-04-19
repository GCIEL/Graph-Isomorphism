using UnityEngine;
using System.Collections;
using System;

public class CameraController : MonoBehaviour
{
    public GameObject camera;
    public GameObject cameraEye;
    
    void LateUpdate()
    {
        float posV = Input.GetAxis("Vertical");
        float posH = Input.GetAxis("Horizontal");
        if (posV > 0f)
        {
            double rotation = cameraEye.transform.localEulerAngles.y * Math.PI / 180;
            double x = 0f;
            double z = 0f;
            if (rotation < (Math.PI / 2))
            {
                x = 0.1 / Math.Cos(rotation);
                z = 0.1 / Math.Sin(rotation);
            }
            else if (rotation > (Math.PI / 2) && rotation < Math.PI)
            {
                x = 0.1 / Math.Sin(rotation - (Math.PI / 2));
                z = 0.1 / -Math.Cos(rotation - (Math.PI / 2));

            }
            else if (rotation > Math.PI && rotation < 3*(Math.PI / 2))
            {
                x = 0.1 / -Math.Sin(3 * (Math.PI / 2) - rotation);
                z = 0.1 / -Math.Cos(3 * (Math.PI / 2) - rotation);
            }
            else if (rotation > 3 * (Math.PI / 2))
            {
                x = 0.1 / -Math.Cos(2*Math.PI - rotation);
                z = 0.1 / Math.Sin(2 * Math.PI - rotation);
            }
            camera.transform.position = camera.transform.position + new Vector3((float)x, 0f, (float)z);
        }
        if (posV < 0f)
        {
            double rotation = cameraEye.transform.localEulerAngles.y * Math.PI / 180;
            double x = 0f;
            double z = 0f;
            if (rotation < (Math.PI / 2))
            {
                x = 0.1 / Math.Cos(rotation);
                z = 0.1 / Math.Sin(rotation);
            }
            else if (rotation > (Math.PI / 2) && rotation < Math.PI)
            {
                x = 0.1 / Math.Sin(rotation - (Math.PI / 2));
                z = 0.1 / -Math.Cos(rotation - (Math.PI / 2));

            }
            else if (rotation > Math.PI && rotation < 3 * (Math.PI / 2))
            {
                x = 0.1 / -Math.Sin(3 * (Math.PI / 2) - rotation);
                z = 0.1 / -Math.Cos(3 * (Math.PI / 2) - rotation);
            }
            else if (rotation > 3 * (Math.PI / 2))
            {
                x = 0.1 / -Math.Cos(2 * Math.PI - rotation);
                z = 0.1 / Math.Sin(2 * Math.PI - rotation);
            }
            camera.transform.position = camera.transform.position + new Vector3((float)-x, 0f, (float)-z);
        }
        if (posH > 0f)
        {
            //camera.transform.position = camera.transform.position + new Vector3(0.5f, 0f, 0f);
        }
        if (posH < 0f)
        {
            //camera.transform.position = camera.transform.position - new Vector3(0.5f, 0f, 0f);
        }
    }
}