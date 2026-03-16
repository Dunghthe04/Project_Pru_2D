using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [Header("Scene Names")]
    [SerializeField] private string characterSelectScene = "CharacterSelect";
    [SerializeField] private string rulesScene = "Rules";

    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip clickSound;

    public void OnStartPressed()
    {
        StartCoroutine(PlayClickThenLoadScene(characterSelectScene));
    }

    public void OnRulesPressed()
    {
        StartCoroutine(PlayClickThenLoadScene(rulesScene));
    }

    public void OnExitPressed()
    {
        audioSource.PlayOneShot(clickSound);
        Application.Quit();
        Debug.Log("Quit Game (chỉ hoạt động khi build).");
    }

    private IEnumerator PlayClickThenLoadScene(string sceneName)
    {
        audioSource.PlayOneShot(clickSound);
        yield return new WaitForSeconds(0.2f);
        SceneManager.LoadScene(sceneName);
    }
}