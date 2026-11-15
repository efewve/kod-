
using UnityEngine;
using System.Collections;

public class AutoHeal : MonoBehaviour
{
    [Header("Player Reference")]
    public PlayerStats player;

    [Header("Healing Settings")]
    [Range(0f, 1f)]
    public float healThreshold = 0.30f;

    public int healAmount = 30;
    public float healCooldown = 3f;

    private bool isHealing = false;

    private void Start()
    {
        if (player == null)
        {
            Debug.LogError("AutoHeal: Player bağlantısı yapılmadı!");
            enabled = false;
            return;
        }

        StartCoroutine(AutoHealLoop());
    }

    private IEnumerator AutoHealLoop()
    {
        while (true)
        {
            if (player.health > 0 && !isHealing)
            {
                float healthPercentage =
                    (float)player.health / player.maxHealth;

                if (healthPercentage <= healThreshold &&
                    player.health < player.maxHealth)
                {
                    StartCoroutine(HealPlayer());
                }
            }

            yield return new WaitForSeconds(0.5f);
        }
    }

    private IEnumerator HealPlayer()
    {
        isHealing = true;

        while (player.health > 0 &&
               player.health < player.maxHealth)
        {
            player.Heal(healAmount);

            Debug.Log("Otomatik iyileşme gerçekleşti!");

            if (player.health >= player.maxHealth)
                break;

            yield return new WaitForSeconds(healCooldown);
        }

        isHealing = false;
    }
}
