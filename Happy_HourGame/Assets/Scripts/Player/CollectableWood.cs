using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class CollectableWood : MonoBehaviourPunCallbacks
{
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            // RPC ile diğer oyunculara bu odunun yok edildiği bilgisini gönder
            photonView.RPC("DestroyWood", RpcTarget.AllBuffered);

            // Odun toplandığında PlayerData güncelle
            PlayerData.UpdateWoodCount(1);

            // UI üzerinde güncelleme yapmak için UIManager'ı bul
            GamePlayUIManager uiManager = FindObjectOfType<GamePlayUIManager>();
            if (uiManager != null)
            {
                // UI'ı güncelle
                uiManager.UpdateWoodCountUI();
            }
        }
    }

    [PunRPC]
    private void DestroyWood()
    {
        // Bu fonksiyon tüm oyuncuların ekranlarında çağrılır
        // Çarptığı diğer objeyi yok et
        Destroy(gameObject);
    }
}
