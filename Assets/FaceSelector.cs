using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceSelector : MonoBehaviour
{
    Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main; // Get the main camera
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Check if the left mouse button was clicked
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

                    // Triangle index
                    int triangleIndex = hitInfo.triangleIndex;
                    int vertexIndex1 = triangles[triangleIndex * 3];
                    int vertexIndex2 = triangles[triangleIndex * 3 + 1];
                    int vertexIndex3 = triangles[triangleIndex * 3 + 2];

                    Vector3 vertex1 = vertices[vertexIndex1];
                    Vector3 vertex2 = vertices[vertexIndex2];
                    Vector3 vertex3 = vertices[vertexIndex3];

                    Debug.Log("Hit triangle vertex indices: " + vertexIndex1 + ", " + vertexIndex2 + ", " + vertexIndex3);
                    Debug.Log("World position of vertices: " + meshCollider.transform.TransformPoint(vertex1) + ", "
                        + meshCollider.transform.TransformPoint(vertex2) + ", " + meshCollider.transform.TransformPoint(vertex3));
                }
            }
        }
    }
}
