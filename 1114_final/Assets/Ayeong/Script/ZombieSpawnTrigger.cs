using UnityEngine;

public class ZombieSpawnTrigger : MonoBehaviour
{
    [Header("Player ����")]
    public string playerTag = "Player";   // �÷��̾� ������Ʈ�� Tag

    [Header("�����ų ����� (5����)")]
    public GameObject[] zombies;          // �̸� ��Ƶ� ���� �ƹ�Ÿ�� (��Ȱ��ȭ ���� ����)

    [Header("����� �ִϸ��̼� ���� �̸�")]
    public string animationStateName = "ZombieWalk";   // Animator�� �ִ� ���� �̸�

    private bool triggered = false;


    private void OnTriggerEnter(Collider other)
    {
        //if (triggered) return;                       // �� ���� ����
        //if (!other.CompareTag(playerTag)) return;    // Player�� �ƴ� �� ����

        triggered = true;

        foreach (GameObject zombie in zombies)
        {
            if (zombie == null) continue;

            // ��Ȱ��ȭ ���¶�� ���� �ѱ�
            if (!zombie.activeSelf)
                zombie.SetActive(true);

            // Animator ã�Ƽ� �ִϸ��̼� ���
            Animator anim = zombie.GetComponent<Animator>();
            if (anim != null && !string.IsNullOrEmpty(animationStateName))
            {
                anim.Play(animationStateName, 0, 0f);
            }
        }
    }
}
