using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
[CustomEditor(typeof(TerrainGenerator))]
public class TerrainGeneratorInspector : Editor
{
	public override void OnInspectorGUI()
	{
		DrawDefaultInspector ();

		TerrainGenerator terrGen = (TerrainGenerator)target;

		GUILayout.Space (30);
		GUILayout.Label ("Terrains with high resolution can need more minutes to build!");
		if (GUILayout.Button ("Build Terrain")) {
			terrGen.build ();
		}
		GUILayout.Space (30);
	}
}
#endif

public enum LargeTerrainResolution { 
	RES_32, RES_64, RES_128, RES_256, RES_512, RES_1024, RES_2048, RES_4096, RES_8192
}

[System.Serializable]
public class TerrainTexture
{
	public Texture2D texture;
	public Texture2D normalMap;
}

[AddComponentMenu("Terrain/Terrain Generator")]
public class TerrainGenerator : MonoBehaviour {

	/// <summary>
	/// Warning: Large resolutions can freeze your application for minutes while building
	/// </summary>
	public LargeTerrainResolution terrainResolution = LargeTerrainResolution.RES_1024;
	[Tooltip("You can define width and length after terrain has generated.")]
	public float terrainSize = 5000f;
	public float terrainHeight = 2000f;
	[Range(1f,100f)]
	/// <summary>
	/// The terrain roughness. Must be a value between 1 and 100!
	/// </summary>
	public float terrainRoughness = 60f;
	[Space]
	public TerrainTexture[] terrainTextures;

	/// <summary>
	/// Clear this terrain.
	/// Warning: Deletes als child objects of this transform
	/// </summary>
	public void clear()
	{
		for (int i = transform.childCount-1; i >= 0; i--) {				
			DestroyImmediate(transform.GetChild (i).gameObject);
		}
	}

