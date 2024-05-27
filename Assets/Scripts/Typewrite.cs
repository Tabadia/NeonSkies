using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Typewrite : MonoBehaviour
{
	TMP_Text txt;
	string writer;

	public float delayBeforeStart = 0f;
	public float delay = 0.1f;
	// Use this for initialization
	void Start()
	{
		txt = GetComponent<TMP_Text>()!;

		if (txt != null)
		{
			writer = txt.text;
			txt.text = "";

			StartCoroutine("TypeWriter");
		}
	}

	IEnumerator TypeWriter()
    {
        txt.text = "";

        yield return new WaitForSeconds(delayBeforeStart);

		foreach (char c in writer)
		{
			if (txt.text.Length > 0)
			{
				txt.text = txt.text.Substring(0, txt.text.Length);
			}
			txt.text += c;
			yield return new WaitForSeconds(delay);
		}

	}
}