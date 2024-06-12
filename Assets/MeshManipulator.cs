using UnityEngine;

public class MeshManipulator : MonoBehaviour
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

    // Public method to trigger push/pull
    public void PushPullFace(float distance)
    {
        // Example: Push/Pull the first face (first three vertices of the first triangle)
        int faceIndex = 0; // Modify this to be dynamic based on selection
        Vector3 faceNormal = mesh.normals[triangles[faceIndex * 3]];

        for (int i = 0; i < 3; i++)
        {
            vertices[triangles[faceIndex * 3 + i]] += faceNormal * distance;
        }

        mesh.vertices = vertices;
        mesh.RecalculateBounds();
        mesh.RecalculateNormals();
    }
}
