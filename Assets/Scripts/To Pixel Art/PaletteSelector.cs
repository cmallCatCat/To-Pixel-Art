using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class PaletteSelector : MonoBehaviour
{
    public GameObject obj1;
    public GameObject obj2;
    public GameObject obj3;

    public void OnValueChanged(int index)
    {
        obj1.SetActive(false);
        obj2.SetActive(false);
        obj3.SetActive(false);
        switch (index)
        {
            case 0:
                obj1.SetActive(true);
                break;
            case 1:
                obj2.SetActive(true);
                break;
            case 2:
                obj3.SetActive(true);
                break;
        }
    }
}
