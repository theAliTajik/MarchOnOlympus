
using System.Collections.Generic;
using NUnit.Framework;

public static class IntentionDeterminationAssertsHelper
{
    public static void DetermineIntention_DoesNotRepeatSameMove_MoreThanTwiceInARow(List<BaseEnemy.MoveData?> m_results)
    {
        // ASSERT: Verify the anti-repetition logic.
        const int Threshold = 3;
        int repeats = 0;
        int biggestRepeatStreak = 0;
        string moveThatWasRepeated = null;
        BaseEnemy.MoveData? previousMove = null;

        foreach (var moveData in m_results)
        {
            if (previousMove.HasValue && moveData.HasValue && previousMove.Value.clientID == moveData.Value.clientID)
            {
                moveThatWasRepeated = moveData.Value.clientID;
                repeats++;
            }
            else
            {
                repeats = 0; // Start counting the new move.
            }

            if (repeats > biggestRepeatStreak)
            {
                biggestRepeatStreak = repeats;
            }
                
            previousMove = moveData;
        }

        // The assertion is that the streak should not be 3 or more.
        Assert.That(biggestRepeatStreak, Is.LessThan(Threshold), $"the move: {moveThatWasRepeated} was repeated {biggestRepeatStreak} times in a row.");
    }
}
