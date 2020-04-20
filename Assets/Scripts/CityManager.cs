using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MANAGER
{
    public class CityManager : MonoBehaviour
    {
        public GameObject[] Buildings;
        public bool[,] floorOccupied = new bool[50, 50];
        private static int _size = 3;
        private static int _x;
        private static int _z;

        // Start is called before the first frame update
        void Start()
        {
            _x = floorOccupied.GetLength(0) / 2;
            _z = floorOccupied.GetLength(0) / 2;
            for (int i = 0; i < floorOccupied.GetLength(0); i++)
            {
                for (int j = 0; j < floorOccupied.GetLength(1); j++)
                {
                    Occupied(i, j);
                }
            }
        }

        public void Occupied(int x, int z)
        {
            int xPos = x - _x;
            int zPos = z - _z;

            Collider[] col = Physics.OverlapBox(new Vector3((xPos * _size) + _size / 2, (_size) + 1f, (zPos * _size) + _size / 2), new Vector3(_size, _size, _size));
            foreach (Collider objects in col)
            {
                if (!floorOccupied[x, z])
                {
                    floorOccupied[x, z] = true;
                }
            }
            if ( x == 0 || z == 0 || x == floorOccupied.GetLength(0)-1 || z == floorOccupied.GetLength(1)-1 )
            {
                Build(xPos, zPos);
            }
            else if (!floorOccupied[x,z])
            {
                if (Random.Range(0,4) == 0)
                {
                    Build(xPos, zPos);
                }
            }
            //Debug.Log(floorOccupied[x,z] +" " + xPos + " " + zPos);
        }

        private void OnDrawGizmos()
        {
            /*for (int i = 0; i < floorOccupied.GetLength(0); i++)
            {
                for (int j = 0; j < floorOccupied.GetLength(1); j++)
                {
                    int xPos = i - _x;
                    int zPos = j - _z;
                    Gizmos.DrawCube(new Vector3((xPos * _size) + _size / 2, (_size / 2) + .1f, (zPos * _size) + _size / 2), new Vector3(_size, _size, _size));
                }
            }*/
        }

        public void Build(int xPos, int zPos)
        {
            GameObject build = Buildings[Random.Range(0, Buildings.Length)];
            var con = Instantiate(build, new Vector3((xPos * _size) + _size / 2, 0, (zPos * _size) + _size / 2), Quaternion.Euler(0, 0, 0));
            con.GetComponent<BuildingScript>().scale = _size;
            con.GetComponent<BuildingScript>().Build();
        }
    }
}