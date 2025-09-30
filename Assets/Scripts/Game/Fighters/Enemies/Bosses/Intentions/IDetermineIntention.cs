public interface IDetermineIntention
{
    public void SetMoves(BaseEnemy.MoveData[] moves);
    
    public BaseEnemy.MoveData? DetermineIntention();
}