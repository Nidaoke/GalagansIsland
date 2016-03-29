using UnityEngine;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour {

    public List<AudioSource> PlayList;
    public List<AudioSource> MenuList;
    public List<AudioSource> NinjaSounds;
    public AudioSource ChooseMage;

    public AudioSource EarthSound;
    public AudioSource FireSound;
    public AudioSource LightSound;

    public static AudioManager Instance;

    private bool MusicLastState = true;
    private bool SoundLastState = true;

    void Awake()
    {
        Instance = this;
    }

    public void PlayAmbient()
    {
        StopAmbient();
        int tmp = Random.Range(0, PlayList.Count);
        PlayList[tmp].Play();
    }

    public void PlayMenu()
    {
        StopAmbient();
        int tmp = Random.Range(0, MenuList.Count);
        MenuList[tmp].Play();
    }

    public void PlayChooseMage()
    {
        StopAmbient();
        ChooseMage.Play();
    }

    private void StopAmbient()
    {
        for (int i = 0; i < PlayList.Count; i++)
        {
            if (PlayList[i].isPlaying)
                PlayList[i].Stop();
        }
        for (int i = 0; i < MenuList.Count; i++)
        {
            if (MenuList[i].isPlaying)
                MenuList[i].Stop();
        }
        ChooseMage.Stop();
    }

    public void PlayEarthSound()
    {
        EarthSound.Play();
    }

    public void PlayFireSound()
    {
        FireSound.Play();
    }

    public void PlayLightSound()
    {
        LightSound.Play();
    }


    public void SoundOn()
    {
        EarthSound.mute = false;
        FireSound.mute = false;
        LightSound.mute = false;
        SoundLastState = true;
    }

    public void SoundOff()
    {
        EarthSound.mute = true;
        FireSound.mute = true;
        LightSound.mute = true;
        SoundLastState = false;
    }

    public void MusicOn()
    {
        MenuList[0].mute = false;
        PlayList[0].mute = false;
        MusicLastState = true;
    }

    public void MusicOff()
    {
        MenuList[0].mute = true;
        PlayList[0].mute = true;
        MusicLastState = false;
    }

    public void MuteSoundForAd()
    {
        if (MusicLastState)
        {
            MenuList[0].mute = true;
            PlayList[0].mute = true;
        }
        if (SoundLastState)
        {
            EarthSound.mute = true;
            FireSound.mute = true;
            LightSound.mute = true;
        }
    }

    public void UnmuteSoundForAd()
    {
        if (MusicLastState)
        {
            MenuList[0].mute = false;
            PlayList[0].mute = false;
        }
        if (SoundLastState)
        {
            EarthSound.mute = false;
            FireSound.mute = false;
            LightSound.mute = false;
        }
    }

    public void PlayRandomNinjaSound()
    {
        int random = Random.RandomRange(0, NinjaSounds.Count);
        NinjaSounds[random].Play();
    }

    public void SetMusicVolume(float value)
    {
        ChooseMage.volume = value;
        MenuList[0].volume = value;
        PlayList[0].volume = value;
    }

    public void SetSoundVolume(float value)
    {
        NinjaSounds[0].volume = value;
        NinjaSounds[1].volume = value;
        NinjaSounds[2].volume = value;
        EarthSound.volume = value;
        FireSound.volume = value;
        LightSound.volume = value*0.5f;
    }
}
