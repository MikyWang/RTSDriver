using System;
using System.Collections;
using System.Collections.Generic;
using MilkSpun.Common;
using MilkSpun.Common.MilkSpun.Scripts.Common;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

namespace MilkSpun.RTSDriver.Main
{
    [RequireComponent(typeof(Animator), typeof(Rigidbody))]
    public class PlayerController : MonoBehaviour
    {
        [Tooltip("跳跃力")] [SerializeField] private float jumpForce = 5f;
        [Tooltip("走路速度")] [SerializeField] private float walkSpeed = 1f;
        [Tooltip("跑步速度")] [SerializeField] private float runSpeed = 2f;
        [Tooltip("冲刺速度")] [SerializeField] private float sprintSpeed = 3f;

        public bool IsJump
        {
            get => _animator.GetBool(IsJump1);
            set => _animator.SetBool(IsJump1, value);
         }

        public bool IsMoving
        {
            get => _animator.GetBool(Moving);
            set => _animator.SetBool(Moving, value);
        }

        public bool IsGround => _world.CheckPositionOnGround(transform.position.x - 0.5f,
                transform.position.y - 0.1f, transform.position.z - 0.5f) ||
            _world.CheckPositionOnGround
            (transform.position.x - 0.5f,
                transform.position.y - 0.1f, transform.position.z + 0.5f) ||
            _world.CheckPositionOnGround
            (transform.position.x + 0.5f,
                transform.position.y - 0.1f, transform.position.z + 0.5f) ||
            _world.CheckPositionOnGround
            (transform.position.x + 0.5f,
                transform.position.y - 0.1f, transform.position.z - 0.5f);

        private PlayerInputSystem.PlayerActions _playerActions;
        private Rigidbody _rigidbody;
        private Animator _animator;
        private IWorld _world;
        private Vector2 _movement;

        private static readonly int IsJump1 = Animator.StringToHash("IsJump");
        private static readonly int Moving = Animator.StringToHash("Moving");
        private static readonly int Jump1 = Animator.StringToHash("Jump");

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _animator = GetComponent<Animator>();
            _world = Locator.World;
        }
        private void Start()
        {
            _playerActions = new PlayerInputSystem().Player;
            _playerActions.Enable();
            _playerActions.Jump.performed += Jump;
        }

        private void FixedUpdate()
        {
            if (IsMoving)
                Move();
        }
        private void Move()
        {
            var targetPos = transform.position +
                new Vector3(_movement.x, 0f, _movement.y) * (Time.deltaTime * walkSpeed);
            transform.LookAt(targetPos);
            _rigidbody.MovePosition(targetPos);
        }

        private void Update()
        {
            IsJump = !IsGround;
            _movement = _playerActions.Movement.ReadValue<Vector2>();
            IsMoving = IsGround && _movement.sqrMagnitude > 0.01f;

        }

        private void Jump(InputAction.CallbackContext context)
        {
            if (!IsGround) return;
            _animator.SetTrigger(Jump1);
            var force = new Vector3(_movement.x * 0.8f, 1f, _movement.y * 0.8f) * jumpForce;
            _rigidbody.AddForce(force, ForceMode.Force);
        }

    }
}
