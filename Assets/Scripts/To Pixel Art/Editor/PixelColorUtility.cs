using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Graphs;
using UnityEngine;
using UnityEngine.TestTools.Utils;

namespace To_Pixel_Art.Editor
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

		public static Color FindClosestColor(Color average, List<Color> colorPalette)
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
			return ColorUtils.AdjustColor(colorPalette[minIndex]);
		}

		public static float Similarity(Color color1, Color color2)
		{
			float difR = color1.r - color2.r;
			float difG = color1.g - color2.g;
			float difB = color1.b - color2.b;
			return difR * difR + difG * difG + difB * difB;
		}

		public static List<Color> GetSample(Texture2D texture2D, int num)
		{
			List<Color> existColor = texture2D.GetPixels().ToList();
			int         i          = num * num / 4;
			existColor = existColor.Where((_, i1) => i1 % i == 0).ToList();
			existColor = RemoveInfrequentElements(existColor, new ColorEqualityComparer(0.01f));
			return existColor;
		}

		public static List<T> RemoveInfrequentElements<T>(List<T> list, IEqualityComparer<T> comparer)
		{
			Dictionary<T, int> occurrences = new Dictionary<T, int>(comparer);
			foreach (T item in list)
			{
				occurrences.TryAdd(item, 0);
				occurrences[item]++;
			}

			int     threshold = 2;
			List<T> result    = new List<T>();
			foreach (KeyValuePair<T, int> pair in occurrences)
			{
				if (pair.Value >= threshold)
				{
					result.Add(pair.Key);
				}
			}
			return result;
		}

		public static void ApplyPalette(Texture2D newTexture2D, Texture2D texture2D, List<Color> palette, int num)
		{
			Color[][] results = new Color[newTexture2D.width][];
			for (int x = 0; x < newTexture2D.width; x++)
			{
				results[x] = new Color[newTexture2D.height];
				for (int y = 0; y < newTexture2D.height; y++)
				{
					Color[] block = texture2D.GetPixels(x * num, y * num, num, num);

					if (block.Length > 0)
					{
						Color average = ToPixel(block);
						average = FindClosestColor(average, palette);
						newTexture2D.SetPixel(x, y, average);
						results[x][y] = average;
					}
				}
			}
			newTexture2D.Apply();
		}
	}
}