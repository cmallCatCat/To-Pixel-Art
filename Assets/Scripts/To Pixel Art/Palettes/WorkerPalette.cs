using System.Collections.Generic;
using UnityEngine;

namespace To_Pixel_Art.Palettes
{
	public abstract class WorkerPalette
	{
		protected readonly int num;


		protected WorkerPalette(int num)
		{
			this.num = num;
		}

		public abstract List<Color> GetPalette(Texture2D texture2D);
	}
}