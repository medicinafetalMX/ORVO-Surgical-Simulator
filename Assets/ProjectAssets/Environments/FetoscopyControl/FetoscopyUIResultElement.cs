using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class FetoscopyUIResultElement : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _username;
    [SerializeField] TextMeshProUGUI _score;
    public void Fill(string username,string score)
    {
        _username.text = username;
        _score.text = score;
    }
}
