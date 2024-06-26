using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StoreManager : MonoBehaviour
{
    public TMP_Text coinTxt;
    public TMP_Text powerupsTxt;

    bool canBuymore;
    private void Start()
    {
        coinTxt.text = PlayerPrefs.GetInt("Coins", 0).ToString();
        powerupsTxt.text = PlayerPrefs.GetInt("PowerUps", 0).ToString();

    }
    public void BuySwapPowerUps(int count)
    {
        if (PlayerPrefs.GetInt("Coins", 0) >= count)
        {
            PlayerPrefs.SetInt("Coins", (PlayerPrefs.GetInt("Coins", 0) - (int)count));
            coinTxt.text = PlayerPrefs.GetInt("Coins", 0).ToString();
            if(canBuymore)
            {
                PlayerPrefs.SetInt("PowerUps", PlayerPrefs.GetInt("PowerUps", 0) + 3);

            }
            else
            {
                PlayerPrefs.SetInt("PowerUps", PlayerPrefs.GetInt("PowerUps", 0) + 1);

            }
            powerupsTxt.text = PlayerPrefs.GetInt("PowerUps", 0).ToString();



        }
    }

    public void CanBuyMore(bool val)
    {
        canBuymore = val;
    }
}
