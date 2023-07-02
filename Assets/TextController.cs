using UnityEngine;
using UnityEngine.UI;

public class TextController : MonoBehaviour
{
	[SerializeField]
	private Text text;
	
	public void Set(string name)
	{
		text.text = name;
	}

}