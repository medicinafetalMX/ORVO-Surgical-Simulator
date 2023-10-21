using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabbedObject : MonoBehaviour
{
    public GameObject LeftHand;
    public GameObject RightHand;
    public GameObject HeldObject;
    public void Grab(HandSides side)
    {
        LeftHand.SetActive(side == HandSides.LeftHand);
        RightHand.SetActive(side == HandSides.RightHand);
    }
}
