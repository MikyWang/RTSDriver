using System;
using System.Collections;
using System.Collections.Generic;
using MilkSpun.Common;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace MilkSpun.RTSDriver.Main
{
    public class Player : MonoBehaviour
    {
        [Title("碰撞器")] [SerializeField] private float radius = 0.5f;
        [Title("刚体")] [SerializeField] private float gravity = -9.8f;
        [SerializeField] private float jumpForce = 5f;

        [ShowInInspector]
        public bool IsGround => _velocity.y == 0;

        private Vector3 _velocity;
        private IWorld _world;

        public float DownSpeed => gravity * Time.deltaTime;
        public Vector3 TopLeftPos => transform.position + new Vector3(-radius, DownSpeed, radius);
        public Vector3 TopRightPos => transform.position + new Vector3(radius, DownSpeed, radius);

        public Vector3 BottomLeftPos =>
            transform.position + new Vector3(-radius, DownSpeed, -radius);

        public Vector3 BottomRightPos =>
            transform.position + new Vector3(radius, DownSpeed, -radius);


        private void Start()
        {
            _world = GameObject.FindWithTag("World").GetComponent<IWorld>();
            _velocity = Vector3.zero;
        }

        private void Update()
        {
            CheckDownSpeed();
            transform.Translate(_velocity, Space.World);
        }

        private void CheckDownSpeed()
        {
            _velocity.y = DownSpeed;

            if (_world.CheckPositionInWorld(TopLeftPos) ||
                _world.CheckPositionInWorld(TopRightPos) ||
                _world.CheckPositionInWorld(BottomLeftPos) ||
                _world.CheckPositionInWorld(BottomRightPos))
            {
                _velocity.y = 0;
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(TopLeftPos, TopRightPos);
            Gizmos.DrawLine(TopLeftPos, BottomLeftPos);
            Gizmos.DrawLine(BottomLeftPos, BottomRightPos);
            Gizmos.DrawLine(BottomRightPos, TopRightPos);

        }

    }
}
