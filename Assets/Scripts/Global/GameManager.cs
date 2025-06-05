using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameSessionParams
{
    public static string enemyClientId;
    public static string deckTemplateClientId;
    public static string EventId;
}


public class GameManager : Singleton<GameManager>
{
    protected override void Init()
    {
    }
}