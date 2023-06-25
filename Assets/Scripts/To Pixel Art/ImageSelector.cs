using StandaloneFileBrowser;
using UnityEngine;

namespace To_Pixel_Art
{
    public class ImageSelector : MonoBehaviour
    {
    

        private string initialDirectory = "C:\\Users\\Administrator\\Pictures";

        private void Start()
        {
            OpenImageFile();
        }

        public void OpenImageFile()
        {
            ExtensionFilter[] extensions = { new ExtensionFilter("Image Files", "jpg", "jpeg", "png") };
            string[]          paths = StandaloneFileBrowser.StandaloneFileBrowser.OpenFilePanel("Select an image file", initialDirectory, extensions, true);

            if (paths.Length > 0)
            {
                Debug.Log(paths[0]);
            }
        }
    }
}