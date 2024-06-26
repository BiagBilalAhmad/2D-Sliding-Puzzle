using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;



public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;


    public AudioSource pannelSound;
    public AudioSource pannelPopSound;
    public AudioSource pannelCloseSound;
    public AudioSource pieceSound;


    public AudioMixer mainMixer, musicMixer;
    public Slider mainSlider, musicSlider;
    private void Awake()
    {
        if(instance==null)
        {
            instance=this;
        }
        else
        {
            Destroy(instance);
        }
    }
    private void Start()
    {
        mainMixer.SetFloat("Vol",PlayerPrefs.GetFloat("Vol",0));
        mainSlider.value = PlayerPrefs.GetFloat("Vol", 0);


        musicMixer.SetFloat("Vol", PlayerPrefs.GetFloat("Music", 0));
        musicSlider.value = PlayerPrefs.GetFloat("Music", 0);
    }
    public void SetMainVloume(float Vol)
    {
        mainMixer.SetFloat("Vol", Vol);
        PlayerPrefs.SetFloat("Vol", Vol);

    }

    public void SetMusicVloume(float Vol)
    {
        musicMixer.SetFloat("Music", Vol);
        PlayerPrefs.SetFloat("Music", Vol);

    }
    public void PlayPannelSound()
    {
        pannelSound.Play();
    }
    public void PlayPannelPopSound()
    {
        pannelPopSound.Play();
    }

    public void PlayPannelCloseSound()
    {
        pannelCloseSound.Play();
    }

    public void PlayPieceSound()
    {
        pieceSound.Play();
    }
}
