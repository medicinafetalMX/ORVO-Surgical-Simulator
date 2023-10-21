using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class VRPlayerSmoothSceneChanger : MonoBehaviour
{
    [SerializeField] GameObject _sphere;
    [SerializeField] Material _blackMaterial;
    private void Start()
    {
        OpenScene();
    }

    public void OpenScene()
    {
        _sphere.SetActive(true);
        _blackMaterial.color = Color.black;
        _blackMaterial.DOBlendableColor(new Color(1, 1, 1, 0), 1)
            .OnComplete(() =>
            {
                _sphere.SetActive(false);
            });
    }

    public void CloseScene()
    {
        _sphere.SetActive(true);
        _blackMaterial.color = new Color(1, 1, 1, 0);
        _blackMaterial.DOBlendableColor(Color.black, 1);
    }
}
