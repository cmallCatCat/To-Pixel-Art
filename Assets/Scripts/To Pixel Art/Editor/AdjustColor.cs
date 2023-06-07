namespace To_Pixel_Art.Editor
{
	public class AdjustColor
	{
		// public static Texture2D AdjustImageColors(
		// 	string inputPath,
		// 	string outputPath,
		// 	float  brightness,
		// 	float  contrast,
		// 	float  saturation)
		// {
		// 	// 加载输入图像
		// 	using var inputStream    = File.OpenRead(inputPath);
		// 	using var originalBitmap = SKBitmap.Decode(inputStream);
		//
		// 	// 创建输出图像
		// 	using var outputSurface = SKSurface.Create(new SKImageInfo(originalBitmap.Width, originalBitmap.Height));
		// 	using var outputCanvas  = outputSurface.Canvas;
		//
		// 	// 设置颜色矩阵以调整亮度、对比度和饱和度
		// 	float[] colorMatrix =
		// 	{ contrast * saturation, 0, 0, 0, brightness, 0, contrast * saturation,
		// 	  0, 0, brightness, 0, 0, contrast * saturation, 0,
		// 	  brightness, 0, 0, 0, 1, 0, 0, 0, 0, 0, 1 };
		//
		// 	// 创建颜色滤镜
		// 	using var colorFilter = SKColorFilter.CreateColorMatrix(colorMatrix);
		//
		// 	// 使用颜色滤镜绘制调整后的图像
		// 	using var paint = new SKPaint
		// 	{ ColorFilter = colorFilter };
		// 	outputCanvas.DrawBitmap(originalBitmap, 0, 0, paint);
		//
		// 	// 将调整后的图像保存到输出文件
		// 	using var outputStream = File.OpenWrite(outputPath);
		// 	using var outputImage  = SKImage.FromSurface(outputSurface);
		// 	outputImage.Encode(SKEncodedImageFormat.Png, 100).SaveTo(outputStream);
		// }
		



	}
}