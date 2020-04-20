using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using UnityStandardAssets.Utility;

public class PlayerEquipment : MonoBehaviour
{
    private int _powerUp;

    private Rigidbody _body;
    private UnityStandardAssets.Characters.FirstPerson.FirstPersonController _fpc;

    //PowerUps
    private RaycastHit _hit;
    private bool _grapple = false;

    void Start()
    {
        _powerUp = Random.Range(0,2);
        _body = this.GetComponentInParent<Rigidbody>();
        _fpc = this.GetComponentInParent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            GrapplingHook();
        }
    }

    private void FixedUpdate()
    {
        if (_grapple)
        {
            StartCoroutine("Grappling", _hit);
        }
    }

    private void SleepDart()
    {

    }

    private void GrapplingHook()
    {
        if (Physics.Raycast(this.transform.position, Camera.main.transform.forward * 1000, out _hit, 50))
        {
            _grapple = true;
            Vector3 vel = Vector3.Normalize(_hit.point - this.transform.position);
            _fpc.grappleVelocity = vel * 25;
        }
    }

    IEnumerator Grappling(RaycastHit hit)
    {
        Debug.Log(_body.velocity);
        if (Vector3.Distance(this.transform.position,hit.point) <= 1f)
        {
            _fpc.grappleVelocity = Vector3.zero;
            _grapple = false;
        }
        yield return null;
    }
}
