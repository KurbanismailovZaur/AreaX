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
	public struct ShotInfo
	{
        public Ray Ray { get; private set; }

        public RaycastHit Hit { get; private set; }

        public float Power { get; set; }

        public ShotInfo(Ray ray, RaycastHit hit, float power)
        {
            Ray = ray;
            Hit = hit;
            Power = power;
        }
    }
}