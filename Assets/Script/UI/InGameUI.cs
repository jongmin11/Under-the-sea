using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// (한종민)인게임 UI용 버튼 효과음 전용 스크립트입니다.
/// </summary>
public class InGameUI : MonoBehaviour
{
    [SerializeField] private AudioSource sfxPlayer;
    [SerializeField] private AudioClip clickSound;
    
    public void InGameClickAudio()
    { 
        if (sfxPlayer && clickSound)
        {
            sfxPlayer.PlayOneShot(clickSound);
        }
    }
}
