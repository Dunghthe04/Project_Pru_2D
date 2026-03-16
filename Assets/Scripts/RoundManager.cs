using UnityEngine;
using TMPro;
using System.Collections;

public class RoundManager : MonoBehaviour
{
    public static RoundManager I;

    public int p1Score = 0;
    public int p2Score = 0;

    public TMP_Text scoreText;
    public TMP_Text roundText;

    FighterHealth p1;
    FighterHealth p2;

    Vector3 p1StartPos;
    Vector3 p2StartPos;

    bool roundEnded = false;
    int currentRound = 1;

    void Awake()
    {
        I = this;
    }

    void Start()
    {
        StartCoroutine(StartRound());
    }

    IEnumerator StartRound()
    {
        ShowRoundText();

        yield return new WaitForSeconds(3f);

        roundText.gameObject.SetActive(false);
    }

    void ShowRoundText()
    {
        if (currentRound == 3)
            roundText.text = "FINAL ROUND";
        else
            roundText.text = "ROUND " + currentRound;

        roundText.gameObject.SetActive(true);
    }

    public void RegisterPlayers(FighterHealth player1, FighterHealth player2)
    {
        p1 = player1;
        p2 = player2;

        p1StartPos = p1.transform.position;
        p2StartPos = p2.transform.position;

        UpdateUI();
    }

    void Update()
    {
        if (p1 == null || p2 == null) return;

        if (!roundEnded)
        {
            if (p1.currentHealth <= 0)
            {
                roundEnded = true;
                Player2WinRound();
            }
            else if (p2.currentHealth <= 0)
            {
                roundEnded = true;
                Player1WinRound();
            }
        }
    }

    void Player1WinRound()
    {
        p1Score++;
        StartCoroutine(NextRound());
    }

    void Player2WinRound()
    {
        p2Score++;
        StartCoroutine(NextRound());
    }

    IEnumerator NextRound()
    {
        UpdateUI();

        if (p1Score >= 2 || p2Score >= 2)
        {
            roundText.text = (p1Score > p2Score) ? "P1 WINS!" : "P2 WINS!";
            roundText.gameObject.SetActive(true);
            yield break;
        }

        currentRound++;

        ShowRoundText();

        yield return new WaitForSeconds(3f);

        ResetPlayers();

        roundEnded = false;
        roundText.gameObject.SetActive(false);
    }

    void ResetPlayers()
    {
        // Reset máu
        p1.currentHealth = p1.maxHealth;
        p2.currentHealth = p2.maxHealth;

        // Reset vị trí
        p1.transform.position = p1StartPos;
        p2.transform.position = p2StartPos;

        // Reset controller
        FighterController c1 = p1.GetComponent<FighterController>();
        FighterController c2 = p2.GetComponent<FighterController>();

        c1.enabled = true;
        c2.enabled = true;

        // Reset Animator
        Animator a1 = p1.GetComponent<Animator>();
        Animator a2 = p2.GetComponent<Animator>();

        a1.Rebind();
        a1.Update(0f);

        a2.Rebind();
        a2.Update(0f);
    }

    void UpdateUI()
    {
        scoreText.text = $"P1 {p1Score} : {p2Score} P2";
    }
}