using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
[RequireComponent(typeof(ActionBasedController))]
public class HandController : MonoBehaviour
{
    [SerializeField] Hand hand;
    private ActionBasedController controller;
    private void Start()
    {
        controller = GetComponent<ActionBasedController>();
    }
    private void Update()
    {
        float gripValue = controller.selectActionValue.action.ReadValue<float>();
        float triggerValue = controller.activateActionValue.action.ReadValue<float>();
        if (gripValue < 0.01f)
            gripValue = 0;
        if (triggerValue < 0.01f)
            triggerValue = 0;
        hand.SetGrip(gripValue);
        hand.SetTrigger(triggerValue);
        Debug.Log(name + " trigger = " + triggerValue);
        Debug.Log(name + " grip = " + gripValue);
    }
}
