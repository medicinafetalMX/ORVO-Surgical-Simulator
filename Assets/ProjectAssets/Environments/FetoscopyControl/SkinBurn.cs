using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkinBurn : MonoBehaviour
{
    [SerializeField] Material[] _materialAlternatives;

    private Renderer renderer;
    private void Awake()
    {
        renderer = GetComponent<Renderer>();
    }
    private void Start()
    {
        renderer.material = _materialAlternatives[Random.Range(0, _materialAlternatives.Length)];
    }
    public void Init()
    {
        //transform.Rotate(transform.up, Random.Range(0, 360f));
    }
}
