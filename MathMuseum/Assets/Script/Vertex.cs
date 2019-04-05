using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vertex : MonoBehaviour {

    public Renderer rend; 

    // Use this for initialization
    void Start () {
        //Fetch the Renderer from the GameObject
        rend = GetComponent<Renderer>();

        //Set the main Color of the Material to green
        //rend.material.shader = Shader.Find("_Color");
        //rend.material.SetColor("_Color", Color.green);
    }

    public void OnTriggerEnter(Collider col)
    {
        if (rend.material.color == Color.yellow)
        {
            rend.material.color = Color.red;
        }
        else if(rend.material.color == Color.red)
        {
            rend.material.color = Color.green;
        }
        else if(rend.material.color == Color.green)
        {
            rend.material.color = Color.blue;
        }
        else
        {
            rend.material.color = Color.yellow;
        }
    }

    // Update is called once per frame
    void Update () {
	   

	}
}
