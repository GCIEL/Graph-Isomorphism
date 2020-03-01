using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Edge : MonoBehaviour {

    public bool staticEdge;
    public Renderer rend;
    public HashSet<Vertex> adjacentVertices;

	// Use this for initialization
	void Start () {
      
    }
    private void Awake()
    {
        rend = GetComponent<Renderer>();
        rend.material.color = Color.black;
        staticEdge = false;
    }
    public void ChangeOpacity()
    {
        // Set opacity
        var temp = GetComponent<Renderer>().material.color;
        temp.a = 0.2f;
        GetComponent<Renderer>().material.color = temp;
        //Debug.Log(GetComponent<Renderer>().material.color.a);
    }

    // Update is called once per frame
    void Update () {
       
    }
}
