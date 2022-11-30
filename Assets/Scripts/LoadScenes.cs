using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScenes : MonoBehaviour
{
   public void StartGame()
    {
        SceneManager.LoadScene("Environment");
        SceneManager.LoadScene("PlayerScene", LoadSceneMode.Additive);
        SceneManager.LoadScene("Buildings", LoadSceneMode.Additive);
        SceneManager.LoadScene("Initialization", LoadSceneMode.Additive);
    }
}
