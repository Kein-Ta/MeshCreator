using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UIElements;
using UnityEngine.Experimental.XR;


public class CreatMesh : MonoBehaviour
{
    public Material material;

    [Header("Z")]
    public float length = 1;
    [Header("X")]
    public float width = 1;
    public int fenDuan = 0;

    private int[] triangles;
    private Vector2[] uvs;
    private Vector3[] vertices;

    private int trianglesCount;
    private Mesh mesh;

    private MeshFilter meshFilter;

    void OnEnable()
    {
        GameObject obj = Create(width, length, Vector3.zero);
        obj.GetComponent<MeshRenderer>().material = material;
    }

    GameObject Create(float width, float length, Vector3 Point)
    {
        GameObject NewObj = gameObject;
        NewObj.AddComponent<MeshFilter>();
        NewObj.AddComponent<MeshRenderer>();
        meshFilter = NewObj.GetComponent<MeshFilter>();

        int vertices_count = 4 + fenDuan * 2;
        vertices = new Vector3[vertices_count];


        //for (int i = 0; i < vertices_count; i++)
        //{
        //    float posx = i > 1 && i
        //    vertices[i] = new Vector3(Point.x - width / 2, Point.y, Point.z);

        //}


        vertices[0] = new Vector3(Point.x + width / 2, Point.y, Point.z);
        vertices[1] = new Vector3(Point.x - width / 2, Point.y, Point.z );
        vertices[2] = new Vector3(Point.x + width / 2, Point.y, Point.z + length);
        vertices[3] = new Vector3(Point.x - width / 2, Point.y, Point.z + length);





        //int SplitTriangle = 1 * 2;
        //trianglesCount = SplitTriangle * 3;
        triangles = new int[6];

        triangles[0] = 0;
        triangles[1] = 1;
        triangles[2] = 2;
        triangles[3] = 2;
        triangles[4] = 1;
        triangles[5] = 3;



        uvs = new Vector2[vertices.Length];
        Vector3[] normals = new Vector3[vertices.Length];

        uvs[0] = new Vector2(1 * width, 1 * length);
        uvs[1] = new Vector2(1 * width, 0);
        uvs[2] = new Vector2(0, 1 * length);
        //uvs[3] = new Vector2(0, 0);


        mesh = new Mesh();
        //mesh.name = "cube";
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.normals = normals;

        mesh.uv = uvs;
        mesh.RecalculateTangents();
        mesh.RecalculateNormals();
        meshFilter.mesh = mesh;
        return NewObj;
    }
}