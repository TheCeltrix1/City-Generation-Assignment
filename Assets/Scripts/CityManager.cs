using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MANAGER
{
    public class CityManager : MonoBehaviour
    {
        public GameObject[] Buildings;
        public static bool[,,] floorOccupied = new bool[50, 5, 50];
        public static int size = 3;
        private static int _x;
        private static int _z;
        private static int _y;
        public GameObject test;
        private static CityManager _instance;

        void OnEnable()
        {
            if (_instance == null)
            {
                _instance = this;
            }

            else
            {
                Destroy(gameObject);
            };
        }

        public static CityManager Instance
        {
            get
            {
                return _instance;
            }
        }

        // Start is called before the first frame update
        void Start()
        {
            _x = floorOccupied.GetLength(0) / 2;
            _z = floorOccupied.GetLength(0) / 2;
            _y = floorOccupied.GetLength(0) / 2;
            for (int i = 0; i < floorOccupied.GetLength(0); i++)
            {
                for (int j = 0; j < floorOccupied.GetLength(1); j++)
                {
                    for (int o = 0; o < floorOccupied.GetLength(2); o++)
                    {
                        Occupied(i, j, o);
                    }
                }
            }
            test.transform.position = new Vector3(size / 2f, size / 2f, size / 2f);
            test.GetComponent<RouteTile>().Initialize(0,0,0);
        }

        //xPos is position relative to centre block
        //x is boolean value
        public void Occupied(int x, int y, int z)
        {
            int xPos = x - _x;
            int zPos = z - _z;
            int yPos = y + _y;

            Collider[] col = Physics.OverlapBox(new Vector3((xPos * size) + size / 2f, (size) + 1f, (zPos * size) + size / 2f), new Vector3(size, size, size));
            foreach (Collider objects in col)
            {
                if (!floorOccupied[x, y, z] && objects.gameObject.tag != "Player")
                {
                    floorOccupied[x, y, z] = true;
                }
            }
            if ((x == 0 || z == 0 || x == floorOccupied.GetLength(0) - 1 || z == floorOccupied.GetLength(2) - 1) && y == 0)
            {
                Build(xPos, yPos, zPos, x, y, z);
                floorOccupied[x, y, z] = true;
            }
            else if (!floorOccupied[x, y, z])
            {
                if (Random.Range(0, 10) == 0)
                {
                    Build(xPos, yPos, zPos, x, y, z);
                    floorOccupied[x, y, z] = true;
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

        public void Build(int xPos, int yPos, int zPos, int x, int y, int z)
        {
            GameObject build = Buildings[Random.Range(0, Buildings.Length)];
            GameObject con = Instantiate(build, new Vector3((xPos * size) + size / 2f, (y * size), (zPos * size) + size / 2f), Quaternion.Euler(0, 0, 0));
            BuildingScript bs = con.GetComponent<BuildingScript>();
            bs.posX = x;
            bs.posY = y;
            bs.posZ = z;
            bs.scale = size;
            bs.Build();
        }

        public static bool CheckTile(int x, int y, int z)
        {
            if (x >= 0 && y >= 0 && z >= 0 && x < floorOccupied.GetLength(0) && y < floorOccupied.GetLength(1) && z < floorOccupied.GetLength(2)) {
                return floorOccupied[x, y, z];
            }
            else
            {
                return true;
            } 
        }
    }
}