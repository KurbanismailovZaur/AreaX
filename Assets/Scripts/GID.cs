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
using Redcode.Paths;
using AreaX.Weapons;

namespace AreaX
{
    public class GID : MonoBehaviour
    {
        [SerializeField]
        private DesertEagle _leftEagle;

        [SerializeField]
        private Player _player;

        [SerializeField]
        private TextMeshPro _mireaText;

        [SerializeField]
        private Transform _startGameTransform;

        [SerializeField]
        private AudioSource _monsterAudioPrefab;

        [SerializeField]
        private AudioSource _audioSource;

        [SerializeField]
        private Scene _scene;

        [SerializeField]
        private MonsterGenerator _monsterGenerator;

        [Header("Audio Clips")]
        [SerializeField]
        private AudioClip _rulesTalking;

        public void StartGame() => Moroutine.Run(this, StartGameEnumerable());

        private IEnumerable StartGameEnumerable()
        {
            _leftEagle.gameObject.SetActive(true);

            var sequence = new Sequence(Ease.OutBounce);
            sequence.Append(_mireaText.DoColorA(0f, 0.5f));
            sequence.Append(_startGameTransform.DoLocalScale(0f, 1f, Ease.OutBounce));
            yield return sequence.Play().WaitForComplete();

            _audioSource.PlayOneShot(_rulesTalking);

            void DoMonstersSounds(float delay)
            {
                Moroutine.Run(Routines.Delay(delay, () =>
                {
                    var point1 = _player.transform.TransformPoint(0f, 0f, -2f);
                    var point2 = _player.transform.TransformPoint(-2f, 0f, -2f);
                    var point3 = _player.transform.TransformPoint(2f, 0f, -2f);

                    var mAudioPath = Path.Create(Vector3.zero, true, point1, point2, point3);
                    var mAudio = Instantiate(_monsterAudioPrefab);

                    mAudio.transform.DoMoveAlongPath(mAudioPath, 5f).Play().OnCompleted(() =>
                    {
                        Destroy(mAudioPath.gameObject);
                        Destroy(mAudio.gameObject);
                    });
                }));
            }

            DoMonstersSounds(3f);
            DoMonstersSounds(15f);

            var collectingMor = _scene.StartCollecting();

            yield return new WaitForSeconds(_rulesTalking.length);
            yield return collectingMor.WaitForComplete();

            _monsterGenerator.StartSpawnMonsters();
        }

        public void ResetAllData()
        {

        }
    }
}