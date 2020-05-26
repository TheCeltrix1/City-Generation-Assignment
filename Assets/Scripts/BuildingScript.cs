using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BuildingScript : MonoBehaviour
{
    public BuildObject structures;
    public int currentHeight;
    public GameObject thisObj;
    public Material centreMaterial;
    public float scale = 2;
    public Quaternion rotation;
    public int posX;
    public int posY;
    public int posZ;
    public int maxHeight;

    private Renderer myRenderer;
    private Material myMaterial;
    private MeshFilter myMesh;
    private GameObject _child;
    private Vector3[] _vertices;
    private MANAGER.CityManager _manager;

    private void Generate()
    {
        _manager = FindObjectOfType<MANAGER.CityManager>();
        myRenderer = this.GetComponent<MeshRenderer>();
        myMesh = this.GetComponent<MeshFilter>();
        this.GetComponent<MeshCollider>().enabled = false;
    }

    public void Build()
    {
        Generate();
        int objVal;
        int matNum;
        if (currentHeight == 0)
        {
            objVal = structures.baseObjects.Length;
            matNum = structures.colourScheme1.Length;
            myMesh.mesh = structures.baseObjects[Random.Range(0, objVal)];
            myMaterial = structures.colourScheme1[Random.Range(0, matNum)];
            CreateTile();
            Destroy(this);
        }
        else if (currentHeight < maxHeight)
        {
            objVal = structures.middleObjects.Length;
            matNum = structures.colourScheme2.Length;
            myMesh.mesh = structures.middleObjects[Random.Range(0, objVal)];
            if (centreMaterial == null) {
                myMaterial = structures.colourScheme2[Random.Range(0, matNum)];
                centreMaterial = myMaterial;
            }
            else
            {
                myMaterial = centreMaterial;
            }
            this.transform.rotation = rotation;
            CreateTile();
            Destroy(this);
        }
        else if (currentHeight == maxHeight)
        {
            objVal = structures.topObjects.Length;
            matNum = structures.colourScheme3.Length;

            if(Random.Range(0, 20) <= 1 && !MANAGER.CityManager.endPointSpawn)
            {
                if ((posX > 0 && posZ > 0 && posX < MANAGER.CityManager.floorOccupied.GetLength(0) - 1 && posZ < MANAGER.CityManager.floorOccupied.GetLength(2) - 1))
                MANAGER.CityManager.endPointSpawn = true;
                myMesh.mesh = MANAGER.CityManager.Instance.endPoint;
                myMaterial = structures.neon;
                this.gameObject.tag = "EndPoint";
                Debug.Log("endgoal " + posX + " " + posY + " " + posZ);
            }
            else
            {
                myMesh.mesh = structures.topObjects[Random.Range(0, objVal)];
                myMaterial = structures.colourScheme3[Random.Range(0, matNum)];
            }

            this.transform.rotation = rotation;
            CreateTile();
            if(myMesh.mesh.name == "pSphere1 Instance" || myMesh.mesh.name == "pSphere2 Instance") this.transform.position = this.transform.position - new Vector3(0, (myMesh.mesh.bounds.size.y / 2), 0);
            else this.transform.position = this.transform.position - new Vector3(0, (myMesh.mesh.bounds.size.y / 8), 0);
            Destroy(this);
        }
        else if (currentHeight > maxHeight)
        {
            Destroy(this.gameObject);
        }
    }

    private void CreateTile()
    {
        Mesh();
        this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + (myMesh.mesh.bounds.size.y/2), this.transform.position.z);
        MeshCollider meshCol = this.GetComponent<MeshCollider>();
        meshCol.enabled = true;
        meshCol.sharedMesh = myMesh.mesh;
        meshCol.convex = false;

        //fix materials
        myRenderer.material = myMaterial;
        Material[] matMesh = new Material[myMesh.mesh.subMeshCount];
        for(int x = 0; x < matMesh.Length; x ++)
        {
            if (x == 0) matMesh[x] = structures.accent;
            if (x == 1) matMesh[x] = myMaterial;
            if (x == 2) matMesh[x] = structures.interior[Random.Range(0,structures.interior.Length)];
            if (x == 3) matMesh[x] = structures.neon;
            if (x == 4) matMesh[x] = structures.neon;
        }
        myRenderer.materials = matMesh;
        NextTile(thisObj);
    }

    private void Mesh()
    {
        _vertices = myMesh.mesh.vertices;
        float meshScale = 1/myMesh.mesh.bounds.size.x;
        var vertices = new Vector3[_vertices.Length];

        for (var i = 0; i < vertices.Length; i++)
        {
            var vertex = _vertices[i];
            vertex.x = vertex.x * scale * meshScale;
            vertex.y = vertex.y * scale * 1 / myMesh.mesh.bounds.size.y;
            vertex.z = vertex.z * scale * meshScale;

            vertices[i] = vertex;
        }
        myMesh.mesh.vertices = vertices;
        myMesh.mesh.RecalculateNormals();
        myMesh.mesh.RecalculateBounds();
        NavObj();
    }

    private void NavObj()
    {
        //this.GetComponent<NavMeshObstacle>().size = myMesh.mesh.bounds.size;
    }

    private void NextTile(GameObject obj)
    {
        if (posY + 1 < MANAGER.CityManager.floorOccupied.GetLength(1) && !MANAGER.CityManager.floorOccupied[posX, posY + 1, posZ])
        {
            MANAGER.CityManager.floorOccupied[posX, posY + 1, posZ] = false;
            _child = Instantiate(obj,
                new Vector3(this.transform.position.x,
                this.transform.position.y + (this.gameObject.GetComponent<MeshRenderer>().bounds.size.y / 2),
                this.transform.position.z),
                Quaternion.Euler(this.transform.rotation.x, this.transform.rotation.y, this.transform.rotation.z), this.transform);
            BuildingScript bs = _child.GetComponent<BuildingScript>();
            if (centreMaterial != null) bs.centreMaterial = centreMaterial;
            bs.posX = posX;
            bs.posY = posY + 1;
            bs.posZ = posZ;
            bs.currentHeight = currentHeight + 1;
            bs.structures = structures;
            bs.Build();
        }
        else
        {
            if (posX == MANAGER.CityManager.floorOccupied.GetLength(0) - 1 && posZ == MANAGER.CityManager.floorOccupied.GetLength(2) - 1) MANAGER.CityManager.finishedIteration = true;
            Destroy(this);
        }
    }
}
