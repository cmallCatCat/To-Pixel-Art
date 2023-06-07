using System.Collections.Generic;
using System.Linq;
using To_Pixel_Art.Editor.Palettes.ExistPalette;
using UnityEngine;

namespace To_Pixel_Art.Editor.Palettes.KMeansPalette
{
	public class KMeansWorkerPalette : WorkerPalette
	{
		private int colorAmount;

		public KMeansWorkerPalette(
			int   num,
			float whiteThreshold,
			float blackThreshold,
			float alphaThreshold,
			int   colorAmount)
			: base(num, whiteThreshold, blackThreshold, alphaThreshold)
		{
			this.colorAmount = colorAmount;
		}

		public override List<Color> GetPalette(Texture2D texture2D)
		{
			List<Color> sample = PixelColorUtility.GetSample(texture2D, num);
			Color[]     colors = KMeans.QuantizeColors(sample.ToArray(), colorAmount);
			return colors.ToList();
		}
		
	}
}