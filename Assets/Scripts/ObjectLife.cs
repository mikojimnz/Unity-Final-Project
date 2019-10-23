using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectLife : MonoBehaviour
{

	public float deathDelay = 0.5f;

	void Start ()
	{
		StartCoroutine (DeleteSelf ());
	}

	private IEnumerator DeleteSelf ()
	{
		yield return new WaitForSeconds (deathDelay);
		GameObject.Destroy (gameObject);
	}
}