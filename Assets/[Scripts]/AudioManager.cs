using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 
 Source file Name - AudioManager.cs
 Name - Vitaliy Karabanov
 ID - 101312885
 Date last Modified - 12/11/2022
 Program description: Just a simple audio manager with instance

 */
public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    public AudioClip[] arrayOfClips;
    
    public AudioSource audioSourceForSoundEffects;
    public AudioSource audioSourceForSoundEffectsEnemy;
    private void Start()
    {
        instance = this;
        arrayOfClips = Resources.LoadAll<AudioClip>("[Sounds]");
        Debug.Log(arrayOfClips.Length);
    }

    public void PlayThisClip(string name)
    {
        foreach (var clip in arrayOfClips)
        {
            if (clip.name == name)
            {
                audioSourceForSoundEffects.clip = clip;
                audioSourceForSoundEffects.Play();
                break;
            }
        }
    }
    public void PlayThisClipEnemy(string name)
    {
        foreach (var clip in arrayOfClips)
        {
            if (clip.name == name && !audioSourceForSoundEffectsEnemy.isPlaying)
            {
                audioSourceForSoundEffectsEnemy.clip = clip;
                audioSourceForSoundEffectsEnemy.Play();
                break;
            }
            else
            {
                StartCoroutine(Delay(0.23f, audioSourceForSoundEffectsEnemy));
            }
        }
    }

    private IEnumerator Delay(float delay, AudioSource source)
    {
        yield return new WaitForSeconds(delay);
        source.Stop();
    }

    public AudioClip ReturnClipWithName(string name)
    {
        AudioClip temp = null;
        foreach (var clip in arrayOfClips)
        {
            if (clip.name == name)
            {
                temp = clip;
            }
        }
        return temp;
    }
}

