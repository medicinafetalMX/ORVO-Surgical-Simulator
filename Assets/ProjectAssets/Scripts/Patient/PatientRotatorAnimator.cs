using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DG.Tweening;

public class PatientRotatorAnimator : MonoBehaviour
{
    [SerializeField] [Range(0, 1)] float rotation = 0.5f;
    [SerializeField] float _step = 0.05f;
    [SerializeField] Animator _animator;
    [SerializeField] AudioSource _audio;
    [SerializeField] AudioClip _rotateSound;
    public enum Rotation { Left,Top,Right}
    private void Start()
    {
        SetToAnimator(0.5f);
    }
    private void SetToAnimator(float value)
    {
        _animator.SetFloat("Rotation", value);
    }
    public void Rotate(string side)
    {
        float lastRotation = rotation;
        Rotation select = (Rotation)Enum.Parse(typeof(Rotation),side);
        switch (select)
        {
            case Rotation.Left:
                rotation -= _step;
                break;
            case Rotation.Top:
                rotation = 0.5f;
                break;
            case Rotation.Right:
                rotation += _step;
                break;
            default:
                break;
        }
        rotation = Mathf.Clamp(rotation, 0, 1);
        DOTween.To(() => lastRotation, x => lastRotation = x, rotation, 1)
            .OnUpdate(() =>
            {
                SetToAnimator(lastRotation);
            });
        _audio.PlayOneShot(_rotateSound);
    }
}
