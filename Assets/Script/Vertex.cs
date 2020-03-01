using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public struct vertexInfo
{
    public bool staticVertex;
    public int vertexNumber;
}

public class Vertex : MonoBehaviour
{

    public Renderer rend;
    public HashSet<Edge> incidentEdges;
    public HashSet<int> adjacentVertexNum;

    // Whether the vertex is part of a static graph
    public vertexInfo information;

    // If static vertex, record the number of colliding dynamic vertex.
    public int numDynamicVertexCollided;

   

    // Use this for initialization
    void Start()
    {

    }

    private void Awake()
    {
        //Fetch the Renderer from the GameObject
        rend = GetComponent<Renderer>();
        //Set the main Color of the Material to white
        rend.material.color = Color.white;
        information = new vertexInfo
        {
            staticVertex = false
        };
        numDynamicVertexCollided = 0;
        adjacentVertexNum = new HashSet<int>();
    }

    public void ChangeOpacity()
    {
        // Set opacity
        var temp = GetComponent<Renderer>().material.color;
        temp.a = 0.2f;
        GetComponent<Renderer>().material.color = temp;
    }

    public void recordAdjacentVerices()
    {
        foreach (Edge edge in incidentEdges)
        {
            foreach (Vertex vertex in edge.adjacentVertices.ToList<Vertex>())
            {
                if (vertex.information.vertexNumber != this.information.vertexNumber)
                {
                    adjacentVertexNum.Add(vertex.information.vertexNumber);
                }
            }
        }
    }


    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "nonMovableVertex")
        {
            Vertex collidedVertex = other.gameObject.GetComponent<Vertex>();
            collidedVertex.numDynamicVertexCollided++;
            if (collidedVertex.numDynamicVertexCollided > 1) return;
            this.transform.position = collidedVertex.transform.position;
            GameManager.Instance.selectedVertex = null;
            GameManager.Instance.moveEdges(this);

            if (GameManager.Instance.mapping.Contains(collidedVertex.information.vertexNumber))
            {
                GameManager.Instance.mapping[collidedVertex.information.vertexNumber] = this;
            }
            else
            {
                GameManager.Instance.mapping.Add(collidedVertex.information.vertexNumber, this);
            }
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.tag == "nonMovableVertex")
        {
            Debug.Log("leaving vertex");
            Vertex collidedVertex = other.gameObject.GetComponent<Vertex>();
            collidedVertex.numDynamicVertexCollided--;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

}
