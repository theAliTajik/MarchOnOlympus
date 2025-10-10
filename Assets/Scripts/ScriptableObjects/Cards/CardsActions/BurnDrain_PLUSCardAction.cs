using System.Collections;
using Game;
using Action = System.Action;

public class BurnDrain_PLUSCardAction : BaseCardAction
{
    private BurnDrain_PLUSCard m_data;
    
    public override void Play(BaseCardData cardData, Action finishCallback, Fighter target, CardDisplay cardDisplay)
    {
        StartCoroutine(WaitAndExecute(finishCallback, 2f,cardData, target, cardDisplay));
    }

    private IEnumerator WaitAndExecute(Action finishCallback, float delay, BaseCardData cardData, Fighter target, CardDisplay cardDisplay)
    {
        m_data = (BurnDrain_PLUSCard)cardData;
        var player = GameInfoHelper.GetPlayer();

        int burn = GameActionHelper.RemoveMechanicOfType(player, MechanicType.BURN);

        int bleed = m_data.ExtraBleed + burn;
        
        GameActionHelper.AddMechanicToFighter(player, bleed, MechanicType.BLEED);
        
        
        if (CombatManager.Instance.CurrentStance == cardData.MStance)
        {
            
        }
        
        finishCallback?.Invoke();
        yield break;
    }

}