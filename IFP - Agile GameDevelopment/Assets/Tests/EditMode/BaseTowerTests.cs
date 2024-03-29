using NUnit.Framework;
using UnityEngine;

public class BaseTowerTests
{
    public class FixedUpdateMethodTests
    {
        // TODO: Add Tests for MineTower

        private BasicTower basicTower = new GameObject().AddComponent<BasicTower>();
        private SniperTower sniperTower = new GameObject().AddComponent<SniperTower>();
        private IceTower iceTower = new GameObject().AddComponent<IceTower>();

        private float[] actionTimersBasicTower = new float[100];
        private float[] actionTimersSniperTower = new float[100];
        private float[] actionTimersIceTower = new float[100];

        [SetUp]
        public void BeforeEveryTest()
        {
            // DRY Principle not apllied due to missing skills :/
            // BasicTower
            actionTimersBasicTower[0] = basicTower.Test_GetActionTimer();
            for (int i = 1; i < actionTimersBasicTower.Length; i++)
            {
                basicTower.FixedUpdate();
                actionTimersBasicTower[i] = basicTower.Test_GetActionTimer();
            }

            // SniperTower
            actionTimersSniperTower[0] = sniperTower.Test_GetActionTimer();
            for (int i = 1; i < actionTimersSniperTower.Length; i++)
            {
                sniperTower.FixedUpdate();
                actionTimersSniperTower[i] = sniperTower.Test_GetActionTimer();
            }

            // IceTower
            actionTimersIceTower[0] = iceTower.Test_GetActionTimer();
            for (int i = 1; i < actionTimersIceTower.Length; i++)
            {
                iceTower.FixedUpdate();
                actionTimersIceTower[i] = iceTower.Test_GetActionTimer();
            }
        }

        // TODO: rest of the tower
        [Test]
        public void ActionTimeUpperBound()
        {
            // BasicTower
            for (int i = 0; i < actionTimersBasicTower.Length - 1; i++)
            {
                Assert.LessOrEqual(actionTimersBasicTower[i], basicTower.Test_SecondsBetweenActions); // Upper bound ActionTimer
            }
            
            // SniperTower
            for (int i = 0; i < actionTimersSniperTower.Length - 1; i++)
            {
                Assert.LessOrEqual(actionTimersSniperTower[i], sniperTower.Test_SecondsBetweenActions); // Upper bound ActionTimer
            }

            // IceTower
            for (int i = 0; i < actionTimersIceTower.Length - 1; i++)
            {
                Assert.LessOrEqual(actionTimersIceTower[i], iceTower.Test_SecondsBetweenActions); // Upper bound ActionTimer
            }
        }

        [Test]
        public void ActionTimeSoftLowerBound()
        {
            // NOTE: purpose of this test is only to indicate bad performance
            //       assertion does not necessarily be true (not in requirements)

            // BasicTower
            for (int i = 0; i < actionTimersBasicTower.Length - 1; i++)
            {
                Assert.GreaterOrEqual(actionTimersBasicTower[i], -basicTower.Test_SecondsBetweenActions); 
            }

            // SniperTower
            for (int i = 0; i < actionTimersSniperTower.Length - 1; i++)
            {
                Assert.GreaterOrEqual(actionTimersSniperTower[i], -sniperTower.Test_SecondsBetweenActions);
            }

            // IceTower
            for (int i = 0; i < actionTimersIceTower.Length - 1; i++)
            {
                Assert.GreaterOrEqual(actionTimersIceTower[i], -iceTower.Test_SecondsBetweenActions);
            }
        }

        [Test]
        public void ActionTimeStopDecreasingWhenNotPositive()
        {
            // BasicTower
            for (int i = 0; i < actionTimersBasicTower.Length - 1; i++)
            {
                if (actionTimersBasicTower[i] <= 0.0f)
                {
                    Assert.LessOrEqual(actionTimersBasicTower[i], actionTimersBasicTower[i + 1]);
                }
            }

            // SniperTower
            for (int i = 0; i < actionTimersBasicTower.Length - 1; i++)
            {
                if (actionTimersBasicTower[i] <= 0.0f)
                {
                    Assert.LessOrEqual(actionTimersBasicTower[i], actionTimersBasicTower[i + 1]);
                }
            }
            
            // IceTower
            for (int i = 0; i < actionTimersBasicTower.Length - 1; i++)
            {
                if (actionTimersBasicTower[i] <= 0.0f)
                {
                    Assert.LessOrEqual(actionTimersBasicTower[i], actionTimersBasicTower[i + 1]);
                }
            }
        }

        [Test]
        public void ActionTimeDecreasedIfPositive()
        {
            // BasicTower
            for (int i = 0; i < actionTimersBasicTower.Length - 1; i++)
            {
                if (actionTimersBasicTower[i] > 0.0f)
                {
                    Assert.GreaterOrEqual(actionTimersBasicTower[i], actionTimersBasicTower[i + 1]);
                }
            }

            // SniperTower
            for (int i = 0; i < actionTimersSniperTower.Length - 1; i++)
            {
                if (actionTimersSniperTower[i] > 0.0f)
                {
                    Assert.GreaterOrEqual(actionTimersSniperTower[i], actionTimersSniperTower[i + 1]);
                }
            }

            // IceTower
            for (int i = 0; i < actionTimersBasicTower.Length - 1; i++)
            {
                if (actionTimersBasicTower[i] > 0.0f)
                {
                    Assert.GreaterOrEqual(actionTimersBasicTower[i], actionTimersBasicTower[i + 1]);
                }
            }
        }

        [Test]
        public void SecondsBetweenActionsNotNegative()
        {
            // BasicTower
            Assert.GreaterOrEqual(basicTower.Test_SecondsBetweenActions, 0);

            // SniperTower
            Assert.GreaterOrEqual(sniperTower.Test_SecondsBetweenActions, 0);

            // IceTower
            Assert.GreaterOrEqual(iceTower.Test_SecondsBetweenActions, 0);
        }
    }
}
