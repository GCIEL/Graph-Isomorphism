using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Construction : MonoBehaviour {

    private SteamVR_TrackedController trackedController;
    public Vertex vertexPrefab;
    public Edge edgePrefab;

    // Use this for initialization
    void Start () {
        GameManager.Instance.buildGraph(Resources.Load("PyramidAdjMatrix", typeof(TextAsset)) as TextAsset, Resources.Load("PyramidPos", typeof(TextAsset)) as TextAsset, vertexPrefab, edgePrefab, false);
        GameManager.Instance.buildGraph(Resources.Load("PyramidAdjMatrix", typeof(TextAsset)) as TextAsset, Resources.Load("PyramidPos", typeof(TextAsset)) as TextAsset, vertexPrefab, edgePrefab, true);
    }
    
	// Update is called once per frame
	void Update () {
        if (GameManager.Instance.mapping.Count == GameManager.Instance.static_vertex_list.Count)
        {
            foreach (Vertex vertex in GameManager.Instance.static_vertex_list)
            {
                Vertex mappedVertex = (Vertex) GameManager.Instance.mapping[vertex.information.vertexNumber];
                HashSet<int> adjacentVertexNum = mappedVertex.adjacentVertexNum;
                foreach (int vertexNum in vertex.adjacentVertexNum)
                {
                    if (!adjacentVertexNum.Contains(vertexNum)) return;
                }
            }
            Debug.Log("done!");
        }
    }

  
}