	/// <summary>
	/// Build a new terrain. Rund clear() before building.
	/// </summary>
	public bool build () {		
		if (transform.childCount > 0) {
			bool goOn = false;
			#if UNITY_EDITOR
			goOn = EditorUtility.DisplayDialog ("Terrain allready exists", "Are you sure to rebuild the terrain? All child objects of Gameobject \""+transform.gameObject.name+"\" will be lost!", "YES", "NO");
			#endif

			if (goOn == false) {
				Debug.LogWarning ("Terrain was not built. If you tried to build during runtime, use the clear() command before building!");
				return false;
			}

			clear ();
		}

		int size = 1024;

		if (terrainSize <= 0) {
			Debug.LogError ("Terrain Size to small!");
			return false;
		}

		if (terrainHeight <= 0) {
			Debug.LogError ("Terrain Height to small!");
			return false;
		}

		if (terrainRoughness < 1 || terrainRoughness > 100) {
			Debug.LogError ("Terrain Roughness not in range!");
			return false;
		}

		size = getSize (terrainResolution);

		Terrain terrain1;
		Terrain terrain2;
		Terrain terrain3;
		Terrain terrain4;



		GameObject tile1 = new GameObject ("Tile 1");
		tile1.transform.position = new Vector3 (0, 0, 0) + transform.position;
		tile1.transform.parent = transform;
		terrain1 = tile1.AddComponent<Terrain> ();
		TerrainCollider col1 = tile1.AddComponent<TerrainCollider> ();

		GameObject tile2 = new GameObject ("Tile 2");
		tile2.transform.position = new Vector3 (0, 0, terrainSize/2) + transform.position;
		tile2.transform.parent = transform;
		terrain2 = tile2.AddComponent<Terrain> ();
		TerrainCollider col2 = tile2.AddComponent<TerrainCollider> ();

		GameObject tile3 = new GameObject ("Tile 3");
		tile3.transform.position = new Vector3 (terrainSize / 2, 0, 0) + transform.position;
		tile3.transform.parent = transform;
		terrain3 = tile3.AddComponent<Terrain> ();
		TerrainCollider col3 = tile3.AddComponent<TerrainCollider> ();

		GameObject tile4 = new GameObject ("Tile 4");
		tile4.transform.position = new Vector3 (terrainSize / 2, 0, terrainSize / 2) + transform.position;
		tile4.transform.parent = transform;
		terrain4 = tile4.AddComponent<Terrain> ();
		TerrainCollider col4 = tile4.AddComponent<TerrainCollider> ();

		terrain1.terrainData = new TerrainData ();
		terrain2.terrainData = new TerrainData ();
		terrain3.terrainData = new TerrainData ();
		terrain4.terrainData = new TerrainData ();

		TerrainData data1 = terrain1.terrainData;
		TerrainData data2 = terrain2.terrainData;
		TerrainData data3 = terrain3.terrainData;
		TerrainData data4 = terrain4.terrainData;


		data1.heightmapResolution = size / 2 + 1;
		data2.heightmapResolution = size / 2 + 1;
		data3.heightmapResolution = size / 2 + 1;
		data4.heightmapResolution = size / 2 + 1;

		data1.size = new Vector3 (terrainSize / 2, terrainHeight, terrainSize / 2);
		data2.size = new Vector3 (terrainSize / 2, terrainHeight, terrainSize / 2);
		data3.size = new Vector3 (terrainSize / 2, terrainHeight, terrainSize / 2);
		data4.size = new Vector3 (terrainSize / 2, terrainHeight, terrainSize / 2);

		float[,] newdata = dsAlg (size+1, terrainRoughness / 100 * 0.7f);

		float[,] newdata1 = new float[size/2+1,size/2+1];
		float[,] newdata2 = new float[size/2+1,size/2+1];
		float[,] newdata3 = new float[size/2+1,size/2+1];
		float[,] newdata4 = new float[size/2+1,size/2+1];

		for (int x = 0; x < size/2+1; x++) {
			for (int y = 0; y < size/2+1; y++) {
				newdata1 [x, y] = newdata [x, y];
				newdata2 [x, y] = newdata [x + size/2, y];
				newdata3 [x, y] = newdata [x, y + size/2];
				newdata4 [x, y] = newdata [x + size/2, y + size/2];
			}
		}

		data1.SetHeights(0, 0, newdata1);
		data2.SetHeights(0, 0, newdata2);
		data3.SetHeights(0, 0, newdata3);
		data4.SetHeights(0, 0, newdata4);

		col1.terrainData = data1;
		col2.terrainData = data2;
		col3.terrainData = data3;
		col4.terrainData = data4;


		if (terrainTextures != null && terrainTextures.Length > 0) {
			SplatPrototype[] sPt = new SplatPrototype[terrainTextures.Length];
			for (int i = 0; i < terrainTextures.Length; i++) {
				sPt [i] = new SplatPrototype ();
				sPt [i].texture = terrainTextures [i].texture;
				sPt [i].normalMap = terrainTextures [i].normalMap;
			}

			data1.splatPrototypes = sPt;
			data2.splatPrototypes = sPt;
			data3.splatPrototypes = sPt;
			data4.splatPrototypes = sPt;
		}

		terrain1.SetNeighbors (null, terrain2, terrain3, null);
		terrain2.SetNeighbors (null, null, terrain4, terrain1);
		terrain3.SetNeighbors (terrain1, terrain4, null, null);
		terrain4.SetNeighbors (terrain2, null, null, terrain3);

		terrain1.Flush ();
		terrain2.Flush ();
		terrain3.Flush ();
		terrain4.Flush ();

		LargeTerrain largeTerrain = GetComponent<LargeTerrain> ();
		if (largeTerrain == null)
			largeTerrain = this.gameObject.AddComponent<LargeTerrain> ();

		largeTerrain.tile1 = terrain1;
		largeTerrain.tile2 = terrain2;
		largeTerrain.tile3 = terrain3;
		largeTerrain.tile4 = terrain4;

		largeTerrain.terrainWidth = terrainSize;
		largeTerrain.terrainLength = terrainSize;
		largeTerrain.terrainHeight = terrainHeight;
		largeTerrain.dontUpdateSize ();

		largeTerrain.map = newdata;

		return true;
	}
		

