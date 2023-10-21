using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StabActivator : MonoBehaviour
{
    [SerializeField] bool _snapRot = false;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Stab"))
        {
            StabAnimationVRController stabController = other.GetComponent<StabAnimationVRController>();
            if (!stabController)
                stabController = other.GetComponentInParent<StabAnimationVRController>();

            if (stabController.StabCoolDown && !stabController.IsStabbed)
            {
                float distanceToPoint = Vector3.Distance(stabController.BeginPoint.position, transform.position);
                if (distanceToPoint <= 0.1f)
                {
                    if (_snapRot)
                    {
                        stabController.transform.rotation = Quaternion.LookRotation(transform.forward, transform.up);
                        stabController.transform.position = transform.position + transform.up * (stabController.transform.position - stabController.BeginPoint.position).magnitude;
                        stabController.SetStabPoint(transform.position);
                        stabController.Stab();
                    }
                    else
                    {
                        stabController.RaycastStabPoint();
                        stabController.Stab();
                    }
                }
            }
        }
    }
}
