using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceModifier : MonoBehaviour
{
    Camera mainCamera;
    public float moveDistance = 0.1f; // Adjustable distance to move vertices

    void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Left mouse button clicked
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;

            if (Physics.Raycast(ray, out hitInfo))
            {
                MeshCollider meshCollider = hitInfo.collider as MeshCollider;

                if (meshCollider != null && meshCollider.sharedMesh != null)
                {
                    Mesh mesh = meshCollider.sharedMesh;
                    int[] triangles = mesh.triangles;
                    Vector3[] vertices = mesh.vertices;

                    // Get the triangle that was hit
                    int triangleIndex = hitInfo.triangleIndex;
                    int vertexIndex1 = triangles[triangleIndex * 3];
                    int vertexIndex2 = triangles[triangleIndex * 3 + 1];
                    int vertexIndex3 = triangles[triangleIndex * 3 + 2];

                    // Calculate the normal of the face
                    Vector3 vertex1 = vertices[vertexIndex1];
                    Vector3 vertex2 = vertices[vertexIndex2];
                    Vector3 vertex3 = vertices[vertexIndex3];
                    Vector3 faceNormal = Vector3.Cross(vertex2 - vertex1, vertex3 - vertex1).normalized;

                    // Modify the vertices along the normal
                    vertices[vertexIndex1] += faceNormal * moveDistance;
                    vertices[vertexIndex2] += faceNormal * moveDistance;
                    vertices[vertexIndex3] += faceNormal * moveDistance;

                    // Update the mesh with the new vertices and recalculate necessary properties
                    mesh.vertices = vertices;
                    mesh.RecalculateNormals();
                    mesh.RecalculateBounds();

                    // Update MeshCollider if necessary
                    if (meshCollider)
                    {
                        meshCollider.sharedMesh = mesh;
                    }
                }
            }
        }
    }
}

