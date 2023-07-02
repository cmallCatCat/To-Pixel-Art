using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using To_Pixel_Art;
using UnityEngine;
using UnityEngine.UI;

public class SliderController : MonoBehaviour
{
	[SerializeField]
	private Text text;
	[SerializeField]
	private Slider slider;
	[SerializeField]
	private Text value;
	[SerializeField]
	private Button reset;

	public void Set(string name, float min, float max, bool intValue, float defaultValue)
	{
		text.text           = name;
		slider.minValue     = min;
		slider.maxValue     = max;
		slider.wholeNumbers = intValue;

		ValueManager.Instance.SetDefaults(name, defaultValue);
		reset.onClick.AddListener(ResetValue);
		slider.onValueChanged.AddListener(SetValue);
		ValueManager.Instance.Register(slider, name);
	}

	private void SetValue(float arg0)
	{
		value.text = arg0.ToString(CultureInfo.CurrentCulture);
	}

	private void ResetValue()
	{
		ValueManager.Instance.ResetValue(slider, name);
	}
}