using UnityEngine;

public class FightCharacterSpawner : MonoBehaviour
{
    [Header("Spawn points in this scene")]
    public Transform p1Spawn;
    public Transform p2Spawn;

    [Header("Character prefabs list (same order as SelectScene)")]
    public GameObject[] characterPrefabs; // size = 5

    void Start()
    {
        int p1 = GameSettings.I != null ? GameSettings.I.p1CharIndex : 0;
        int p2 = GameSettings.I != null ? GameSettings.I.p2CharIndex : 0;

        if (characterPrefabs == null || characterPrefabs.Length == 0)
        {
            Debug.LogError("Chưa gán characterPrefabs trong Inspector!");
            return;
        }

        Instantiate(characterPrefabs[p1], p1Spawn.position, Quaternion.identity);
        Instantiate(characterPrefabs[p2], p2Spawn.position, Quaternion.identity);
    }
}