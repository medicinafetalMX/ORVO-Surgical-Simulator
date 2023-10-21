using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Instruction : MonoBehaviour
{
    [Header("Instructions")]
    [SerializeField] protected Instruction _continueInstruction;
    public abstract void Connect();
    public abstract void Disconnect();
}
