using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MeshModifier : MonoBehaviour
{
    Camera mainCamera;
    public float moveDistance = 0.1f;
    public float intensity = 1.0f;
    public bool isPushing = true; // Controls pushing or pulling

    Mesh mesh;
    Vector3[] originalVertices;
    int[] triangles;

    public float maxMovement = 0.5f; // Maximum allowable vertex movement to prevent distortion

    void Start()
    {
        mainCamera = Camera.main;
        mesh = GetComponent<MeshFilter>().mesh;
        originalVertices = mesh.vertices;
        triangles = mesh.triangles;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hitInfo))
            {
                MoveFace(hitInfo);
            }
        }
    }

    void MoveFace(RaycastHit hitInfo)
    {
        MeshCollider meshCollider = hitInfo.collider as MeshCollider;
        if (meshCollider != null && meshCollider.sharedMesh != null)
        {
            int triangleIndex = hitInfo.triangleIndex;
            int[] faceVerticesIndices = {
                triangles[triangleIndex * 3],
                triangles[triangleIndex * 3 + 1],
                triangles[triangleIndex * 3 + 2]
            };

            Vector3 faceNormal = CalculateNormal(faceVerticesIndices);
            Vector3 movement = faceNormal * moveDistance * intensity * (isPushing ? 1 : -1);

            if (CanMoveFace(faceVerticesIndices, movement))
            {
                ApplyMovement(faceVerticesIndices, movement);
                mesh.vertices = originalVertices;
                mesh.RecalculateNormals();
                meshCollider.sharedMesh = mesh;
            }
            else
            {
                Debug.Log("Move operation canceled: would result in invalid mesh configuration.");
            }
        }
    }

    void ApplyMovement(int[] faceVerticesIndices, Vector3 movement)
    {
        foreach (int index in faceVerticesIndices)
        {
            originalVertices[index] += movement;
        }
    }

    Vector3 CalculateNormal(int[] vertexIndices)
    {
        Vector3 p1 = originalVertices[vertexIndices[0]];
        Vector3 p2 = originalVertices[vertexIndices[1]];
        Vector3 p3 = originalVertices[vertexIndices[2]];
        return Vector3.Cross(p2 - p1, p3 - p1).normalized;
    }

    bool CanMoveFace(int[] faceVerticesIndices, Vector3 proposedMovement)
    {
        foreach (var index in faceVerticesIndices)
        {
            Vector3 newPosition = originalVertices[index] + proposedMovement;
            if (Vector3.Distance(originalVertices[index], newPosition) > maxMovement)
            {
                return false;
            }
        }
        // Additional geometric or integrity checks can be added here
        return true;
    }
}
