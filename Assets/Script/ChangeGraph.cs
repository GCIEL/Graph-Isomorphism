using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeGraph : MonoBehaviour
{
    private SteamVR_TrackedController trackedController;

    // Start is called before the first frame update
    void Start()
    {
        trackedController = GetComponent<SteamVR_TrackedController>();
        if (trackedController == null)
        {
            trackedController = GetComponentInParent<SteamVR_TrackedController>();
        }
        trackedController.TriggerClicked += TriggerClicked;
    }
    private void TriggerClicked(object sender, ClickedEventArgs e)
    {
        GameManager.Instance.changeGraph();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
