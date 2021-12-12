using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace MilkSpun.RTSDriver.Main
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private float speed = 1f;
        [SerializeField] private float dampTime = 0.01f;
        [SerializeField] private float minSize = 1f;
        [SerializeField] private float maxSize = 10f;
        [SerializeField] private float zoomSpeed = 1f;

        private float _horizontalAxis;
        private float _verticalAxis;
        private float _zoomAxis;
        private Vector3 _horizontalPos;
        private Vector3 _verticalPos;
        private Vector3 _moveVelocity;
        private float _zoomVelocity;
        private Vector3 _desirePosition;
        private Camera _camera;

        private void Awake()
        {
            _camera = GetComponentInChildren<Camera>();
        }

        private void Start()
        {
            InitControl();
        }

        private void Update()
        {
            GetInput();
        }
        private void FixedUpdate()
        {
            Moving();
            Zoom();
        }

        private void InitControl()
        {
            var transform1 = transform;
            _horizontalPos = transform1.right;
            _verticalPos = transform1.up;
        }

        private void GetInput()
        {
            _horizontalAxis = Input.GetAxisRaw("Horizontal");
            _verticalAxis = Input.GetAxisRaw("Vertical");
            _zoomAxis = Input.GetAxisRaw("Mouse ScrollWheel");
        }

        private void Moving()
        {
            var prePos = transform.position;
            var xPos = _horizontalPos * _horizontalAxis * speed;
            var zPos = _verticalPos * _verticalAxis * speed;
            _desirePosition = prePos + xPos + zPos;
            _desirePosition.y = prePos.y;

            transform.position = Vector3.SmoothDamp(prePos, _desirePosition, ref _moveVelocity,
                dampTime);
        }

        private void Zoom()
        {
            var size = _camera.orthographicSize + (zoomSpeed * _zoomAxis);
            size = Mathf.Max(size, minSize);
            size = MathF.Min(size, maxSize);
            _camera.orthographicSize = Mathf.SmoothDamp(_camera.orthographicSize, size, ref
                _zoomVelocity, dampTime);

        }
    }
}
