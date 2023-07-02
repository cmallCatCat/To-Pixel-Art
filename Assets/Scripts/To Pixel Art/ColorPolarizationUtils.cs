using UnityEngine;

namespace To_Pixel_Art
{
	public static class ColorPolarizationUtils
	{
		// 调整颜色亮度并使灰度颜色受到更强烈的影响
		public static Color AdjustColor(Color color, float intensity)
		{
			// 将 RGB 颜色转换为 HSL 颜色
			Color.RGBToHSV(color, out float hue, out float saturation, out float value);

			// 根据饱和度计算权重
			float weight = 1 - saturation;
			weight *= intensity;

			// 根据权重调整明度/亮度
			if (value > 0.5f)
				value += weight * (1 - value);
			else
				value -= weight * value;

			// 将 HSL 颜色转换回 RGB 颜色
			return Color.HSVToRGB(hue, saturation, value);
		}
	}
}