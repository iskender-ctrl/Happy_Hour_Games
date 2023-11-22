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
    private GameObject targetEnemy; // Hedef düşman

    public int woodCount = 0; // Oyuncunun odun sayısı
    RaycastHit hit;

    void Start()
    {
        clickableLayer = LayerMask.GetMask("Clickable");
        mainCamera = Camera.main;
        navMeshAgent = GetComponentInChildren<NavMeshAgent>();
    }

    void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                GameObject clickedObject = hit.collider.gameObject;

                if (clickedObject.CompareTag("Player"))
                {
                    // Tıklanan nesne bir oyuncu karakteri ise seç
                    SelectCharacter(clickedObject);
                }
                else if (clickedObject.CompareTag("Enemy"))
                {
                    print(clickedObject);
                    // Tıklanan nesne bir düşman ise hedef düşmanı ayarla
                    SetTargetEnemy(clickedObject);
                }
                else if (clickedObject.gameObject.tag != "Obstacle")
                {
                    // Tıklanan yer bir karakter veya düşman değilse, seçili karaktere git
                    MoveToDestination(hit.point);
                }
            }
        }

        // Seçili karakterin child objelerini kontrol et ve animasyonları oynat
        PlayAnimations();

        // Eğer hedef düşman varsa, ona doğru hareket et ve belirli bir mesafeye yaklaşınca attack animasyonunu çalıştır
        if (targetEnemy != null)
        {
            MoveToDestination(targetEnemy.transform.position);

            float distanceToEnemy = Vector3.Distance(selectedCharacter.transform.position, targetEnemy.transform.position);
            print(distanceToEnemy);
            if (distanceToEnemy < 0.7f) // Belirli bir mesafe içinde ise attack animasyonunu çalıştır
            {
                AttackEnemy();
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

        // Hedef düşman varsa sıfırla
        targetEnemy = null;

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

    void SetTargetEnemy(GameObject enemy)
    {
        // Hedef düşmanı ayarla
        targetEnemy = enemy;
    }

    void PlayAnimations()
    {
        // Seçili karakterin child objelerini kontrol et ve animasyonları oynat
        if (selectedCharacter != null)
        {
            NavMeshAgent navMeshAgent = selectedCharacter.GetComponentInChildren<NavMeshAgent>();
            Animator animator = selectedCharacter.GetComponent<Animator>();

            if (navMeshAgent != null && animator != null)
            {
                // Karakter hareket ediyorsa
                if (navMeshAgent.velocity.magnitude > 0.1f)
                {
                    animator.SetBool("IsWalking", true);
                }
                else
                {
                    animator.SetBool("IsWalking", false);
                }
            }
            else
            {
                Debug.LogWarning("NavMeshAgent or Animator component not found on the selected character: " + selectedCharacter.name);
            }
        }
    }

    void AttackEnemy()
    {
        // Attack animasyonunu çalıştır
        Animator animator = selectedCharacter.GetComponent<Animator>();
        if (animator != null)
        {
            // NavMeshAgent'i bul
            NavMeshAgent navMeshAgent = selectedCharacter.GetComponentInChildren<NavMeshAgent>();

            if (navMeshAgent != null)
            {
                // NavMeshAgent'i durdur
                navMeshAgent.isStopped = true;
            }

            // Attack animasyonunu çalıştır
            animator.SetBool("Attack", true);
        }
    }


    bool IsCameraMoving()
    {
#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBGL
        // Fare ve klavye kontrolü
        return Input.GetAxis("Horizontal") != 0f || Input.GetAxis("Vertical") != 0f;
#elif UNITY_ANDROID || UNITY_IOS
        // Mobil cihaz kontrolü
        return Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved;
#else
        return false;
#endif
    }
}
