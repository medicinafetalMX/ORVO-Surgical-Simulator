using UnityEngine;
using DG.Tweening;

public class FlyingToolLock : MonoBehaviour
{
    [SerializeField] AudioClip _lockedSound;

    private Vector3 initialScale;
    private Vector3 initialRotation;
    private AudioSource audio;
    private void Awake()
    {
        audio = GetComponent<AudioSource>();
        initialRotation = transform.localRotation.eulerAngles;
        initialScale = transform.localScale;
    }
    public void ForceLock()
    {
        transform.localScale = initialScale;
    }

    public void ForceUnlock()
    {
        transform.localScale = Vector3.zero;
    }

    public void Lock()
    {
        transform.localScale = Vector3.zero;
        transform.DOScale(initialScale, 1);
    }

    public void Unlock()
    {
        transform.localScale = initialScale;
        transform.DOScale(Vector3.zero, 1);
    }

    public void TryPick()
    {
        transform.DOShakeRotation(1, Vector3.forward * 90f, 10, 360, true)
            .OnComplete(() => transform.localRotation = Quaternion.Euler(initialRotation));
        audio.PlayOneShot(_lockedSound);
    }
}
