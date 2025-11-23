using UnityEngine;

public class ZombieController : MonoBehaviour
{
    Animator anim;
    bool dead = false;

    void Awake()
    {
        anim = GetComponent<Animator>();
    }

    public void Die()
    {
        if (dead) return;

        dead = true;

        anim.SetBool("isDead", true);

        // 이동/추격 중단
        ZombieProximityTrigger ai = GetComponent<ZombieProximityTrigger>();
        if (ai != null)
            ai.enabled = false;

        Destroy(gameObject, 5f);
    }
}
