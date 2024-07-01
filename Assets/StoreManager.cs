using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class StoreManager : MonoBehaviour
{
    public TMP_Text coinTxt;
    public TMP_Text powerupsTxt;

    public Button singlePowerup;
    public Button multiplePowerup;

    bool canBuymore;
    private void Start()
    {
        coinTxt.text = GameController.instance.coinCount.ToString();
        powerupsTxt.text = GameController.instance.powerupCount.ToString();

        if (GameController.instance.coinCount < 900)
        {
            multiplePowerup.interactable = false;
        }

        if (GameController.instance.coinCount < 300)
        {
            singlePowerup.interactable = false;
        }
    }

    public void BuySinglePowerup()
    {
        if (GameController.instance.coinCount >= 300)
        {
            GameController.instance.powerupCount += 1;
            powerupsTxt.text = GameController.instance.powerupCount.ToString();

            GameController.instance.coinCount -= 300;
            coinTxt.text = GameController.instance.coinCount.ToString();

            PlayerPrefs.SetInt("Powerups", GameController.instance.powerupCount);
        }

        if (GameController.instance.coinCount < 300)
        {
            singlePowerup.interactable = false;
        }
    }

    public void BuyMultiplePowerup()
    {
        if (GameController.instance.coinCount >= 900)
        {
            GameController.instance.powerupCount += 4;
            powerupsTxt.text = GameController.instance.powerupCount.ToString();

            GameController.instance.coinCount -= 900;
            coinTxt.text = GameController.instance.coinCount.ToString();

            PlayerPrefs.SetInt("Powerups", GameController.instance.powerupCount);
        }

        if (GameController.instance.coinCount < 900)
        {
            multiplePowerup.interactable = false;
        }
    }
}
