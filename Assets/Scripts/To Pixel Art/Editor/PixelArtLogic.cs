using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Timers;
using NUnit.Framework;
using To_Pixel_Art.Editor.Palettes;
using To_Pixel_Art.Editor.Palettes.ExistPalette;
using To_Pixel_Art.Editor.Palettes.KMeansPalette;
using To_Pixel_Art.Editor.Palettes.MeanShiftPalette;
using UnityEditor;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace To_Pixel_Art.Editor
{
	public partial class PixelArt : EditorWindow
	{
		private WorkerPalette worker;
		private Texture2D[]   texture2Ds;

		public void Work()
		{
			Init(true);

			for (int index = 0; index < texture2Ds.Length; index++)
			{
				Texture2D texture2D = texture2Ds[index];
				string    oldPath   = AssetDatabase.GetAssetPath(texture2D);
				FileUtility.SetReadable(oldPath, true);
				texture2D = ImageColorAdjuster.AdjustImageColors(texture2D, brightness, contrast, hue, saturation);
				List<Color> finalPalette = worker.GetPalette(texture2D);
				texture2D = ImageColorAdjuster.ReduceNoise(texture2D, kernelSize * 2 + 1, spatialFactor, colorFactor);
				Texture2D newTexture2D = new Texture2D(texture2D.width / num, texture2D.height / num);
				PixelColorUtility.ApplyPalette(newTexture2D, texture2D, finalPalette, num);
				string newPath = replaceBool ? oldPath : FileUtility.ModifyFileName(oldPath, additive);
				FileUtility.CreateNewTexture(newTexture2D, newPath);
				FileUtility.CopyTextureImporter(oldPath, newPath, num);
				FileUtility.SetReadable(newPath, false);
			}
		}

		private void Preview()
		{
			Init(false);
			
			if (texture2Ds.Length > 0)
			{
				Texture2D texture2D = texture2Ds[0];
				string    oldPath   = AssetDatabase.GetAssetPath(texture2D);
				FileUtility.SetReadable(oldPath, true);


				List<Color> finalPalette = worker.GetPalette(texture2D);

				texture2D = ImageColorAdjuster.AdjustImageColors(texture2D, brightness, contrast, hue, saturation);
				
				texture2D = ImageColorAdjuster.ReduceNoise(texture2D, kernelSize * 2 + 1, spatialFactor, colorFactor);

				Texture2D newTexture2D = new Texture2D(texture2D.width / num, texture2D.height / num);
				PixelColorUtility.ApplyPalette(newTexture2D, texture2D, finalPalette, num);

				DrawPreview(newTexture2D);
				// DrawPreview(texture2D);
			}
			
		}


		private void Init(bool generate)
		{
			if (additive == "")
			{
				additive = " pixel";
			}
			switch (paletteType)
			{
				case PaletteType.Existent:
					worker = new ExistWorkerPalette(
						palette, num, whiteThreshold, blackThreshold, alphaThreshold);
					break;
				case PaletteType.KMeans:
					worker = new KMeansWorkerPalette(num, whiteThreshold,
						blackThreshold, alphaThreshold, colorAmount);
					break;
				case PaletteType.MeanShift:
					worker = new MeanShiftWorkerPalette(num, whiteThreshold,
						blackThreshold, alphaThreshold, bandwidth);
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}

			texture2Ds = Selection.GetFiltered(typeof(Texture2D), SelectionMode.DeepAssets).Select(o => (Texture2D)o).ToArray();

			if (texture2Ds.Length == 0 && !generate)
			{
				preview = new Texture2D(1, 1, TextureFormat.RGBAFloat, false);
			}
		}
	}
}