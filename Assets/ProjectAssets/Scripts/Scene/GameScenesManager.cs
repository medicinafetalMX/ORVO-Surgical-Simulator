using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameScenesManager : MonoBehaviour
{
    public static GameScenesManager Instance;
    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if(Instance!=this)
        {
            Destroy(gameObject);
        }
    }
    public enum GameScenes
    {
        MFM, Surgery
    }

    private IEnumerator TransitionToScene(string scenename)
    {
        FindObjectOfType<VRPlayerSmoothSceneChanger>().CloseScene();
        yield return new WaitForSeconds(1);
        yield return SceneManager.LoadSceneAsync(scenename);
    }

    public void ResetCurrentScene()
    {
        StartCoroutine(TransitionToScene(SceneManager.GetActiveScene().name));
    }

    public void GoTo(GameScenes gameScene)
    {
        StartCoroutine(TransitionToScene(gameScene.ToString()));
    }
}
