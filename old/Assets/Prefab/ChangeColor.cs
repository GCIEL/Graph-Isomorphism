using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ChangeColor : MonoBehaviour
{
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void RedChanged()
    {
        GetComponent<Renderer>().material.color = Color.red;
    }
    public void GreenChanged(float value)
    {
        GetComponent<Renderer>().material.color = Color.green;
    }
    public void BlueChanged(float value)
    {
        GetComponent<Renderer>().material.color = Color.blue;
    }
}
