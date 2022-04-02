using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Redcode.Extensions;
using Redcode.Moroutines;
using Redcode.Moroutines.Extensions;
using Redcode.Tweens;
using Redcode.Tweens.Extensions;

namespace AreaX
{
	public class Player : MonoBehaviour
	{
        [SerializeField]
        private Transform _camera;

        [SerializeField]
        private CharacterController _controller;

        [SerializeField]
        [Range(1f, 10f)]
        private float _moveSpeed = 1f;

        private void Update()
        {
            HandleMove(OVRInput.Controller.LTouch);
            HandleMove(OVRInput.Controller.RTouch);
        }

        private void HandleMove(OVRInput.Controller controller)
        {
            var moveVector = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, controller);
            
            _controller.SimpleMove(_camera.TransformDirection(moveVector.InsertY(0f)) * _moveSpeed);
        }
    }
}