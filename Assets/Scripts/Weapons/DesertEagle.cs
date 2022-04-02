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

namespace AreaX.Weapons
{
    public class DesertEagle : MonoBehaviour
    {
        [SerializeField]
        private ControllerBinding _binding;

        [SerializeField]
        private LineRenderer _aim;

        private Moroutine _aimMoroutine;

        [SerializeField]
        private Animator _animator;

        private bool _isShooting;

        [SerializeField]
        private AudioSource _audioSource;

        [SerializeField]
        private AudioClip _shotClip;

        [SerializeField]
        private GameObject _fireshotPrefab;

        [SerializeField]
        private Transform _bulletGenerationPoint;

        [SerializeField]
        private Rigidbody _bulletPrefab;

        private void Start() => _aimMoroutine = Moroutine.Create(this, AimEnumerable());

        private void Update()
        {
            if (OVRInput.GetDown(OVRInput.Button.PrimaryHandTrigger, _binding.Controller))
                StartAim();
            else if (OVRInput.GetUp(OVRInput.Button.PrimaryHandTrigger, _binding.Controller))
                StopAim();

            if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger, _binding.Controller))
                Shoot();
        }

        private void StartAim()
        {
            _aim.gameObject.SetActive(true);
            _aimMoroutine.Run();
        }

        private void StopAim()
        {
            _aim.gameObject.SetActive(false);
            _aimMoroutine.Stop();
        }

        private IEnumerable AimEnumerable()
        {
            while (true)
            {
                var rotation = OVRInput.GetLocalControllerRotation(_binding.Controller);
                Vector3 hitPoint;

                var hit = Physics.Raycast(_aim.transform.position, rotation * Vector3.forward, out RaycastHit hitInfo);
                hitPoint = hit ? hitInfo.point : _aim.transform.position + (rotation * Vector3.forward) * 50f;

                _aim.SetPosition(0, _aim.transform.position);
                _aim.SetPosition(1, hitPoint);

                yield return null;
            }
        }

        private void Shoot()
        {
            if (_isShooting)
                return;

            _isShooting = true;
            _animator.SetTrigger("Shoot");
            _audioSource.PlayOneShot(_shotClip);

            var fireshot = Instantiate(_fireshotPrefab, _aim.transform.position, Quaternion.identity);
            Moroutine.Run(Routines.Delay(0.1f, () => Destroy(fireshot)));


        }

        private void BulletDropEvent()
        {
            var rotation = OVRInput.GetLocalControllerRotation(_binding.Controller);

            var bullet = Instantiate(_bulletPrefab, _bulletGenerationPoint.position, rotation);
            bullet.AddForce(rotation * Vector3.up * 0.2f, ForceMode.Impulse);

            Moroutine.Run(Routines.Delay(5f, () => Destroy(bullet.gameObject)));
        }

        private void ShotCompletedEvent() => _isShooting = false;
    }
}