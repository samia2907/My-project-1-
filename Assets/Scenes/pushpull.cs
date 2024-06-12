using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;
public class PushPullObject : MonoBehaviour
{
    private Mesh mesh;
    private Vector3[] vertices;
    private int[] triangles;

    void Start()
    {
        mesh = GetComponent<MeshFilter>().mesh;
        vertices = mesh.vertices;
        triangles = mesh.triangles;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                int hitTriangleIndex = hit.triangleIndex;
                Debug.Log("Hit triangle index: " + hitTriangleIndex);

                // Calculate the average position of the triangle's vertices to get its "center"
                Vector3 faceCenter = (vertices[triangles[hitTriangleIndex * 3]] +
                                      vertices[triangles[hitTriangleIndex * 3 + 1]] +
                                      vertices[triangles[hitTriangleIndex * 3 + 2]]) / 3;

                // Push or pull the face's vertices along the hit normal
                float pushDistance = 0.1f; // How far to push/pull
                vertices[triangles[hitTriangleIndex * 3]] += hit.normal * pushDistance;
                vertices[triangles[hitTriangleIndex * 3 + 1]] += hit.normal * pushDistance;
                vertices[triangles[hitTriangleIndex * 3 + 2]] += hit.normal * pushDistance;

                mesh.vertices = vertices;
                mesh.RecalculateBounds();
                mesh.RecalculateNormals(); // To ensure lighting reacts properly
            }
        }
    }
}
