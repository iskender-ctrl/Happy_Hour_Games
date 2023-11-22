using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class GamePlayUIManager : MonoBehaviour
{
    public TextMeshProUGUI woodCountText;

    void Start()
    {
        UpdateWoodCountUI();
    }

    public void UpdateWoodCountUI()
    {
        // Oyuncunun odun sayısını UI üzerinde göster
        if (woodCountText != null)
        {
            woodCountText.text = PlayerData.WoodCount.ToString();
        }
    }
}
