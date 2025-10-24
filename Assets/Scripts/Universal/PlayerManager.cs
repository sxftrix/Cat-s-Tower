using UnityEngine;
using TMPro;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance; // Singleton for global access

    [Header("Gold Settings")]
    public int startingGold = 20;
    private int currentGold;

    [Header("UI Reference (Optional)")]
    public TMP_Text goldText; // TextMeshProUGUI reference

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        currentGold = startingGold;
        UpdateGoldUI();
    }

    public void SetGoldText(GameObject textObject)
    {
        if (textObject != null)
        {
            TMP_Text tmp = textObject.GetComponent<TMP_Text>();
            if (tmp != null)
            {
                goldText = tmp;
                UpdateGoldUI();
            }
            else
            {
                Debug.LogWarning("The assigned GameObject does not have a TMP_Text component.");
            }
        }
    }

    public void AddGold(int amount)
    {
        currentGold += amount;
        UpdateGoldUI();
    }

    public bool SpendGold(int amount)
    {
        if (currentGold >= amount)
        {
            currentGold -= amount;
            UpdateGoldUI();
            return true;
        }
        else
        {
            Debug.Log("Not enough gold!");
            return false;
        }
    }

    public int GetGold()
    {
        return currentGold;
    }

    private void UpdateGoldUI()
    {
        if (goldText != null)
            goldText.text = $"Milk: {currentGold}";
    }
}
