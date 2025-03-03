using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SoundPool
{
	public AudioClip[] clipPool;
	public int clip_nextIndex;

	public int sourcePoolSize = 10;
	public PooledSource[] sourcePool;
	public int source_nextIndex;
	

	public void PlaySoundAtPosition(Vector2 pos, float volume, float pitch = 1, float variation = 0) {
		PooledSource ps = GetPooledSource();
		ps.Play(clipPool[clip_nextIndex], pos, pitch, variation);
		ps.vMult = volume * SFX.Falloff(pos);
		clip_nextIndex++;
		if (clip_nextIndex >= clipPool.Length) clip_nextIndex = 0;
    }

	public PooledSource GetPooledSource() {
		source_nextIndex++;
		if(source_nextIndex >= sourcePool.Length) {
			source_nextIndex = 0;
		}
		return sourcePool[source_nextIndex];
    }

	public static SoundPool PoolFromFolder(string folderpath) {
		SoundPool newPool = new SoundPool();
		newPool.clipPool = SFX.instance.LoadFolder(folderpath);
		return newPool;
    }

	//public void DeliverCreatedSourcePool(PooledSource[] newPool) {
	//	sourcePool = newPool;
 //   }
}
