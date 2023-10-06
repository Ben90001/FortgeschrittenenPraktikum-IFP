using UnityEngine;
using UnityEditor;
using NUnit.Framework;

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
}