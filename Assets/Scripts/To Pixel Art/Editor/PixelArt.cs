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
		private Settings  settings;
		private Texture2D previewImage;

		private void Awake()
		{
			settings = Resources.Load<Settings>("DefaultSettings");
		}

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
		private ExtensionFilter[] extensions =
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
			EditorGUILayout.LabelField("Basic Settings", EditorStyles.boldLabel);
			settings.num           = EditorGUILayout.IntSlider("Num", settings.num, 2, 20);
			settings.edgeThreshold = EditorGUILayout.Slider("Edge Threshold", settings.edgeThreshold, 0.05f, 1.5f);
			settings.paletteType   = (PaletteType)EditorGUILayout.EnumPopup("Palette Type", settings.paletteType);
			switch (settings.paletteType)
			{
				case PaletteType.Existent:
					settings.palette = (Palette)EditorGUILayout.EnumPopup("Palette", settings.palette);
					break;
				case PaletteType.KMeans:
					settings.colorAmount = EditorGUILayout.IntSlider("Color Amount", settings.colorAmount, 2, 64);
					break;
				case PaletteType.MeanShift:
					settings.bandwidth = EditorGUILayout.Slider("Bandwidth", settings.bandwidth, 0.03f, 0.25f);
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}

			EditorGUILayout.LabelField("Color Adjustment", EditorStyles.boldLabel);
			settings.brightness   = EditorGUILayout.Slider("Brightness", settings.brightness, 0f, 2f);
			settings.saturation   = EditorGUILayout.Slider("Saturation", settings.saturation, 0f, 2f);
			settings.hue          = EditorGUILayout.Slider("Hue", settings.hue, 0f, 2f);
			settings.contrast     = EditorGUILayout.Slider("Contrast", settings.contrast, 0f, 2f);
			settings.polarization = EditorGUILayout.Slider("Polarization", settings.polarization, 0f, 2f);

			EditorGUILayout.LabelField("Reduce Noise", EditorStyles.boldLabel);
			settings.reduceNoise = EditorGUILayout.Toggle("Reduce Noise", settings.reduceNoise);
			if (settings.reduceNoise)
			{
				settings.spatialSigma = EditorGUILayout.Slider("Spatial Sigma", settings.spatialSigma, 0.5f, 5.0f);
				settings.colorSigma   = EditorGUILayout.Slider("Color Sigma", settings.colorSigma, 0.1f, 0.5f);
				settings.filterRadius = EditorGUILayout.IntSlider("Filter Radius", settings.filterRadius, 1, 10);
			}
			DisplayOutputAndPreview();
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
				paths = StandaloneFileBrowser.StandaloneFileBrowser.OpenFilePanel("Select an image file",
					"C:\\Users\\Administrator\\Pictures",
					extensions, true);
			}

			if (GUILayout.Button("Preview"))
			{
				// preview = MainLogic.Work(paths, targetPath, settings, false);
			}

			if (GUILayout.Button("Generate"))
			{
				// MainLogic.Work(paths, targetPath, settings, true);
			}

			EditorGUILayout.EndHorizontal();

			DrawPreviewTexture();
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