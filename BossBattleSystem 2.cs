using UnityEngine;
using System;

public class BossBattleSystem : MonoBehaviour
{
    [Header("Boss Ayarları")]
    public GameObject bossPrefab;       // Çağrılacak Boss'un Prefab'ı
    public Transform spawnPoint;        // Boss'un duracağı yer
    public float bossFightTimeLimit = 30f; // Boss'u yenmek için verilen süre (saniye)

    private GameObject currentBoss;
    private float remainingTime;
    private bool isBossFightActive = false;

    // Arayüz (UI) veya diğer sistemlerin dinleyebileceği olaylar (Event)
    public static event Action OnBossFightStarted;
    public static event Action OnBossWon;
    public static event Action OnBossLost;

    // Boss savaşını başlatmak için çağrılacak fonksiyon (Örn: Bir butona bağlanabilir)
    public void StartBossFight()
    {
        if (isBossFightActive) return;

        isBossFightActive = true;
        remainingTime = bossFightTimeLimit;

        // Eğer sahnede başka bir canavar varsa önce onu yok et veya durdur
        // Boss'u sahneye getir
        if (bossPrefab != null && spawnPoint != null)
        {
            currentBoss = Instantiate(bossPrefab, spawnPoint.position, spawnPoint.rotation);
            Debug.Log("Boss Savaşı Başladı! Kalan Süre: " + remainingTime + " saniye");
            
            OnBossFightStarted?.Invoke();
        }
        else
        {
            Debug.LogWarning("Boss Prefab veya Spawn Point atanmamış!");
            isBossFightActive = false;
        }
    }

    void Update()
    {
        if (!isBossFightActive) return;

        // Süreyi herFRAME geriye doğru saydır
        remainingTime -= Time.deltaTime;

        // Süre bittiyse ve boss hâlâ yaşıyorsa savaşı kaybettin
        if (remainingTime <= 0)
        {
            FailBossFight();
        }
    }

    // Boss'un canı bittiğinde (Öldüğünde) bu fonksiyon dışarıdan çağrılmalı
    public void WinBossFight()
    {
        if (!isBossFightActive) return;

        isBossFightActive = false;
        Debug.Log("Tebrikler! Boss'u yendin! Yeni aşamaya geçiliyor.");
        
        OnBossWon?.Invoke();

        if (currentBoss != null)
        {
            Destroy(currentBoss);
        }
    }

    // Süre bittiğinde çalışacak fonksiyon
    void FailBossFight()
    {
        isBossFightActive = false;
        Debug.Log("Süre bitti! Boss'u zamanında kesemedin, savaş kaybedildi.");
        
        OnBossLost?.Invoke();

        if (currentBoss != null)
        {
            Destroy(currentBoss);
        }
    }

    // Dışarıdan boss savaşının aktif olup olmadığını sorgulamak için
    public bool IsBossFightActive()
    {
        return isBossFightActive;
    }
}
