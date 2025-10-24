using UnityEngine;

[CreateAssetMenu(fileName = "TowerDatabase", menuName = "Game/Tower Database")]
public class TowerDatabase : ScriptableObject
{
    [Header("Hairball Launcher")]
    public TowerStats hairballLauncher = new TowerStats
    {
        towerName = "Hairball Launcher",
        description = "Single-target tower that shoots hairballs at one enemy.",
        baseDamage = 1f,
        baseFireRate = 1.5f,
        baseRange = 5f,
        level2Damage = 1.5f,
        level2FireRate = 1f,
        level2Range = 5f,
        costLevel2 = 50,
        level3Damage = 1.5f,  // base damage but 2x every 5th shot
        level3FireRate = 1f,
        level3Range = 5f,
        costLevel3 = 200
    };

    [Header("Whisker Spinner")]
    public TowerStats whiskerSpinner = new TowerStats
    {
        towerName = "Whisker Spinner",
        description = "Shoots sharp whiskers in all directions.",
        baseDamage = 1f,
        baseFireRate = 1.5f,
        baseRange = 3f,
        level2Damage = 1f,
        level2FireRate = 1.5f,
        level2Range = 3f,
        costLevel2 = 50,
        level3Damage = 1f,
        level3FireRate = 0.75f, // temporary 2x buff
        level3Range = 3f,
        costLevel3 = 200
    };

    [Header("Twin Tails")]
    public TowerStats twinTails = new TowerStats
    {
        towerName = "Twin Tails",
        description = "Shoots two enemies at once, chance to slow.",
        baseDamage = 0.5f,
        baseFireRate = 1f,
        baseRange = 6f,
        level2Damage = 1f,
        level2FireRate = 1f,
        level2Range = 6f,
        costLevel2 = 50,
        level3Damage = 1f,
        level3FireRate = 1f,
        level3Range = 6f,
        costLevel3 = 200,

        hasSlowEffect = true,
        slowChance = 0.2f,      // 20% chance at level 3
        slowPercent = 0.25f,    // 25% slow
        slowDuration = 2f       // lasts 2 seconds
    };
}
