using UnityEngine;
using System.Collections;

public class ZombieAnimation : MonoBehaviour
{
    private Animator animator;

    [Header("Death Settings")]
    public float transitionDuration = 0.2f;         // CrossFade 전환 시간
    public float destroyDelay = 5f;                 // 파괴까지 대기 시간

    private bool isDead = false;                    // 중복 실행 방지

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void PlayDeath()
    {
        if (isDead) return;
        isDead = true;

        animator.CrossFade("ZombieDying", transitionDuration, 0, 0f);

        StartCoroutine(DestroyAfterDelay());
    }

    private IEnumerator DestroyAfterDelay()
    {
        yield return new WaitForSeconds(destroyDelay);
        Destroy(gameObject);
    }
}