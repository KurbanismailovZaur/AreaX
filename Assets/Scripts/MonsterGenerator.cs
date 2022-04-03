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
using AreaX.Monsters;

namespace AreaX
{
    public class MonsterGenerator : MonoBehaviour
    {
        [SerializeField]
        private Player _player;

        [SerializeField]
        [Range(1f, 30f)]
        private float _interval = 1f;

        [SerializeField]
        private Transform[] _spawnPoints;

        [SerializeField]
        private Goblin _goblinPrefab;

        [SerializeField]
        private float _goblinSpeed = 1f;

        private void Start()
        {
            //StartSpawnMonsters();
            //var goblin = Instantiate(_goblinPrefab, _spawnPoints.GetRandomElement().position, Quaternion.identity);
            //goblin.Player = _player;
            //goblin.WalkSpeed = _goblinSpeed;
        }

        public void StartSpawnMonsters()
        {
            Moroutine.Run(this, IntervalDecreasingEnumerable());
            Moroutine.Run(this, SpawnMonstersEnumerable());
            Moroutine.Run(this, SpeedIncreasingEnumerable());
        }

        private IEnumerable IntervalDecreasingEnumerable()
        {
            for (int i = 0; i < 3; i++)
            {
                yield return new WaitForSeconds(30f);
                _interval /= 2f;
            }
        }

        private IEnumerable SpeedIncreasingEnumerable()
        {
            while (true)
            {
                yield return new WaitForSeconds(10f);
                _goblinSpeed = Mathf.Min(_goblinSpeed + 1f, 8f);
            }
        }

        private IEnumerable SpawnMonstersEnumerable()
        {
            while (true)
            {
                var goblin = Instantiate(_goblinPrefab, _spawnPoints.GetRandomElement().position, Quaternion.identity);
                goblin.Player = _player;
                goblin.WalkSpeed = _goblinSpeed;

                yield return new WaitForSeconds(_interval);
            }
        }
    }
}