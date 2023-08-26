using UnityEngine;
using UnityEngine.UI;

public class ImageController : MonoBehaviour
{
	[SerializeField]
	private Image image;

	public void Set(Texture2D preview, float width, float height)
	{
		image.sprite = Sprite.Create(preview, new Rect(0, 0, preview.width, preview.height), new Vector2(0.5f, 0.5f));
		((RectTransform)transform).sizeDelta = new Vector2(width, height);
	}
}