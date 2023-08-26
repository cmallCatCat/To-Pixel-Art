using To_Pixel_Art;
using UnityEngine;
using UnityEngine.UI;

public class ButtonController : MonoBehaviour
{
	[SerializeField]
	private Text text;
	[SerializeField]
	private Button button;

	public void Set(string name)
	{
		text.text = name;
		
		button.onClick.AddListener(() => ValueManager.Instance.ButtonDown(name));
	}
}