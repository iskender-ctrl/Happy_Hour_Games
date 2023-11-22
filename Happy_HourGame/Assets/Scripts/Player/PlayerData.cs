using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData
{
   // Oyuncunun seviye bilgisi
    public static int PlayerLevel { get; private set; } = 1;

    // Oyuncunun topladığı odun sayısı
    public static int WoodCount { get; private set; } = 0;

    // Oyuncunun seviyesini güncelleme fonksiyonu
    public static void UpdatePlayerLevel(int newLevel)
    {
        PlayerLevel = newLevel;
    }

    // Oyuncunun odun sayısını güncelleme fonksiyonu
    public static void UpdateWoodCount(int woodAmount)
    {
        WoodCount += woodAmount;
    }
}
