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
	private InputField field;
	[SerializeField]
	private Button reset;

	public void Set(string name, float min, float max, bool intValue, float defaultValue)
	{
		text.text           = name;
		slider.minValue     = min;
		slider.maxValue     = max;
		slider.wholeNumbers = intValue;
		field.characterValidation
			= intValue ? InputField.CharacterValidation.Integer : InputField.CharacterValidation.Decimal;

		slider.onValueChanged.AddListener(arg0 => ValueManager.Instance.SetChangedValue(name, arg0));
		field.onValueChanged.AddListener(arg0 =>
		{
			if (float.TryParse(arg0, out float value))
			{
				ValueManager.Instance.SetChangedValue(name, value);
			}
		});

		reset.onClick.AddListener(() => ValueManager.Instance.ResetValue(slider, name));

		ValueManager.Instance.SetDefaults(name, defaultValue);
		ValueManager.Instance.RegisterValueChanged(name, arg0 =>
		{
			slider.SetValueWithoutNotify(arg0);
			field.SetTextWithoutNotify(arg0.ToString(CultureInfo.CurrentCulture));
		});
	}
}