using UnityEngine;
using UnityEngine.UI;

public class CurrencyManager : MonoBehaviour
{
   
  
    public Text moneyText;
    public int money = 70;

    void Start()
    {
        UpdateMoneyText();
    }

    public void AddMoney(int amount)
    {
        money += amount;
        UpdateMoneyText();
    }

    public void SpendMoney(int amount)
    {
        if (money >= amount)
        {
            money -= amount;
            UpdateMoneyText();
        }
        else
        {
            // Geld nicht ausreichend
            Debug.Log("Nicht genügend Geld!");
        }
    }

    void UpdateMoneyText()
    {
        moneyText.text = "Geld: " + money;
    }
}
