using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

public class SetValue : MonoBehaviour
{
    private Text text;
    private void Start()
    {
        text = GetComponent<Text>();
    }
    
    public void Set(float value)
    {
        text.text = value.ToString(CultureInfo.CurrentCulture);
    }
}
