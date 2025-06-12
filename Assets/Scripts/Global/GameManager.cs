using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameSessionParams
{
    public static string EnemyClientId;
    public static string DeckTemplateClientId;
    public static string EventId;
    public static string WaveClientId;
}


public class GameManager : Singleton<GameManager>
{
    protected override void Init()
    {
    }
}