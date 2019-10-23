using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	public static GameManager instance = null;
	private BoardManager boardScript;

	void Awake ()
	{
		if (instance == null) {
			instance = this;
		} else if (instance != this) {
			Destroy (gameObject);
		}

		boardScript = GetComponent<BoardManager> ();
		InitGame ();
	}

	void InitGame ()
	{
		boardScript.SetupScene ();
	}
}