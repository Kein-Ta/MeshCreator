using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UIElements;
using UnityEngine.Experimental.XR;

[ExecuteInEditMode]
public class CreatMesh : MonoBehaviour
{
    public Material material;

    private Vector3 centerPos;
    
    public float width,height;

    public int subdivisionX, subdivisionY;
    
    private Vector2[] uvs;
    private Vector3[] vertices;
    private Mesh mesh;
    private MeshFilter meshFilter;

    private void Awake()
    {
        centerPos = transform.position;
        CreateMesh(centerPos, width, height, subdivisionX, subdivisionY);
        gameObject.GetComponent<MeshRenderer>().material = material;
    }

    private void Update()
    {
        gameObject.GetComponent<MeshFilter>().mesh = UpdateMesh(centerPos, width, height, subdivisionX, subdivisionY);
    }

    GameObject CreateMesh(Vector3 _centerPos, float _width,float _height,int _subdivisionX,int _subdivisionY)
    {
        GameObject meshObj = gameObject;
        meshObj.AddComponent<MeshFilter>();
        meshObj.AddComponent<MeshRenderer>();
        meshFilter = meshObj.GetComponent<MeshFilter>();
        _subdivisionX = _subdivisionX < 1 ? 1 : _subdivisionX;
        _subdivisionY = _subdivisionY < 1 ? 1 : _subdivisionY;
        vertices = new Vector3[(_subdivisionX + 1) * (_subdivisionY + 1)];
        for (int i = 0, y = 0; y <= _subdivisionY; y++)
        {
            for (int x = 0; x <= _subdivisionX; x++, i++)
            {
                float xpos = x * (_width / _subdivisionX);
                float ypos = y * (_height / _subdivisionY);
                vertices[i] = new Vector3(_centerPos.x + xpos - _width / 2, ypos, 0);
            }
        }

        int[] triangles = new int[_subdivisionX * _subdivisionY * 6];
        for (int ti = 0, vi = 0, y = 0 ; y < _subdivisionY; y++,vi++)
        {
            for (int x = 0; x < _subdivisionX; x++, ti += 6, vi++)
            {
                triangles[ti] = vi;
                triangles[ti + 1] = vi + _subdivisionX + 1;
                triangles[ti + 2] = vi + 1;

                triangles[ti + 3] = vi + 1;
                triangles[ti + 4] = vi + _subdivisionX + 1;
                triangles[ti + 5] = vi + _subdivisionX + 2;
            }
        }

        Vector3[] normals = new Vector3[vertices.Length];

        uvs = new Vector2[vertices.Length];
        for (int uv = 0, v = 0; v <= _subdivisionY; v++)
        {
            for (int u = 0; u <= _subdivisionX; u++, uv++)
            {
                uvs[uv] = new Vector2((float)u / _subdivisionX, (float)v / _subdivisionY);
            }
        }

        
        mesh = new Mesh();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.normals = normals;
        mesh.uv = uvs;

        mesh.RecalculateTangents();
        mesh.RecalculateNormals();
        meshFilter.mesh = mesh;

        return meshObj;
    }

    Mesh UpdateMesh(Vector3 _centerPos, float _width, float _height, int _subdivisionX, int _subdivisionY)
    {
        Mesh newMesh = new Mesh();
        _subdivisionX = _subdivisionX < 1 ? 1 : _subdivisionX;
        _subdivisionY = _subdivisionY < 1 ? 1 : _subdivisionY;
        vertices = new Vector3[(_subdivisionX + 1) * (_subdivisionY + 1)];
        for (int i = 0, y = 0; y <= _subdivisionY; y++)
        {
            for (int x = 0; x <= _subdivisionX; x++, i++)
            {
                float xpos = x * (_width / _subdivisionX);
                float ypos = y * (_height / _subdivisionY);
                vertices[i] = new Vector3(_centerPos.x + xpos - _width / 2, ypos, 0);
            }
        }
        newMesh.vertices = vertices;

        int[] triangles = new int[_subdivisionX * _subdivisionY * 6];
        for (int ti = 0, vi = 0, y = 0; y < _subdivisionY; y++, vi++)
        {
            for (int x = 0; x < _subdivisionX; x++, ti += 6, vi++)
            {
                triangles[ti] = vi;
                triangles[ti + 1] = vi + _subdivisionX + 1;
                triangles[ti + 2] = vi + 1;

                triangles[ti + 3] = vi + 1;
                triangles[ti + 4] = vi + _subdivisionX + 1;
                triangles[ti + 5] = vi + _subdivisionX + 2;
            }
        }
        newMesh.triangles = triangles;

        Vector3[] normals = new Vector3[vertices.Length];
        newMesh.normals = normals;

        uvs = new Vector2[vertices.Length];
        for (int uv = 0, v = 0; v <= _subdivisionY; v++)
        {
            for (int u = 0; u <= _subdivisionX; u++, uv++)
            {
                uvs[uv] = new Vector2((float)u / _subdivisionX, (float)v / _subdivisionY);
            }
        }
        newMesh.uv = uvs;

        newMesh.RecalculateTangents();
        newMesh.RecalculateNormals();

        return newMesh;
    }
}