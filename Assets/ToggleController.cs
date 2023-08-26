using System;
using To_Pixel_Art;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ToggleController : MonoBehaviour
{
	[SerializeField]
	private Text text;
	[SerializeField]
	private Toggle toggle;
	[SerializeField]
	private Button reset;

	public void Set(string name, float defaultValue)
	{
		text.text = name;

		toggle.onValueChanged.AddListener(arg0 => { ValueManager.Instance.SetChangedValue(name, arg0 ? 1f : 0f); });

		reset.onClick.AddListener(() => ValueManager.Instance.ResetValue(toggle, name));

		ValueManager.Instance.SetDefaults(name, defaultValue);
		ValueManager.Instance.RegisterValueChanged(name, arg0 =>
		{
			// ReSharper disable once CompareOfFloatsByEqualityOperator
			toggle.SetIsOnWithoutNotify(arg0 == 1f);
		});
	}

	public void AddListener(UnityAction<bool> SetValue)
	{
		toggle.onValueChanged.AddListener(SetValue);
	}
}