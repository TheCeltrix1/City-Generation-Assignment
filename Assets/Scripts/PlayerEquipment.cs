using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEquipment : MonoBehaviour
{
    private int _powerUp;

    private Rigidbody _body;

    //PowerUps
    private RaycastHit hit;

    void Start()
    {
        _powerUp = Random.Range(0,2);
        _body = this.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void SleepDart()
    {

    }

    private void GrapplingHook()
    {
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, 50))
        {
        }
    }
}
