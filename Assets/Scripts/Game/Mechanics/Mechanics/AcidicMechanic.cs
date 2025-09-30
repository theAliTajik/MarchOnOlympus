
using Game;
using UnityEngine;

public class AcidicMechanic : BaseMechanic
{
    public AcidicMechanic()
    {
        
    }

    public AcidicMechanic(int stack, IHaveMechanics mOwner, int guardMin = 0)
    {
        m_stack.SetValue(stack);
        m_mechanicOwner = mOwner;

        m_stack.SetGuard(guardMin);
        DelayPlayerBurn();
    }

    private bool isPlayerBurnActive = false;
    private async void DelayPlayerBurn()
    {
        await System.Threading.Tasks.Task.Delay(100);
        isPlayerBurnActive = true;
    }
    
    public override MechanicType GetMechanicType()
    {
        return MechanicType.ACIDIC;
    }

    public override bool TryReduceStack(CombatPhase phase, bool isMyTurn, bool isFirstTimeInTurn = false)
    {
        BurnPlayer(phase, isMyTurn);
        if (phase != CombatPhase.TURN_START || !isFirstTimeInTurn)
        {
            return false;
        }

        int blockRemoveAmount = 10;
        int currentBlock = GameInfoHelper.GetMechanicStack(m_mechanicOwner, MechanicType.BLOCK);
        CustomDebug.Log($"Current block: {currentBlock}. Acid reduced {blockRemoveAmount}", Categories.Mechanics.Acidic, DebugTag.LOGIC);
        GameActionHelper.ReduceMechanicStack(m_mechanicOwner, blockRemoveAmount, MechanicType.BLOCK);
        return true;
    }

    private bool firstCardPlayed = true;
    private void BurnPlayer(CombatPhase phase, bool isMyTurn)
    {
        if(!isPlayerBurnActive) return;
        if (phase != CombatPhase.CARD_PLAYED) return;
        if(!isMyTurn) return;
        
        if (firstCardPlayed)
        {
            firstCardPlayed = false;
            return;
        }
        
        int burnAmount = 1;
        GameActionHelper.AddMechanicToOwner(m_mechanicOwner, burnAmount, MechanicType.BURN);
        
    }
}
