using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewConstruction : MonoBehaviour {

    private SteamVR_TrackedController trackedController;

    // Use this for initialization
    void Start () {
        GameManager.Instance.buildGraph(Resources.Load("PyramidAdjMatrix", typeof(TextAsset)) as TextAsset, Resources.Load("PyramidPos", typeof(TextAsset)) as TextAsset, Resources.Load("Sphere", typeof(Vertex)) as Vertex, Resources.Load("Cylinder", typeof(Edge)) as Edge);
    }
    
	// Update is called once per frame
	void Update () {

    }

  
}
