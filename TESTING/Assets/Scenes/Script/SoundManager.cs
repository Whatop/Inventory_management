using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class SoundManager: MonoBehaviour
{
    public AudioSource bgSound;
    public AudioMixer mixer;
    public AudioClip[] bglist;

    public static SoundManager instance;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(instance);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }


    // 이렇게 하면 씬마다 하지만..
    // 언더테일은 방마다 하니 안됨!
    // 그래서 스테이지에서 게임 매니저로 방있으면 저거하는..ㅋㅋ..
    // 아마 방 번호를 따로 지정해줘야겠지! 음 음..
    private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        for(int i = 0; i < bglist.Length; i++)
        {
            if(arg0.name == bglist[i].name)
               BGSoundPlay(bglist[i]);
        }
    }

    public void BGSoundVolume(float val)
    {
        mixer.SetFloat("BGSound", Mathf.Log10(val)*20);
    }
    public void SFXSoundVolume(float val)
    {
        mixer.SetFloat("SFXSound", Mathf.Log10(val) * 20);
    }

    // 사용방법
    // SoundManager.instance.SFXPlay("",clip);
    // 이게뭐냐
    public void SFXPlay(string sfxName, AudioClip clip)
    {
        GameObject go = new GameObject(sfxName + "Sound");
        AudioSource audioSource = go.AddComponent<AudioSource>();
        audioSource.clip = clip;
        audioSource.outputAudioMixerGroup = mixer.FindMatchingGroups("SFX")[0];
        audioSource.volume = 0.1f;
        audioSource.Play();

        Destroy(go, clip.length);
    }

    public void BGSoundPlay(AudioClip clip)
    {
        bgSound.outputAudioMixerGroup = mixer.FindMatchingGroups("BG")[0];
        bgSound.clip = clip;
        bgSound.loop = true;
        bgSound.volume = 0.1f;
        bgSound.Play();
    }

}
