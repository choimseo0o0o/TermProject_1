using UnityEngine;

public class Bullet : MonoBehaviour
{
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Zombie"))
        {
            ZombieController z = collision.gameObject.GetComponentInParent<ZombieController>();
            if (z != null)
                z.Die();
        }

        Destroy(gameObject);
    }
}
