using UnityEngine;

public class DeathScript : MonoBehaviour
{
    [SerializeField] private AudioSource source;
    [SerializeField] private AudioClip deathSound;
    [SerializeField] private float volumeScale;

    private void OnEnable()
    {
        source.PlayOneShot(deathSound, volumeScale);
    }
}
