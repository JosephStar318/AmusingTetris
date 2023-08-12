using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerPrefsHelper
{
    private static readonly string HighScore = "HighScore";
    private static readonly string Premium = "Premium";
    private static readonly string ClassicMode = "ClassicMode";
    private static readonly string SoundState = "SoundState";

    public static int GetHighScore()
    {
        return PlayerPrefs.GetInt(HighScore, 0);
    }
    public static bool GetPremiumState()
    {
        return PlayerPrefs.GetInt(Premium, 0) == 0 ? false : true;
    }
    public static bool GetSoundState()
    {
        return PlayerPrefs.GetInt(SoundState, 1) == 0 ? false : true;
    }
    public static bool GetClassicModeState()
    {
        return PlayerPrefs.GetInt(ClassicMode, 0) == 0 ? false : true;
    }
    public static void SetHighScore(int score)
    {
        PlayerPrefs.SetInt(HighScore, score);
    }
    public static void SetPremiumState(bool state)
    {
        PlayerPrefs.SetInt(Premium, state == true ? 1 : 0);
    }
    public static void SetSoundState(bool state)
    {
        PlayerPrefs.SetInt(SoundState, state == true ? 1 : 0);
    }
    public static void SetClassicMode(bool state)
    {
        PlayerPrefs.SetInt(ClassicMode, state == true ? 1 : 0);
    }

}
