using UnityEngine;
using TMPro;

public class PlayerCollider : MonoBehaviour
{
    public TextMeshProUGUI Notice_Dead;
    public GameObject RestartButton;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Zombie"))
        {
            Notice_Dead.gameObject.SetActive(true);
            RestartButton.SetActive(true);
            Debug.Log("Player has collided with a Zombie!");
            other.gameObject.SetActive(false);
        }
    }
}
