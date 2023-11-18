using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
public class MultiplayerManager : MonoBehaviourPunCallbacks
{
    public int playerLevel = 1;
    void Start()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Photon'a başarıyla bağlandı.");

        // Oyuncu uygun bir odaya katılmaya çalışır
    }

    public void TryJoinRoom()
    {
        if (!PhotonNetwork.InLobby)
        {
            Debug.Log("Lobiye katılmaya çalışılıyor...");
            PhotonNetwork.JoinLobby(); // Eğer lobiye bağlı değilse, lobiye bağlanma işlemi gerçekleştir
            return;
        }

        // Oyuncu uygun bir oda bulana kadar odaları listeler ve uygun olanı seçer
        ListRooms();
    }

    void ListRooms()
    {
        TypedLobby typedLobby = new TypedLobby("MyLobby", LobbyType.Default); // "MyLobby" adındaki lobideki odaları listeler
        PhotonNetwork.GetCustomRoomList(typedLobby, "ComplianceLevel >= " + playerLevel); // Uygunluk düzeyine göre filtreleme yapar
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        foreach (RoomInfo roomInfo in roomList)
        {
            int roomComplianceLevel = (int)roomInfo.CustomProperties["ComplianceLevel"];

            if (playerLevel >= roomComplianceLevel)
            {
                Debug.Log("Uygun oda bulundu, odaya katılıyor. Uygunluk Düzeyi: " + roomComplianceLevel);
                PhotonNetwork.JoinRoom(roomInfo.Name);
                return;
            }
        }

        Debug.Log("Uygun oda bulunamadı, yeni oda oluşturuluyor.");
        CreateRoom();
    }

    void CreateRoom()
    {
        if (PhotonNetwork.InLobby)
        {
            RoomOptions roomOptions = new RoomOptions();
            roomOptions.MaxPlayers = 4;

            ExitGames.Client.Photon.Hashtable customRoomProperties = new ExitGames.Client.Photon.Hashtable();
            customRoomProperties.Add("ComplianceLevel", playerLevel);
            roomOptions.CustomRoomProperties = customRoomProperties;
            roomOptions.CustomRoomPropertiesForLobby = new string[] { "ComplianceLevel" };

            PhotonNetwork.CreateRoom(null, roomOptions);
        }
        else
        {
            Debug.LogError("Lobby'de değil, oda oluşturulamaz.");
        }
    }
}
