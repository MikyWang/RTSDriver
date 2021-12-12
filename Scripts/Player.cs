using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MilkSpun.RTSDriver.Main
{
    public class Player : MonoBehaviour
    {
        private float _horizontal;
        private float _vertical;
        private Vector3 _velocity;

        private void Update()
        {
            GetPlayerInput(); 
            
        }

        private void GetPlayerInput()
        {
            _horizontal = Input.GetAxis("Horizontal");
            _vertical = Input.GetAxis("Vertical");
        }

    }
}
