using UnityEngine;
using UnityEngine.UI;

public class ManaBarUI : MonoBehaviour
{
    public FighterHealth fighter;
    public Image manaBar;

    void Update()
    {
        if (fighter == null || manaBar == null) return;

        float manaPercent = (float)fighter.currentMana / fighter.maxMana;
        manaBar.fillAmount = manaPercent;
    }
}
