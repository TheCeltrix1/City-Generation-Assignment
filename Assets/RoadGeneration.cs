using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadGeneration : MonoBehaviour
{
    /*
    public int posX;
    public int posY;
    public int posZ;
    public int x;
    public int y;
    public int z;
    private bool[] _direction = new bool[4];
    public int slope;
    private float _size;
    public GameObject prefab;
    private static int recursion = 20;

    private void OnEnable()
    {
        _size = MANAGER.CityManager.size;  
    }

    public void Create()
    {
            if (slope == -1)
            {
                int q = Random.Range(0, 2);
                if (!MANAGER.CityManager.CheckTile(posX + x, posY - 1, posZ + z) && q == 0)
                {
                    SpawnTile(x, -1, z);
                }
                else if (!MANAGER.CityManager.CheckTile(posX + x, posY, posZ + z) )
                {
                    SpawnTile(x, 0, z);
                }

            }
            else if (slope == 1)
            {

                int q = Random.Range(0, 2);
                if (!MANAGER.CityManager.CheckTile(posX + x, posY + 1, posZ + z) && q == 0)
                {
                    SpawnTile(x, 1, z);
                }
                else if (!MANAGER.CityManager.CheckTile(posX + x, posY, posZ + z) && q == 1)
                {
                    SpawnTile(x, 0, z);
                }

            }
            else if (slope == 0)
            {
            int x1 = 0;
            int z1 = 0;
            for (int i = 0; i < 4; i++) {
                if (i == 0)
                {
                    x1 = 1;
                    z1 = 0;
                }
                else if (i == 1)
                {
                    x1 = -1;
                    z1 = 0;
                }
                else if (i == 2)
                {
                    x1 = 0;
                    z1 = 1;
                }
                else if (i == 3)
                {
                    x1 = 0;
                    z1 = -1;
                }
                if ((x1 != x || z1 != x))
                    {
                    int q = Random.Range(0, 3);
                    Debug.Log(MANAGER.CityManager.CheckTile(posX + x1, posY, posZ + z1));
                    //Debug.Log(q + " " + MANAGER.CityManager.CheckTile(posX + x1, posY, posZ + z1));
                    if (!MANAGER.CityManager.CheckTile(posX + x1, posY, posZ + z1) && q == 0)
                    {
                        SpawnTile(x1, 0, z1);
                    }
                    else if (!MANAGER.CityManager.CheckTile(posX + x1, posY - 1, posZ + z1) && q == 1)
                    {
                        SpawnTile(x1, -1, z1);
                    }
                    else if (!MANAGER.CityManager.CheckTile(posX + x1, posY + 1, posZ + z1) && q == 2)
                    {
                        SpawnTile(x1, 1, z1);
                    }
                }
            }
        }
    }

    private void SpawnTile(int x, int y, int z)
    {
        recursion -= 1;
        //Debug.Log(recursion);
        if (recursion > 0) {
            MANAGER.CityManager.floorOccupied[posX + x, posY + y, posZ + z] = true;
            GameObject child = Instantiate(prefab, new Vector3(
                ((posX + x - (MANAGER.CityManager.floorOccupied.GetLength(0) / 2)) * _size) + _size / 2,
                (posY * _size) + _size / 2,
                ((posZ + z - (MANAGER.CityManager.floorOccupied.GetLength(2) / 2)) * _size) + _size / 2),
                this.transform.rotation);
            RoadGeneration rg = child.GetComponent<RoadGeneration>();
            rg.x = x;
            rg.y = y;
            rg.z = z;
            rg.posX = posX + x;
            rg.posY = posY + y;
            rg.posZ = posZ + z;
            rg.slope = y;
            rg.Create();
        }
    }
    */
}
