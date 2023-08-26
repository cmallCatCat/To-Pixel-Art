using System;
using System.Collections.Generic;
using System.Diagnostics;
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
		public static Texture2D Work(string[] texture2DPaths, string targetPath, string settingString, bool generate)
		{
			// 初始化
			Texture2D   texture2D  = null;
			Texture2D[] texture2Ds = LoadTexturesFromPaths(texture2DPaths);

			Dictionary<string, float> settings      = DataManager.ParseData(settingString);
			int                       num           = (int)settings["大小比例"];
			PaletteType               paletteType   = (PaletteType)settings["调色板类型"];
			Palette                   palette       = (Palette)settings["固定调色板"];
			int                       colorAmount   = (int)settings["色彩数量"];
			float                     bandwidth     = settings["带宽"];
			float                     edgeThreshold = settings["描边强度"];
			float                     brightness    = settings["亮度"];
			float                     saturation    = settings["饱和度"];
			float                     hue           = settings["色相"];
			float                     contrast      = settings["对比度"];
			float                     polarization  = settings["色彩极化"];
			bool                      reduceNoise   = settings["启用降噪"] > 0f;
			float                     spatialSigma  = settings["空间权重"];
			float                     colorSigma    = settings["颜色权重"];
			int                       filterRadius  = (int)settings["滤波器半径"];

			WorkerPalette worker = paletteType switch
			{ PaletteType.Existent  => new ExistWorkerPalette(num, palette),
			  PaletteType.KMeans    => new KMeansWorkerPalette(num, colorAmount),
			  PaletteType.MeanShift => new MeanShiftWorkerPalette(num, bandwidth),
			  _                     => throw new ArgumentOutOfRangeException() };

			for (int index = 0; index < texture2Ds.Length; index++)
			{
				texture2D = texture2Ds[index];
				// 创建与预处理
				string    oldPath      = texture2DPaths[index];
				Texture2D newTexture2D = new Texture2D(texture2D.width / num, texture2D.height / num);
				texture2D = ImageColorAdjuster.AdjustImageColors(texture2D, brightness, contrast, hue, saturation);
				// 边缘处理
				texture2D = ImageColorAdjuster.ApplyEdgeDetection(texture2D, edgeThreshold);
				// 降噪
				if (reduceNoise)
				{
					texture2D = ImageColorAdjuster.ApplyBilateralFilter(texture2D, spatialSigma, colorSigma, filterRadius);
				}
				// 取色
				List<Color> finalPalette = worker.GetPalette(texture2D);
				// 生成
				PixelColorUtility.ApplyPalette(newTexture2D, texture2D, finalPalette, num, polarization);
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