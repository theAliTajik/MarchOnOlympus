using Game;

public class HauntMechanic : BaseMechanic
{
    public HauntMechanic()
    {
        
    }

    public HauntMechanic(int stack, IHaveMechanics mOwner, int guardMin = 0)
    {
        m_stack.SetValue(stack);
        m_mechanicOwner = mOwner;
        this.OnChange += ChangeToPanic;
        ChangeToPanic();
        
        m_stack.SetGuard(guardMin);
    }

    private void ChangeToPanic(MechanicType obj = MechanicType.HAUNT)
    {
        if (m_stack >= 10)
        {
            Fighter target = GameInfoHelper.GetPlayer();
			GameActionHelper.AddMechanicToFighter(target, 1, MechanicType.PANIC);
            ReduceStack(10);
        }
    }

    public override MechanicType GetMechanicType()
    {
        return MechanicType.HAUNT;
    }

    public override bool TryReduceStack(CombatPhase phase, bool isMyTurn, bool isFirstTimeInTurn = false)
    {

        return false;
    }
}
