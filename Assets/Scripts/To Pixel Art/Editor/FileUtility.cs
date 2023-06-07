using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace To_Pixel_Art.Editor
{
	public static class FileUtility
	{
		public static void CopyTextureImporter(string oldPath, string newPath, int num)
		{
			TextureImporter TI    = AssetImporter.GetAtPath(oldPath) as TextureImporter;
			TextureImporter newTI = AssetImporter.GetAtPath(newPath) as TextureImporter;
			if (TI == null)
			{
				Debug.LogError("TextureImporter is null");
				Debug.Log(oldPath);
				return;
			}
			if (newTI == null)
			{
				Debug.LogError("TextureImporter is null");
				Debug.Log(newPath);
				return;
			}

			TextureImporterSettings tis = new TextureImporterSettings();
			TI.ReadTextureSettings(tis);
			newTI.SetTextureSettings(tis);

			newTI.filterMode          = FilterMode.Point;
			newTI.spritePixelsPerUnit = TI.spritePixelsPerUnit / num;
			newTI.spritesheet = TI.spritesheet.Select(spriteMetaData => new SpriteMetaData
			{ name      = spriteMetaData.name,
			  alignment = spriteMetaData.alignment,
			  border    = spriteMetaData.border / num,
			  pivot     = spriteMetaData.pivot,
			  rect = new Rect(spriteMetaData.rect.x / num, spriteMetaData.rect.y / num,
				  spriteMetaData.rect.width / num,
				  spriteMetaData.rect.height / num) }).ToArray();

			AssetDatabase.ImportAsset(newPath);
		}

		public static string ConvertRelativeToAbsolutePath(string relativePath)
		{
			// 获取Unity项目“Assets”文件夹的绝对路径
			string assetsFolderPath = Application.dataPath;

			// 查找相对路径中的"Assets"部分
			int assetsIndex = relativePath.IndexOf("Assets", StringComparison.Ordinal);

			// 若存在"Assets"，则从相对路径中删除它，并确保剩余路径以正确的路径分隔符开始
			if (assetsIndex >= 0)
			{
				relativePath = relativePath.Remove(assetsIndex, "Assets".Length + 1);
			}

			// 将“Assets”文件夹的绝对路径与剩余路径结合起来
			string absolutePath = Path.Combine(assetsFolderPath, relativePath);

			return absolutePath;
		}


		private static void SaveTexture2DToPath(Texture2D texture, string absolutePath)
		{
			byte[]     dataBytes  = texture.EncodeToPNG();
			FileStream fileStream = File.Open(absolutePath, FileMode.OpenOrCreate);
			fileStream.Write(dataBytes, 0, dataBytes.Length);
			fileStream.Close();
		}


		public static void CreateNewTexture(Texture2D newTexture2D, string newPath)
		{
			string absolutePath = ConvertRelativeToAbsolutePath(newPath);
			SaveTexture2DToPath(newTexture2D, absolutePath);
			AssetDatabase.SaveAssets();
			AssetDatabase.Refresh();
		}

		public static void SetReadable(string oldRelativePath, bool readable)
		{
			TextureImporter TI = AssetImporter.GetAtPath(oldRelativePath) as TextureImporter;
			if (TI == null) return;
			TI.isReadable = readable;
			AssetDatabase.ImportAsset(oldRelativePath);
		}

		public static string ModifyFileName(string ori_path, string additive)
		{
			// 在原文件名后添加"_pro"并保留扩展名
			string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(ori_path);
			string extension                = Path.GetExtension(ori_path);
			string modifiedFileName         = fileNameWithoutExtension + additive + extension;

			// 检查输入参数是否包含路径信息，如果有，则还原路径
			if (ori_path.Contains("/"))
			{
				string path = Path.GetDirectoryName(ori_path);
				return path + "/" + modifiedFileName;
			}
			else
			{
				return modifiedFileName;
			}
		}

		public static string GetNewPath(string oldPath, string additive)
		{
			return Path.GetFileNameWithoutExtension(oldPath) + additive + Path.GetExtension(oldPath);
		}
	}
}