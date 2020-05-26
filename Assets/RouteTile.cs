using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MANAGER
{
    public class RouteTile : MonoBehaviour
    {

        #region Fields

        public static int recursionCount = 0;
        private int mySlope; //is current tile a slope up (1), a downslope (-1) or a flat segment
        private int myRotation;
        private int level;
        public Transform tilePrefab;
        public Mesh slopeMesh;
        public Mesh flatMesh;
        private Transform child;
        private Vector3[] _vertices;

        private int maxLevel = 100;
        private MANAGER.CityManager _cityManager;

        private int inc_x, inc_y, inc_z;


        #endregion

        #region Properties	
        #endregion

        #region Methods

        public void Initialize(int slope, int rotationDeg)
        {
            mySlope = slope;
            myRotation = rotationDeg;
            Run();
        }

        public void SetIncrements(int degrees, int slope)
        {
            switch (degrees)
            {
                case 0:
                    inc_x = 0;
                    inc_z = -1;
                    break;
                case 90:
                    inc_x = -1;
                    inc_z = 0;
                    break;
                case 180:
                    inc_x = 0;
                    inc_z = 1;
                    break;
                case 270:
                    inc_x = 1;
                    inc_z = 0;
                    break;
                default:
                    break;
            }

            if (slope == -1 || slope == 0 || slope == 1)
            {
                inc_y = slope;
            }
        }

        #region Unity Methods

        // Use this for internal initialization
        void Awake()
        {
            _cityManager = MANAGER.CityManager.Instance;
            inc_x = inc_y = inc_z = 0;
        }

        public void Run()
        {
            /*int x = Mathf.RoundToInt(CityManager.floorOccupied.GetLength(0)/2 + (transform.position.x / CityManager.size));
            int y = Mathf.RoundToInt((transform.position.y / CityManager.size));
            if (mySlope == -1) y = Mathf.RoundToInt((transform.position.y / CityManager.size) - 1);
            int z = Mathf.RoundToInt(CityManager.floorOccupied.GetLength(2)/2 + (transform.position.z / CityManager.size));


            if (recursionCount == 0)         //initial tile "crossroads"
            {
                for (int nextRotation = 0; nextRotation < 360; nextRotation += 90)
                {
                    SetIncrements(nextRotation, 0);

                    //Debug.Log("Before");
                    PlaceBlock(0, nextRotation, x, y, z);
                    //Debug.Log("After");
                }

            }
            else if (recursionCount < maxLevel)
            {
                int corrector = (maxLevel - recursionCount) / 15;
                int random = Random.Range(0, 100);
                if (random < (80 + corrector))
                {
                    int nextRotation = myRotation;

                    random = Random.Range(0, 100);
                    if ((random > 80) && (mySlope == 0)) //going up
                    {
                        SetIncrements(nextRotation, mySlope);

                        PlaceBlock(1, nextRotation, x, y, z);
                    }
                    else if ((random < 20) && (mySlope == 0)) //going down
                    {
                        SetIncrements(nextRotation, mySlope);

                        PlaceBlock(-1, nextRotation, x, y, z);
                    }
                    else //flat path
                    {
                        SetIncrements(nextRotation, mySlope);

                        PlaceBlock(0, nextRotation, x, y, z);
                    }
                }
                //end move forward
                random = Random.Range(0, 100);
                if (((random + corrector) > 50) && (mySlope == 0))
                {
                    int nextRotation = myRotation;
                    random = Random.Range(0, 100);
                    if (random > 50) //left turn
                    {
                        nextRotation = (int)Mathf.Repeat(nextRotation - 90, 360);
                    }
                    else  //right turn
                    {
                        nextRotation = (int)Mathf.Repeat(nextRotation + 90, 360);
                    }
                    SetIncrements(nextRotation, 0);
                    PlaceBlock(0, nextRotation, x, y, z);
                }
            }*/
            MeshFilter mesh = this.GetComponent<MeshFilter>();
            switch (mySlope)
            {
                case 0:
                    mesh.mesh = flatMesh;
                    break;

                case -1:
                    mesh.mesh = slopeMesh;
                    this.transform.rotation = Quaternion.Euler(this.transform.rotation.x, this.transform.rotation.y, this.transform.rotation.z-90);
                    break;

                case 1:
                    mesh.mesh = slopeMesh;
                    break;
            }
            Mesh();
        }

        private void PlaceBlock(int slope, int nextRotation, int x, int y, int z)
        {
            Vector3 incVector = Vector3.right * inc_x + Vector3.up * inc_y + Vector3.forward * inc_z;
            Quaternion nextQuat = Quaternion.Euler(0f, nextRotation, 0f);

            //Debug.Log(CityManager.CheckTile(x + inc_x, y + inc_y, z + inc_z));
            if (CityManager.CheckTile(x + inc_x, y + inc_y, z + inc_z) == false)
            {
                CityManager.floorOccupied[x + inc_x, y + inc_y, z + inc_z] = true;
                child = Instantiate(tilePrefab, transform.position + (incVector * CityManager.size), nextQuat);
                child.parent = this.transform;
                child.GetComponent<RouteTile>().Initialize(slope, nextRotation);
                recursionCount++;
            }
            else if(recursionCount > 0)
            {
                Destroy(this);
            }
        }

        private void Mesh()
        {
            MeshFilter myMesh = this.GetComponent<MeshFilter>();

            _vertices = myMesh.mesh.vertices;
            float meshScale = 1 / myMesh.mesh.bounds.size.y;
            var vertices = new Vector3[_vertices.Length];

            for (var i = 0; i < vertices.Length; i++)
            {
                var vertex = _vertices[i];
                vertex.x = vertex.x * CityManager.size * meshScale;
                vertex.y = vertex.y * CityManager.size * meshScale;
                vertex.z = vertex.z * CityManager.size * meshScale;

                vertices[i] = vertex;
            }
            myMesh.mesh.vertices = vertices;
            myMesh.mesh.RecalculateNormals();
            myMesh.mesh.RecalculateBounds();
            this.GetComponent<MeshCollider>().sharedMesh = myMesh.mesh;
        }
        #endregion

        #endregion

    }
}