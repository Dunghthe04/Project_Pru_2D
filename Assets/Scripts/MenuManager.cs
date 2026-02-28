using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [Header("Scene Names")]
    [SerializeField] private string characterSelectScene = "CharacterSelect";
    [SerializeField] private string rulesScene = "Rules";

    public void OnStartPressed()
    {
        SceneManager.LoadScene(characterSelectScene);
    }

    public void OnRulesPressed()
    {
        SceneManager.LoadScene(rulesScene);
    }

    public void OnExitPressed()
    {
        // Trong Editor thì không thoát, chỉ thoát khi build exe
        Application.Quit();
        Debug.Log("Quit Game (chỉ hoạt động khi build).");
    }
}