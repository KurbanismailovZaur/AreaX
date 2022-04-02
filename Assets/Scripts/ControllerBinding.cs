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
	public class ControllerBinding : MonoBehaviour
	{
		[SerializeField]
		private OVRInput.Controller _controller;

        public OVRInput.Controller Controller => _controller;

        private void Update()
        {
            var newPos = transform.parent.position + OVRInput.GetLocalControllerPosition(_controller);
            var newRot = OVRInput.GetLocalControllerRotation(_controller);

            transform.SetPositionAndRotation(newPos, newRot);
        }
    }
}