using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct ChunkBiome
{
	public string name;//
	[SerializeField]
	public Chunk[] chunks;
}
