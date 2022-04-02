using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    [SerializeField]
    private Transform _centerEye;

    private Vector3 _initialPos;

    [SerializeField]
    private Transform _leftHand;

    [SerializeField]
    private Transform _rightHand;

    [SerializeField]
    private Rigidbody _bulletPrefab;

    [SerializeField]
    [Range(1f, 100f)]
    private float _bulletImpulse = 1f;

    private IEnumerator Start()
    {
        _initialPos = _centerEye.position;

        yield return new WaitForSeconds(1f);
        var controllers = OVRInput.GetConnectedControllers();
        print(controllers);
    }

    private void UpdateController(Transform controller, OVRInput.Controller type)
    {
        controller.position = _initialPos + OVRInput.GetLocalControllerPosition(type);
        controller.rotation = OVRInput.GetLocalControllerRotation(type);
    }

    private void UpdateControllers()
    {
        UpdateController(_leftHand, OVRInput.Controller.LTouch);
        UpdateController(_rightHand, OVRInput.Controller.RTouch);
    }

    private void Update()
    {
        UpdateControllers();

        if (OVRInput.GetDown(OVRInput.Button.One, OVRInput.Controller.RTouch))
        {
            var bullet = Instantiate(_bulletPrefab, _rightHand.position, Quaternion.identity);
            bullet.AddForce(OVRInput.GetLocalControllerRotation(OVRInput.Controller.RHand) * Vector3.forward * _bulletImpulse, ForceMode.Impulse);
        }
    }
}
