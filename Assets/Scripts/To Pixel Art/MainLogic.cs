using System;
using System.Collections.Generic;
using System.IO;
using To_Pixel_Art.Palettes;
using To_Pixel_Art.Palettes.ExistPalette;
using To_Pixel_Art.Palettes.KMeansPalette;
using To_Pixel_Art.Palettes.MeanShiftPalette;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace To_Pixel_Art
{
	public static class MainLogic
	{
		public static Texture2D Work(string[] texture2DPaths, string targetPath, Settings settings, bool generate)
		{
			// 初始化
			Texture2D   texture2D  = null;
			Texture2D[] texture2Ds = LoadTexturesFromPaths(texture2DPaths);
			settings.Deconstruct(out int num, out PaletteType paletteType, out Palette palette, out int colorAmount,
				out float bandwidth, out float brightness, out float saturation, out float hue, out float contrast,
				out bool reduceNoise,
				out int kernelSize, out float spatialFactor, out float colorFactor, out float effect);
			WorkerPalette worker;
			switch (paletteType)
			{
				case PaletteType.Existent:
					worker = new ExistWorkerPalette(num, palette);
					break;
				case PaletteType.KMeans:
					worker = new KMeansWorkerPalette(num, colorAmount);
					break;
				case PaletteType.MeanShift:
					worker = new MeanShiftWorkerPalette(num, bandwidth);
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}

			for (int index = 0; index < texture2Ds.Length; index++)
			{
				texture2D = texture2Ds[index];
				// 创建
				string    oldPath      = texture2DPaths[index];
				Texture2D newTexture2D = new Texture2D(texture2D.width / num, texture2D.height / num);
				// 预处理
				texture2D = ImageColorAdjuster.AdjustImageColors(texture2D, brightness, contrast, hue, saturation);
				// 取色
				List<Color> finalPalette = worker.GetPalette(texture2D);
				// 降噪
				if (reduceNoise)
				{
					texture2D = ImageColorAdjuster.ReduceNoise(texture2D, kernelSize * 2 + 1, spatialFactor, colorFactor);
				}
				// 生成
				PixelColorUtility.ApplyPalette(newTexture2D, texture2D, finalPalette, num, effect);
				// 保存
				if (!generate && index == 0)
				{
					return newTexture2D;
				}
				string newPath = ConvertPath(oldPath, targetPath);
				Save(newPath, newTexture2D);
			}
			return texture2D;
		}

		public static Texture2D[] LoadTexturesFromPaths(string[] texture2DPaths)
		{
			int         length   = texture2DPaths.Length;
			Texture2D[] textures = new Texture2D[length];

			for (int i = 0; i < length; i++)
			{
				string path = texture2DPaths[i];

				if (File.Exists(path))
				{
					byte[]    imageData = File.ReadAllBytes(path);
					Texture2D texture   = new Texture2D(2, 2);
					if (texture.LoadImage(imageData))
					{
						textures[i] = texture;
					}
					else
					{
						Debug.LogError("Failed to load texture from path: " + path);
					}
				}
				else
				{
					Debug.LogError("File does not exist at path: " + path);
				}
			}

			return textures;
		}

		public static void Save(string path, Texture2D texture)
		{
			if (texture == null)
			{
				Debug.LogError("Texture is null. Cannot save.");
				return;
			}

			byte[] imageData = texture.EncodeToPNG();

			if (imageData != null)
			{
				try
				{
					File.WriteAllBytes(path, imageData);
					Debug.Log("Texture saved to: " + path);
				}
				catch (Exception e)
				{
					Debug.LogError("Error saving texture to path: " + path + "\n" + e.Message);
				}
			}
			else
			{
				Debug.LogError("Failed to encode texture to PNG.");
			}
		}

		public static string ConvertPath(string inputFilePath, string outputFolderPath)
		{
			if (!File.Exists(inputFilePath))
			{
				Debug.LogError("Input file does not exist: " + inputFilePath);
				return string.Empty;
			}

			if (!Directory.Exists(outputFolderPath))
			{
				Debug.LogError("Output folder does not exist: " + outputFolderPath);
				return string.Empty;
			}

			string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(inputFilePath);
			string newFileName              = fileNameWithoutExtension + "_pix" + ".png";
			string outputPath               = Path.Combine(outputFolderPath, newFileName);

			return outputPath;
		}
	}
}