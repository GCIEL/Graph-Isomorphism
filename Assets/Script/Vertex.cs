using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vertex : MonoBehaviour {

    public Renderer rend;
    public HashSet<Edge> adjacentEdges;

    // Use this for initialization
    void Start () {
        //Fetch the Renderer from the GameObject
        rend = GetComponent<Renderer>();
        //Set the main Color of the Material to green
        rend.material.color = Color.green;
    }

    // Update is called once per frame
    void Update () {
	   

	}
}
