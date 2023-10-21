using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(HandPhysical))]
public class HandSound : MonoBehaviour
{
    private HandPhysical hand;
    [SerializeField] AudioSource _audio;
    [SerializeField] AudioClip _grabSound;
    [SerializeField] AudioClip _releaseSound;

    private void Awake()
    {
        hand = GetComponent<HandPhysical>();
    }

    private void Start()
    {
        hand.OnGrab += PlayGrabSound;
        hand.OnRelease += PlayReleaseSound;
    }
    private void OnDestroy()
    {
        hand.OnGrab -= PlayGrabSound;
        hand.OnRelease -= PlayReleaseSound;
    }

    private void PlayGrabSound(GameObject arg0)
    {
        _audio.PlayOneShot(_grabSound);
    }

    private void PlayReleaseSound(GameObject arg0)
    {
        _audio.PlayOneShot(_releaseSound);
    }

    
}
