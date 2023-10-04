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
        //add all other inheriting towers and test them as well

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
        
        [Test]
        public void ActionTimerTests()
        {
            for (int i = 0; i < ActionTimers.Length - 1; i++)
            {
                if (ActionTimers[i] <= 0)
                {
                    Assert.GreaterOrEqual(ActionTimers[i-1], 0);
                    Assert.LessOrEqual(ActionTimers[i], ActionTimers[i + 1]);
                    Assert.AreEqual(ActionTimers[i + 1], BasicTower.SecondsBetweenActions);
                }
                else
                {
                    Assert.GreaterOrEqual(ActionTimers[i], ActionTimers[i + 1]);
                }
            }
            //TODO: rest of the tower
        }
    }


}
