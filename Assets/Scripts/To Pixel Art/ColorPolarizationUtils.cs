using UnityEngine;

namespace To_Pixel_Art
{
	public static class ColorPolarizationUtils
	{
		public static float CloseToGray(Color color)
		{
			float differenceRG = Mathf.Abs(color.r - color.g);
			float differenceRB = Mathf.Abs(color.r - color.b);
			float differenceGB = Mathf.Abs(color.g - color.b);

			return Mathf.Max(differenceRG, differenceRB, differenceGB);
		}

		public static Color AdjustColor(Color color, float effect)
		{
			float a = color.a;
			// 计算颜色的亮度      
			float brightness = (color.r + color.g + color.b) / 3;

			float closeToGray         = CloseToGray(color);
			float closeToWhiteOrBlack = Mathf.Min(Mathf.Abs(brightness - 0f), Mathf.Abs(brightness - 1f)) * 2.5f;
			float sum                 = closeToGray + closeToWhiteOrBlack;
			float t                   = Mathf.Clamp01(sum * effect);
			float targetBrightness    = Mathf.Lerp(brightness, brightness > 0.5f ? 1 : 0, t);
			float scaleFactor         = targetBrightness / brightness;
			color *= scaleFactor;

			color.a = a;
			return color;
		}
	}
}