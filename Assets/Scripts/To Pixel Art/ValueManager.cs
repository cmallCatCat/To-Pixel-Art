using System;
using System.Collections.Generic;
using System.Reflection;
using Accord.IO;
using To_Pixel_Art.Palettes.ExistPalette;
using UnityEngine;
using UnityEngine.UI;

namespace To_Pixel_Art
{
	public class ValueManager : MonoBehaviour
	{
		private Dictionary<string, float>      settings   = new Dictionary<string, float>();
		private Dictionary<string, Selectable> uiElements = new Dictionary<string, Selectable>();

		public void SetSettings(Dictionary<string, float> newSettings)
		{
			foreach ((string key, float value) in newSettings) { }
		}

		public void Register<T>(T control, string settingName) where T : Selectable
		{
			if (!uiElements.ContainsKey(settingName))
			{
				uiElements.Add(settingName, control);
				ResetValue(control, settingName);
			}
		}

		public void UnRegister(string settingName)
		{
			uiElements.Remove(settingName);
		}

		public float GetDefaultValue(string settingName)
		{
			return settings[settingName];
		}

		public void ResetValue<T>(T control, string settingName) where T : Selectable
		{
			switch (control)
			{
				case Slider slider:
				{
					float value = GetDefaultValue(settingName);
					slider.value = value;
					slider.onValueChanged.Invoke(value);
					break;
				}
				case Dropdown dropdown:
				{
					int value = (int)GetDefaultValue(settingName);
					dropdown.value = value;
					dropdown.onValueChanged.Invoke(value);
					break;
				}
				case Toggle toggle:
				{
					bool value = GetDefaultValue(settingName) > 0f;
					toggle.isOn = value;
					toggle.onValueChanged.Invoke(value);
					break;
				}
				default:
					Debug.LogError($"Unsupported control type: {typeof(T)}");
					break;
			}
		}
		
		public void ResetAll()
		{
			foreach (KeyValuePair<string, Selectable> pair in uiElements)
			{
				ResetValue(pair.Value, pair.Key);
			}
		}

		public T GetControl<T>(string settingName) where T : Selectable
		{
			if (uiElements.TryGetValue(settingName, out Selectable control))
			{
				return control as T;
			}
			else
			{
				Debug.LogError($"Control not found for setting: {settingName}");
				return null;
			}
		}

		private static ValueManager valueManager;

		public static ValueManager Instance =>
			valueManager == null ? valueManager = FindObjectOfType<ValueManager>() : valueManager;

		public void SetDefaults(string key, float value)
		{
			settings[key] = value;
		}
	}
}