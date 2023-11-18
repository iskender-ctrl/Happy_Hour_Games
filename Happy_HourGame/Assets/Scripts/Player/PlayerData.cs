using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData
{
    // Oyuncunun seviye bilgisi
    public static int PlayerLevel { get; private set; } = 1;

    // Oyuncunun seviyesini güncelleme fonksiyonu
    public static void UpdatePlayerLevel(int newLevel)
    {
        PlayerLevel = newLevel;
    }
}
