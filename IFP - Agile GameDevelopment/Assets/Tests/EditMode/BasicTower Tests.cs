using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class BasicTowerTests
{
    public class PerformActionMethodTests
    {
        private BasicTower BasicTower = new GameObject().AddComponent<BasicTower>();
        
        


        [SetUp]
        public void BeforeEveryTest()
        {
        }

        [Test]
        public void PerformActionTrueIfEnemieNotNull()
        {
            Enemy Enemy = null;

        }
    }
}
