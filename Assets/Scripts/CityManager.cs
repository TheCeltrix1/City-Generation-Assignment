using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MANAGER
{
    public class CityManager : MonoBehaviour
    {
        #region Variables
        public GameObject[] buildings;
        //public static GameObject[] roadsToBuild ;   create list
        public static bool[,,] floorOccupied = new bool[50, 15, 50];
        private int[,] floorGrayScale = new int[floorOccupied.GetLength(0), floorOccupied.GetLength(2)];
        private bool[,] _columnBuilt = new bool[floorOccupied.GetLength(0), floorOccupied.GetLength(2)];
        public static int size = 6;
        private static int _x;
        private static int _z;
        private static int _y;
        public GameObject roadF;
        public GameObject roadS;
        private float _testYSize;
        private static CityManager _instance;

        public GameObject endPoint;
        public static bool endPointSpawn = false;
        private Quaternion roadRotation;

        //Perlin Generation Stuff
        private int _perlinTextSizeX = 256;
        private int _perlinTextSizeY = 256;
        private int _perlinGridSizeX = floorOccupied.GetLength(0);
        private int _perlinGridSizeY = floorOccupied.GetLength(2);
        private float _noiseScale = 50;
        private Vector2 _gridOffset;
        private Texture2D _perlinTexture;
        public GameObject navMeshHolder;
        #endregion
        #region Singleton
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
        #endregion 

        void Start()
        {
            _testYSize = roadF.transform.lossyScale.y / 2;
            //generate perlin noise and ascribe it to buildings height
            PerlinNoise();
            Generate();
            navMeshHolder.GetComponent<UnityEngine.AI.NavMeshSurface>().BuildNavMesh();
        }

        #region Perlin Generation
        private void PerlinNoise()
        {
            _perlinTexture = new Texture2D(_perlinTextSizeX,_perlinTextSizeY);
            for (int x = 0; x < floorGrayScale.GetLength(0); x ++)
            {
                for (int y = 0; y < floorGrayScale.GetLength(1); y++)
                {
                    SampleNoise(x, y);
                }
            }
            _perlinTexture.Apply();
        }

        private void SampleNoise(int x, int y)
        {
            float xCoord = (float)x / _perlinTextSizeX * _noiseScale + _gridOffset.x;
            float yCoord = (float)y / _perlinTextSizeY * _noiseScale + _gridOffset.y;

            floorGrayScale[x,y] = (int)(Mathf.PerlinNoise(xCoord,yCoord)*10);
            //Debug.Log(floorGrayScale[x, y]);
        }

        private void Generate()
        {
            _x = floorOccupied.GetLength(0) / 2;
            _z = floorOccupied.GetLength(0) / 2;
            _y = floorOccupied.GetLength(0) / 2;
            for (int i = 0; i < floorOccupied.GetLength(1); i++)
            {
                for (int j = 0; j < floorOccupied.GetLength(0); j++)
                {
                    for (int o = 0; o < floorOccupied.GetLength(2); o++)
                    {
                        Occupied(j, i, o);
                    }
                }
            }
        }
        #endregion

        //xPos is position relative to centre block
        public void Occupied(int x, int y, int z)
        {
            int xPos = x - _x;
            int zPos = z - _z;
            int yPos = y + _y;

            //need a way to tell the script that the column has a building in it. New Array?
            if (!_columnBuilt[x,z]) {
                Collider[] col = Physics.OverlapBox(new Vector3((xPos * size), (y * size), (zPos * size)), new Vector3(size / 2, size / 2, size / 2));
                foreach (Collider objects in col)
                {
                    if (!floorOccupied[x, y, z] || objects.gameObject.tag != "Player")
                    {
                        floorOccupied[x, y, z] = true;
                    }
                }
                if ((x == 0 || z == 0 || x == floorOccupied.GetLength(0) - 1 || z == floorOccupied.GetLength(2) - 1) && floorGrayScale[x, z] >= 0)
                {
                    Build(xPos, yPos, zPos, x, y, z, floorOccupied.GetLength(1) - 1);
                    floorOccupied[x, y, z] = true;
                    //floorGrayScale[x, z] = -1;
                }

                else if (!floorOccupied[x, y, z])
                {
                    if (floorGrayScale[x, z] >= 3)
                    {
                        if (floorGrayScale[x, z] >= 3)
                        {
                            Build(xPos, yPos, zPos, x, 1, z, floorGrayScale[x, z]);
                        }
                        else Build(xPos, yPos, zPos, x, floorGrayScale[x, z] - 2, z, floorGrayScale[x, z]);

                        floorOccupied[x, y, z] = true;
                        //floorGrayScale[x, z] = -1;
                    }

                    else if (floorGrayScale[x, z] == 2)
                    {
                        RoadAngle(x, z);
                        floorOccupied[x, y, z] = true;
                    }

                    else if (floorGrayScale[x, z] == 1 || floorGrayScale[x, z] == 0)
                    {
                        //floorGrayScale[x, z] = -1;
                        //GameObject prefab = Instantiate(roadF, new Vector3((xPos * size), (1f - _testYSize) * (float)size, (zPos * size)), Quaternion.identity);
                        //prefab.transform.localScale = prefab.transform.localScale * size;
                        floorOccupied[x, y, z] = true;
                    }
                }
                GameObject ofwo = Instantiate(roadF, new Vector3((xPos * size), (-_testYSize) * (float)size, (zPos * size)), Quaternion.identity);
                ofwo.transform.localScale = ofwo.transform.localScale * size;

                _columnBuilt[x, z] = true;
            }
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

        public void Build(int xPos, int yPos, int zPos, int x, int y, int z, int height)
        {
            GameObject build = buildings[Random.Range(0, buildings.Length)];
            GameObject con = Instantiate(build, new Vector3((xPos * size), (y * size), (zPos * size)), (Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(0,((int)Random.Range(0,3)) * 90,0))));
            BuildingScript bs = con.GetComponent<BuildingScript>();
            bs.posX = x;
            bs.posY = y;
            bs.posZ = z;
            bs.scale = size;
            bs.maxHeight = height;
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

        private void RoadAngle(int x, int z)
        {
            //Vector3 angle = new Vector3();
            int i1 = floorGrayScale[x + 1, z];
            int i2 = floorGrayScale[x - 1, z];
            int i3 = floorGrayScale[x, z + 1];
            int i4 = floorGrayScale[x, z - 1];

            if (i1 >= 2 && i2 >= 2 && i3 >= 2 && i4 >= 2)
            {
                GameObject prefab = Instantiate(roadF, new Vector3(((x - _x) * size), (1f - _testYSize) * (float)size, ((z - _z) * size)), Quaternion.identity);
                prefab.transform.localScale = prefab.transform.localScale * size;
            }
            else if (i1 < 2 && i2 < 2 && i3 < 2 && i4 < 2)
            {
                GameObject prefab = Instantiate(roadF, new Vector3(((x - _x) * size), (-_testYSize) * (float)size, ((z - _z) * size)), Quaternion.identity);
                prefab.transform.localScale = prefab.transform.localScale * size;
            }
            else
            {
                if (i1 == 3 || i2 == 3 || i3 == 3 || i4 == 3)
                {
                    if (i1 <= 1 || i2 <= 1 || i3 <= 1 || i4 <= 1)
                    {
                        if (i1 <= 1)
                        {
                            GameObject prefab = Instantiate(roadS, new Vector3(((x - _x) * size), (1f-_testYSize) * (float)size, ((z - _z) * size)), Quaternion.Euler(new Vector3(0,0,0)));
                            prefab.transform.localScale = prefab.transform.localScale * size;
                        }
                        else if (i3 <= 1)
                        {
                            GameObject prefab = Instantiate(roadS, new Vector3(((x - _x) * size), (1f-_testYSize) * (float)size, ((z - _z) * size)), Quaternion.Euler(new Vector3(0, 270, 0)));
                            prefab.transform.localScale = prefab.transform.localScale * size;
                        }
                        else if (i2 <= 1)
                        {
                            GameObject prefab = Instantiate(roadS, new Vector3(((x - _x) * size), (1f - _testYSize) * (float)size, ((z - _z) * size)), Quaternion.Euler(new Vector3(0, -180, 0)));
                            prefab.transform.localScale = prefab.transform.localScale * size;
                        }
                        else if (i4 <= 1)
                        {
                            GameObject prefab = Instantiate(roadS, new Vector3(((x - _x) * size), (1f-_testYSize) * (float)size, ((z - _z) * size)), Quaternion.Euler(new Vector3(0, 90, 0)));
                            prefab.transform.localScale = prefab.transform.localScale * size;
                        }
                    }
                    else if (i1 == 2 || i2 == 2 || i3 == 2 || i4 == 2)
                    {
                        GameObject prefab = Instantiate(roadF, new Vector3(((x - _x) * size), (1f-_testYSize) * (float)size, ((z - _z) * size)), Quaternion.identity);
                        prefab.transform.localScale = prefab.transform.localScale * size;
                    }
                }
            }
            //floorGrayScale[x, z] = -1;
        }
    }
}