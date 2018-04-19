using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class LargeTerrain : MonoBehaviour {
	[HideInInspector]
	public Terrain tile1, tile2, tile3, tile4;
	[Space]
	public float terrainWidth;
	public float terrainLength;
	public float terrainHeight;
	public bool allowLocalScale = false;

	[HideInInspector]
	public float[,] map;

	private float otW, otL, otH;

	void Start () {
		dontUpdateSize ();
	}

	void Update () {
		if (allowLocalScale == false) transform.localScale = new Vector3 (1, 1, 1);
		if (terrainWidth != otW || terrainLength != otL || terrainHeight != otH) {
			
			tile1.transform.position = new Vector3 (0, 0, 0);
			tile2.transform.position = new Vector3 (0, 0, terrainLength / 2);
			tile3.transform.position = new Vector3 (terrainWidth / 2, 0, 0);
			tile4.transform.position = new Vector3 (terrainWidth / 2, 0, terrainLength / 2);

			TerrainData data1 = tile1.terrainData;
			TerrainData data2 = tile2.terrainData;
			TerrainData data3 = tile3.terrainData;
			TerrainData data4 = tile4.terrainData;

			data1.size = new Vector3 (terrainWidth / 2, terrainHeight, terrainLength / 2);
			data2.size = new Vector3 (terrainWidth / 2, terrainHeight, terrainLength / 2);
			data3.size = new Vector3 (terrainWidth / 2, terrainHeight, terrainLength / 2);
			data4.size = new Vector3 (terrainWidth / 2, terrainHeight, terrainLength / 2);

			otW = terrainWidth;
			otL = terrainLength;
			otH = terrainHeight;
		}
	}

	/// <summary>
	/// Prevent one time from updating size, after terrain size has changed manually
	/// </summary>
	public void dontUpdateSize()
	{
		otW = terrainWidth;
		otL = terrainLength;
		otH = terrainHeight;
	}

	/// <summary>
	/// Gets the height at position in world units.
	/// </summary>
	/// <returns>Return the height at position.</returns>
	/// <param name="position">Position in world units</param>
	public float SampleHeight(Vector3 position)
	{
		float offsetX = transform.position.x;
		float offsetZ = transform.position.z;

		Vector3 corrPos = position - new Vector3 (offsetX, 0, offsetZ);

		if (corrPos.x < terrainWidth / 2) {
			if (corrPos.z < terrainLength / 2) {
				return tile1.SampleHeight (position);
			}
			else
				return tile2.SampleHeight (position);
		} else {
			if (corrPos.z < terrainLength / 2) {
				return tile3.SampleHeight (position);
			}
			else
				return tile4.SampleHeight (position);
		}
	}
}
