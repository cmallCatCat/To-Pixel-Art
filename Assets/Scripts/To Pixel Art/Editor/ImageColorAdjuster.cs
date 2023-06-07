﻿using System.Drawing;
using System.IO;
using Accord.Imaging.Filters;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Processing.Processors;
using UnityEngine;
using Color = UnityEngine.Color;
using Image = SixLabors.ImageSharp.Image;

namespace To_Pixel_Art.Editor
{
	public static class ImageColorAdjuster
	{
		public static Texture2D AdjustImageColors(
			Texture2D inputTexture, float brightness, float contrast, float hue, float saturation)
		{
			// 将Unity Texture2D转换为ImageSharp Image
			byte[]    inputData     = inputTexture.EncodeToPNG();
			using var inputStream   = new MemoryStream(inputData);
			using var originalImage = Image.Load<Rgba32>(inputStream);

			// 调整亮度、对比度、色相和饱和度
			originalImage.Mutate(x =>
			{
				x.Brightness(brightness);
				x.Contrast(contrast);
				x.Saturate(saturation);
				x.Hue(hue * 180.0f - 90.0f);
			});

			// 将调整后的ImageSharp Image转换为Unity Texture2D
			using var outputStream = new MemoryStream();
			originalImage.Save(outputStream, new SixLabors.ImageSharp.Formats.Png.PngEncoder());
			var outputTexture = new Texture2D(inputTexture.width, inputTexture.height);
			outputTexture.LoadImage(outputStream.ToArray());

			return outputTexture;
		}

		// public static Texture2D ReduceNoise(Texture2D inputTexture, int kernelSize = 3, double spatialFactor = 10,
		// 	double                                    colorFactor = 60)
		// {
		// 	// 将Unity Texture2D转换为ImageSharp Image
		// 	byte[]    inputData     = inputTexture.EncodeToPNG();
		// 	using var inputStream   = new MemoryStream(inputData);
		// 	using var originalImage = Image.Load<Rgba32>(inputStream);
		//
		// 	// 将ImageSharp Image转换为System.Drawing.Bitmap
		// 	var       converter = new SixLabors.ImageSharp.Formats.Bmp.BmpEncoder();
		// 	using var bmpStream = new MemoryStream();
		// 	originalImage.Save(bmpStream, converter);
		// 	bmpStream.Position = 0;
		// 	var bitmap = new Bitmap(bmpStream);
		//
		// 	// 应用Accord的BilateralSmoothing降噪滤波器
		// 	var filter = new BilateralSmoothing();
		// 	filter.KernelSize    = kernelSize;
		// 	filter.ColorFactor   = colorFactor;
		// 	filter.SpatialFactor = spatialFactor;
		// 	var resultBitmap = filter.Apply(bitmap);
		//
		// 	// 将处理后的System.Drawing.Bitmap转换为Unity Texture2D
		// 	using var resultStream = new MemoryStream();
		// 	resultBitmap.Save(resultStream, System.Drawing.Imaging.ImageFormat.Png);
		// 	var outputTexture = new Texture2D(inputTexture.width, inputTexture.height);
		// 	outputTexture.LoadImage(resultStream.ToArray());
		//
		// 	return outputTexture;
		// }
		
		public static Texture2D ReduceNoise(Texture2D inputTexture, int kernelSize = 3, double spatialFactor = 10,
			double                                    colorFactor = 60)
		{
			// 将Unity Texture2D转换为ImageSharp Image
			byte[]    inputData     = inputTexture.EncodeToPNG();
			using var inputStream   = new MemoryStream(inputData);
			using var originalImage = Image.Load<Rgba32>(inputStream);

			// 保存透明度信息
			float[][] alphaValues = new float[inputTexture.width][];
			for (int index = 0; index < inputTexture.width; index++)
			{
				alphaValues[index] = new float[inputTexture.height];
			}
			for (int y = 0; y < inputTexture.height; y++)
			{
				for (int x = 0; x < inputTexture.width; x++)
				{
					// 注意：纹理坐标翻转为ImageSharp坐标
					alphaValues[x][y] = originalImage[x, inputTexture.height - 1 - y].A / 255f;
				}
			}

			// 将ImageSharp Image转换为System.Drawing.Bitmap
			var       converter = new SixLabors.ImageSharp.Formats.Bmp.BmpEncoder();
			using var bmpStream = new MemoryStream();
			originalImage.Save(bmpStream, converter);
			bmpStream.Position = 0;
			var bitmap = new Bitmap(bmpStream);

			// 应用Accord的BilateralSmoothing降噪滤波器
			var filter = new BilateralSmoothing();
			filter.KernelSize    = kernelSize;
			filter.ColorFactor   = colorFactor;
			filter.SpatialFactor = spatialFactor;
			var resultBitmap = filter.Apply(bitmap);

			// 将处理后的System.Drawing.Bitmap转换为Unity Texture2D
			using var resultStream = new MemoryStream();
			resultBitmap.Save(resultStream, System.Drawing.Imaging.ImageFormat.Png);
			var outputTexture = new Texture2D(inputTexture.width, inputTexture.height);
			outputTexture.LoadImage(resultStream.ToArray());

			// 应用透明度信息
			for (int y = 0; y < outputTexture.height; y++)
			{
				for (int x = 0; x < outputTexture.width; x++)
				{
					Color color = outputTexture.GetPixel(x, y);
					color.a = alphaValues[x][y];
					outputTexture.SetPixel(x, y, color);
				}
			}

			outputTexture.Apply();

			return outputTexture;
		}

		
	}
}