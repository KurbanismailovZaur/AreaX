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
    public class Scene : MonoBehaviour
    {
        [SerializeField]
        private Material _gridMat;

        [SerializeField]
        private Material _transparentGridMat;

        [SerializeField]
        private Renderer[] _renderers;

        [SerializeField]
        private float _fromHeight;

        [SerializeField]
        private float _duration;

        [SerializeField]
        private float _alphaDuration;

        [SerializeField]
        private float _delay;

        public Moroutine StartCollecting() => Moroutine.Run(this, StartCollectingEnumerable());

        private IEnumerator StartCollectingEnumerable()
        {
            foreach (var renderer in _renderers)
            {
                var rend = renderer;
                rend.material = _transparentGridMat;

                var sequence = new Sequence(gameObject, Ease.OutCirc);
                sequence.AppendCallback(() => rend.gameObject.SetActive(true));
                sequence.Append(Tween.Float(gameObject, _fromHeight, rend.transform.position.y, rend.transform.SetPositionY, _duration, Ease.OutCirc));
                sequence.Insert(0f, rend.material.DoColorA(gameObject, 1f, _alphaDuration).OnPhaseCompleted(() => rend.material = _gridMat));
                sequence.Play();

                yield return new WaitForSeconds(_delay);
            }
        }
    }
}