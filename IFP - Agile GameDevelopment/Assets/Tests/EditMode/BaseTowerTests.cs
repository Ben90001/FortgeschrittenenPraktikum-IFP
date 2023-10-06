using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class BaseTowerTests
{
    public class FixedUpdateMethodTests
    {
        //TODO: Add Tests for MineTower

        private BasicTower BasicTower = new GameObject().AddComponent<BasicTower>();
        private SniperTower SniperTower = new GameObject().AddComponent<SniperTower>();
        private IceTower IceTower = new GameObject().AddComponent<IceTower>();
        
        float[] ActionTimersBasciTower = new float[100];
        float[] ActionTimersSniperTower = new float[100];
        float[] ActionTimersIceTower = new float[100];


        [SetUp]
        public void BeforeEveryTest()
        {
            //DRY Principle not apllied due to missing skills :/
            //BasicTower
            ActionTimersBasciTower[0] = BasicTower.GetActionTimer();
            for (int i = 1; i < ActionTimersBasciTower.Length; i++)
            {
                BasicTower.FixedUpdate();
                ActionTimersBasciTower[i] = BasicTower.GetActionTimer();
            }
            //SniperTower
            ActionTimersSniperTower[0] = SniperTower.GetActionTimer();
            for (int i = 1; i < ActionTimersSniperTower.Length; i++)
            {
                SniperTower.FixedUpdate();
                ActionTimersSniperTower[i] = SniperTower.GetActionTimer();
            }
            //IceTower
            ActionTimersIceTower[0] = IceTower.GetActionTimer();
            for (int i = 1; i < ActionTimersIceTower.Length; i++)
            {
                IceTower.FixedUpdate();
                ActionTimersIceTower[i] = IceTower.GetActionTimer();
            }

        }

        //TODO: rest of the tower
        [Test]
        public void ActionTimeUpperBound()
        {
            //BasicTower
            for (int i = 0; i < ActionTimersBasciTower.Length - 1; i++)
            {
                Assert.LessOrEqual(ActionTimersBasciTower[i], BasicTower.SecondsBetweenActions); //upper bound ActionTimer
            }
            //SniperTower
            for (int i = 0; i < ActionTimersSniperTower.Length - 1; i++)
            {
                Assert.LessOrEqual(ActionTimersSniperTower[i], SniperTower.SecondsBetweenActions); //upper bound ActionTimer
            }
            //IceTower
            for (int i = 0; i < ActionTimersIceTower.Length - 1; i++)
            {
                Assert.LessOrEqual(ActionTimersIceTower[i], IceTower.SecondsBetweenActions); //upper bound ActionTimer
            }
        }

        [Test]
        public void ActionTimeSoftLowerBound()
        {
            //NOTE: purpose of this test is only to indicate bad performance
            //      assertion does not necessarily be true (not in requirements)

            //BasicTower
            for (int i = 0; i < ActionTimersBasciTower.Length - 1; i++)
            {
                Assert.GreaterOrEqual(ActionTimersBasciTower[i], -BasicTower.SecondsBetweenActions); 
            }
            //SniperTower
            for (int i = 0; i < ActionTimersSniperTower.Length - 1; i++)
            {
                Assert.GreaterOrEqual(ActionTimersSniperTower[i], -SniperTower.SecondsBetweenActions);
            }
            //IceTower
            for (int i = 0; i < ActionTimersIceTower.Length - 1; i++)
            {
                Assert.GreaterOrEqual(ActionTimersIceTower[i], -IceTower.SecondsBetweenActions);
            }
        }

        [Test]
        public void ActionTimeStopDecreasingWhenNotPositive()
        {
            //BasicTower
            for (int i = 0; i < ActionTimersBasciTower.Length - 1; i++)
            {
                if (ActionTimersBasciTower[i] <= 0.0f)
                {
                    Assert.LessOrEqual(ActionTimersBasciTower[i], ActionTimersBasciTower[i + 1]);
                }
            }
            //SniperTower
            for (int i = 0; i < ActionTimersBasciTower.Length - 1; i++)
            {
                if (ActionTimersBasciTower[i] <= 0.0f)
                {
                    Assert.LessOrEqual(ActionTimersBasciTower[i], ActionTimersBasciTower[i + 1]);
                }
            }
            //IceTower
            for (int i = 0; i < ActionTimersBasciTower.Length - 1; i++)
            {
                if (ActionTimersBasciTower[i] <= 0.0f)
                {
                    Assert.LessOrEqual(ActionTimersBasciTower[i], ActionTimersBasciTower[i + 1]);
                }
            }
        }

        [Test]
        public void ActionTimeDecreasedIfPositive()
        {
            //BasicTower
            for (int i = 0; i < ActionTimersBasciTower.Length - 1; i++)
            {
                if (ActionTimersBasciTower[i] > 0.0f)
                {
                    Assert.GreaterOrEqual(ActionTimersBasciTower[i], ActionTimersBasciTower[i + 1]);
                }
            }
            //SniperTower
            for (int i = 0; i < ActionTimersSniperTower.Length - 1; i++)
            {
                if (ActionTimersSniperTower[i] > 0.0f)
                {
                    Assert.GreaterOrEqual(ActionTimersSniperTower[i], ActionTimersSniperTower[i + 1]);
                }
            }
            //IceTower
            for (int i = 0; i < ActionTimersBasciTower.Length - 1; i++)
            {
                if (ActionTimersBasciTower[i] > 0.0f)
                {
                    Assert.GreaterOrEqual(ActionTimersBasciTower[i], ActionTimersBasciTower[i + 1]);
                }
            }
        }

        [Test]
        public void SecondsBetweenActionsNotNegative()
        {
            //BasicTower
            Assert.GreaterOrEqual(BasicTower.SecondsBetweenActions, 0);
            //SniperTower
            Assert.GreaterOrEqual(SniperTower.SecondsBetweenActions, 0);
            //IceTower
            Assert.GreaterOrEqual(IceTower.SecondsBetweenActions, 0);
        }
    }
}
