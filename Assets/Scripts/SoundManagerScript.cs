using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class SoundManagerScript : MonoBehaviour
{

    public static SoundManagerScript instance;

    public enum soundsEnum
    {
        laser,
        step,
        explosion,
        music,
        laserBall

    }

    [Serializable]
    public struct Sound
    {
        public soundsEnum soundEnum;
        public AudioSource audioSource;
        public float time;
    }

    public Sound[] sounds;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }



    public void PlaySound(soundsEnum sound)
    {
        foreach (var item in sounds)
        {
            if (item.soundEnum.Equals(sound) && !item.audioSource.isPlaying)
            {
                
                item.audioSource.Play();
            }
        }
    }

    public void StopSound(soundsEnum sound)
    {
        foreach (var item in sounds)
        {
            if (item.soundEnum.Equals(sound) && !item.audioSource.isPlaying)
            {

                item.audioSource.Stop();
            }
        }
    }

    private static bool CanPlaySound(Sound sound)
    {
        switch (sound.soundEnum)
        {
            default: return true;
                case soundsEnum.laser:
                float lastTimePlayed = sound.time;
                float stepTimerMax = 0.2f;
                if(lastTimePlayed + stepTimerMax < Time.time)
                {
                    sound.time = Time.time;
                    return true;
                }
                else
                {
                    return false;
                }
        }
    }
}
