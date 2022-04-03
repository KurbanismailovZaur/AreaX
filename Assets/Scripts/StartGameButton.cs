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
using AreaX.InputEventSystem;
using UnityEngine.Events;

namespace AreaX
{
    public class StartGameButton : MonoBehaviour, IShotable
    {
        [SerializeField]
        private MeshRenderer _renderer;

        [SerializeField]
        private Color _aimedColor;

        [SerializeField]
        private Color _bluredColor;

        [SerializeField]
        private float _animationDuration;

        private Playable _colorPlayable;

        [SerializeField]
        private Collider _collider;

        public UnityEvent GameStarted;

        private void Start()
        {
            _renderer.material.EnableKeyword("_EMISSION");
            //_colorPlayable = 
        }

        public void AimEnter()
        {
            if (_colorPlayable != null)
                _colorPlayable.Reset();

            _colorPlayable = _renderer.material.DoColorProperty(gameObject, "_EmissionColor", _aimedColor, _animationDuration).Play();
        }

        public void AimExit()
        {
            if (_colorPlayable != null)
                _colorPlayable.Reset();

            _colorPlayable = _renderer.material.DoColorProperty(gameObject, "_EmissionColor", _bluredColor, _animationDuration).Play();
        }

        public void TakeShot(ShotInfo hitInfo)
        {
            _collider.enabled = false;
            GameStarted?.Invoke();
        }
    }
}