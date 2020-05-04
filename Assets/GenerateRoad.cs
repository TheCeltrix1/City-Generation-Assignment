using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateRoad : MonoBehaviour
{
    public int posX;
    public int posY;
    public int posZ;
    public Mesh[] meshes = new Mesh[3];
    public int tileType;
    public GameObject prefab;
    private Vector3 _newPos;
    private float _size;
    private static int recursion = 180;
    private static bool first = true;
    private bool[] _availableSpaces = new bool[3];
    private MANAGER.CityManager _city;

    public void Begin()
    {
        _city = MANAGER.CityManager.Instance;
        if (recursion <= 0)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _size = MANAGER.CityManager.size;
            switch (tileType)
            {
                case 0:
                    //meshstuff
                    break;

                case 1:
                    //meshstuff
                    break;

                case 2:
                    //meshstuff
                    break;

                default:
                    posX = MANAGER.CityManager.floorOccupied.GetLength(0) / 2;
                    posY = 0;
                    posZ = MANAGER.CityManager.floorOccupied.GetLength(2) / 2;
                    float xPos = posX - MANAGER.CityManager.floorOccupied.GetLength(0) / 2;
                    float zPos = posZ - MANAGER.CityManager.floorOccupied.GetLength(2) / 2;
                    this.transform.position = new Vector3((xPos * _size) + _size / 2, (posY * _size), (zPos * _size) + _size / 2);
                    break;
            }
            //Debug.Log(posX + "  " + posY + "  " + posZ);
            CheckNewTile();
        }
    }

    private void CheckNewTile()
    {
        /*if (first)
        {*/
            for (int j = 0; j < 4; j++)
            {
                /*switch (j)
                {
                    case 0:
                        DirectionCheck(1, 0);
                        break;

                    case 1:
                        DirectionCheck(0, 1);
                        break;

                    case 2:
                        DirectionCheck(-1, 0);
                        break;

                    case 3:
                        DirectionCheck(0, -1);
                        break;
                }
            }
       /*}
        else {*/
                int i = Random.Range(0, 4);
                switch (j)
                {
                    case 0:
                        DirectionCheck(1, 0);
                        break;

                    case 1:
                        DirectionCheck(0, 1);
                        break;

                    case 2:
                        DirectionCheck(-1, 0);
                        break;

                    case 3:
                        DirectionCheck(0, -1);
                        break;
                }
            }
        //Destroy(this);
    }

    private void SpawnNewTile(int i, int y, int z)
    {
        recursion--;
        GameObject newTile = Instantiate(prefab, this.transform.position + (new Vector3(i, y, z) * _size),this.transform.rotation);
        MANAGER.CityManager.floorOccupied[i,y,z] = true;
        GenerateRoad road = newTile.GetComponent<GenerateRoad>();
        road.tileType = 0;
        road.posX = posX + i;
        road.posY = posY + y;
        road.posZ = posZ + z;
        road.Begin();
    }

    private void DirectionCheck(int x, int z)
    {
        for (int i = -1; i < 2; i ++) {
           _availableSpaces[i+1] = MANAGER.CityManager.CheckTile(posX + i, posY + i, posZ + z);
            //Debug.Log(_availableSpaces[0]+ " " + _availableSpaces[1] + " " + _availableSpaces[2]);
        }
        if (!_availableSpaces[0] && !_availableSpaces[1] && !_availableSpaces[2])
        {
            SpawnNewTile(x,(int)Random.Range(-1,2),z);
        }
        else if (!_availableSpaces[0] && !_availableSpaces[1])
        {
            SpawnNewTile(x, (int)Random.Range(-1, 1), z);
        }
        else if (!_availableSpaces[0] && !_availableSpaces[2])
        {
            int ran = (int)Random.Range(0, 2);
            if (ran == 0) ran = -1;
            SpawnNewTile(x, ran, z);
        }
        else if (!_availableSpaces[1] && !_availableSpaces[2])
        {
            SpawnNewTile(x, (int)Random.Range(0, 2), z);
        }
        else if (!_availableSpaces[0])
        {
            SpawnNewTile(x, -1, z);
        }
        else if (!_availableSpaces[1])
        {
            SpawnNewTile(x, 0, z);
        }
        else if (!_availableSpaces[2])
        {
            SpawnNewTile(x, 1, z);
        }
        /*else
        {
            int redo = Random.Range(0,1);
            if (redo == 0)
            {
                CheckNewTile();
            }
        }*/
    }
}
