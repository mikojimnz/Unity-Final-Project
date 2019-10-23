using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using System;

public class BoardManager : MonoBehaviour
{

	[Serializable]
	public class Count
	{
		public int minimum;
		public int maximum;

		public Count (int min, int max)
		{
			minimum = min;
			maximum = max;
		}
	}

	public float objectSpawnRate = 5f;
	public Count width = new Count (-5, 5);
	public Count height = new Count (-5, 5);
	public float length = 50;
	public Count objCont = new Count (5, 10);
	public Count pointCont = new Count (5, 15);
	public GameObject[] staticObj;
	public GameObject[] pointObj;

	public void SetupScene ()
	{
		StartCoroutine (BeginGeneration ());
	}

	private void LayoutObjectAtRandom (GameObject[] tileArray, int minimum, int maximum)
	{
		int objectCount = Random.Range (minimum, maximum + 1);

		for (int i = 0; i < objectCount; i++) {
			Vector3 randomPosition = new Vector3 (Random.Range (width.minimum, width.maximum), Random.Range (height.minimum, height.maximum), GameObject.FindGameObjectWithTag ("Player").transform.position.z + length);
			GameObject tileChoice = tileArray [Random.Range (0, tileArray.Length)];
			Instantiate (tileChoice, randomPosition, Quaternion.identity);
		}
	}

	private IEnumerator BeginGeneration ()
	{
		while (true) {
			AddObjects ();
			yield return new WaitForSeconds (objectSpawnRate);
		}
	}

	private void AddObjects ()
	{
		LayoutObjectAtRandom (staticObj, objCont.minimum, objCont.maximum);
		LayoutObjectAtRandom (pointObj, pointCont.minimum, pointCont.maximum);
	}
}