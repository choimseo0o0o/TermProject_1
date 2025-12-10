using UnityEngine;

public class HUD_FollowCamera : MonoBehaviour
{
    public Transform cam;
    public float distance = 0.6f;
    public Vector3 offset = new Vector3(0, 0.15f, 0);

    void LateUpdate()
    {
        transform.position = cam.position + cam.forward * distance + cam.TransformVector(offset);
        transform.rotation = Quaternion.LookRotation(cam.forward, cam.up);
    }
}
