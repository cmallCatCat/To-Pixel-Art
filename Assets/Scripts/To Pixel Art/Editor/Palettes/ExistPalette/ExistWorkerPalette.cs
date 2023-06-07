using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace To_Pixel_Art.Editor.Palettes.ExistPalette
{
	public class ExistWorkerPalette : WorkerPalette
	{
		private Color[] colorPalette;


		public ExistWorkerPalette(
			Palette        palette,
			int            num,
			float          whiteThreshold,
			float          blackThreshold,
			float          alphaThreshold)
			: base(num,  whiteThreshold, blackThreshold, alphaThreshold)
		{
			string path = palette switch
			{ Palette.Color256 => "256Color",
			  Palette.Color128 => "128Color",
			  Palette.Color64  => "64Color",
			  Palette.Color48  => "48Color",
			  Palette.Color32  => "32Color",
			  _                => throw new ArgumentOutOfRangeException() };

			TextAsset colorAsset   = Resources.Load<TextAsset>(path);
			string[]  colorStrings = colorAsset.text.Split('\n');
			colorPalette = new Color[colorStrings.Length];
			for (int i = 0; i < colorStrings.Length; i++)
			{
				string readLine = colorStrings[i].Trim();
				if (ColorUtility.TryParseHtmlString(readLine, out Color fromHtml))
				{
					colorPalette[i] = fromHtml;
				}
				else
				{
					Debug.LogWarning("can not parse this color" + readLine);
				}
			}
		}

		public ExistWorkerPalette(
			Color[]        colorPalette,
			int            num,
			float          whiteThreshold,
			float          blackThreshold,
			float          alphaThreshold)
			: base(num, whiteThreshold, blackThreshold, alphaThreshold)
		{
			this.colorPalette = colorPalette;
		}


		public override List<Color> GetPalette(Texture2D texture2D)
		{
			return colorPalette.ToList();
		}
	}
}