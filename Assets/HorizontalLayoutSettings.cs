using System;
using System.Collections.Generic;
using To_Pixel_Art;
using To_Pixel_Art.Palettes.ExistPalette;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Object = UnityEngine.Object;

public class HorizontalLayoutSettings : MonoBehaviour
{
	[SerializeField]
	private GameObject titlePrefab;
	[SerializeField]
	private GameObject sliderPrefab;
	[SerializeField]
	private GameObject enumPrefab;
	[SerializeField]
	private GameObject togglePrefab;

	private void Awake()
	{
		
		TextController   基础设置  = CreateTitle("基础设置");
		SliderController 大小比例  = CreateSlider("大小比例", 2f, 20f, true);
		EnumController   调色板类型 = CreateEnum<PaletteType>("调色板类型");
		EnumController   固定调色板 = CreateEnum<Palette>("固定调色板");
		SliderController 色彩数量  = CreateSlider("色彩数量", 2f, 64f, true);
		SliderController 带宽    = CreateSlider("带宽", 0.03f, 0.25f, false);
		调色板类型.AddListener(arg0 =>
		{
			switch (arg0)
			{
				case 0:
					固定调色板.gameObject.SetActive(true);
					色彩数量.gameObject.SetActive(false);
					带宽.gameObject.SetActive(false);
					break;
				case 1:
					固定调色板.gameObject.SetActive(false);
					色彩数量.gameObject.SetActive(true);
					带宽.gameObject.SetActive(false);
					break;
				case 2:
					固定调色板.gameObject.SetActive(false);
					色彩数量.gameObject.SetActive(false);
					带宽.gameObject.SetActive(true);
					break;
			}
		});
		SliderController 描边强度  = CreateSlider("描边强度", 0.05f, 1.5f, false);
		TextController   色彩调整  = CreateTitle("色彩调整");
		SliderController 亮度    = CreateSlider("亮度", 0f, 2f, false);
		SliderController 饱和度   = CreateSlider("饱和度", 0f, 2f, false);
		SliderController 色相    = CreateSlider("色相", 0f, 2f, false);
		SliderController 对比度   = CreateSlider("对比度", 0f, 2f, false);
		SliderController 色彩极化  = CreateSlider("色彩极化", 0f, 2f, false);
		ToggleController 降噪    = CreateToggle("降噪");
		SliderController 空间权重  = CreateSlider("空间权重", 0.5f, 5f, false);
		SliderController 颜色权重  = CreateSlider("颜色权重", 0.1f, 0.5f, false);
		SliderController 滤波器半径 = CreateSlider("滤波器半径", 1f, 9f, true);
		降噪.AddListener(arg0 =>
		{
			if (arg0)
			{
				空间权重.gameObject.SetActive(true);
				颜色权重.gameObject.SetActive(true);
				滤波器半径.gameObject.SetActive(true);
			}
			else
			{
				空间权重.gameObject.SetActive(false);
				颜色权重.gameObject.SetActive(false);
				滤波器半径.gameObject.SetActive(false);
			}
		});
		
		string                    text       = Resources.Load<TextAsset>("Data").text;
		Dictionary<string, float> dictionary = DataManager.ParseData(text);
		foreach ((string key, float value) in dictionary)
		{
			ValueManager.Instance.SetDefaults(key, value);
		}
		
		ValueManager.Instance.ResetAll();
	}

	private TextController CreateTitle(string name)
	{
		GameObject instantiate = Instantiate(titlePrefab, transform);
		instantiate.name = name;
		TextController component = instantiate.GetComponent<TextController>();
		component.Set(name);
		return component;
	}

	private SliderController CreateSlider(string name, float min, float max, bool intValue)
	{
		GameObject instantiate = Instantiate(sliderPrefab, transform);
		instantiate.name = name;
		SliderController sliderController = instantiate.GetComponent<SliderController>();
		sliderController.Set(name, min, max, intValue,0f);
		return sliderController;
	}

	private EnumController CreateEnum<T>(string name) where T : Enum
	{
		GameObject instantiate = Instantiate(enumPrefab, transform);
		instantiate.name = name;
		EnumController enumController = instantiate.GetComponent<EnumController>();
		enumController.Set<T>(name,0f);
		return enumController;
	}

	private ToggleController CreateToggle(string name)
	{
		GameObject instantiate = Instantiate(togglePrefab, transform);
		instantiate.name = name;
		ToggleController toggleController = instantiate.GetComponent<ToggleController>();
		toggleController.Set(name,0f);
		return toggleController;
	}

	public GameObject CreateToTransform(GameObject gameObjectToInstantiate, Transform targetTransform)
	{
		GameObject newObject   = Instantiate(gameObjectToInstantiate, targetTransform.parent);
		int        targetIndex = targetTransform.GetSiblingIndex();
		newObject.transform.SetSiblingIndex(targetIndex + 1);
		return newObject;
	}
}