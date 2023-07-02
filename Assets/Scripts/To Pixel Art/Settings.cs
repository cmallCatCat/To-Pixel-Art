using System;
using To_Pixel_Art;
using To_Pixel_Art.Palettes.ExistPalette;
using UnityEngine;

[Serializable]
[CreateAssetMenu(menuName = "To Pixel Art/Settings")]
public class Settings : ScriptableObject
{
	public int         num;
	public PaletteType paletteType;
	public Palette     palette;
	public int         colorAmount;
	public float       bandwidth;
	public float       edgeThreshold;

	public float brightness;
	public float saturation;
	public float hue;
	public float contrast;
	public float polarization;

	public bool  reduceNoise;
	public float   spatialSigma;
	public float colorSigma;
	public int filterRadius;

	public void Deconstruct(
		out int   num,          out PaletteType paletteType,   out Palette palette,      out int   colorAmount,
		out float bandwidth,    out float       edgeThreshold, out float   brightness,   out float saturation,
		out float hue,          out float       contrast,      out float   polarization, out bool  reduceNoise,
		out float   spatialSigma, out float       colorSigma,    out int   filterRadius)
	{
		num           = this.num;
		paletteType   = this.paletteType;
		palette       = this.palette;
		colorAmount   = this.colorAmount;
		bandwidth     = this.bandwidth;
		edgeThreshold = this.edgeThreshold;
		brightness    = this.brightness;
		saturation    = this.saturation;
		hue           = this.hue;
		contrast      = this.contrast;
		polarization  = this.polarization;
		reduceNoise   = this.reduceNoise;
		spatialSigma  = this.spatialSigma;
		colorSigma    = this.colorSigma;
		filterRadius  = this.filterRadius;
	}
}