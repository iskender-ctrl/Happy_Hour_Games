using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float moveSpeed = 5f; // Oyuncu hareket hızı
    public string targetPlayerTag = "Player"; // Hedef oyuncunun etiketi

    private bool isPlayerSelected = false;

    void Start()
    {
        SetStartPosition();
    }

    void Update()
    {
        HandleInput();
    }

    void SetStartPosition()
    {
        GameObject targetPlayer = GameObject.FindGameObjectWithTag(targetPlayerTag);

        if (targetPlayer != null)
        {
            // Oyuncunun pozisyonunu al
            Vector3 targetPosition = new Vector3(targetPlayer.transform.position.x, targetPlayer.transform.position.y + 5, targetPlayer.transform.position.z - 5);

            // Oyuncunun başlangıç pozisyonunu hedef oyuncunun pozisyonu olarak ayarla
            transform.position = targetPosition;
        }
        else
        {
            Debug.LogError("Hedef oyuncu bulunamadı. Lütfen doğru etiket kullanın.");
        }
    }

    void HandleInput()
    {
        Vector3 moveDirection = Vector3.zero;

#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBGL
        // Fare ve klavye kontrolü
        moveDirection.x = Input.GetAxis("Horizontal");
        moveDirection.z = Input.GetAxis("Vertical");

        // Kamera hareketi yapıldığı durumda karakterin seçimini kaldır
        if (isPlayerSelected)
        {
            DeselectPlayer();
        }
#elif UNITY_ANDROID || UNITY_IOS
        // Mobil cihaz kontrolü
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Moved)
            {
                moveDirection.x = touch.deltaPosition.x;
                moveDirection.z = touch.deltaPosition.y;

                // Kamera hareketi yapıldığı durumda karakterin seçimini kaldır
                if (isPlayerSelected)
                {
                    DeselectPlayer();
                }
            }
        }
#endif

        MovePlayer(moveDirection);
    }

    void MovePlayer(Vector3 moveDirection)
    {
        // Oyuncuyu hareket ettir
        transform.Translate(moveDirection.normalized * moveSpeed * Time.deltaTime, Space.World);
    }

    void DeselectPlayer()
    {
        // Karakterin seçimini kaldır
        isPlayerSelected = false;

        // Burada gerekirse başka işlemler yapabilirsiniz
    }
}
