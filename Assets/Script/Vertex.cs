using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vertex : MonoBehaviour {

    public Renderer rend;
    public HashSet<Edge> adjacentEdges;

    // Whether the vertex is part of a static graph
    public bool staticVertex;

    // Use this for initialization
    void Start () {

    }

    private void Awake()
    {
        //Fetch the Renderer from the GameObject
        rend = GetComponent<Renderer>();
        //Set the main Color of the Material to white
        rend.material.color = Color.white;

        staticVertex = false;
    }

    public void ChangeOpacity()
    {
        // Set opacity
        var temp = GetComponent<Renderer>().material.color;
        temp.a = 0.1f;
        GetComponent<Renderer>().material.color = temp;
        //Debug.Log(GetComponent<Renderer>().material.color.a);
    }


    void OnTriggerEnter(Collider other)
    {
        if (other.name == "Sphere(Clone)")
        {
            Vertex collidedVertex = other.gameObject.GetComponent<Vertex>();
            if (collidedVertex.staticVertex)
            {
                Debug.Log("Collided with static vertex");
                Debug.Log("this pos" + this.transform.position);
                Debug.Log("other pos" + collidedVertex.transform.position);
                this.transform.position = collidedVertex.transform.position;
                Debug.Log("this pos after" + this.transform.position);
                //GameManager.Instance.selectedVertex = null;
            }
        }
    }

    // Update is called once per frame
    void Update () {
 
    }
}
