using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FakeCity : MonoBehaviour
{

    private Transform[] kiddies;

    // Start is called before the first frame update
    void Start()
    {
        int kid = 0;
        while (kid < this.transform.childCount)
        {
            this.transform.GetChild(kid).GetComponent<BuildingScript>().Build();
            //kiddies[0] = this.transform.GetChild(kid);
            kid++;
        }
    }
}
