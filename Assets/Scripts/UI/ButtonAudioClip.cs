using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonAudioClip : MonoBehaviour
{
    private AudioSource my_AudioSource;
    private ManagerVars vars;
    private void Awake()
    {
        vars = ManagerVars.GetManagerVars();
        my_AudioSource = GetComponent<AudioSource>();

        EventCenter.AddListener(EventDefine.PlayButtonAudioClip, PlayButtonAudioClip);
        EventCenter.AddListener<bool>(EventDefine.IsMusicOn, IsMusicOn);
    }
    private void OnDestroy()
    {
        EventCenter.RemoveListener(EventDefine.PlayButtonAudioClip, PlayButtonAudioClip);
        EventCenter.RemoveListener<bool>(EventDefine.IsMusicOn, IsMusicOn);
    }
    void PlayButtonAudioClip()
    {
        my_AudioSource.PlayOneShot(vars.buttonClip);
    }

    //是否开启音效
    void IsMusicOn(bool value)
    {
        my_AudioSource.mute = !value;
    }
}
