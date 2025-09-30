
using System;
using Game;
using Game.ModifiableParam;
using UnityEngine.XR.WSA;

public class ScyllaChaosTentacle : ScyllaTentacle
{
    private bool firstTime = true;
    private IParamModifier<int> m_energyModifier;
    public override BaseEnemy.MoveData? DetermineIntention()
    {

        if (firstTime)
        {
            firstTime = false;
            AddAliveBenifits();
        }
        return base.DetermineIntention();
    }

    private void OnDestroy()
    {
        GameplayEvents.GamePhaseChanged -= OnPhaseChange;
    }

    private void OnPhaseChange(EGamePhase phase)
    {
        if(phase != EGamePhase.CARD_PLAYED) return;
        
        bool heads = UnityEngine.Random.value < 0.5f;
        if (heads)
        {
            GameActionHelper.AddMechanicToOwner(m_mind, m_data.Move5StrOrDexGain, MechanicType.STRENGTH);
        }
        else
        {
            GameActionHelper.AddMechanicToOwner(m_mind, m_data.Move5StrOrDexGain, MechanicType.DEXTERITY);
        }
    }

    public override void OnHPDeath()
    {
        base.OnHPDeath();
        GameActionHelper.RemoveModifyEnergy(m_energyModifier);
        GameplayEvents.GamePhaseChanged -= OnPhaseChange;
    }

    public override void Revive()
    {
        base.Revive();
        AddAliveBenifits();
    }

    private void AddAliveBenifits()
    {
        m_energyModifier = new AddValueModifier<int>(m_data.Move5AliveEnergyGain);
        GameActionHelper.ModifyEnergy(m_energyModifier);

        GameplayEvents.GamePhaseChanged += OnPhaseChange;
    }
}
