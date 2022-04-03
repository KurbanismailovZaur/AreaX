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

namespace AreaX.InputEventSystem
{
	public interface IShotable
	{
		void AimEnter();

		void AimExit();

		void TakeShot(ShotInfo shotInfo);
	}
}