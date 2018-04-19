using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuntimeExample : MonoBehaviour {

	public TerrainGenerator terrainGenerator;
	public GameObject cube;

	void Start () {
		StartCoroutine (generate());
	}
	
	IEnumerator generate()
	{
		while (true) {
			terrainGenerator.terrainResolution = LargeTerrainResolution.RES_256;
			terrainGenerator.clear ();		// Terrain has to be cleared before build
			terrainGenerator.build ();
			yield return new WaitForSeconds (2);
		}
	}
}
