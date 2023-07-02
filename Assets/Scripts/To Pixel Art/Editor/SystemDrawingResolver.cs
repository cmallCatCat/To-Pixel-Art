using UnityEditor;

namespace To_Pixel_Art.Editor
{
	public class SystemDrawingResolver : AssetPostprocessor
	{
		private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
		{
			foreach (string asset in importedAssets)
			{
				if (asset.EndsWith("System.Drawing.Common.dll"))
				{
					var pluginImporter = (PluginImporter)AssetImporter.GetAtPath(asset);
					if (pluginImporter)
					{
						pluginImporter.SetCompatibleWithAnyPlatform(false);
						pluginImporter.SetCompatibleWithEditor(true);
						pluginImporter.SetCompatibleWithPlatform(BuildTarget.StandaloneWindows, true);
						pluginImporter.SetCompatibleWithPlatform(BuildTarget.StandaloneWindows64, true);

						// 如果需要支持其他平台，请在这里添加

						pluginImporter.SaveAndReimport();
					}
				}
			}
		}
	}
}