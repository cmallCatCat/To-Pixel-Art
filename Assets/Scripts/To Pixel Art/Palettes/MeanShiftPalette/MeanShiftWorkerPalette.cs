using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.TestTools.Utils;

namespace To_Pixel_Art.Palettes.MeanShiftPalette
{
	public class MeanShiftWorkerPalette : WorkerPalette
	{
		private float bandwidth;

		public MeanShiftWorkerPalette(int num, float bandwidth) : base(num)
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