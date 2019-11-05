using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Edge : MonoBehaviour {

    public Renderer rend;
    public HashSet<Vertex> adjacentVertices;

	// Use this for initialization
	void Start () {
        rend = GetComponent<Renderer>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
