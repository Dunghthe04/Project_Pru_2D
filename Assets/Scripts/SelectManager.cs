using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

#region DATA CLASSES
[System.Serializable]
public class CharacterEntry
{
    public string name;
    public Sprite portrait;
    public GameObject prefab;
}

[System.Serializable]
public class MapEntry
{
    public string name;
    public Sprite preview;
}
#endregion

public class SelectManager : MonoBehaviour
{
    [Header("CHARACTER DATA")]
    public List<CharacterEntry> characters = new();

    [Header("MAP DATA")]
    public List<MapEntry> maps = new(); // size = 2

    [Header("UI - CHARACTER SLOTS")]
    public List<RectTransform> characterSlots = new();
    public RectTransform p1Cursor;
    public RectTransform p2Cursor;

    [Header("UI - MAP SLOTS")]
    public List<RectTransform> mapSlots = new();
    public RectTransform mapCursor;

    [Header("STATUS TEXT")]
    public TMP_Text p1Status;
    public TMP_Text p2Status;
    public TMP_Text mapStatus;

    [Header("FIGHT SCENES")]
    [SerializeField] private string fightMap1Scene = "Fight_Map1";
    [SerializeField] private string fightMap2Scene = "Fight_Map2";

    int p1Index = 0;
    int p2Index = 0;
    int mapIndex = 0;

    bool p1Ready = false;
    bool p2Ready = false;
    bool mapLocked = false;

    void Start()
    {
        RefreshUI();
    }

    void Update()
    {
        HandlePlayer1();
        HandlePlayer2();
        HandleMap();

        // ===== START MATCH =====
        if (p1Ready && p2Ready && mapLocked)
        {
            GameSettings.I.p1CharIndex = p1Index;
            GameSettings.I.p2CharIndex = p2Index;
            GameSettings.I.mapIndex = mapIndex;

            string sceneToLoad =
                (mapIndex == 0) ? fightMap1Scene : fightMap2Scene;

            SceneManager.LoadScene(sceneToLoad);
        }
    }

    // =========================
    // PLAYER 1 INPUT
    // =========================
    void HandlePlayer1()
    {
        if (!p1Ready)
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                p1Index = Wrap(p1Index - 1, characterSlots.Count);
                RefreshUI();
            }

            if (Input.GetKeyDown(KeyCode.D))
            {
                p1Index = Wrap(p1Index + 1, characterSlots.Count);
                RefreshUI();
            }
        }

        if (Input.GetKeyDown(KeyCode.J))
        {
            p1Ready = true;
            RefreshUI();
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            p1Ready = false;
            RefreshUI();
        }
    }

    // =========================
    // PLAYER 2 INPUT
    // =========================
    void HandlePlayer2()
    {
        if (!p2Ready)
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                p2Index = Wrap(p2Index - 1, characterSlots.Count);
                RefreshUI();
            }

            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                p2Index = Wrap(p2Index + 1, characterSlots.Count);
                RefreshUI();
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            p2Ready = true;
            RefreshUI();
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            p2Ready = false;
            RefreshUI();
        }
    }

    // =========================
    // MAP SELECT
    // =========================
    void HandleMap()
    {
        if (!mapLocked)
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                mapIndex = Wrap(mapIndex - 1, mapSlots.Count);
                RefreshUI();
            }

            if (Input.GetKeyDown(KeyCode.S))
            {
                mapIndex = Wrap(mapIndex + 1, mapSlots.Count);
                RefreshUI();
            }
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            mapLocked = true;
            RefreshUI();
        }

        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            mapLocked = false;
            RefreshUI();
        }
    }

    // =========================
    // UI UPDATE
    // =========================
    void RefreshUI()
    {
        if (characterSlots.Count > 0)
        {
            p1Cursor.position = characterSlots[p1Index].position;
            p2Cursor.position = characterSlots[p2Index].position;
        }

        if (mapSlots.Count > 0)
        {
            mapCursor.position = mapSlots[mapIndex].position;
        }

        p1Status.text =
            p1Ready ?
            $"P1 READY : {characters[p1Index].name}" :
            $"P1 Choosing : {characters[p1Index].name}";

        p2Status.text =
            p2Ready ?
            $"P2 READY : {characters[p2Index].name}" :
            $"P2 Choosing : {characters[p2Index].name}";

        mapStatus.text =
            mapLocked ?
            $"MAP LOCKED : {maps[mapIndex].name}" :
            $"{maps[mapIndex].name}";
    }

    int Wrap(int i, int max)
    {
        if (max <= 0) return 0;
        if (i < 0) return max - 1;
        if (i >= max) return 0;
        return i;
    }
}