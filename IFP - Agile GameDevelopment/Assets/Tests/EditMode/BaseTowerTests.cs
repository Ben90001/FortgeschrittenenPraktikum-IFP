using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class BaseTowerTests
{
    public class FixedUpdateMethodTests
    {
        private BasicTower BasicTower = new GameObject().AddComponent<BasicTower>();
        private SniperTower SniperTower = new GameObject().AddComponent<SniperTower>();
        private IceTower IceTower = new GameObject().AddComponent<IceTower>();
        

        float[] ActionTimers = new float[100];

        [SetUp]
        public void BeforeEveryTest()
        {
            ActionTimers[0] = BasicTower.GetActionTimer();
            for (int i = 1; i < ActionTimers.Length; i++)
            {
                BasicTower.FixedUpdate();
                ActionTimers[i] = BasicTower.GetActionTimer();
            }
        }

        //TODO: rest of the tower
        [Test]
        public void ActionTimeUpperBound()
        {
            for (int i = 0; i < ActionTimers.Length - 1; i++)
            {
                Assert.LessOrEqual(ActionTimers[i], BasicTower.SecondsBetweenActions); //upper bound ActionTimer
            }
        }

        [Test]
        public void ActionTimeSoftLowerBound()
        {
            //NOTE: purpose of this test is only to indicate bad performance
            //      assertion does not necessarily be true (not in requirements)
            for (int i = 0; i < ActionTimers.Length - 1; i++)
            {
                Assert.GreaterOrEqual(ActionTimers[i], -BasicTower.SecondsBetweenActions); 
            }
        }

        [Test]
        public void ActionTimeStopDecreasingWhenNotPositive()
        {
            for (int i = 0; i < ActionTimers.Length - 1; i++)
            {
                if (ActionTimers[i] <= 0.0f)
                {
                    Assert.LessOrEqual(ActionTimers[i], ActionTimers[i + 1]);
                }
            }
        }

        [Test]
        public void ActionTimeDecreasedIfPositive()
        {
            for (int i = 0; i < ActionTimers.Length - 1; i++)
            {
                if (ActionTimers[i] > 0.0f)
                {
                    Assert.GreaterOrEqual(ActionTimers[i], ActionTimers[i + 1]);
                }

            } //TODO: rest of the tower
        }
    }
}
