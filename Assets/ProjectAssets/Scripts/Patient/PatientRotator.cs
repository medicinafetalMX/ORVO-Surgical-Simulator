using System;
using UnityEngine;
using DG.Tweening;

public class PatientRotator : MonoBehaviour
{
    [SerializeField] float _angleSteps;
    [SerializeField] AudioSource _audio;
    [SerializeField] AudioClip _rotateSound;
    private PatientRotation currentRotation = PatientRotation.Front;
    private float angle = 0;
    public enum PatientRotation
    {
        Left,Front,Right
    }

    public void RotatePatient(string rotation)
    {
        PatientRotation patientRotation = (PatientRotation) Enum.Parse(typeof(PatientRotation),rotation);
        currentRotation = patientRotation;
        switch (patientRotation)
        {
            case PatientRotation.Left:
                angle -= _angleSteps;
                break;
            case PatientRotation.Front:
                angle = 0;
                break;
            case PatientRotation.Right:
                angle += _angleSteps;
                break;
            default:
                Debug.LogError("Wrong patient rotation");
                break;
        }
        angle = Mathf.Clamp(angle, -90f, 90f);
        transform.DORotate(Vector3.right * angle, 1f);
        _audio.PlayOneShot(_rotateSound);
    }
}
