using UnityEngine;
using static PublicControllerValue;

namespace SimpleFPS
{
    public class Controller : MonoBehaviour
    {
        public Transform rightController; // 오른손 컨트롤러 Transform
        public float rayLength = 100f; // Ray 길이
        public float rotationSpeed = 100f; // 회전 속도
        public float scaleSpeed = 1f; // 크기 변화 속도
        private Transform hitObject;            // 현재 Ray가 맞은 오브젝트
        private LineRenderer lineRenderer;
        private Renderer objectRenderer;

        // === 잡기 관련 ===
        private bool isGrabbing = false;
        private Transform grabbedObject;
        private Vector3 grabOffset;
        float IndexTimer;

        void Start()
        {

            // 라인렌더러 초기화
            lineRenderer = gameObject.AddComponent<LineRenderer>();
            lineRenderer.startWidth = 0.01f;
            lineRenderer.endWidth = 0.005f;
            lineRenderer.material = new Material(Shader.Find("Unlit/Color"));
            lineRenderer.material.color = Color.red;
            lineRenderer.positionCount = 2;
        }

        void Update()
        {
            if (rightController == null) return;

            GunTimer += Time.deltaTime;
            IndexTimer += Time.deltaTime;

            // === Raycast 처리 ===
            Ray ray = new Ray(rightController.position, rightController.forward);
            RaycastHit hit;
            Vector3 endPos = rightController.position + rightController.forward * rayLength;

            // if (Physics.Raycast(ray, out hit, rayLength, LayerMask.GetMask("Continue")))
            // {
            //     Point_Continue = true;
            //     IsOkayToPressContB = true;
            // }

            // if (Physics.Raycast(ray, out hit, rayLength, LayerMask.GetMask("Zombie")))
            // {
            // }

            // === 버튼 입력 처리 ===

            // A 버튼
            if (OVRInput.GetDown(OVRInput.Button.One) && Point_Continue)
                RightContA = true;


            // B 버튼 누름
            if (OVRInput.GetDown(OVRInput.Button.Two) && IsOkayToPressContB)
                RightContB = true;

            if(OVRInput.GetDown(OVRInput.Button.Four))
                RightThumbClick = !RightThumbClick;

            // 썸스틱: 이동 감지
            Vector2 thumbstick = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, OVRInput.Controller.LTouch);
            if (thumbstick.magnitude > 0.1f)
            {
                PlayerIsMoving = true;
                // Debug.Log($"Thumbstick Input: {thumbstick}");
            }
            else
                PlayerIsMoving = false;


            // 그랩 버튼 
            float rightGrip = OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger, OVRInput.Controller.RTouch);

            if (rightGrip > 0.6f && GunTimer > 0.5f && !RightContTrig)
            {
                RightContTrig = true;
                GunTimer = 0f;
                FireMotionOn = true;
            }

            if (GunTimer > 0.2f && GunTimer < 0.5f)
                FireMotionOff = true;


            // 트리거: 크기 조절
            // if (hitObject != null)
            // HandleTriggerScaling(hitObject);

            // 라인렌더러 업데이트
            lineRenderer.SetPosition(0, rightController.position);
            lineRenderer.SetPosition(1, endPos);
        }

        void OnDrawGizmosSelected() // Ray 시각화
        {
            if (rightController != null)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawRay(rightController.position, rightController.forward * rayLength);
            }
        }
    }
}
