using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour
{
    public GameObject camera;
    
    void LateUpdate()
    {
       
        float posV = Input.GetAxis("Vertical");
        float posH = Input.GetAxis("Horizontal");
        if (posV > 0f)
        {
            camera.transform.position = camera.transform.position + new Vector3(0f, 0f, 0.5f);
        }
        if (posV < 0f)
        { 
            camera.transform.position = camera.transform.position - new Vector3(0f, 0f, 0.5f);
        }
        if (posH > 0f)
        {
            camera.transform.position = camera.transform.position + new Vector3(0.5f, 0f, 0f);
        }
        if (posH < 0f)
        {
            camera.transform.position = camera.transform.position - new Vector3(0.5f, 0f, 0f);
        }
    }
}