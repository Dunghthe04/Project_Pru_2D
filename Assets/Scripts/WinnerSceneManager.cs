using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinnerSceneManager : MonoBehaviour
{
    [SerializeField] private string mainMenuScene = "MainMenu";

    public void BackToMenu()
    {
        SceneManager.LoadScene(mainMenuScene);
    }
}
