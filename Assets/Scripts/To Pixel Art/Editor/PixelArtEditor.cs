using System;
using To_Pixel_Art.Editor.Palettes.ExistPalette;
using UnityEditor;
using UnityEngine;

namespace To_Pixel_Art.Editor
{
	public partial class PixelArt
	{
		#region 参数
		private float       whiteThreshold = 0.8f;
		private int         num            = 6;
		private bool        replaceBool    = false;
		private string      additive       = " pix";
		public  PaletteType paletteType    = PaletteType.KMeans;
		private float       blackThreshold = 0.1f;
		private float       alphaThreshold = 0.7f;
		private int         colorAmount    = 16;
		private float       bandwidth      = 0.05f;
		private Palette     palette        = Palette.Color64;
		private float       brightness     = 1f;
		private float       saturation     = 1f;
		private float       hue            = 0.5f;
		private float       contrast       = 1f;
		private int         kernelSize     = 1;
		private float       spatialFactor  = 10f;
		private float       colorFactor    = 60f;
		#endregion

		#region 显示
		private bool      advanced;
		private Texture2D preview;
		#endregion

		#region DisplayWindows
		[MenuItem("Window/New To Pixel")]
		private static void ShowWindow()
		{
			PixelArt window = GetWindow<PixelArt>();
			window.titleContent = new GUIContent("New Pixel Art");
		}

		private void OnGUI()
		{
			InitializeDefaults();

			num = EditorGUILayout.IntSlider(new GUIContent("Num", "Every Num pixels will be merged into one pixel"), num, 2, 20);
			EditorGUILayout.Space();

			DisplayAdvancedSettings();
			EditorGUILayout.Space();

			DisplayOutputAndPreview();
		}

		private void DisplayAdvancedSettings()
		{
			advanced = EditorGUILayout.Foldout(advanced, "Advanced");

			if (advanced)
			{
				DisplayPaletteSettings();
				DisplayConnectivitySettings();
				DisplayAdjustmentSettings();
				DisplayAlphaThreshold();
			}
		}

		private void InitializeDefaults()
		{
			if (additive == "")
			{
				additive = " pixel";
			}
		}

		private void DisplayAlphaThreshold()
		{
			alphaThreshold = EditorGUILayout.Slider(
				new GUIContent("Alpha Threshold",
					"When the alpha of a pixel is higher than Alpha Threshold, the pixel is displayed as black. Reduce this can bring the outline effect."),
				alphaThreshold, 0.001f, 1);
			EditorGUILayout.Space();
		}

		private void DisplayConnectivitySettings() { }

		private void DisplayOutputAndPreview()
		{
			DisplayReplaceOrAdditive();

			DisplayHelpBox();

			EditorGUILayout.BeginHorizontal();
			if (GUILayout.Button("Preview") || preview == null)
			{
				Preview();
			}

			if (GUILayout.Button("Generate"))
			{
				Work();
			}

			EditorGUILayout.EndHorizontal();

			DrawPreviewTexture();
		}

		private void DisplayReplaceOrAdditive()
		{
			replaceBool = EditorGUILayout.Toggle(
				new GUIContent("Replace the original texture", "If true, the output texture will replace the original texture."),
				replaceBool);
			if (replaceBool)
				additive = "";
			else
				additive = EditorGUILayout.TextField(
					new GUIContent("Additive",
						"Name additive for the new Texture. For example, the name of original texture is test.png, and Additive is _1, then the name of output texture is test_1.png."),
					additive);
		}

		private void DisplayAdjustmentSettings()
		{
			brightness = EditorGUILayout.Slider(
				new GUIContent("Brightness",
					"The brightness of the new Texture."),
				brightness, 0f, 2f);
			saturation = EditorGUILayout.Slider(
				new GUIContent("Saturation",
					"The saturation of the new Texture."),
				saturation, 0f, 2f);
			hue = EditorGUILayout.Slider(
				new GUIContent("Hue",
					"The hue of the new Texture."),
				hue, 0f, 2f);
			contrast = EditorGUILayout.Slider(
				new GUIContent("Contrast",
					"The contrast of the new Texture."),
				contrast, 0f, 2f);
			kernelSize = EditorGUILayout.IntSlider(
				new GUIContent("Kernel Size",
					"The kernel size of the new Texture."),
				kernelSize, 1, 5);
			spatialFactor = EditorGUILayout.Slider(
				new GUIContent("Spatial Factor",
					"The spatial factor of the new Texture."),
				spatialFactor, 0f, 100f);
			colorFactor = EditorGUILayout.Slider(
				new GUIContent("Color Factor",
					"The color factor of the new Texture."),
				colorFactor, 0f, 200f);

			EditorGUILayout.Space();
		}

		private void DisplayPaletteSettings()
		{
			paletteType = (PaletteType)EditorGUILayout.EnumPopup(
				new GUIContent("Palette Strategy", "Which type of palette to use."), paletteType);
			switch (paletteType)
			{
				case PaletteType.Existent:
					palette = (Palette)EditorGUILayout.EnumPopup(
						new GUIContent("Palette Type",
							"How many colors are in the existent palette. Palettes with fewer colors tend to be more like early video games"),
						palette);
					break;
				case PaletteType.KMeans:
					colorAmount = EditorGUILayout.IntSlider(
						new GUIContent("ColorAmount",
							"How differences in two similar color can be accepted. Bigger value usually lead to less color and and vice versa."),
						colorAmount, 3, 64);
					break;
				case PaletteType.MeanShift:
					bandwidth = EditorGUILayout.Slider(
						new GUIContent("Bandwidth",
							"The bandwidth of the Mean Shift algorithm."),
						bandwidth, 0.03f, 0.25f);
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}

			EditorGUILayout.Space();
		}

		private static void DisplayHelpBox()
		{
			EditorGUILayout.HelpBox(
				"The Import Settings will be adjusted appropriately according to the original texture import settings and inherent attributes.",
				MessageType.Info);
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

		private void DrawPreview(Texture2D preview)
		{
			this.preview                     = preview;
			this.preview.filterMode          = FilterMode.Point;
			this.preview.alphaIsTransparency = true;
			this.preview.Apply();
		}
		#endregion
	}
}