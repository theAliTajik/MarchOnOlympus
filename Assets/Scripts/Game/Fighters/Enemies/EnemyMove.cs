using System.Collections;
using System.Collections.Generic;
using Game;
using UnityEngine;

[System.Serializable]
public abstract class EnemyActionBase
{
    public int damage;
}

[System.Serializable]
public class AttackAction : EnemyActionBase
{
    public int Damage;
}

[System.Serializable]
public class MultiAttackAction : EnemyActionBase
{
    public int Damage;
    public int NumOfAttacks;
}

[System.Serializable]
public class BuffAction : EnemyActionBase
{
    public MechanicType BuffType;
    public int BuffStack;
}

[System.Serializable]
public class CastDebuffAction : EnemyActionBase
{
    public MechanicType DebuffType;
    public int DebuffStack;
}

[System.Serializable]
public class EscapeAction: EnemyActionBase { }

[System.Serializable]
public class SleepAction: EnemyActionBase { }

[System.Serializable]
public class DontAction : EnemyActionBase { }


[System.Serializable]
public class Move
{
    public bool DetermineNextMove = false;
    public int NextMovePointer = -1;
    public List<EnemyActionBase> Actions = new List<EnemyActionBase>();

}