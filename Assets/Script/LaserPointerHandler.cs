using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserPointerHandler : MonoBehaviour
{
    public SteamVR_LaserPointer laserPointer;

    public bool selected;

    // Start is called before the first frame update
    void Start()
    {
        laserPointer = gameObject.GetComponent<SteamVR_LaserPointer>();
        laserPointer.PointerIn += PointerInside;
        laserPointer.PointerOut += PointerOutside;
        selected = false;
    }
    // Update is called once per frame
    void Update()
    {
        //Debug.Log("laser handler!");
    }

    public void PointerInside(object sender, PointerEventArgs e)
    {
        //Debug.Log("pointer is inside this object" + e.target.name);
        if (e.target.name == "Sphere(Clone)" && selected == false)
        {
            selected = true;
            Renderer rend = e.target.gameObject.GetComponent<Renderer>();
            if (rend != null) {
                if (rend.material.color == Color.yellow)
                {
                    rend.material.color = Color.red;
                }
                else if (rend.material.color == Color.red)
                {
                    rend.material.color = Color.green;
                }
                else if (rend.material.color == Color.green)
                {
                    rend.material.color = Color.blue;
                }
                else
                {
                    rend.material.color = Color.yellow;
                }
            }
        }
    }

    public void PointerOutside(object sender, PointerEventArgs e)
    {
        //Debug.Log("pointer is outside this object" + e.target.name);

        if (e.target.name == "Sphere(Clone)" && selected == true)
        {
            selected = false;
        }
    }

    public bool get_selected_value()
    {
        return selected;
    }
}
