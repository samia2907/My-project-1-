using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshSubdivider : MonoBehaviour
{
    private Mesh mesh;

    void Start()
    {
        mesh = GetComponent<MeshFilter>().mesh;
    }

    // Call this method with the index of the triangle to subdivide
    public void SubdivideFace(int triangleIndex)
    {
        Vector3[] oldVertices = mesh.vertices;
        int[] oldTriangles = mesh.triangles;

        // Indices of the original triangle vertices
        int i0 = oldTriangles[triangleIndex * 3];
        int i1 = oldTriangles[triangleIndex * 3 + 1];
        int i2 = oldTriangles[triangleIndex * 3 + 2];

        // Get the original vertices
        Vector3 v0 = oldVertices[i0];
        Vector3 v1 = oldVertices[i1];
        Vector3 v2 = oldVertices[i2];

        // Calculate midpoints
        Vector3 v01 = (v0 + v1) / 2;
        Vector3 v12 = (v1 + v2) / 2;
        Vector3 v20 = (v2 + v0) / 2;

        // Add new vertices
        List<Vector3> newVertices = new List<Vector3>(oldVertices);
        newVertices.Add(v01);
        newVertices.Add(v12);
        newVertices.Add(v20);
        int i01 = newVertices.Count - 3;
        int i12 = newVertices.Count - 2;
        int i20 = newVertices.Count - 1;

        // Create new triangles
        List<int> newTriangles = new List<int>(oldTriangles);
        // Replace the original triangle with the new ones
        newTriangles[triangleIndex * 3] = i0;
        newTriangles[triangleIndex * 3 + 1] = i01;
        newTriangles[triangleIndex * 3 + 2] = i20;

        newTriangles.Add(i01);
        newTriangles.Add(i1);
        newTriangles.Add(i12);

        newTriangles.Add(i01);
        newTriangles.Add(i12);
        newTriangles.Add(i20);

        newTriangles.Add(i20);
        newTriangles.Add(i12);
        newTriangles.Add(i2);

        // Update the mesh
        mesh.vertices = newVertices.ToArray();
        mesh.triangles = newTriangles.ToArray();

        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
    }
}
