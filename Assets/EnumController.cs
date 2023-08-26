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
		dropdown.onValueChanged.AddListener(arg0 => ValueManager.Instance.SetChangedValue(name, arg0));
		
		reset.onClick.AddListener(() => ValueManager.Instance.ResetValue(dropdown, name));
		
		ValueManager.Instance.SetDefaults(name, defaultValue);
		ValueManager.Instance.RegisterValueChanged(name, arg0 =>
		{
			dropdown.SetValueWithoutNotify((int)arg0);
		});
	}

	public void AddListener(UnityAction<int> SetValue)
	{
		dropdown.onValueChanged.AddListener(SetValue);
	}
}