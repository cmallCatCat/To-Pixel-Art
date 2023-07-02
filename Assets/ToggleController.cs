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

		ValueManager.Instance.SetDefaults(name, defaultValue);
		reset.onClick.AddListener(ResetValue);
		ValueManager.Instance.Register(toggle, name);
	}

	private void ResetValue()
	{
		ValueManager.Instance.ResetValue(toggle, name);
	}

	public void AddListener(UnityAction<bool> SetValue)
	{
		toggle.onValueChanged.AddListener(SetValue);
	}
}