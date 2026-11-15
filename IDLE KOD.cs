using UnityEngine;
using System;

public class OfflineProgress : MonoBehaviour
{
    private const string SaveTimeKey = "LastExitTime";
    private const string GoldKey = "PlayerGold";

    public int goldPerMinute = 10;
    public int maxOfflineHours = 24;

    public void SaveExitTime()
    {
        long currentTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

        PlayerPrefs.SetString(SaveTimeKey, currentTime.ToString());
        PlayerPrefs.SetInt(GoldKey, PlayerPrefs.GetInt(GoldKey, 0));

        PlayerPrefs.Save();

        Debug.Log("Çıkış zamanı kaydedildi.");
    }

    public void CalculateOfflineRewards()
    {
        if (!PlayerPrefs.HasKey(SaveTimeKey))
            return;

        long savedTime = long.Parse(
            PlayerPrefs.GetString(SaveTimeKey)
        );

        long currentTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

        long elapsedSeconds = Math.Max(0, currentTime - savedTime);

        long maxSeconds = maxOfflineHours * 3600L;

        elapsedSeconds = Math.Min(elapsedSeconds, maxSeconds);

        int elapsedMinutes = (int)(elapsedSeconds / 60);

        int earnedGold = elapsedMinutes * goldPerMinute;

        int currentGold = PlayerPrefs.GetInt(GoldKey, 0);

        PlayerPrefs.SetInt(GoldKey, currentGold + earnedGold);

        PlayerPrefs.DeleteKey(SaveTimeKey);
        PlayerPrefs.Save();

        Debug.Log("Çevrimdışı süre: " + elapsedMinutes + " dakika");
        Debug.Log("Kazanılan altın: " + earnedGold);
    }

    private void Start()
    {
        CalculateOfflineRewards();
    }
}