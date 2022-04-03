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
using TMPro;

namespace AreaX
{
    public class RecordFixer : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI _text;

        private IEnumerator Start()
        {
            var time = Time.time;

            while (true)
            {
                _text.text = (Time.time - time).ToString("0.0");
            }
        }
    }
}