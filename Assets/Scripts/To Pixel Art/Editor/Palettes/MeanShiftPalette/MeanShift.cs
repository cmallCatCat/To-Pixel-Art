using System.Collections.Generic;
using UnityEngine;

namespace To_Pixel_Art.Editor.Palettes.MeanShiftPalette
{
	public class MeanShift
	{
		public float Bandwidth { get; set; }

		// 构造函数
		public MeanShift(float bandwidth)
		{
			Bandwidth = bandwidth;
		}

		// 计算两个颜色之间的距离
		private float ColorDistance2(Color c1, Color c2)
		{
			return PixelColorUtility.Similarity(c1, c2);
		}

		// 根据核函数计算权重
		private float GaussianKernel(float distance, float bandwidth)
		{
			float exponent = -(distance * distance) / (2 * bandwidth * bandwidth);
			return Mathf.Exp(exponent);
		}

		// 更新中心点
		private Color UpdateCentroid(Color centroid, List<Color> colors)
		{
			Vector3 sum         = Vector3.zero;
			float   totalWeight = 0;

			foreach (Color color in colors)
			{
				float distance = ColorDistance2(centroid, color);
				if (distance <= Bandwidth * Bandwidth)
				{
					float weight = GaussianKernel(distance, Bandwidth);
					sum         += new Vector3(color.r, color.g, color.b) * weight;
					totalWeight += weight;
				}
			}

			if (totalWeight > 0)
			{
				return new Color(sum.x / totalWeight, sum.y / totalWeight, sum.z / totalWeight);
			}
			else
			{
				return centroid;
			}
		}

		// Mean Shift 颜色聚类算法
		public List<Color> Cluster(List<Color> colors, float convergenceThreshold = 5e-3f)
		{
			List<Color> centroids                   = new List<Color>(colors);
			float       convergenceThresholdSquared = convergenceThreshold * convergenceThreshold;

			for (int i = 0; i < centroids.Count; i++)
			{
				Color previousCentroid;
				do
				{
					previousCentroid = centroids[i];
					centroids[i]     = UpdateCentroid(previousCentroid, colors);
				} while (PixelColorUtility.Similarity(previousCentroid, centroids[i]) > convergenceThresholdSquared);
			}
			return centroids;
		}
	}
}