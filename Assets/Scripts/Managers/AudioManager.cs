using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    public Dictionary<string, AudioClip> soundDictionary;
    private AudioSource audioSource;

    public List<AudioClip> soundList;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        audioSource = GetComponent<AudioSource>();
        soundDictionary = new Dictionary<string, AudioClip>();

        foreach (AudioClip clip in soundList)
        {
            Debug.Log(clip.name);
            AddSound(clip.name, clip);
        }
    }

    public void AddSound(string name, AudioClip clip)
    {
        soundDictionary.Add(name, clip);
    }

    public void PlaySound(string name)
    {
        if (soundDictionary.ContainsKey(name))
        {
            audioSource.PlayOneShot(soundDictionary[name]);
        }
    }
}
