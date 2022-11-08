using System;
using System.Numerics;
using UnityEngine;
using UnityEngine.Serialization;
using Quaternion = UnityEngine.Quaternion;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

namespace Assets.Scripts.A.I_test
{
    public class FollowCameraScript : MonoBehaviour
    {
        public Transform target;
        public Vector3 offset;
        public float rotateSpeed = 0.1f;
        public float zoomSpeed = 2f;
        public float currentZoom = 0.5f;
        private Vector3 _maximumZoom;
        private Vector3 _minimumZoom;
        public Vector2 turn;
        
        // Start is called before the first frame update
        private void Start()
        {
            _maximumZoom = new Vector3(offset.x, offset.y, 2.3f * offset.z);
            _minimumZoom = new Vector3(offset.x, offset.y, 0.3f * offset.z);
            // offset = target.transform.position - transform.position;
        }

        private void Update()
        {
            var rightClickHold = Input.GetMouseButton(1);
            if (!rightClickHold)
            {
                Cursor.lockState = CursorLockMode.None;
                return;
            }
            turn.x += Input.GetAxis("Mouse X");
            turn.y += Input.GetAxis("Mouse Y");
            transform.localRotation = Quaternion.Euler(-turn.y, turn.x, 0);
            Cursor.lockState = CursorLockMode.Locked;
        }

        private void LateUpdate()
        {
            ZoomControl();
        }

        // Update is called once per frame
        private void FixedUpdate()
        {
            if (!target) {
                return;
            }
            SmoothFollow();
        }

        private void SmoothFollow()
        {
            var desiredPosition = target.position + offset;
            var smoothSpeed = rotateSpeed * Time.deltaTime;
            var smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
            transform.position = smoothedPosition;
            transform.LookAt(target);
        }

        private void ZoomControl()
        {
            var mouseWheel = Input.mouseScrollDelta.y;
            if (mouseWheel == 0) return;
            var smoothZoom = rotateSpeed * Time.deltaTime;
            IncrementZoomLerp(mouseWheel);
        }

        private void IncrementZoomLerp(float amount)
        {
            currentZoom += zoomSpeed * amount * Time.deltaTime;
            currentZoom = currentZoom switch
            {
                > 1 => 1,
                < 0 => 0,
                _ => currentZoom
            };
            var newOffset = Vector3.Lerp(_maximumZoom, _minimumZoom, currentZoom);
            offset.z = newOffset.z;
        }
    }
}
