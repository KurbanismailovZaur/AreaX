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
using UnityEngine.UI;

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

        private int _health { get; set; } = 100;

        private readonly float _healthImageWidth = 1600f;

        [SerializeField]
        private Image _healthImage;

        [SerializeField]
        private AudioSource _audioSource;

        [SerializeField]
        private AudioClip[] _painClips;

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

        public void TakeDamage(int damage)
        {
            _health = (int)MathF.Max(_health - damage, 0);
            _healthImage.fillAmount = _health / 100f;

            _audioSource.PlayOneShot(_painClips.GetRandomElement());
        }
    }
}