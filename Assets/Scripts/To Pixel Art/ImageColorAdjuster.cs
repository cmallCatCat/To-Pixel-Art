using System;
using System.Drawing;
using System.IO;
using Accord.Imaging;
using Accord.Imaging.Filters;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Bmp;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using Unity.Mathematics;
using UnityEngine;
using Color = UnityEngine.Color;
using Image = SixLabors.ImageSharp.Image;

namespace To_Pixel_Art
{
	public static class ImageColorAdjuster
	{
		public static Texture2D AdjustImageColors(
			Texture2D inputTexture, float brightness, float contrast, float hue, float saturation)
		{
			// 将Unity Texture2D转换为ImageSharp Image
			byte[]              inputData     = inputTexture.EncodeToPNG();
			using MemoryStream  inputStream   = new MemoryStream(inputData);
			using Image<Rgba32> originalImage = Image.Load<Rgba32>(inputStream);

			// 备份原始透明度值
			float[][] originalAlphas = new float[originalImage.Width][];
			for (int x = 0; x < originalImage.Width; ++x)
			{
				originalAlphas[x] = new float[originalImage.Height];
				for (int y = 0; y < originalImage.Height; ++y)
				{
					originalAlphas[x][y] = originalImage[x, y].A / 255f;
				}
			}

			// 调整亮度、对比度、色相和饱和度
			originalImage.Mutate(x =>
			{
				x.Brightness(brightness);
				x.Contrast(contrast);
				x.Saturate(saturation);
				x.Hue(hue * 180.0f - 90.0f);
			});

			// 恢复原始透明度值
			for (int x = 0; x < originalImage.Width; ++x)
			{
				for (int y = 0; y < originalImage.Height; ++y)
				{
					Rgba32 pixel = originalImage[x, y];
					pixel.A             = (byte)(originalAlphas[x][y] * 255);
					originalImage[x, y] = pixel;
				}
			}

			// 将调整后的ImageSharp Image转换为Unity Texture2D
			using MemoryStream outputStream = new MemoryStream();
			originalImage.Save(outputStream, new SixLabors.ImageSharp.Formats.Png.PngEncoder());
			Texture2D outputTexture = new Texture2D(inputTexture.width, inputTexture.height);
			outputTexture.LoadImage(outputStream.ToArray());

			return outputTexture;
		}

		private static ComputeShader computeShader;

		public static ComputeShader ComputeShader
		{
			get
			{
				if (computeShader == null)
				{
					computeShader = Resources.Load<ComputeShader>("BilateralFilter");
				}
				return computeShader;
			}
		}


		/// <summary>
		/// 双边滤波降噪
		/// </summary>
		/// <param name="spatialSigma">通常在[0.5,5.0]之间，默认为1</param>
		/// <param name="colorSigma">通常在[0.1,0.5]之间，默认为0.2</param>
		/// <param name="filterRadius">通常在[1,10]之间，默认为3(值较大时严重影响性能)</param>
		/// <returns></returns>
		public static Texture2D ApplyBilateralFilter(Texture2D source, float spatialSigma, float colorSigma, int filterRadius)
		{
			int width  = source.width;
			int height = source.height;

			RenderTexture result = new RenderTexture(width, height, 0, RenderTextureFormat.ARGB32);
			result.enableRandomWrite = true;
			result.Create();

			int kernelHandle = ComputeShader.FindKernel("BilateralFilterCS");
			ComputeShader.SetTexture(kernelHandle, "Result", result);
			ComputeShader.SetTexture(kernelHandle, "Source", source);
			ComputeShader.SetInt("_Width", width);
			ComputeShader.SetInt("_Height", height);
			ComputeShader.SetFloat("_SpatialSigma", spatialSigma);
			ComputeShader.SetFloat("_ColorSigma", colorSigma);
			ComputeShader.SetInt("_FilterRadius", filterRadius);

			int threadGroupX = Mathf.CeilToInt(width / 8.0f);
			int threadGroupY = Mathf.CeilToInt(height / 8.0f);

			ComputeShader.Dispatch(kernelHandle, threadGroupX, threadGroupY, 1);

			Texture2D output = new Texture2D(width, height, TextureFormat.ARGB32, false);
			RenderTexture.active = result;
			output.ReadPixels(new Rect(0, 0, width, height), 0, 0);
			output.Apply();

			RenderTexture.active = null;
			result.Release();

			return output;
		}


		public static Texture2D ApplyEdgeDetection(Texture2D sourceTexture, float threshold)
		{
			int width  = sourceTexture.width;
			int height = sourceTexture.height;

			Texture2D resultTexture = new Texture2D(width, height);

			for (int y = 0; y < height; y++)
			{
				for (int x = 0; x < width; x++)
				{
					Color sourceColor = sourceTexture.GetPixel(x, y);

					// 检查透明度（Alpha值）
					if (sourceColor.a < 0.05f) // 可根据需要调整该值
					{
						// 设置结果纹理对应像素为透明
						resultTexture.SetPixel(x, y, new Color(0, 0, 0, 0));
						continue;
					}

					Color colorX1 = x > 0 ? sourceTexture.GetPixel(x - 1, y) : sourceColor;
					Color colorX2 = x < width - 1 ? sourceTexture.GetPixel(x + 1, y) : sourceColor;
					Color colorY1 = y > 0 ? sourceTexture.GetPixel(x, y - 1) : sourceColor;
					Color colorY2 = y < height - 1 ? sourceTexture.GetPixel(x, y + 1) : sourceColor;

					float edgeX = Mathf.Abs(colorX1.r - colorX2.r) + Mathf.Abs(colorX1.g - colorX2.g) +
					              Mathf.Abs(colorX1.b - colorX2.b);
					float edgeY = Mathf.Abs(colorY1.r - colorY2.r) + Mathf.Abs(colorY1.g - colorY2.g) +
					              Mathf.Abs(colorY1.b - colorY2.b);

					if (edgeX + edgeY > threshold)
					{
						// 边缘处设置为黑色
						resultTexture.SetPixel(x, y, Color.black);
					}
					else
					{
						resultTexture.SetPixel(x, y, sourceColor);
					}
				}
			}

			resultTexture.Apply();
			return resultTexture;
		}
	}
}