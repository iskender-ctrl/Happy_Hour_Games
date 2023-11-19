using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class PlayerController : MonoBehaviour
{
    public LayerMask clickableLayer; // Tıklanabilir nesnelerin layer'ı
    private Camera mainCamera; // Ana kamera
    private NavMeshAgent navMeshAgent; // NavMeshAgent bileşeni

    private GameObject selectedCharacter; // Seçilen karakter

    void Start()
    {
        mainCamera = Camera.main;
        navMeshAgent = GetComponentInChildren<NavMeshAgent>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                GameObject clickedObject = hit.collider.gameObject;

                if (clickedObject.CompareTag("Player"))
                {
                    // Tıklanan nesne bir oyuncu karakteri ise seç
                    SelectCharacter(clickedObject);
                }
                else
                {
                    // Tıklanan yer bir karakter değilse, seçili karaktere git
                    MoveToDestination(hit.point);
                }
            }
        }
    }

    void SelectCharacter(GameObject character)
    {
        // Eğer zaten seçili bir karakter varsa, seçimi kaldır
        if (selectedCharacter != null && selectedCharacter != character)
        {
            DeselectCharacter();
        }

        // Seçili karakteri güncelle
        selectedCharacter = character;

        // Seçilen karakterde NavMeshAgent varsa, hareketi durdur
        NavMeshAgent selectedNavMeshAgent = selectedCharacter.GetComponentInChildren<NavMeshAgent>();
        if (selectedNavMeshAgent != null)
        {
            selectedNavMeshAgent.isStopped = true;
        }
    }

    void DeselectCharacter()
    {
        // Seçili karakterin seçimini kaldır
        if (selectedCharacter != null)
        {
            NavMeshAgent selectedNavMeshAgent = selectedCharacter.GetComponentInChildren<NavMeshAgent>();
            if (selectedNavMeshAgent != null)
            {
                selectedNavMeshAgent.isStopped = false;
            }
            selectedCharacter = null;
        }
    }

    void MoveToDestination(Vector3 destination)
    {
        // Hedefe gitmek için NavMeshAgent kullan
        if (selectedCharacter != null)
        {
            NavMeshAgent navMeshAgent = selectedCharacter.GetComponentInChildren<NavMeshAgent>();

            if (navMeshAgent != null)
            {
                // NavMeshAgent'ı başlat ve hedefe git
                navMeshAgent.isStopped = false;
                navMeshAgent.SetDestination(destination);
            }
        }
    }
}
