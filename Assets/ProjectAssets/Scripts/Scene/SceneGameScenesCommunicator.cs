using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SceneGameScenesCommunicator : MonoBehaviour
{
    public void ReloadScene()
    {
        GameScenesManager.Instance.ResetCurrentScene();
    }
    public void LoadScene(string sceneName)
    {
        var gameScene = (GameScenesManager.GameScenes) Enum.Parse(typeof(GameScenesManager.GameScenes), sceneName);
        GameScenesManager.Instance.GoTo(gameScene);
    }
}
