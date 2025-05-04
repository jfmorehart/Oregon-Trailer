using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolumeAdjuster : MonoBehaviour
{
    public AudioSettings.VolumeCategory audioCategory;

    float volume_setting;
    bool active;
    AudioSource src;


    // Start is called before the first frame update
    void Start()
    {
        active = TryGetComponent(out AudioSource aud);
        if (active) {
            volume_setting = aud.volume;
            src = aud;
	    }
    }

    // Update is called once per frame
    void Update()
    {
        if (!active) return;
        switch (audioCategory) {
            case AudioSettings.VolumeCategory.master:
                src.volume = volume_setting * AudioSettings.settings_master_volume;
                break;
			case AudioSettings.VolumeCategory.music:
				src.volume = volume_setting * AudioSettings.settings_music_volume;
				break;
			case AudioSettings.VolumeCategory.sfx:
				src.volume = volume_setting * AudioSettings.settings_sfx_volume;
				break;
		}
    }
}
