using System.Collections.Generic;

namespace To_Pixel_Art
{
	public class DataManager
	{
		public static Dictionary<string, float> ParseData(string data)
		{
			Dictionary<string, float> result = new Dictionary<string, float>();
			string[]                  lines  = data.Split('\n');
			foreach (string line in lines)
			{
				string[] keyValue = line.Split(':');
				if(keyValue.Length == 2)
				{
					string key   = keyValue[0].Trim();
					float  value = float.Parse(keyValue[1].Trim());
					result[key] = value;
				}
			}
			return result;
		}
	}
}