using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Globals
{
    public enum GameState {
        TitleScreen,
        Playing,
        Dying,
        ShowScoreAllowRestart
    }
    public static GameState CurrentGameState = GameState.TitleScreen;

    //keep track of scoring
    public static float BestTime = 0;
    public static float CurrentTime = 0;

    public const string BestTimePlayerPrefsKey = "BestTime";
    public static void SaveToPlayerPrefs(string key, float val)
    {
        PlayerPrefs.SetFloat(key, val);
    }
    public static float LoadFromPlayerPrefs(string key)
    {
        float val = PlayerPrefs.GetFloat(key, 0f);
        return val;
    }
}
