using UnityEngine;

namespace To_Pixel_Art.Editor
{
	public static class ColorUtils
	{
		private static float threshold                  = 0.20f;
		private static float adjustmentFactorGray       = 0.5f;
		private static float adjustmentFactorBrightness = 0.5f;

		public static bool IsColorCloseToGray(Color color)
		{
			float differenceRG = Mathf.Abs(color.r - color.g);
			float differenceRB = Mathf.Abs(color.r - color.b);
			float differenceGB = Mathf.Abs(color.g - color.b);

			if (differenceRG < threshold && differenceRB < threshold && differenceGB < threshold)
			{
				return true;
			}

			return false;
		}

		public static Color AdjustColor(Color color)
		{
			// 计算颜色的亮度
			float brightness = (color.r + color.g + color.b) / 3;
			if (IsColorCloseToGray(color))
			{
				// 根据亮度将颜色调整为接近纯白或纯黑，但缓和过渡
				float targetBrightness;
				if (brightness > 0.5f)
				{
					targetBrightness = Mathf.Lerp(brightness, 1, adjustmentFactorGray);
				}
				else
				{
					targetBrightness = Mathf.Lerp(brightness, 0, adjustmentFactorGray);
				}

				// 使用目标亮度计算新颜色
				float scaleFactor = targetBrightness / brightness;
				color *= scaleFactor;
			}
			else
			{
				// 如果颜色较亮或较暗，则增强其亮度或暗度，并缓和过渡
				float targetBrightness;
				if (brightness > 0.8f)
				{
					targetBrightness = Mathf.Lerp(brightness, 1, adjustmentFactorBrightness);
				}
				else if (brightness < 0.2f)
				{
					targetBrightness = Mathf.Lerp(brightness, 0, adjustmentFactorBrightness);
				}
				else
				{
					return color;
				}

				// 使用目标亮度计算新颜色
				float scaleFactor = targetBrightness / brightness;
				color *= scaleFactor;
			}

			return color;
		}
	}
}