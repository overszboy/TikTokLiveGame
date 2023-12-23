using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioService : MonoBehaviour
{
  

  [SerializeField] private AudioClip slotFx;
  [SerializeField] private AudioClip winnFx;
   
[SerializeField] private AudioSource audioSource;


public void PlaySlotFx()
{
    audioSource.Stop();
    audioSource.clip=slotFx;
    audioSource.Play();
}
public void PlayWinFx()
{
    audioSource.Stop();
    audioSource.clip=winnFx;
    audioSource.Play();
}

}
