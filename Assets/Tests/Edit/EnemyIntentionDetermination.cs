using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Enemy.Intention
{
    [TestFixture]
    public class ConditionalRandomDeterminationTests
    {
        private ConditionalRandomIntentionDeterminer m_determiner;
        private List<BaseEnemy.MoveData?> m_results;
        private const int SIMULATION_RUNS = 150;

        [SetUp]
        public void Setup()
        {
            // ARRANGE: Create the list of all possible moves.
            var movesList = new List<BaseEnemy.MoveData>();

            // Moves that should be included
            for (int i = 0; i < 3; i++)
            {
                var move = new BaseEnemy.MoveData("Move " + i) { chance = 10, probabilities = new[] { 0.5f, 0 }, Condition = () => true};
                movesList.Add(move);
            }

            // Moves that should be excluded by the condition
            for (int i = 0; i < 3; i++)
            {
                var move = new BaseEnemy.MoveData("No " + i) { chance = 10, probabilities = new[] { 0.5f, 0 }, Condition = () => false};
                movesList.Add(move);
            }

            // Moves with no condition (should be included)
            for (int i = 0; i < 3; i++)
            {
                var move = new BaseEnemy.MoveData("Null " + i) { chance = 10, probabilities = new[] { 0.5f, 0 } };
                movesList.Add(move);
            }

            m_determiner = new ConditionalRandomIntentionDeterminer(movesList);

            // ACT: Run the determination process. This is shared because all tests assert against its results.
            m_results = new List<BaseEnemy.MoveData?>();
            for (int i = 0; i < SIMULATION_RUNS; i++)
            {
                m_results.Add(m_determiner.DetermineIntention());
            }
        }

        [Test]
        public void DetermineIntention_NeverSelectsMoves_WhenConditionIsFalse()
        {
            // ASSERT: Verify that moves with a `false` condition are never chosen.
            Assert.That(m_results.All(m => m.HasValue && !m.Value.clientID.StartsWith("No")), Is.True,
                "A move with a false condition was incorrectly selected.");
        }

        [Test]
        public void DetermineIntention_CanSelectMoves_WhenConditionIsNull()
        {
            // ASSERT: Verify that moves with a `null` condition can be chosen.
            Assert.That(m_results.Any(m => m.HasValue && m.Value.clientID.StartsWith("Null")), Is.True,
                "A move with a null condition was never selected.");
        }
        
        [Test]
        public void DetermineIntention_AlwaysReturnsAValidMove()
        {
            // ASSERT: Verify that the determiner never fails to select a move (given valid inputs).
            Assert.That(m_results, Has.None.Null);
        }

        [Test]
        public void DetermineIntention_DoesNotRepeatSameMove_MoreThanTwiceInARow()
        {
            IntentionDeterminationAssertsHelper.DetermineIntention_DoesNotRepeatSameMove_MoreThanTwiceInARow(m_results);
        }
    }
}