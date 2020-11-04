using UnityEngine;
using UnityEngine.UI;

public class CoinCollector : MonoBehaviour
{
    public Text coinsAmount;

    private void Start()
    {
        coinsAmount.text = PlayerPrefs.GetInt("Coins", 0).ToString();
    }
}
