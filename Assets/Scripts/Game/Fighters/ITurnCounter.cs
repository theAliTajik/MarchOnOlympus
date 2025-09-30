public interface ITurnCounter
{
    public int GetGameTurn();
    public int GetRelativeTurn();

    public void NextTurn();
}