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

    private Renderer myRenderer;
    private Material myMaterial;
    private MeshFilter myMesh;
    private GameObject _child;
    private Vector3[] _vertices;

    private void Generate()
    {
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
        else if (currentHeight < structures.maxHeight)
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
        else if (currentHeight == structures.maxHeight)
        {
            objVal = structures.topObjects.Length;
            matNum = structures.colourScheme3.Length;
            myMesh.mesh = structures.topObjects[Random.Range(0, objVal)];
            myMaterial = structures.colourScheme3[Random.Range(0, matNum)];
            this.transform.rotation = rotation;
            CreateTile();
            Destroy(this);
        }
        else if (currentHeight > structures.maxHeight)
        {
            Destroy(this.gameObject);
        }
    }

    private void CreateTile()
    {
        Mesh();
        //this.GetComponent<Rigidbody>().mass = scale * .75f;
        this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + (myMesh.mesh.bounds.size.y/2), this.transform.position.z);
        this.GetComponent<MeshCollider>().enabled = true;
        this.gameObject.GetComponent<MeshCollider>().sharedMesh = myMesh.mesh;
        this.gameObject.GetComponent<MeshCollider>().convex = true;
        myRenderer.material = myMaterial;
        NextTile(thisObj);
    }

    private void Mesh()
    {
        _vertices = myMesh.mesh.vertices;
        var vertices = new Vector3[_vertices.Length];

        for (var i = 0; i < vertices.Length; i++)
        {
            var vertex = _vertices[i];
            vertex.x = vertex.x * scale;
            vertex.y = vertex.y * scale;
            vertex.z = vertex.z * scale;

            vertices[i] = vertex;
        }

        myMesh.mesh.vertices = vertices;
        myMesh.mesh.RecalculateNormals();
        myMesh.mesh.RecalculateBounds();
        NavObj();
    }

    private void NavObj()
    {
        this.GetComponent<NavMeshObstacle>().size = myMesh.mesh.bounds.size;
    }

    private void NextTile(GameObject obj)
    {
        _child = Instantiate(obj, 
            new Vector3(this.transform.position.x, 
            this.transform.position.y + (this.gameObject.GetComponent<MeshRenderer>().bounds.size.y / 2), 
            this.transform.position.z),
            Quaternion.Euler(this.transform.rotation.x, this.transform.rotation.y, this.transform.rotation.z),this.transform);
        if(centreMaterial != null) _child.GetComponent<BuildingScript>().centreMaterial = centreMaterial;
        _child.GetComponent<BuildingScript>().currentHeight = currentHeight + 1;
        _child.GetComponent<BuildingScript>().structures = structures;
        _child.GetComponent<BuildingScript>().Build();
    }
}
