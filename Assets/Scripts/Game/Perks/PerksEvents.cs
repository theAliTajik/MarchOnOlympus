using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerksEvents
{
    public static event System.Action TurnEnded;
    public static void SendTurnEnded()
    {
        TurnEnded?.Invoke();
    }
}
