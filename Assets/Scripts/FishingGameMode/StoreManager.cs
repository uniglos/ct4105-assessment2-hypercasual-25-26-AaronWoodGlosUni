using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(ScoreSystem))]
public class StoreManager : MonoBehaviour
{
    ScoreSystem scoreSystem;
    [SerializeField] FishingRod Rod;

    [Tooltip("If false flat increase will be applied. If True increase amount will multiply ")]
    public bool increaseMultiplies = false;

    [Tooltip("Base Cost of upgrade")]
    public float FishLineBaseCost = 10;
    [Tooltip("How much cost is multiplied by every purchase")]
    public float LineCostIncreaseMulti = 1.5f;
    [Tooltip("How much the fishing line length will increase every time the player upgrades")]
    public float fishLineUpgradeAmount = 10;
    [Tooltip("Max Number of times Fishing Line length can be upgraded")]
    public int MaxNumberOfLineUpgrades = 5;
    int currentLineUpgrade = 0;
    public float fishingLineUpgradeAmountMulti = 1.25f;
    [Space]

    [Tooltip("Base Cost of upgrade")]
    public float RodSpeedBaseCost = 10;
    [Tooltip("How much cost is multiplied by every purchase")]
    public float RodCostIncreaseMulti = 1.5f;
    [Tooltip("How much the Bait speed will increase every time the player upgrades")]
    public float fishingRodSpeedUpgradeAmount = 0.5f;

   
    public float fishingRodSpeedUpgradeAmountMulti = 1.25f;

  [Tooltip("Max Number of times Fishing Line Speed can be upgraded")]
    public int MaxNumberRodSpeedUpgrade = 5;
    int currentRodUpgrade = 0;

    [Space]

    [Tooltip("Base Cost of upgrade")]
    public float BaitSizeBaseCost = 10;
    [Tooltip("How much cost is multiplied by every purchase")]
    public float BaitCostIncreaseMulti = 1.5f;
    [Tooltip("How much the bait size will increase every time the player upgrades")]
    public float baitSizeUpgradeAmount = 0.25f;
    [Tooltip("Max Number of times BaitSize can be upgraded")]
    public int MaxNumberBaitSizeAmount = 5;
    int currentBaitUpgrade = 0;
    public float fishingBaitAmountMulti = 1.25f;

    // Start is called before the first frame update
    void Start()
    {
        scoreSystem = GetComponent<ScoreSystem>();
    }
    public void UpgradeRodSpeed()
    {
        int cost = (int)RodSpeedBaseCost;
        if (scoreSystem.currentScore < cost) return;
        if (currentRodUpgrade >= MaxNumberRodSpeedUpgrade) return;

        //increase cost 
        if (increaseMultiplies) Rod.RodSpead += fishingRodSpeedUpgradeAmount * (fishingRodSpeedUpgradeAmountMulti * currentRodUpgrade);
        else Rod.RodSpead += fishingRodSpeedUpgradeAmount;
        currentRodUpgrade++;
        RodSpeedBaseCost = RodSpeedBaseCost * RodCostIncreaseMulti;

        scoreSystem.AdjustPoints(-cost);
    }


    public void UpgradeLineLength()
    {
        int cost = (int)FishLineBaseCost;
        if (scoreSystem.currentScore < cost) return;
        if (currentLineUpgrade >= MaxNumberOfLineUpgrades) return;

        //increase cost 
        if (increaseMultiplies) Rod._FishingLineLength += fishLineUpgradeAmount * (fishingLineUpgradeAmountMulti * currentLineUpgrade);
        else Rod._FishingLineLength += fishLineUpgradeAmount;
        currentLineUpgrade++;
        FishLineBaseCost = FishLineBaseCost * LineCostIncreaseMulti;

        scoreSystem.AdjustPoints(-cost);
    }

    public void UpgeadeBait()
    {
        int cost = (int)BaitSizeBaseCost;
        if (scoreSystem.currentScore < cost) return;
        if (currentBaitUpgrade >= MaxNumberBaitSizeAmount) return;

        //increase cost 
        if (increaseMultiplies) Rod._baitSize += baitSizeUpgradeAmount * (fishingBaitAmountMulti * currentBaitUpgrade);
        else Rod._baitSize += baitSizeUpgradeAmount; 
        currentBaitUpgrade++;
        BaitSizeBaseCost = BaitSizeBaseCost * BaitCostIncreaseMulti;

        scoreSystem.AdjustPoints(-cost);
    }

}
