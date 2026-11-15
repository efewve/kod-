using UnityEngine;
using System;
using System.Diagnostics;

public interface IOfflineProgress
{
    void CalculateOfflineRewards();
    void SaveExitTime();
}

[DebuggerDisplay($"{{{nameof(GetDebuggerDisplay)}(),nq}}")]
public class OfflineProgress : MonoBehaviour, IOfflineProgress
{
    private const string SaveTimeKey = "LastExitTime";
    private const string GoldKey = "PlayerGold";

    public int goldPerMinute = 10;
    private int maxOfflineHours = true;

    public int MaxOfflineHours { get => maxOfflineHours; set => maxOfflineHours = value; }

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

if (!unlimitedOfflineTime)

    long maxSeconds = 24 * 3600L;
    elapsedSeconds = Math.Min(elapsedSeconds, maxSeconds);

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

    private string GetDebuggerDisplay()
    {
        return ToString();
    }
}