using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserPointerHandler : MonoBehaviour
{
    public SteamVR_LaserPointer laserPointer;
    private SteamVR_TrackedController trackedController;

    private bool selected;
    private Renderer rend;

    // Start is called before the first frame update
    void Start()
    {
        laserPointer = gameObject.GetComponent<SteamVR_LaserPointer>();
        laserPointer.PointerIn += PointerInside;
        laserPointer.PointerOut += PointerOutside;
        selected = false;

        trackedController = GetComponent<SteamVR_TrackedController>();
        if (trackedController == null)
        {
            trackedController = GetComponentInParent<SteamVR_TrackedController>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("laser handler!");
    }


    public void PointerInside(object sender, PointerEventArgs e)
    {
        Debug.Log("pointer is inside" );
        if (e.target.name == "Sphere(Clone)" && selected == false)
        {
            selected = true;
            if(GameManager.Instance.selectedVertex == null) GameManager.Instance.selectedVertex = e.target.gameObject;
            rend = e.target.gameObject.GetComponent<Renderer>();
            
        }
    }
    

    public void PointerOutside(object sender, PointerEventArgs e)
    {
        Debug.Log("pointer is outside");
        if (e.target.name == "Sphere(Clone)" && selected == true)
        {
            selected = false;
            GameManager.Instance.selectedVertex = null;
        }
    }

    public bool get_selected_value()
    {
        return selected;
    }
}
