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
	public class HealthFader : MonoBehaviour
	{
        [SerializeField]
        private Transform _head;

        [SerializeField]
        private CanvasGroup _group;

        private void Update()
        {
            _group.alpha = Mathf.Clamp01(Vector3.Dot(_head.forward, transform.forward));
        }
    }
}