	private int getSize(LargeTerrainResolution res)
	{
		switch (res) {
		case LargeTerrainResolution.RES_32:
			return 32;
		case LargeTerrainResolution.RES_64:
			return 64;
		case LargeTerrainResolution.RES_128:
			return 128;
		case LargeTerrainResolution.RES_256:
			return 256;
		case LargeTerrainResolution.RES_512:
			return 512;
		case LargeTerrainResolution.RES_1024:
			return 1024;
		case LargeTerrainResolution.RES_2048:
			return 2048;
		case LargeTerrainResolution.RES_4096:
			return 4096;
		case LargeTerrainResolution.RES_8192:
			return 8192;
		default:
			return 1024;
		}
	}

	private float[,] dsAlg(int size, float rough)
	{
		float[,] map = new float[size, size];
		map [0, 0] = rough * Random.value;
		map [0, size-1] = rough * Random.value;
		map [size-1,0] = rough * Random.value;
		map [size-1, size-1] = rough * Random.value;



		for (int step = 1; step < size; step*=2) {
			int stepsize = (size - 1) / step;

			for (int x = 0; x < size - 1; x += stepsize) {
				for (int y = 0; y < size - 1; y += stepsize) {
					float middle = map [x, y] + map [x + stepsize, y] + map [x + stepsize, y + stepsize] + map [x, y + stepsize];
					middle /= 4f;
					middle += rough * rand();
					middle = middle < 0 ? 0 : middle;
					middle = middle > 1 ? 1 : middle;
					map [x + stepsize / 2, y + stepsize / 2] = middle;

					int nx, ny;
					float diamond;

					//nx = x - stepsize / 2 < 0 ? size - 1 - stepsize / 2 : x - stepsize / 2;
					nx = x - stepsize / 2 < 0 ? x + stepsize / 2 : x - stepsize / 2;
					diamond = map [x, y] + middle + map [nx, y + stepsize / 2] + map [x, y + stepsize];
					diamond /= 4f;
					diamond += rough * rand();
					diamond = diamond < 0 ? 0 : diamond;
					diamond = diamond > 1 ? 1 : diamond;
					map [x, y + stepsize / 2] = diamond;

					//nx = x + stepsize / 2 > size-1 ? stepsize / 2 : x + stepsize / 2;
					nx = x + stepsize / 2 > size-1 ? x + stepsize / 2 : x + stepsize / 2;
					diamond = map [x + stepsize, y] + middle + map [nx, y + stepsize / 2] + map [x + stepsize, y + stepsize];
					diamond /= 4f;
					diamond += rough * rand();
					diamond = diamond < 0 ? 0 : diamond;
					diamond = diamond > 1 ? 1 : diamond;
					map [x + stepsize, y + stepsize / 2] = diamond;


					//ny = y - stepsize / 2 < 0 ? size - 1 - stepsize / 2 : y - stepsize / 2;
					ny = y - stepsize / 2 < 0 ? y + stepsize / 2 : y - stepsize / 2;
					diamond = map [x, y] + middle + map [x + stepsize / 2, ny] + map [x + stepsize, y];
					diamond /= 4f;
					diamond += rough * rand();
					diamond = diamond < 0 ? 0 : diamond;
					diamond = diamond > 1 ? 1 : diamond;
					map [x + stepsize / 2, y] = diamond;

					//ny = y + stepsize / 2 > size-1 ? stepsize / 2 : y + stepsize / 2;
					ny = y + stepsize / 2 > size-1 ? y + stepsize / 2 : y + stepsize / 2;
					diamond = map [x, y + stepsize] + middle + map [x + stepsize / 2, ny] + map [x + stepsize, y + stepsize];
					diamond /= 4f;
					diamond += rough * rand();
					diamond = diamond < 0 ? 0 : diamond;
					diamond = diamond > 1 ? 1 : diamond;
					map [x + stepsize / 2, y + stepsize] = diamond;
				}
			}

			rough /= 2;
		}

		return map;
	}

	private float rand()
	{
		float r = Random.value > 0.5f ? Random.value : -Random.value;
		return r;
	}


}
