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
    public bool invisible;

    private RaycastHit _hit;
    private bool _grapple = false;
    private float _invisibleTimer;
    private float _invisibleCooldown;
    private bool _invisibleReady;

    void Start()
    {
        _powerUp = Random.Range(0,2);
        _body = this.GetComponentInParent<Rigidbody>();
        _fpc = this.GetComponentInParent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>();
        //random power up selection;
        _powerUp = Random.Range(0,1);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            switch (_powerUp)
                {
                case 0:
                    GrapplingHook();
                    break;

                case 1:
                    SleepDart();
                    break;

                case 2:
                    Invisible();
                    break;
                }
        }
    }

    private void FixedUpdate()
    {
        if (_grapple)
        {
            StartCoroutine("Grappling", _hit);
        }
        if (invisible)
        {
            StartCoroutine("InvisibleTimer");
        }
        if (!invisible && !_invisibleReady)
        {
            _invisibleCooldown += Time.deltaTime;
            if (_invisibleCooldown >= 10)
            {
                _invisibleCooldown = 0;
                _invisibleReady = true;
            }
        }
    }

    private void SleepDart()
    {
        if (Physics.Raycast(this.transform.position, Camera.main.transform.forward * 1000, out _hit, 50))
        {
            if (_hit.transform.gameObject.GetComponent<Enemy>())
            {
                _hit.transform.gameObject.GetComponent<Enemy>().drugged = true;
            }
        }
    }

    private void Invisible()
    {
        if (_invisibleReady) {
            _invisibleReady = false;
            invisible = true;
        }
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
        if (Vector3.Distance(this.transform.position,hit.point) <= 1f)
        {
            _fpc.grappleVelocity = Vector3.zero;
            _grapple = false;
        }
        yield return null;
    }

    IEnumerator InvisibleTimer()
    {
        _invisibleTimer += Time.deltaTime;
        if (_invisibleTimer >= 5)
        {
            _invisibleTimer = 0;
            invisible = false;
        }
        yield return null;
    }
}
