using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FetoscopyTarget : MonoBehaviour
{
    public bool IsAlive { get; set; }
    [Range(0,1)]public float life = 1;
    [SerializeField] public CirculationIntersectionType type;
    [SerializeField] float _damageSpeed = 0.3f;
    private bool isDamaging = false;
    
    private void Start()
    {
        IsAlive = true;
    }
    private void Update()
    {
        if (isDamaging)
        {
            life -= Time.deltaTime * _damageSpeed;
            if(life<=0)
            {
                IsAlive = false;
                life = 0;
                isDamaging = false;
            }
        }
    }
    public void Damage()
    {
        if(IsAlive)
            isDamaging = true;
    }

    public void StopDamage()
    {
        isDamaging = false;
        if (life <= 0.15f)
        {
            IsAlive = false;
            life = 0f;
        }
    }
}
