using UnityEngine;

public class EarthClearScript : MonoBehaviour
{
    private void OnCollisionExit(Collision collision)
    {
        if (collision.collider.tag != null)
        {
            if (collision.collider.tag == "Grass")
            {
                ObjectPoolScript.Despawn(gameObject);
            }
        }
    }
}
