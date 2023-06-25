using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayByBool : MonoBehaviour
{
    public GameObject[] objects;

    public void SetDisplay(bool display)
    {
        foreach (GameObject t in objects)
        {
            t.SetActive(display);
        }
    }
}
