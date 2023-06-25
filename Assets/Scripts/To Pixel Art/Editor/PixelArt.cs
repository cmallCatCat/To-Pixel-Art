using System;
using StandaloneFileBrowser;
using To_Pixel_Art.Palettes.ExistPalette;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace To_Pixel_Art.Editor
{
	public class PixelArt : EditorWindow
	{
		private Settings  settings = new Settings();
		private Texture2D previewImage;

		private Texture2D preview
		{
			get => previewImage ? previewImage : previewImage = new Texture2D(1, 1, TextureFormat.ARGB32, false);
			set
			{
				previewImage                     = value;
				previewImage.filterMode          = FilterMode.Point;
				previewImage.alphaIsTransparency = true;
				previewImage.Apply();
			}
		}

		private string[] paths;
		private ExtensionFilter[] extensions = new[]
		{ new ExtensionFilter("Image Files", "jpg", "jpeg", "png") };
		private string targetPath;

		#region DisplayWindows
		[MenuItem("Window/New To Pixel")]
		private static void ShowWindow()
		{
			PixelArt window = GetWindow<PixelArt>();
			window.titleContent = new GUIContent("New Pixel Art");
		}

		private void OnGUI()
		{
			settings.num = EditorGUILayout.IntSlider(new GUIContent("Num", "Every Num pixels will be merged into one pixel"),
				settings.num,
				2, 20);
			EditorGUILayout.Space();

			DisplayAdvancedSettings();
			EditorGUILayout.Space();

			DisplayOutputAndPreview();
		}

		private void DisplayAdvancedSettings()
		{
			DisplayPaletteSettings();
			DisplayAdjustmentSettings();
			DisplayColorPolarization();
		}

		private void DisplayColorPolarization()
		{
			settings.polarization = EditorGUILayout.Slider(
				new GUIContent("ColorPolarization"),
				settings.polarization, 0.5f, 1.5f);
			EditorGUILayout.Space();
		}

		private void DisplayOutputAndPreview()
		{
			if (GUILayout.Button("Set Output Path"))
			{
				targetPath = EditorUtility.OpenFolderPanel("Output Path", "", "");
			}
			if (string.IsNullOrEmpty(targetPath))
			{
				return;
			}

			EditorGUILayout.BeginHorizontal();

			if (GUILayout.Button("Select"))
			{
				paths = StandaloneFileBrowser.StandaloneFileBrowser.OpenFilePanel("Select an image file", "C:\\Users\\Administrator\\Pictures",
					extensions, true);
			}
			
			if (GUILayout.Button("Preview"))
			{
				preview = MainLogic.Work(paths, targetPath, settings, false);
			}

			if (GUILayout.Button("Generate"))
			{
				MainLogic.Work(paths, targetPath, settings, true);
			}

			EditorGUILayout.EndHorizontal();

			DrawPreviewTexture();
		}

		private void DisplayAdjustmentSettings()
		{
			settings.brightness = EditorGUILayout.Slider(
				new GUIContent("Brightness",
					"The brightness of the new Texture."),
				settings.brightness, 0f, 2f);
			settings.saturation = EditorGUILayout.Slider(
				new GUIContent("Saturation",
					"The saturation of the new Texture."),
				settings.saturation, 0f, 2f);
			settings.hue = EditorGUILayout.Slider(
				new GUIContent("Hue",
					"The hue of the new Texture."),
				settings.hue, 0f, 2f);
			settings.contrast = EditorGUILayout.Slider(
				new GUIContent("Contrast",
					"The contrast of the new Texture."),
				settings.contrast, 0f, 2f);
			settings.reduceNoise= EditorGUILayout.BeginToggleGroup("Reduce Noise", settings.reduceNoise);
			settings.kernelSize = EditorGUILayout.IntSlider(
				new GUIContent("Kernel Size",
					"The kernel size of the new Texture."),
				settings.kernelSize, 1, 6);
			settings.spatialFactor = EditorGUILayout.Slider(
				new GUIContent("Spatial Factor",
					"The spatial factor of the new Texture."),
				settings.spatialFactor, 0f, 100f);
			settings.colorFactor = EditorGUILayout.Slider(
				new GUIContent("Color Factor",
					"The color factor of the new Texture."),
				settings.colorFactor, 0f, 200f);
			EditorGUILayout.EndToggleGroup();

			EditorGUILayout.Space();
		}

		private void DisplayPaletteSettings()
		{
			settings.paletteType = (PaletteType)EditorGUILayout.EnumPopup(
				new GUIContent("Palette Strategy", "Which type of palette to use."),
				settings.paletteType);
			switch (settings.paletteType)
			{
				case PaletteType.Existent:
					settings.palette = (Palette)EditorGUILayout.EnumPopup(
						new GUIContent("Palette Type",
							"How many colors are in the existent palette. Palettes with fewer colors tend to be more like early video games"),
						settings.palette);
					break;
				case PaletteType.KMeans:
					settings.colorAmount = EditorGUILayout.IntSlider(
						new GUIContent("ColorAmount",
							"How differences in two similar color can be accepted. Bigger value usually lead to less color and and vice versa."),
						settings.colorAmount, 3, 64);
					break;
				case PaletteType.MeanShift:
					settings.bandwidth = EditorGUILayout.Slider(
						new GUIContent("Bandwidth",
							"The bandwidth of the Mean Shift algorithm."),
						settings.bandwidth, 0.03f, 0.25f);
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}

			EditorGUILayout.Space();
		}

		private void DrawPreviewTexture()
		{
			EditorGUILayout.Space(20);
			float width       = EditorGUIUtility.currentViewWidth * 2 / 3;
			float height      = preview.height * width / preview.width;
			Rect  controlRect = EditorGUILayout.GetControlRect(false, preview.height);
			controlRect.x      = width / 4;
			controlRect.width  = width;
			controlRect.height = height;
			GUI.DrawTexture(controlRect, preview);
		}
		#endregion
	}
}