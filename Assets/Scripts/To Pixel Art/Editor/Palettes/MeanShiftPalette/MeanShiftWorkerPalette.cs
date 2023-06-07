using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Accord.Math.Comparers;
using To_Pixel_Art.Editor.Palettes.ExistPalette;
using UnityEngine;
using UnityEngine.TestTools.Utils;
using Debug = UnityEngine.Debug;

namespace To_Pixel_Art.Editor.Palettes.MeanShiftPalette
{
	public class MeanShiftWorkerPalette : WorkerPalette
	{
		private float bandwidth;

		public MeanShiftWorkerPalette(
			int   num,
			float whiteThreshold,
			float blackThreshold,
			float alphaThreshold,
			float bandwidth)
			: base(num, whiteThreshold, blackThreshold, alphaThreshold)
		{
			this.bandwidth = bandwidth;
		}

		public override List<Color> GetPalette(Texture2D texture2D)
		{
			
			List<Color> sample    = PixelColorUtility.GetSample(texture2D, num);
			MeanShift   meanShift = new MeanShift(bandwidth);
			List<Color> colors    = meanShift.Cluster(sample);
			colors = colors.Distinct(new ColorEqualityComparer(0.01f)).ToList();
			return colors;
		}
	}
}