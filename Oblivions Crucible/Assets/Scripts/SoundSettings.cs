using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundSettings : MonoBehaviour
{
    public Slider  masterSlider, musicSlider, sfxSlider;

    public void ToggleMusic()
    {
        AudioManager.instance.ToggleMusic();
    }

    public void ToggleSfx()
    {
        AudioManager.instance.ToggleSFX();
    }


    public void MusicVol()
    {
        AudioManager.instance.MusicVol(masterSlider.value * musicSlider.value);
    }

    public void SfxVol()
    {
        AudioManager.instance.SfxVol( sfxSlider.value);
    }
}
