using UnityEngine;

public class GameSettings : MonoBehaviour
{
    public static GameSettings I { get; private set; }

    public int p1CharIndex = 0;
    public int p2CharIndex = 0;
    public int mapIndex = 0;

    private void Awake()
    {
        if (I != null) { Destroy(gameObject); return; }
        I = this;
        DontDestroyOnLoad(gameObject);
    }
}