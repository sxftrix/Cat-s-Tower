using UnityEngine;

[System.Serializable]
public class EnemyStats
{
    public string enemyName;
    public float baseHP;
    public float hpGrowthPerWave;
    public float speed;
    public int baseGold;

    // Returns the HP scaled by current wave number
    public float GetHP(int wave)
    {
        return baseHP + (hpGrowthPerWave * wave);
    }

    // Gold scaling formula: gold = round((HP/4) * (1 + 0.05 * (wave - 1)))
    public int GetGoldReward(int wave)
    {
        float scaledHP = GetHP(wave);
        float gold = (scaledHP / 4f) * (1 + 0.05f * (wave - 1));
        return Mathf.RoundToInt(gold);
    }
}
