using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
public class AudioManager : NetworkBehaviour
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
            AddSound(clip.name, clip);
        }
    }

    public void AddSound(string name, AudioClip clip)
    {
        soundDictionary.Add(name, clip);
    }

    [Command(requiresAuthority = false)]
    public void cmdLoopSound(string name)
    {
        if (soundDictionary.ContainsKey(name))
            soundLoop(name);
    }

    [ClientRpc]
    void soundLoop(string name)
    {
        audioSource.loop = true;
        PlaySound(name);
    }








    [Command(requiresAuthority = false)]
    public void cmDPlaySound(string name)
    {
        if (soundDictionary.ContainsKey(name))
            PlaySound(name);
        else
            Debug.Log("sound not found : " +name);
    }

    [ClientRpc]
    public void PlaySound(string name)
    {
        //Debug.Log(name);
        audioSource.loop = false;
        audioSource.clip = soundDictionary[name];
        audioSource.Play();
        
    }

    public void PlaySoundL(string name)
    {
        audioSource.loop = false;
        audioSource.clip = soundDictionary[name];
        audioSource.Play();
    }
    [Command(requiresAuthority =false)]
    public void cmdStopSound()
    {
        
        stopSound();
    }
    [ClientRpc]
    void stopSound()
    {
        audioSource.loop = false;
        audioSource.Stop();
    }
}
