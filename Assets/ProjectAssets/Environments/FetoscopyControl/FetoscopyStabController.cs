using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

public class FetoscopyStabController : MonoBehaviour
{
    [SerializeField] InputActionReference _moveInput;
    [SerializeField] BezierRoute _bezierRoute;
    [SerializeField] Vector2 _angles;
    [SerializeField][Range(0,1)] float _time = 1;

    private float currentTime;
    private void Update()
    {
        if(currentTime!= _time)
        {
            currentTime = _time;
            Vector3 position = _bezierRoute.Evaluate(_time);
            float rotation = Mathf.Lerp(_angles.x, _angles.y, _time);
            transform.position = position;
            transform.localRotation = Quaternion.Euler(Vector3.right * rotation);
        }
    }
}
