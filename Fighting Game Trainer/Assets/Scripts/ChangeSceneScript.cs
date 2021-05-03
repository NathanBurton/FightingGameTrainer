using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeSceneScript : MonoBehaviour
{
    public GameObject excersiseTracker;

    public void LoadScene(int scene)
    {
        DontDestroyOnLoad(excersiseTracker);
        SceneManager.LoadScene(scene);
    }
}
