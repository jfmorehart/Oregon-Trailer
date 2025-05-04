using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioSettings : MonoBehaviour
{
	public enum VolumeCategory { master, music, sfx};

    public static float settings_master_volume;
    public static float settings_music_volume;
	public static float settings_sfx_volume;

	public Slider[] sliders;

	public float music_multiplier, sfx_multiplier = 1;

	private void Awake()
	{
		Load();
	}

	public void SetGlobalVolume(float f) {
		settings_sfx_volume = f;
		settings_music_volume = f * music_multiplier;
		settings_sfx_volume = f * sfx_multiplier;
		Save();
    }
	public void SetMusicVolume(float f)
	{
		music_multiplier = f;
		settings_music_volume = settings_master_volume * f;
		Save();
	}
	public void SetSFXVolume(float f)
	{
		sfx_multiplier = f;
		settings_sfx_volume = f * settings_master_volume;
		Save();
	}

	void Save() {

		PlayerPrefs.SetFloat("music", music_multiplier);
		PlayerPrefs.SetFloat("sfx", sfx_multiplier);
		PlayerPrefs.SetFloat("master", settings_master_volume);
	}

	void Load() {
		settings_master_volume = PlayerPrefs.GetFloat("master", 1);
		sfx_multiplier = PlayerPrefs.GetFloat("sfx", 1);
		music_multiplier = PlayerPrefs.GetFloat("music", 1);

		settings_sfx_volume = sfx_multiplier * settings_master_volume;
		settings_music_volume = music_multiplier * settings_master_volume;

		if(sliders.Length == 3) {
			sliders[0].value = settings_master_volume;
			sliders[1].value = sfx_multiplier;
			sliders[2].value = music_multiplier;
		}
	}
}
