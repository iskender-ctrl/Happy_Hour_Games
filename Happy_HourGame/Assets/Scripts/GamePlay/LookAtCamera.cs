using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
public class LookAtCamera : MonoBehaviour
{
    public Image childImage;

    private void Start()
    {
        SetObjectColor();
    }

    private void Update()
    {
        // Kameranın pozisyonunu ve rotasyonunu al
        Transform cameraTransform = Camera.main.transform;

        // Yatay ve dikey dönüşleri hesapla
        float horizontalRotation = cameraTransform.eulerAngles.y;
        float verticalRotation = cameraTransform.eulerAngles.x;

        // Objeyi kameraya doğru döndür
        transform.eulerAngles = new Vector3(verticalRotation, horizontalRotation, 0f);
    }

    private void SetObjectColor()
    {
        // Örnek olarak, PhotonView'a sahip olan oyuncunun rengini kontrol et
        PhotonView photonView = transform.parent.GetComponent<PhotonView>();

        if (photonView != null)
        {
            // Oyuncu yerel oyuncu ise
            if (photonView.IsMine)
            {
                // Child objenin rengini beyaz yap
                childImage.color = Color.white;
            }
            else
            {
                // Diğer oyuncuların rengini kırmızı yap
                childImage.color = Color.red;
            }
        }
    }
}
