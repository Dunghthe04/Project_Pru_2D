using UnityEngine;

public class FightCharacterSpawner : MonoBehaviour
{
    [Header("Spawn points in this scene")]
    public Transform p1Spawn;
    public Transform p2Spawn;

    [Header("Character prefabs list (same order as SelectScene)")]
    public GameObject[] characterPrefabs; // size = 5

    public float characterScale = 2f;
    public float narutoScale = 3f;
    public HealthBarUI p1HealthBar;
    public HealthBarUI p2HealthBar;

    float GetScale(GameObject prefab) =>
        prefab.name.Contains("Naruto") ? narutoScale : characterScale;

    void Start()
    {
        int p1 = GameSettings.I != null ? GameSettings.I.p1CharIndex : 0;
        int p2 = GameSettings.I != null ? GameSettings.I.p2CharIndex : 0;



        GameObject player1 =
            Instantiate(characterPrefabs[p1], p1Spawn.position, Quaternion.identity);

        GameObject player2 =
            Instantiate(characterPrefabs[p2], p2Spawn.position, Quaternion.identity);

        // scale nhân vật
        float s1 = GetScale(characterPrefabs[p1]);
        float s2 = GetScale(characterPrefabs[p2]);
        player1.transform.localScale = Vector3.one * s1;
        player2.transform.localScale = new Vector3(-s2, s2, s2);

        player1.GetComponent<FighterController>().playerID = 1;
        player2.GetComponent<FighterController>().playerID = 2;
        // GÁN MÁU CHO UI
        p1HealthBar.fighter = player1.GetComponent<FighterHealth>();
        p2HealthBar.fighter = player2.GetComponent<FighterHealth>();
    }
}