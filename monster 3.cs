using UnityEngine;
using System;

public class Monster : MonoBehaviour
{
    [Header("Canavar Özellikleri")]
    public string monsterName = "Goblin";
    public float maxHealth = 50f;
    private float currentHealth;
    public int goldReward = 15;

    // Canavar öldüğünde diğer sistemlerin (Para yöneticisi vb.) haberdar olması için bir Event
    public static event Action<int> OnMonsterDeath;

    void Start()
    {
        currentHealth = maxHealth;
    }

    // Hasar alma fonksiyonu (Otomatik savaş kodundan buraya saldırı gelebilir)
    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        Debug.Log(monsterName + " hasar aldı! Kalan Can: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log(monsterName + " öldü! Kazanılan Altın: " + goldReward);

        // Para sistemine altın ve XP gönder
        if (PlayerCurrencyAndXP.Instance != null)
        {
            PlayerCurrencyAndXP.Instance.AddGold(goldReward);
            PlayerCurrencyAndXP.Instance.AddXP(xpReward);
        }

        // --- YERE EŞYA BIRAKMA (DROP) ---
        if (itemDropPrefab != null)
        {
            // Canavarın öldüğü konumda eşyayı oluştur
            Instantiate(itemDropPrefab, transform.position, Quaternion.identity);
        }

        // Canavarı sahneden kaldır
        Destroy(gameObject);
    }
}