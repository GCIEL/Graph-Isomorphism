using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

}
