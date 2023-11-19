using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon.StructWrapping;
using Photon.Pun;
using UnityEngine;

public class GamePlayManager : MonoBehaviour
{
    [Header("Spawn Points")]
    [SerializeField] Transform[] spawnPoints;

    string charactersName;
    // Start is called before the first frame update
    void Start()
    {
        charactersName = "Skeleton";
        // Oyuncu sırasını belirle (Photon tarafından atanan benzersiz oyuncu numarasını kullanabilirsiniz)
        int playerIndex = PhotonNetwork.LocalPlayer.ActorNumber - 1;

        // Sıra numarasına göre spawn noktasını seç
        if (playerIndex < spawnPoints.Length)
        {
            Transform spawnPoint = spawnPoints[playerIndex];

            // Spawn karakteri
            SpawnCharacter(spawnPoint);
        }
        else
        {
            Debug.LogError("Yeterli spawn noktası yok!");
        }
    }

    //Spawn Characters With Photon
    void SpawnCharacter(Transform spawnPoint)
    {
        // charactersName değişkenini kullanarak karakterinizi instantiate edin
        GameObject character = PhotonNetwork.Instantiate(charactersName, spawnPoint.position, spawnPoint.rotation);

        PhotonView photonView = character.GetComponent<PhotonView>();

        if (photonView.IsMine)
        {
            character.gameObject.tag = "Player";
        }
    }
}
