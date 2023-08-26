using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace To_Pixel_Art
{
	public class ValueManager : MonoBehaviour
	{
		private Dictionary<string, float> settings = new Dictionary<string, float>();
		private Dictionary<string, float> changed  = new Dictionary<string, float>();

		private Dictionary<string, UnityAction<float>> onValueChanged = new Dictionary<string, UnityAction<float>>();

		private Dictionary<string, UnityAction> buttonDown = new Dictionary<string, UnityAction>();

		public string outPutPath = "C:\\Users\\Administrator\\Pictures";

		public string selectImage;

		public void ResetValue<T>(T control, string settingName) where T : Selectable
		{
			SetChangedValue(settingName, settings[settingName]);
		}

		public void RegisterValueChanged(string settingName, UnityAction<float> callback)
		{
			if (onValueChanged.TryGetValue(settingName, out UnityAction<float> action))
			{
				onValueChanged[settingName] -= callback;
				onValueChanged[settingName] += callback;
			}
			else
			{
				onValueChanged[settingName] = callback;
			}
		}

		public void RegisterButtonDown(string name, UnityAction callback)
		{
			if (buttonDown.TryGetValue(name, out UnityAction action))
			{
				buttonDown[name] -= callback;
				buttonDown[name] += callback;
			}
			else
			{
				buttonDown[name] = callback;
			}
		}

		public void ButtonDown(string name)
		{
			if (buttonDown.TryGetValue(name, out UnityAction action))
			{
				action?.Invoke();
			}
		}

		public void UnRegisterValueChanged(string settingName, UnityAction<float> callback)
		{
			onValueChanged[settingName] -= callback;
		}

		public void SetChangedValue(string settingName, float value)
		{
			changed[settingName] = value;
			if (onValueChanged.TryGetValue(settingName, out UnityAction<float> action))
			{
				action?.Invoke(value);
			}
		}

		public void SetSettingsValue(string settingName, float value)
		{
			settings[settingName] = value;
		}

		public void SetDefaults(string key, float value)
		{
			SetSettingsValue(key, value);
			SetChangedValue(key, value);
		}

		public string GetValueString()
		{
			return DataManager.ConvertData(changed);
		}

		private static ValueManager VALUE_MANAGER;

		public static ValueManager Instance =>
			VALUE_MANAGER == null ? VALUE_MANAGER = FindObjectOfType<ValueManager>() : VALUE_MANAGER;
	}
}