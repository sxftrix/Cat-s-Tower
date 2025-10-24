using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    [Header("References")]
    public TowerDatabase towerDatabase;
    public HairballLauncher tower1;
    public WhiskerSpinner tower2;
    public TwinTails tower3;

    [Header("UI Buttons")]
    public Button tower1Button;
    public Button tower2Button;
    public Button tower3Button;

    void Start()
    {
        // Assign upgrade actions to buttons
        tower1Button.onClick.AddListener(() => TryUpgradeTower(tower1));
        tower2Button.onClick.AddListener(() => TryUpgradeTower(tower2));
        tower3Button.onClick.AddListener(() => TryUpgradeTower(tower3));
    }

    void TryUpgradeTower(MonoBehaviour tower)
    {
        if (tower == null) return;

        int cost = 0;
        bool canUpgrade = false;

        // Determine tower type and cost
        if (tower is HairballLauncher h)
        {
            cost = (h.level == 1) ? towerDatabase.hairballLauncher.costLevel2 :
                   (h.level == 2) ? towerDatabase.hairballLauncher.costLevel3 : 0;
            canUpgrade = (h.level < 3);
        }
        else if (tower is WhiskerSpinner w)
        {
            cost = (w.level == 1) ? towerDatabase.whiskerSpinner.costLevel2 :
                   (w.level == 2) ? towerDatabase.whiskerSpinner.costLevel3 : 0;
            canUpgrade = (w.level < 3);
        }
        else if (tower is TwinTails t)
        {
            cost = (t.level == 1) ? towerDatabase.twinTails.costLevel2 :
                   (t.level == 2) ? towerDatabase.twinTails.costLevel3 : 0;
            canUpgrade = (t.level < 3);
        }

        // Check and spend gold
        if (canUpgrade && PlayerManager.Instance != null)
        {
            if (PlayerManager.Instance.SpendGold(cost))
            {
                if (tower is HairballLauncher) ((HairballLauncher)tower).Upgrade();
                if (tower is WhiskerSpinner) ((WhiskerSpinner)tower).Upgrade();
                if (tower is TwinTails) ((TwinTails)tower).Upgrade();

                Debug.Log($"{tower.name} upgraded successfully!");
            }
            else
            {
                Debug.Log("Not enough gold to upgrade!");
            }
        }
    }
}
