using NUnit.Framework;
using UnityEngine;

public class HUDTests
{
    [Test]
    public void ShowGameOverScreen_ActivatesDefeatScreen()
    {
    
        GameObject hudObject = new GameObject("HUD");
        HUD hud = hudObject.AddComponent<HUD>();

        GameObject defeatScreen = new GameObject("DefeatScreen");
        hud.DefeatScreen = defeatScreen;

        hud.ShowGameOverScreen(0, 0);
        Assert.IsTrue(defeatScreen.activeSelf, "Das DefeatScreen wurde nicht aktiviert.");
    }

    [Test]
    public void ShowGameOverScreen_SetsTimeScaleToZero()
    {
        
        GameObject hudObject = new GameObject("HUD");
        HUD hud = hudObject.AddComponent<HUD>();

        hud.ShowGameOverScreen(0, 0);

        Assert.AreEqual(0f, Time.timeScale, "Die TimeScale wurde nicht auf Null gesetzt.");
    }
}