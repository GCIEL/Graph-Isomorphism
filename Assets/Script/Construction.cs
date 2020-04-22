using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class Construction : MonoBehaviour {

    private SteamVR_TrackedController trackedController;
    public Vertex vertexPrefab;
    public Edge edgePrefab;

    // Use this for initialization
    void Start () {
        GameManager.Instance.drawGraph(vertexPrefab, edgePrefab);
    }

}
