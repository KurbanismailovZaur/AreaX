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
	public class SunSwitcher : MonoBehaviour
	{
		[SerializeField]
		private Light _light;

        [SerializeField]
        private float _cycleDuration;

        private void Start()
        {
            var sequence = new Sequence(gameObject);
            sequence.Append(_light.DoColorTemperature(1500f, _cycleDuration / 2f));
            sequence.Append(Tween.Float(1500f, 20000f, t => _light.colorTemperature = t, _cycleDuration));
            sequence.Append(Tween.Float(20000f, 6500f, t => _light.colorTemperature = t, _cycleDuration / 2f));
            sequence.Repeat();
        }
    }
}