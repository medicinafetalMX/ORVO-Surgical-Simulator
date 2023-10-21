using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class PatientInsideVisibility : MonoBehaviour
{
    public bool IsVisible = false;
    [SerializeField] Material _bellyMaterial;
    [SerializeField] float _invisibleAlpha;
    [SerializeField] Image _eyeIcon;
    [SerializeField] Image _fetusesEyeIcon;
    [SerializeField] Sprite _eyeOpen;
    [SerializeField] Sprite _eyeClose;
    [SerializeField] AudioSource _audio;
    [SerializeField] AudioClip _viewSound;
    [SerializeField] AudioClip _noViewSound;
    [SerializeField] GameObject _fetuses;
    public void MakeVisible()
    {
        IsVisible = true;
        _audio.PlayOneShot(_viewSound);
        Color currentColor = _bellyMaterial.color;
        DOTween.To(() => currentColor.a, x => currentColor.a = x, 1, 0.5f)
            .OnUpdate(() => { _bellyMaterial.color = currentColor; });
        _eyeIcon.sprite = _eyeOpen;
    }

    public void MakeInvisible()
    {
        IsVisible = false;
        _audio.PlayOneShot(_noViewSound);
        Color currentColor = _bellyMaterial.color;
        DOTween.To(() => currentColor.a, x => currentColor.a = x, _invisibleAlpha, 0.5f)
            .OnUpdate(() => { _bellyMaterial.color = currentColor; });
        _eyeIcon.sprite = _eyeClose;
    }
    public void Toggle()
    {
        IsVisible = !IsVisible;
        if (IsVisible)
            MakeVisible();
        else
            MakeInvisible();
    }
    public void ToggleChilds()
    {
        _fetuses.SetActive(!_fetuses.activeSelf);
        if (_fetuses.activeSelf) 
        {
            _audio.PlayOneShot(_viewSound);
            _fetusesEyeIcon.sprite = _eyeOpen;
        }
        else 
        {
            _audio.PlayOneShot(_noViewSound);
            _fetusesEyeIcon.sprite = _eyeClose;
        }
    }
}