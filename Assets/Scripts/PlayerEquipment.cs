using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.CrossPlatformInput;
using UnityStandardAssets.Utility;
using UnityEngine.SceneManagement;

public class PlayerEquipment : MonoBehaviour
{
    #region Variables
    private int _powerUp;

    private Rigidbody _body;
    private UnityStandardAssets.Characters.FirstPerson.FirstPersonController _fpc;
    private GameObject _cannie;

    //PowerUps
    public static bool invisible;

    private RaycastHit _hit;
    private bool _grapple = false;
    private float _invisibleTimer;
    private float _invisibleTime = 10;
    private float _invisibleDuration = 5;
    private float _invisibleCooldown;
    private Slider _invisibleCanvasValue;

    private int _ammo = 6;
    private Text _ammoCount;
    private bool _invisibleReady;
    private Mesh _endMesh;
    #endregion

    void Start()
    {
        _cannie = GameObject.Find("Canvas");
        _powerUp = Random.Range(0,3);
        _body = this.GetComponent<Rigidbody>();
        _fpc = this.GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>();
        //random power up selection;
        //_powerUp = Random.Range(1,2);
        _endMesh = /*MANAGER.CityManager.Instance.endPoint*/ null;
        switch (_powerUp)
        {
            case 0:
                _cannie.transform.GetChild(1).gameObject.SetActive(true);
                _cannie.transform.GetChild(2).gameObject.SetActive(false);
                _cannie.transform.GetChild(3).gameObject.SetActive(false);
                break;

            case 1:
                _cannie.transform.GetChild(1).gameObject.SetActive(false);
                _cannie.transform.GetChild(2).gameObject.SetActive(true);
                _cannie.transform.GetChild(3).gameObject.SetActive(false);
                _ammoCount = _cannie.transform.GetChild(2).GetChild(0).GetComponent<Text>();
                _ammoCount.text = "Ammo: " + _ammo;
                break;

            case 2:
                _cannie.transform.GetChild(1).gameObject.SetActive(false);
                _cannie.transform.GetChild(2).gameObject.SetActive(false);
                _cannie.transform.GetChild(3).gameObject.SetActive(true);
                _invisibleCanvasValue = _cannie.transform.GetChild(3).GetChild(0).GetComponent<Slider>();
                _invisibleCanvasValue.maxValue = _invisibleTime;
                break;
        }
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
        if (_grapple && _powerUp == 0)
        {
            StartCoroutine("Grappling", _hit);
        }
        if (invisible && _powerUp == 2)
        {
            StartCoroutine("InvisibleTimer");
        }
        else if (!invisible && !_invisibleReady && _powerUp == 2)
        {
            //Debug.Log("awlifnpiowanopiw");
            _invisibleCooldown += Time.deltaTime;
            _invisibleCanvasValue.value = _invisibleCooldown;
            if (_invisibleCooldown >= _invisibleTime)
            {
                _invisibleCooldown = 0;
                _invisibleReady = true;
            }
        }
    }

    private void SleepDart()
    {
        if (_ammo > 0)
        {
            _ammo--;
            _ammoCount.text = "Ammo: " + _ammo;
            if (Physics.Raycast(this.transform.position, Camera.main.transform.forward * 1000, out _hit, 50))
            {
                if (_hit.transform.gameObject.GetComponent<Enemy>())
                {
                    _hit.transform.gameObject.GetComponent<Enemy>().drugged = true;
                    _hit.transform.gameObject.GetComponent<Enemy>().drugTimer = 0;
                }
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

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<MeshFilter>()) {
            if (collision.gameObject.tag == "EndPoint")
            {
                Cursor.visible = true;
                SceneManager.LoadSceneAsync(0);
            }
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
        _invisibleCanvasValue.value = _invisibleCanvasValue.maxValue - (_invisibleTimer * (_invisibleTime / _invisibleDuration));
        if (_invisibleTimer >= _invisibleDuration)
        {
            _invisibleTimer = 0;
            invisible = false;
        }
        yield return null;
    }
}
