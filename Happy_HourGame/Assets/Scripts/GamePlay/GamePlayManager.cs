using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon.StructWrapping;
using Photon.Pun;
using Unity.VisualScripting;
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

    //Spawn Characters With Photon and Add Tags
    void SpawnCharacter(Transform spawnPoint)
    {
        // charactersName değişkenini kullanarak karakterinizi instantiate edin
        GameObject character = PhotonNetwork.Instantiate(charactersName, spawnPoint.position, spawnPoint.rotation);

        PhotonView photonView = character.GetComponent<PhotonView>();

        if (photonView.IsMine)
        {
            character.gameObject.tag = "Player";
            character.AddComponent<PlayerController>();
            SetChildrenTags(character, "Player");
            SetChildrenAddComponent(character);
        }
    }

    //Add Tags For Player Childs for Click and Movement
    void SetChildrenTags(GameObject parent, string tag)
    {
        foreach (Transform child in parent.transform)
        {
            child.gameObject.tag = tag;
            SetChildrenTags(child.gameObject, tag);
        }
    }
    void SetChildrenAddComponent(GameObject parent)
    {
        foreach (Transform child in parent.transform)
        {
            child.AddComponent<CollectableWood>();
        }
    }
}

