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
using UnityEngine.AI;
using AreaX.InputEventSystem;

namespace AreaX.Monsters
{
    public class Goblin : MonoBehaviour, IShotable
    {
        [SerializeField]
        private Animator _animator;

        [SerializeField]
        private NavMeshAgent _agent;

        [SerializeField]
        [Range(1f, 10f)]
        private float _walkSpeed = 1f;

        public float WalkSpeed
        {
            get => _walkSpeed;
            set
            {
                _walkSpeed = value;
                _agent.speed = value;

                _animator.SetBool("Run", _walkSpeed > 5f);
            }
        }

        public Player Player { get; set; }

        private int _damage = 10;

        [Header("Audio Clips")]
        [SerializeField]
        private AudioSource _audioSource;

        [SerializeField]
        private AudioClip[] _footStepClips;

        [SerializeField]
        private AudioClip[] _voicesClips;

        [SerializeField]
        private AudioClip _attackMissClip;

        [SerializeField]
        private AudioClip _attackClip;

        private int _health = 100;

        private Moroutine _destinationMor;

        private Moroutine _attackMor;

        private Moroutine _voiceMor;

        [SerializeField]
        private Transform _bloodParticlesPrefab;

        private void Start()
        {
            _agent.speed = _walkSpeed;

            _destinationMor = Moroutine.Run(this, UpdateDestinationEnumerable());
            _attackMor = Moroutine.Run(this, AttackEnumerable());
            _voiceMor = Moroutine.Run(this, VoiceEnumerable());
        }

        private IEnumerable UpdateDestinationEnumerable()
        {
            while (true)
            {
                yield return new WaitForSeconds(0.25f);
                _agent.destination = Player.transform.position;
            }
        }

        private IEnumerable AttackEnumerable()
        {
            while (true)
            {
                yield return new WaitForSeconds(0.1f);

                if (Vector3.Distance(transform.position, Player.transform.position.WithY(0f)) >= 1.5f)
                    continue;

                _animator.SetTrigger("Attack");
            }
        }

        private IEnumerable VoiceEnumerable()
        {
            while (true)
            {
                yield return new WaitForSeconds(UnityEngine.Random.Range(5f, 20f));
                _audioSource.PlayOneShot(_voicesClips.GetRandomElement());
            }
        }

        private void LeftHandAttack() => HandAttack();

        private void RightHandAttack() => HandAttack();

        private void HandAttack()
        {
            if (Vector3.Distance(transform.position, Player.transform.position.WithY(0f)) >= 3f)
            {
                _audioSource.PlayOneShot(_attackMissClip);
                return;
            }

            Player.TakeDamage(_damage);
            _audioSource.PlayOneShot(_attackClip);
        }

        private void Footstep() => _audioSource.PlayOneShot(_footStepClips.GetRandomElement());

        public void AimEnter() { }

        public void AimExit() { }

        public void TakeShot(ShotInfo shotInfo)
        {
            var bloodParticles = Instantiate(_bloodParticlesPrefab, shotInfo.Hit.point, Quaternion.LookRotation(shotInfo.Hit.normal));
            Moroutine.Run(Routines.Delay(1f, () => Destroy(bloodParticles.gameObject)));

            _health = (int)Mathf.Max(_health - shotInfo.Power, 0f);

            if (_destinationMor.IsRunning)
            {
                _destinationMor.Stop();
                _attackMor.Stop();
            }

            _agent.isStopped = true;

            if (_health != 0)
            {
                _animator.SetTrigger("Get Hit");

                Moroutine.Run(this, Routines.Delay(0.8f, () =>
                {
                    if (_animator.GetBool("Dead"))
                        return;

                    var targetPos = transform.position - transform.forward * 1.33f;
                    transform.DoPosition(targetPos, 0.33f).Play().OnCompleted(() =>
                    {
                        _destinationMor.Run();
                        _attackMor.Run();

                        _agent.isStopped = false;
                    });
                }));
            }
            else
                Die();
        }

        private void Die()
        {
            if (_animator.GetBool("Dead"))
                return;

            _animator.SetBool("Dead", true);
            Moroutine.Run(this, HardFixProblemEnumerable());

            Moroutine.Run(Routines.Delay(5f, () =>
            {
                transform.DoPositionY(-1f, 1f).Play().OnCompleted(() => Destroy(gameObject));
            }));

            foreach (var collider in GetComponentsInChildren<Collider>())
                collider.enabled = false;
        }

        private IEnumerable HardFixProblemEnumerable()
        {
            var time = Time.time;

            while (true)
            {
                _agent.isStopped = true;
                yield return null;
                
                if (Time.time - time >= 3f)
                    yield break;
            }
        }
    }
}