using System;
using System.Linq;
using To_Pixel_Art;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class EnumController : MonoBehaviour
{
	[SerializeField]
	private Text text;
	[SerializeField]
	private Dropdown dropdown;
	[SerializeField]
	private Button reset;

	public void Set<T>(string name, float defaultValue) where T: Enum
	{
		text.text = name;
		string[] names = typeof(T).GetEnumNames();
		dropdown.options = names.Select(s => new Dropdown.OptionData(s)).ToList();
		
		ValueManager.Instance.SetDefaults(name, defaultValue);
		reset.onClick.AddListener(ResetValue);
		ValueManager.Instance.Register(dropdown, name);
	}

	public void AddListener(UnityAction<int> SetValue)
	{
		dropdown.onValueChanged.AddListener(SetValue);
	}

	private void OnDestroy()
	{
		ValueManager.Instance.UnRegister( name);
	}

	private void ResetValue()
	{
		ValueManager.Instance.ResetValue(dropdown, name);
	}
}