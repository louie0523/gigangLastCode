using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SfxManager : MonoBehaviour
{
    public static SfxManager instance;

    public Dictionary<string, AudioClip> clips = new Dictionary<string, AudioClip>();

    public AudioSource Sfx;
    public AudioSource Bgm;


    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            Sfx = GetComponent<AudioSource>();
            Bgm = transform.GetChild(0).GetComponent<AudioSource>();
            LoadClip();
            DontDestroyOnLoad(gameObject);
        } else
        {
            Destroy(gameObject);
        }
    }

    void LoadClip()
    {
        AudioClip[] clip = Resources.LoadAll<AudioClip>("Sound");

        foreach(AudioClip clipItem in clip)
        {
            clips.Add(clipItem.name, clipItem);
        }
    }

    public void PlaySfx(string name, float volume = 1f)
    {
        if (clips.TryGetValue(name, out AudioClip clip))
        {
            Sfx.PlayOneShot(clip, volume);
        }
    }

    public void PlayBgm(string name, float volume = 1f)
    {
        if (clips.TryGetValue(name, out AudioClip clip))
        {
            Bgm.Stop();
            Bgm.clip = clip;
            Bgm.volume = volume;
            Bgm.Play();
        }
    }


}
