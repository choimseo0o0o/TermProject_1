using UnityEngine;

public class WeaponFreeAim : MonoBehaviour
{
    public float sensitivity = 3f;   // 마우스 감도
    public float maxAngle = 20f;     // 카메라 기준 최대 조준 각도

    float yaw;   // 좌우
    float pitch; // 상하

    void Update()
    {
        float dx = Input.GetAxis("Mouse X");
        float dy = Input.GetAxis("Mouse Y");

        yaw   += dx * sensitivity;
        pitch -= dy * sensitivity;

        yaw   = Mathf.Clamp(yaw,   -maxAngle, maxAngle);
        pitch = Mathf.Clamp(pitch, -maxAngle, maxAngle);

        transform.localRotation = Quaternion.Euler(pitch, yaw, 0f);
    }
}
