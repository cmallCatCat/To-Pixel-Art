using Accord.MachineLearning;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

namespace To_Pixel_Art.Palettes.KMeansPalette
{
	public static class KMeans
	{
		public static Color[] QuantizeColors(Color[] inputColors, int numberOfClusters)
		{
			// Convert input Colors to double[][] for Accord.NET KMeans
			double[][] inputData = new double[inputColors.Length][];
			for (int i = 0; i < inputColors.Length; i++)
			{
				inputData[i] = new double[] { inputColors[i].r, inputColors[i].g, inputColors[i].b};
			}

			// Perform K-means clustering
			Accord.MachineLearning.KMeans kMeans   = new Accord.MachineLearning.KMeans(numberOfClusters);
			KMeansClusterCollection       clusters = kMeans.Learn(inputData);

			// Assign each input color to its cluster centroid
			Color[] outputColors = new Color[inputColors.Length];
			for (int i = 0; i < inputColors.Length; i++)
			{
				int clusterIndex = clusters.Decide(inputData[i]);
				Vector3 centroid = new Vector3((float)clusters.Centroids[clusterIndex][0],
					(float)clusters.Centroids[clusterIndex][1],
					(float)clusters.Centroids[clusterIndex][2]);
				outputColors[i] = new Color(centroid.x, centroid.y, centroid.z);
			}
			return outputColors;
		}
		
	}
}