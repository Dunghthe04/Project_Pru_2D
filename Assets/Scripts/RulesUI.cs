using UnityEngine;
using UnityEngine.SceneManagement;

public class RulesUI : MonoBehaviour
{
    [SerializeField] private string mainMenuScene = "MainMenu";

    public void OnBackPressed()
    {
        SceneManager.LoadScene(mainMenuScene);
    }
}