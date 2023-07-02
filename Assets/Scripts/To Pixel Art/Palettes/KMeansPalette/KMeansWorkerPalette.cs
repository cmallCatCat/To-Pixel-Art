using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;
using UnityEngine.TestTools.Utils;
using Debug = UnityEngine.Debug;

namespace To_Pixel_Art.Palettes.KMeansPalette
{
	public class KMeansWorkerPalette : WorkerPalette
	{
		private int colorAmount;

		public KMeansWorkerPalette(int num, int colorAmount) : base(num)
		{
			this.colorAmount = colorAmount;
		}

		public override List<Color> GetPalette(Texture2D texture2D)
		{
			List<Color> sample = PixelColorUtility.GetSample(texture2D, num);
			Color[]     colors = KMeans.QuantizeColors(sample.ToArray(), colorAmount);
			List<Color> list   = colors.Distinct(new ColorEqualityComparer(0.01f)).ToList();
			return list;
		}
	}
}