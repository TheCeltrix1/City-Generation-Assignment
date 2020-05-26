using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class Enemy : MonoBehaviour
{
    public Vector3 primaryTarget;
    public Vector3 secondaryTarget;
    private Vector3 _currentTarget;
    private NavMeshAgent _agent;
    public LayerMask mask;
    private RaycastHit _hit;
    private Vector3 _dir;

    private AudioSource _audioSource;

    private string _state;
    private float _waitTimer;
    private float _waitTime = 5;

    private float _alertTimer;
    private float _alertTime = 5;

    public float drugTimer;
    private float _drugTime = 5;
    public bool drugged = false;

    void Start()
    {
        _audioSource = this.GetComponent<AudioSource>();
        primaryTarget = this.transform.position;
        _state = "Patrol";
        _agent = this.GetComponent<NavMeshAgent>();
        _currentTarget = secondaryTarget;
        Debug.Log(primaryTarget + " " + secondaryTarget + " " + _currentTarget);
    }

    // Update is called once per frame
    void Update()
    {
        if (drugged) _state = "Drugged";
        Vision();
        State();
        _agent.SetDestination(_currentTarget);
        Debug.Log(Vector3.Distance(this.transform.position, _currentTarget));
    }

    /*private void CalculateDistance()
    {
        if (Vector3.Distance(this.transform.position, primaryTarget) <= 3)
        {
            _currentTarget = secondaryTarget;
        }
        else if (Vector3.Distance(this.transform.position, secondaryTarget) <= 3)
        {
            _currentTarget = primaryTarget;
        }
    }*/

    private void NewWaypoint()
    {
        if(Vector3.Distance(this.transform.position, _currentTarget) <= 3)
        {
            _waitTimer += Time.deltaTime;
            if (_waitTimer >= _waitTime)
            {
                if (Vector3.Distance(this.transform.position, primaryTarget) <= 3)
                {
                    _currentTarget = secondaryTarget;
                }
                else if (Vector3.Distance(this.transform.position, secondaryTarget) <= 3)
                {
                    _currentTarget = primaryTarget;
                }
                else 
                {
                    int ran = Random.Range(0,2);
                    if (ran == 1)
                    {
                        _currentTarget = secondaryTarget;
                    }
                    else
                    {
                        _currentTarget = primaryTarget;
                    }
                }
                _waitTimer = 0;
            }
        }
    }

    private void Vision()
    {
        if (!PlayerEquipment.invisible)
        {
            Collider[] col = Physics.OverlapCapsule(this.transform.position + Vector3.forward, this.transform.position + (Vector3.forward * 5), 3, mask);
            foreach (Collider obj in col)
            {
                if (obj.tag == "Player")
                {
                    _dir = (obj.transform.position - this.transform.position).normalized;
                    Physics.Raycast(this.transform.position, _dir, out _hit);
                    if (_hit.transform.tag == "Player")
                    {
                        if (_state != "Alert") _audioSource.Play();
                        _currentTarget = _hit.transform.position;
                        _state = "Alert";
                    }
                }
            }
        }
    }

    private void Seek()
    {
        Physics.Raycast(this.transform.position, _dir, out _hit);
        if (_hit.transform.tag == "Player")
        {
            _alertTimer = 0;
            _currentTarget = _hit.transform.position;
        }
        else
        {
            _alertTimer += Time.deltaTime;
        }

        if (_alertTimer >= _alertTime)
        {
            _state = "Patrol";
            _alertTimer = 0;
        }

    }

    private void SoberUp()
    {
        drugTimer += Time.deltaTime;
        if (drugTimer >= _drugTime)
        {
            drugged = false;
            _state = "Patrol";
            drugTimer = 0;
        }
    }

    private void State()
    {
        switch (_state)
        {
            case "Patrol":
                NewWaypoint();
                break;

            case "Alert":
                Seek();
                break;

            case "Drugged":
                SoberUp();
                break;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Player")
        {
            SceneManager.LoadSceneAsync(0);
        }
    }
}
