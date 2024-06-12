using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshCollider))]
public class MeshColliderUpdater : MonoBehaviour
{
    private MeshCollider meshCollider;
    private MeshFilter meshFilter;

    void Start()
    {
        meshCollider = GetComponent<MeshCollider>();
        meshFilter = GetComponent<MeshFilter>();
    }

    // Call this method after modifying the mesh
    public void UpdateCollider()
    {
        meshCollider.sharedMesh = null; // Clear the current collider mesh
        meshCollider.sharedMesh = meshFilter.mesh; // Assign the updated mesh
    }
}
