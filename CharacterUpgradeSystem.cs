using UnityEngine;
using System;

public class CharacterUpgradeSystem : MonoBehaviour
{
    [Header("Karakter Stat Referansı")]
    public CharacterStats playerStats; // Yükseltilecek karakterin ScriptableObject verisi

    [Header("Saldırı Yükseltme Ayarları")]
    public long attackUpgradeCost = 50;       // İlk saldırı yükseltme maliyeti
    public float attackCostMultiplier = 1.5f; // Her seviyede maliyetin artış kat sayısı
    public float attackIncreaseAmount = 2f;   // Her seviyede artacak saldırı gücü
    public int attackLevel = 1;

    [Header("Can Yükseltme Ayarları")]
    public long healthUpgradeCost = 40;       // İlk can yükseltme maliyeti
    public float healthCostMultiplier = 1.5f; // Her seviyede maliyet artış kat sayısı
    public float healthIncreaseAmount = 20f;  // Her seviyede artacak maksimum can
    public int healthLevel = 1;

    // Arayüzün (UI) güncellenmesi için olaylar
    public static event Action OnUpgradeCompleted;

    // Kayıt anahtarları
    private const string AttackLevelKey = "AttackUpgradeLevel";
    private const string HealthLevelKey = "HealthUpgradeLevel";

    void Start()
    {
        LoadUpgrades();
    }

    // --- SALDIRI GÜCÜNÜ YÜKSELT ---
    public void UpgradeAttack()
    {
        if (PlayerCurrencyAndXP.Instance == null) return;

        // Yeterli altın var mı kontrol et ve harca
        if (PlayerCurrencyAndXP.Instance.SpendGold(attackUpgradeCost))
        {
            attackLevel++;
            playerStats.attackDamage += attackIncreaseAmount;

            // Sonraki seviye için maliyeti artır
            attackUpgradeCost = (long)(attackUpgradeCost * attackCostMultiplier);

            SaveUpgrades();
            OnUpgradeCompleted?.Invoke();

            Debug.Log("Saldırı Yükseltildi! Yeni Hasar: " + playerStats.attackDamage + " | Seviye: " + attackLevel);
        }
        else
        {
            Debug.Log("Yetersiz altın! Saldırı yükseltilemedi.");
        }
    }

    // --- MAKSİMUM CANI YÜKSELT ---
    public void UpgradeHealth()
    {
        if (PlayerCurrencyAndXP.Instance == null) return;

        // Yeterli altın var mı kontrol et ve harca
        if (PlayerCurrencyAndXP.Instance.SpendGold(healthUpgradeCost))
        {
            healthLevel++;
            playerStats.maxHealth += healthIncreaseAmount;
            playerStats.currentHealth = playerStats.maxHealth; // Canlandırma ve canı doldurma

            // Sonraki seviye için maliyeti artır
            healthUpgradeCost = (long)(healthUpgradeCost * healthCostMultiplier);

            SaveUpgrades();
            OnUpgradeCompleted?.Invoke();

            Debug.Log("Can Yükseltildi! Yeni Max Can: " + playerStats.maxHealth + " | Seviye: " + healthLevel);
        }
        else
        {
            Debug.Log("Yetersiz altın! Can yükseltilemedi.");
        }
    }

    // --- KAYIT SİSTEMİ (SAVE / LOAD) ---
    void SaveUpgrades()
    {
        PlayerPrefs.SetInt(AttackLevelKey, attackLevel);
        PlayerPrefs.SetInt(HealthLevelKey, healthLevel);
        PlayerPrefs.Save();
    }

    void LoadUpgrades()
    {
        attackLevel = PlayerPrefs.GetInt(AttackLevelKey, 1);
        healthLevel = PlayerPrefs.GetInt(HealthLevelKey, 1);

        // Kaydedilen seviyelere göre statları ve maliyetleri başlangıca uygula
        for (int i = 1; i < attackLevel; i++)
        {
            playerStats.attackDamage += attackIncreaseAmount;
            attackUpgradeCost = (long)(attackUpgradeCost * attackCostMultiplier);
        }

        for (int i = 1; i < healthLevel; i++)
        {
            playerStats.maxHealth += healthIncreaseAmount;
            playerStats.currentHealth = playerStats.maxHealth;
            healthUpgradeCost = (long)(healthUpgradeCost * healthCostMultiplier);
        }
    }
}
