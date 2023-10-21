using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineScroll : MonoBehaviour
{
    [SerializeField] Material _lineMaterial;
    [SerializeField] float _speed;
    private void Update()
    {
        _lineMaterial.mainTextureOffset += Vector2.right * _speed * Time.deltaTime;
    }
}
