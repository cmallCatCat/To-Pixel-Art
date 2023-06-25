using To_Pixel_Art.Palettes.ExistPalette;
using UnityEngine;
using UnityEngine.UI;

namespace To_Pixel_Art
{
	public class DisplayValueManager : MonoBehaviour
	{
		public Slider   numSlider;
		public Dropdown paletteTypeDropdown;
		public Dropdown paletteDropdown;
		public Slider   colorAmountSlider;
		public Slider   bandwidthSlider;
		[Space]
		public Slider brightnessSlider;
		public Slider saturationSlider;
		public Slider hueSlider;
		public Slider contrastSlider;
		public Slider polarizationSlider;
		[Space]
		public Toggle reduceNoiseToggle;
		public Slider kernelSizeSlider;
		public Slider spatialFactorSlider;
		public Slider colorFactorSlider;

		public int         Num           => (int)numSlider.value;
		public PaletteType PaletteType   => (PaletteType)paletteTypeDropdown.value;
		public Palette     Palette       => (Palette)paletteDropdown.value;
		public int         ColorAmount   => (int)colorAmountSlider.value;
		public float       Bandwidth     => bandwidthSlider.value;
		public float       Brightness    => brightnessSlider.value;
		public float       Saturation    => saturationSlider.value;
		public float       Hue           => hueSlider.value;
		public float       Contrast      => contrastSlider.value;
		public bool        ReduceNoise   => reduceNoiseToggle.isOn;
		public int         KernelSize    => (int)kernelSizeSlider.value;
		public float       SpatialFactor => spatialFactorSlider.value;
		public float       ColorFactor   => colorFactorSlider.value;
		public float       Polarization  => polarizationSlider.value;

	
	
	}
}
