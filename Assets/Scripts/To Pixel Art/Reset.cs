using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Reset : MonoBehaviour
{
    private Slider slider;
    private float startValue;

    private void   Start()
    {
        slider = GetComponent<Slider>();
        startValue = slider.value;
    }
    

    public void ResetValue()
    {
        slider.value = startValue;
    }
}
