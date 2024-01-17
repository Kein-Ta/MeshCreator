using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UIElements;
using UnityEngine.Experimental.XR;


public class CreatMesh : MonoBehaviour
{
    public Material material;

    public Vector3 centerPos;
    public float width,height;
    public int subdivisionX, subdivisionY;
    
    private Vector2[] uvs;
    private Vector3[] vertices;
    private Mesh mesh;
    private MeshFilter meshFilter;

    private void Awake()
    {
        centerPos = transform.position;
        Create();
        gameObject.GetComponent<MeshRenderer>().material = material;
    }


    GameObject Create()
    {
        GameObject NewObj = gameObject;
        NewObj.AddComponent<MeshFilter>();
        NewObj.AddComponent<MeshRenderer>();
        meshFilter = NewObj.GetComponent<MeshFilter>();

        vertices = new Vector3[(subdivisionX + 1) * (subdivisionY + 1)];
        for (int i = 0, y = 0; y <= subdivisionY; y++)
        {
            for (int x = 0; x <= subdivisionX; x++, i++)
            {
                float xpos = x * (width / subdivisionX);
                float ypos = y * (height / subdivisionY);
                vertices[i] = new Vector3( centerPos.x + xpos - width / 2, ypos, 0);
            }
        }

        int[] triangles = new int[subdivisionX * subdivisionY * 6];
        for (int ti = 0, vi = 0, y = 0 ; y < subdivisionY; y++,vi++)
        {
            for (int x = 0; x < subdivisionX; x++, ti += 6, vi++)
            {
                triangles[ti] = vi;
                triangles[ti + 1] = vi + subdivisionX + 1;
                triangles[ti + 2] = vi + 1;

                triangles[ti + 3] = vi + 1;
                triangles[ti + 4] = vi + subdivisionX + 1;
                triangles[ti + 5] = vi + subdivisionX + 2;
            }
        }



        uvs = new Vector2[vertices.Length];
        Vector3[] normals = new Vector3[vertices.Length];

        uvs[0] = new Vector2(1, 1 );
        uvs[1] = new Vector2(1, 0);
        uvs[2] = new Vector2(0, 1);
        uvs[3] = new Vector2(0, 0);


        mesh = new Mesh();
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