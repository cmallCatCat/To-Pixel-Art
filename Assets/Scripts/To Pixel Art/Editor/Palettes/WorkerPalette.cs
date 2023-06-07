using System.Collections.Generic;
using UnityEngine;

namespace To_Pixel_Art.Editor.Palettes
{
	public abstract class WorkerPalette
	{
		protected readonly int            num;
		protected readonly float          whiteThreshold;
		protected readonly float          blackThreshold;
		protected readonly float          alphaThreshold;


		protected WorkerPalette(
			int            num,
			float          whiteThreshold,
			float          blackThreshold,
			float          alphaThreshold)
		{
			this.num            = num;
			this.whiteThreshold = whiteThreshold;
			this.blackThreshold = blackThreshold;
			this.alphaThreshold = alphaThreshold;
		}
		
		public abstract List<Color> GetPalette(Texture2D texture2D);

	}
}