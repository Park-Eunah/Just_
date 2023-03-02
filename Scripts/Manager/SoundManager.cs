using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(AudioSource))]
public class SoundManager : MonoBehaviour
{
    public static SoundManager instance = null;
    #region singleton
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            //Destroy(this.gameObject);
        }
        DontDestroyOnLoad(this.gameObject);
    }
    #endregion singleton

    public Slider mainSlider;
    public Slider effectSlider;

    public AudioMixer mixer;

    [SerializeField]
    AudioSource myaudio;//bgm

    [SerializeField]
    AudioSource myaudio2;//벨소리


    [SerializeField]
    private AudioClip mainBgm;
    [SerializeField]
    private AudioClip fadeInBgm;
    [SerializeField]
    private AudioClip ClickBgm;
    [SerializeField]
    private AudioClip BreakTimeSound;

    private float mainVolume = 0.5f; //메인 bgm의 기본 음량 0.5
    private float effectVolume = 0.5f; //효과음 기본 음량 0.5

    private void Start()
    {
        myaudio = GetComponent<AudioSource>();
        //MainBgm();
    }
    public void MainBgm()
    {
        if (SceneManager.GetActiveScene().name == "Opening")
            myaudio.PlayOneShot(mainBgm);
        myaudio.volume=mainVolume;
    }
    public void MainBgmOff()
    {
        myaudio.Stop();
    }
    public void FadeInBgm()
    {
        myaudio.PlayOneShot(fadeInBgm);
        myaudio.volume = effectVolume;
    }
    public void ClickSound()
    {
        myaudio.PlayOneShot(ClickBgm);
        myaudio.volume = effectVolume;
    }
    public void BreakTimeBellSound()
    {
        myaudio.PlayOneShot(BreakTimeSound);
    }
    public void PhoneBellSound()
    {
        myaudio2.Play();
    }
    public void PhoneBellSoundStop()
    {
        myaudio2.Stop();
    }

    public void SetMainVolume()
    {
        mainVolume = mainSlider.value;
        myaudio.volume = mainVolume;
    }

    public void SetEffectVolume()
    {
        effectVolume = effectSlider.value;
        //효과음 음량 = effectVolume;
    }
}
