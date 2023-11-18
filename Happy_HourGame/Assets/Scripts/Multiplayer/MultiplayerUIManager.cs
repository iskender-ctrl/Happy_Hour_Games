using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using UnityEngine.UI;
public class MultiplayerUIManager : MonoBehaviourPunCallbacks
{
    public GameObject playerNamePrefab;
    public Transform playerNameUIContainer;
    [SerializeField] GameObject lobbyPanel;
    [SerializeField] string nextSceneName;
    [SerializeField] Button playButton;
    private Dictionary<int, GameObject> playerUIs = new Dictionary<int, GameObject>();

    void Start()
    {
        playButton.GetComponent<Button>().onClick.AddListener(LoadSceneOnClick);
        // Oyuncuya random bir isim atar ve Photon'a güncelleme yapar
        string randomName = GenerateRandomName();
        PhotonNetwork.NickName = randomName;
        UpdatePlayerName(randomName);

        // Eğer Photon bağlı değilse veya oyuncu bağlı değilse, doğrudan UI'yi güncelle
        if (!PhotonNetwork.IsConnected || !PhotonNetwork.InRoom)
        {
            UpdatePlayerUI(PhotonNetwork.LocalPlayer);
        }
    }

    string GenerateRandomName()
    {
        // Basit bir random isim üretme örneği
        string[] names = { "Player1", "Player2", "Player3", "Player4", "Player5" };
        int randomIndex = Random.Range(0, names.Length);
        return names[randomIndex];
    }

    void UpdatePlayerName(string newName)
    {
        // Photon tarafından sağlanan PlayerNameUpdate özelliği kullanılarak ismi günceller
        if (PhotonNetwork.IsConnectedAndReady)
        {
            ExitGames.Client.Photon.Hashtable playerCustomProperties = new ExitGames.Client.Photon.Hashtable();
            playerCustomProperties.Add("PlayerName", newName);
            PhotonNetwork.SetPlayerCustomProperties(playerCustomProperties);
        }
    }

    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        // Yeni oyuncu odaya katıldığında bu fonksiyon çağrılır
        UpdatePlayerUI(newPlayer);

        // Yeni oyuncuya odadaki diğer oyuncuları göster
        foreach (var player in PhotonNetwork.PlayerListOthers)
        {
            UpdatePlayerUI(player);
        }
    }

    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
        // Bir oyuncu odadan ayrıldığında bu fonksiyon çağrılır
        RemovePlayerUI(otherPlayer);
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Odaya başarıyla katıldınız!");
        lobbyPanel.SetActive(true);

        // Odaya katıldığında, mevcut oyuncuların isimlerini göster
        foreach (var player in PhotonNetwork.PlayerList)
        {
            if (player.ActorNumber != PhotonNetwork.LocalPlayer.ActorNumber)
            {
                UpdatePlayerUI(player);
            }
        }
        if (PhotonNetwork.IsMasterClient)
        {
            playButton.gameObject.SetActive(true);
        }
    }

    public override void OnPlayerPropertiesUpdate(Photon.Realtime.Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        // Oyuncu özellikleri güncellendiğinde bu fonksiyon çağrılır
        if (changedProps.ContainsKey("PlayerName"))
        {
            UpdatePlayerUI(targetPlayer);
        }
    }

    void UpdatePlayerUI(Photon.Realtime.Player player)
    {
        // Oyuncunun UI'sini günceller
        if (!playerUIs.ContainsKey(player.ActorNumber))
        {
            GameObject playerNameObject = Instantiate(playerNamePrefab, playerNameUIContainer);
            TextMeshProUGUI playerNameText = playerNameObject.GetComponentInChildren<TextMeshProUGUI>();

            if (playerNameObject != null && playerNameText != null)
            {
                playerNameText.text = player.NickName;
                playerUIs.Add(player.ActorNumber, playerNameObject);
            }
            else
            {
                Debug.LogError("UI veya TextMeshProUGUI bulunamadı!");
            }
        }
    }

    void RemovePlayerUI(Photon.Realtime.Player player)
    {
        // Oyuncunun UI'sini kaldırır
        if (playerUIs.ContainsKey(player.ActorNumber))
        {
            Destroy(playerUIs[player.ActorNumber]);
            playerUIs.Remove(player.ActorNumber);
        }
    }
    void LoadSceneOnClick()
    {
        // Sahneyi değiştir
        PhotonNetwork.LoadLevel(nextSceneName);
    }
}
