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
        [SerializeField] private float height = 2f;
        [Title("刚体")] [SerializeField] private float gravity = -9.8f;
        [SerializeField] private float jumpForce = 5f;

        [ShowInInspector]
        public bool IsGround => _velocity.y == 0;



        private Vector3 _velocity;
        private IWorld _world;

        public float DownSpeed => gravity * Time.deltaTime;
        public Vector3 TopLeftPos => transform.position + new Vector3(-radius, 0f, radius);
        public Vector3 TopRightPos => transform.position + new Vector3(radius, 0f, radius);

        public Vector3 BottomLeftPos =>
            transform.position + new Vector3(-radius, 0f, -radius);

        public Vector3 BottomRightPos =>
            transform.position + new Vector3(radius, 0f, -radius);


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

            if (_world.CheckPositionOnGround(TopLeftPos.x, TopLeftPos.y + DownSpeed, TopLeftPos.z) ||
                _world.CheckPositionOnGround(TopRightPos.x, TopRightPos.y + DownSpeed,
                    TopRightPos.z) ||
                _world.CheckPositionOnGround(BottomLeftPos.x, BottomLeftPos.y + DownSpeed,
                    BottomLeftPos.z) ||
                _world.CheckPositionOnGround(BottomRightPos.x, BottomRightPos.y + DownSpeed,
                    BottomRightPos.z))
            {
                _velocity.y = 0;
            }
        }

        private void CheckUpSpeed() { }

        private void OnDrawGizmos()
        {
            var heightPos = Vector3.up * height;
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(TopLeftPos, TopRightPos);
            Gizmos.DrawLine(TopLeftPos, BottomLeftPos);
            Gizmos.DrawLine(BottomLeftPos, BottomRightPos);
            Gizmos.DrawLine(BottomRightPos, TopRightPos);
            Gizmos.DrawLine(TopLeftPos + heightPos, TopRightPos + heightPos);
            Gizmos.DrawLine(TopLeftPos + heightPos, BottomLeftPos + heightPos);
            Gizmos.DrawLine(BottomLeftPos + heightPos, BottomRightPos + heightPos);
            Gizmos.DrawLine(BottomRightPos + heightPos, TopRightPos + heightPos);
            Gizmos.DrawLine(TopLeftPos, TopLeftPos + heightPos);
            Gizmos.DrawLine(TopRightPos, TopRightPos + heightPos);
            Gizmos.DrawLine(BottomLeftPos, BottomLeftPos + heightPos);
            Gizmos.DrawLine(BottomRightPos, BottomRightPos + heightPos);

        }

    }
}
