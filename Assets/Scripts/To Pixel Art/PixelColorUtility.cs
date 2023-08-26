using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.TestTools.Utils;

namespace To_Pixel_Art
{
	public static class PixelColorUtility
	{
		public static Color ToPixel(
			Color[] cs)
		{
			float rAv = 0;
			float gAv = 0;
			float bAv = 0;
			float aAv = 0;

			for (int i = 0; i < cs.Length; i++)
			{
				rAv += cs[i].r * cs[i].a;
				gAv += cs[i].g * cs[i].a;
				bAv += cs[i].b * cs[i].a;
				aAv += cs[i].a;
			}

			rAv /= cs.Length;
			gAv /= cs.Length;
			bAv /= cs.Length;
			aAv /= cs.Length;
			return new Color(rAv, gAv, bAv, aAv);
		}

		public static Color FindClosestColor(Color average, List<Color> colorPalette, float polarization)
		{
			if (average.a == 0)
			{
				return Color.clear;
			}
			float min      = 99;
			int   minIndex = -1;
			for (int i = 0; i < colorPalette.Count; i++)
			{
				float similarity = Similarity(average, colorPalette[i]);
				if (similarity < min)
				{
					min      = similarity;
					minIndex = i;
				}
			}
			return ColorPolarizationUtils.AdjustColor(colorPalette[minIndex], polarization);
		}

		public static float Similarity(Color average, Color color)
		{
			float r = average.r - color.r;
			float g = average.g - color.g;
			float b = average.b - color.b;
			return r * r + g * g + b * b;
		}

		public static List<Color> GetSample(Texture2D texture2D, int num)
		{
			int         i          = num * num / 4;
			List<Color> existColor = texture2D.GetPixels().ToList().Where((_, i1) => i1 % i == 0).ToList();
			existColor = RemoveInfrequentElements(existColor, new ColorEqualityComparer(0.01f));
			return existColor;
		}

		public static List<T> RemoveInfrequentElements<T>(List<T> list, IEqualityComparer<T> comparer)
		{
			int minAmount = list.Count / 5000;

			return list.GroupBy(item => item, comparer)
				.Where(group => group.Count() >= minAmount)
				.Select(group => group.Key)
				.ToList();
		}

		public static void ApplyPalette(
			Texture2D newTexture2D, Texture2D texture2D, List<Color> palette, int num, float polarization)
		{
			for (int x = 0; x < newTexture2D.width; x++)
			{
				for (int y = 0; y < newTexture2D.height; y++)
				{
					Color[] block = texture2D.GetPixels(x * num, y * num, num, num);

					if (block.Length > 0)
					{
						Color average = ToPixel(block);
						average = FindClosestColor(average, palette, polarization);
						newTexture2D.SetPixel(x, y, average);
					}
				}
			}
			newTexture2D.Apply();
		}
	}
}