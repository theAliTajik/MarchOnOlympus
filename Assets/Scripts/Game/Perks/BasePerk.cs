using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game;

public interface IPhaseOrdered
{
    public EGamePhase[] GetPhases();
    public float GetPriority();

    public void OnPhaseActivate(EGamePhase phase, Action callback);
}

public abstract class BasePerk : MonoBehaviour, IPhaseOrdered
{
    public virtual void Config(BasePerkData perkData){}
    
    public virtual void OnAdd(){}
    
    public virtual void OnRemove(){}

    public abstract EGamePhase[] GetPhases();

    public abstract float GetPriority();

    public abstract void OnPhaseActivate(EGamePhase phase, Action callback);

    public virtual void RemoveSelf()
    {
        PerksManager.Instance.RemovePerk(this);
    }
}
