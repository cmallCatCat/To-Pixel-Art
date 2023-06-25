using To_Pixel_Art.Palettes.ExistPalette;

namespace To_Pixel_Art
{
	public class Settings
	{
		public int         num           = 6;
		public PaletteType paletteType   = PaletteType.KMeans;
		public Palette     palette       = Palette.Color64;
		public int         colorAmount   = 16;
		public float       bandwidth     = 0.05f;
		public float       brightness    = 1f;
		public float       saturation    = 1f;
		public float       hue           = 0.5f;
		public float       contrast      = 1f;
		public float       polarization  = 1f;
		public bool        reduceNoise   = true;
		public int         kernelSize    = 1;
		public float       spatialFactor = 10f;
		public float       colorFactor   = 60f;

		public void Deconstruct(out int num, out PaletteType paletteType, out Palette palette, out int colorAmount,
			out float bandwidth, out float brightness, out float saturation, out float hue, out float contrast,
			out bool reduceNoise,
			out int kernelSize, out float spatialFactor, out float colorFactor, out float polarization)
		{
			num           = this.num;
			paletteType   = this.paletteType;
			palette       = this.palette;
			colorAmount   = this.colorAmount;
			bandwidth     = this.bandwidth;
			brightness    = this.brightness;
			saturation    = this.saturation;
			hue           = this.hue;
			contrast      = this.contrast;
			reduceNoise   = this.reduceNoise;
			kernelSize    = this.kernelSize;
			spatialFactor = this.spatialFactor;
			colorFactor   = this.colorFactor;
			polarization  = this.polarization;
		}
	}
}