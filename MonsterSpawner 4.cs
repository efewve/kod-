using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    [Header("Spawn Ayarları")]
    public GameObject monsterPrefab; // Oluşturulacak canavarın Prefab'ı
    public Transform spawnPoint;     // Canavarın duracağı konum (Pozisyon)
    
    private GameObject currentMonster;

    void Start()
    {
        SpawnNewMonster();
    }

    // Canavar öldüğünde olayları dinlemeye başla
    private void OnEnable()
    {
        Monster.OnMonsterDeath += HandleMonsterDeath;
    }

    // Dinlemeyi kapat (Hata önlemek için önemli)
    private void OnDisable()
    {
        Monster.OnMonsterDeath -= HandleMonsterDeath;
    }

    public void SpawnNewMonster()
    {
        // Eğer sahnede halihazırda bir canavar yoksa yeni üret
        if (monsterPrefab != null && spawnPoint != null)
        {
            currentMonster = Instantiate(monsterPrefab, spawnPoint.position, spawnPoint.rotation);
            Debug.Log("Yeni bir canavar belirdi!");
        }
        else
        {
            Debug.LogWarning("Monster Prefab veya Spawn Point atanmamış!");
        }
    }

    // Canavar öldüğünde tetiklenecek fonksiyon
    void HandleMonsterDeath(int reward)
    {
        // 1 saniye sonra yeni canavar getir (İstersen süreyi değiştirebilirsin)
        Invoke("SpawnNewMonster", 1f);
    }
}