using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HUDUIManager : MonoBehaviour
{
    public static HUDUIManager Instance { get; private set; }

    public TextMeshProUGUI FoodCounter1;
    public TextMeshProUGUI FoodCounter2;
    public TextMeshProUGUI FoodCounter3;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void UpdateEnemyCounter(string EnemyType, int count)
    {
        switch(EnemyType)
        {
            case "Grape":
                FoodCounter1.text = count.ToString();
                break;
            case "Watermelon":
                FoodCounter2.text = count.ToString();
                break;
            case "Banana":
                FoodCounter3.text = count.ToString();
                break;
        }
    }

    public void ResetAllCounters()
    {
        FoodCounter1.text = "0";
        FoodCounter2.text = "0";
        FoodCounter3.text = "0";
    }
}
