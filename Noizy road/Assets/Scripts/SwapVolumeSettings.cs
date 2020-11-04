using UnityEngine;
using UnityEngine.UI;

public class SwapVolumeSettings : MonoBehaviour
{
    [SerializeField] private Sprite onVolume;
    [SerializeField] private Sprite offVolume;


    private void Start()
    {
        if (PlayerPrefs.GetInt("Volume", 0) == 0)
        {
            PlayerPrefs.SetInt("Volume", 0);
            GetComponent<Image>().sprite = offVolume;
        }
        else
        {
            GetComponent<Image>().sprite = onVolume;
            PlayerPrefs.SetInt("Volume", 1);
        }
    }

    public void SwapVolumeSprite()
    {
        if (GetComponent<Image>().sprite == onVolume)
        {
            GetComponent<Image>().sprite = offVolume;
            PlayerPrefs.SetInt("Volume", 0);
        }
        else
        {
            GetComponent<Image>().sprite = onVolume;
            PlayerPrefs.SetInt("Volume", 1);
        }
    }
}
