using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class PushPullTool : MonoBehaviour
{
    private Mesh originalMesh;
    private Mesh modifiedMesh;
    private List<int> selectedFaces = new List<int>();
    private bool isPushing = true;
    private float extrusionDistance = 0f;
    private int selectedFaceIndex = -1;
    private bool isMouseDragging = false;
    private Vector3 initialMousePosition;
    public bool isToolActive = false;

    public Button toolButton;
    public Camera selectionCamera;
    public Transform frontFace;

    public
    void Start()
    {
        if (selectionCamera == null)
            selectionCamera = Camera.main;
        toolButton.onClick.AddListener(ToggleTool);
        //frontFace = transform.Find("FrontFace");
        /*if (isToolActive)
        { 
            Debug.LogError("staaaaaaaaaaaaaart");
        if ( == null)
        {
            Debug.LogError("MeshFilter component missing from the GameObject");
            return;
        }
      
        originalMesh = GetComponent<MeshFilter>().sharedMesh;
        if (originalMesh == null)
        {
            Debug.LogError("No original mesh found on the MeshFilter");
            return;
        }
        Debug.LogError("step 2 ");
        modifiedMesh = Instantiate(originalMesh);
        Debug.LogError("step 3 ");
        GetComponent<MeshFilter>().mesh = modifiedMesh;
        }*/
    }

    void push(string direction, bool isPush)
    {
        //Debug.LogError("hello");
        //1 front
        //push
        if (direction == "Back")
        {
            if (isPush)
            {
                frontFace.localScale += new Vector3(0, 0, 1f) * extendAmount;
                frontFace.position -= frontFace.forward * .5f * extendAmount;
            }
            else
            {
                //pull
                frontFace.localScale += new Vector3(0, 0, -1f) * extendAmount;
                frontFace.position += frontFace.forward * .5f * extendAmount;
            }
        }


        //2 back
        if (direction == "Front")
        {
            if (isPush)
            {
                frontFace.localScale += new Vector3(0, 0, 1f) * extendAmount;
                frontFace.position += frontFace.forward * .5f * extendAmount;
            }
            else
            {
                //pull
                frontFace.localScale += new Vector3(0, 0, -1f) * extendAmount;
                frontFace.position -= frontFace.forward * .5f * extendAmount;
            }
        }

        //3 down
        if (direction == "Bottom")
        {
            if (isPush)
            {
                frontFace.localScale += new Vector3(0, 1f, 0) * extendAmount;
                frontFace.position -= frontFace.up * .5f * extendAmount;
            }
            else
            {
                frontFace.localScale += new Vector3(0, -1f, 0) * extendAmount;
                frontFace.position += frontFace.up * .5f * extendAmount;
            }
        }

        //4 up
        if (direction == "Top")
        {
            if (isPush)
            {
                frontFace.localScale += new Vector3(0, 1f, 0) * extendAmount;
                frontFace.position += frontFace.up * .5f * extendAmount;
            }
            else
            {
                frontFace.localScale += new Vector3(0, -1f, 0) * extendAmount;
                frontFace.position -= frontFace.up * .5f * extendAmount;
            }
        }

        //5 left
        if (direction == "Left")
        {
            if (isPush)
            {
                frontFace.localScale += new Vector3(1f, 0, 0) * extendAmount;
                frontFace.position -= frontFace.right * .5f * extendAmount;
            }
            else
            {
                frontFace.localScale += new Vector3(-1f, 0, 0) * extendAmount;
                frontFace.position += frontFace.right * .5f * extendAmount;
            }
        }

        //6 right
        if (direction == "Right")
        {
            if (isPush)
            {
                frontFace.localScale += new Vector3(1f, 0, 0) * extendAmount;
                frontFace.position += frontFace.right * .5f * extendAmount;
            }
            else
            {
                frontFace.localScale += new Vector3(-1f, 0, 0) * extendAmount;
                frontFace.position -= frontFace.right * .5f * extendAmount;
            }
        }
    }

    private string[] cubeDirections = { "Right", "Left", "Top", "Bottom", "Front", "Back" };
    bool isPush = true;
    Vector3 lastMousePosition;
    float extendAmount;
    float moveAmount;
    float extendSpeed = 0.1f;
    float moveSpeed = 0.1f;

    string movingSide = "";
    bool isMousePressed = true;

    bool hasDraggedAway = false; // Flag to track if the mouse has dragged away from the face after the initial click
    Vector3 initialClickDirection;
    Vector3 initialClickPosition;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))// try to insure the work by using space 
        {
            if (isPush == true)
                isPush = false;
            else
                isPush = true;
        }
        if (Input.GetMouseButtonDown(0))//left mouse pressed
        {
            initialClickPosition = Input.mousePosition;
            lastMousePosition = Input.mousePosition;
            Debug.Log("updated mouse positiond");
            isMousePressed = true;

            Ray ray = selectionCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            float directionThreshold = 0.9f;
            if (Physics.Raycast(ray, out hit))
            {
                Vector3 hitNormal = hit.normal;
                Vector3 hitPoint = hit.point;
                // Calculate the direction from the cube to the hit point on initial click

                initialClickDirection = (hitNormal - frontFace.position).normalized;


                Debug.Log("Hit normal: " + hitNormal);

                // Calculate dot products between hit normal and local axes of the cube
                float dotX = Vector3.Dot(hitNormal, frontFace.right);
                float dotY = Vector3.Dot(hitNormal, frontFace.up);
                float dotZ = Vector3.Dot(hitNormal, frontFace.forward);

                // Determine the direction to extend the cube based on the maximum dot product
                Vector3 direction = Vector3.zero;
                string clickedDirection = "";

                if (Mathf.Abs(dotX) > Mathf.Abs(dotY) && Mathf.Abs(dotX) > Mathf.Abs(dotZ))
                {
                    direction = dotX > 0 ? frontFace.right : -frontFace.right;
                    clickedDirection = dotX > 0 ? cubeDirections[0] : cubeDirections[1];
                }
                else if (Mathf.Abs(dotY) > Mathf.Abs(dotX) && Mathf.Abs(dotY) > Mathf.Abs(dotZ))
                {
                    direction = dotY > 0 ? frontFace.up : -frontFace.up;
                    clickedDirection = dotY > 0 ? cubeDirections[2] : cubeDirections[3];
                }
                else if (Mathf.Abs(dotZ) > Mathf.Abs(dotX) && Mathf.Abs(dotZ) > Mathf.Abs(dotY))
                {
                    direction = dotZ > 0 ? frontFace.forward : -frontFace.forward;
                    clickedDirection = dotZ > 0 ? cubeDirections[4] : cubeDirections[5];
                }

                Debug.Log("Clicked on the " + clickedDirection + " side of the cube.");
                // "Right", "Left", "Top", "Bottom", "Front", "Back"
                //front back up down left right
                movingSide = clickedDirection;
                

            }

        }
        if (Input.GetMouseButtonUp(0))//left mouse released
        {
            Debug.Log("released");
            isMousePressed = false;
            movingSide = "";
        }
        if (Input.GetMouseButton(0) && isMousePressed) //left mouse held down
        {//left mouse held
            //Debug.Log("holding down");

            Vector3 mouseDelta = Input.mousePosition - lastMousePosition;
            //Debug.Log("MOUSEDELTA:" + mouseDelta);


            //Debug.Log("updated mouse positiond");

            if (mouseDelta.magnitude < 0.01f)
                return;

            Vector3 currentMousePosition = Input.mousePosition;

            // Check if the current mouse y-coordinate is greater than the initial click y-coordinate
            if (currentMousePosition.y > lastMousePosition.y)
            {
                Debug.Log("dragging up");
                isPush = true;
                //isDraggingUp = true;
            }
            else if (currentMousePosition.y < lastMousePosition.y)
            {
                Debug.Log("dragging down");
                isPush = false;
                // isDraggingUp = false;
            }

            switch (movingSide)
            {
                case "Right":
                    push("Right", isPush);
                    break;
                case "Left":
                    push("Left", isPush);
                    break;
                case "Top":
                    push("Top", isPush);
                    break;
                case "Bottom":
                    push("Bottom", isPush);
                    break;
                case "Front":
                    push("Front", isPush);
                    break;
                case "Back":
                    push("Back", isPush);
                    break;
            }


            extendAmount = mouseDelta.magnitude * extendSpeed;
            moveAmount = mouseDelta.magnitude * moveSpeed;

            lastMousePosition = Input.mousePosition;


            //Debug.LogError("hello");
            //1 front
            //push
            //frontFace.localScale += new Vector3(0, 0, 1f);
            //frontFace.position -= frontFace.forward *.5f ;
            //pull
            //frontFace.localScale += new Vector3(0, 0, -1f);
            //frontFace.position -= frontFace.forward *.5f ;


            //2 back
            //push
            //frontFace.localScale += new Vector3(0, 0, 1f);
            //frontFace.position += frontFace.forward *.5f;
            //pull
            //frontFace.localScale += new Vector3(0, 0, -1f);
            //frontFace.position += frontFace.forward *.5f;

            //3 down
            //push
            //frontFace.localScale += new Vector3(0, 1f, 0);
            //frontFace.position -= frontFace.up * .5f;
            //pull
            //frontFace.localScale += new Vector3(0, -1f, 0);
            //frontFace.position -= frontFace.up * .5f;

            //4 up
            //push
            // frontFace.localScale += new Vector3(0, 1f, 0);
            // frontFace.position += frontFace.up * .5f;
            //pull
            // frontFace.localScale += new Vector3(0, -1f, 0);
            // frontFace.position += frontFace.up * .5f;

            //5 left
            //push
            //frontFace.localScale += new Vector3(1f, 0, 0);
            //frontFace.position -= frontFace.right *.5f ;
            //pull
            //frontFace.localScale += new Vector3(-1f, 0, 0);
            //frontFace.position -= frontFace.right *.5f ;

            //6 right
            //push
            //frontFace.localScale += new Vector3(1f, 0, 0);
            //frontFace.position += frontFace.right *.5f ;
            //pull
            //frontFace.localScale += new Vector3(-1f, 0, 0);
            //frontFace.position += frontFace.right *.5f ;

            /*
            foreach(Transform child in transform)
            {
                if (child != frontFace)
                {
                    Vector3 newpos = child.localPosition;
                    newpos.z -= extendAmount / 2f;
                    child.localPosition = newpos;

                    child.localScale = Vector3.one;
                }
            }*/
          /*if (isToolActive)
            {
                /* GetComponent<MeshFilter>();
                  originalMesh = GetComponent<MeshFilter>().sharedMesh;
                  modifiedMesh = Instantiate(originalMesh);*/

             /* originalMesh = HandleFaceSelection();
                modifiedMesh = Instantiate(originalMesh);
                Debug.LogError("step 5 ");
                HandleExtrusionInput();
                Debug.LogError("step 6");

                modifiedMesh = DeformMesh(modifiedMesh, selectedFaces, extrusionDistance, isPushing);
                Debug.LogError("step 7 ");
                GetComponent<MeshFilter>().mesh = modifiedMesh;
                Debug.LogError("step 8 ");
                isToolActive = !isToolActive;
            }*/
        }
    }

 public void ToggleTool()
    {
        Debug.LogError("start");
        //isToolActive = !isToolActive;
        Debug.LogError("Tool Active state: " + isToolActive);
        //Debug.Log("Tool Active state: " + isToolActive);

    }

  /*public Mesh HandleFaceSelection()
    {
        Debug.LogError("hhhhhhhhhhhhhandle faaaaaaaaaace ");
        if (Input.GetMouseButtonDown(0))
        {

            // GetComponent<MeshFilter>();
            // originalMesh = GetComponent<MeshFilter>().sharedMesh;
            // modifiedMesh = Instantiate(originalMesh);*/
        /*  Debug.LogError("we taaaaaaaaaaaaap ");
            Ray ray = selectionCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                Debug.LogError("haaaaaaaaaaaanlde face ");
                originalMesh = GetComponent<MeshFilter>().sharedMesh;
                MeshFilter meshFilter = hit.collider.GetComponent<MeshFilter>();
                if (meshFilter != null)
                {
                    Mesh mesh = meshFilter.sharedMesh;
                    int[] triangles = mesh.triangles;
                    Vector3[] vertices = mesh.vertices;

                    int faceIndex = FindFaceIndex(hit.point, vertices, triangles);
                    if (faceIndex != -1)
                    {
                        selectedFaceIndex = faceIndex;
                        UpdateSelectedFaces(faceIndex, selectedFaces);
                    }
                }
            }
        }
        return originalMesh;
    }*/

  /*void HandleExtrusionInput()
    {
        Debug.LogError("haaaaaaaaaaaanlde exttttttttttt ");

        if (Input.GetMouseButtonDown(0))
        {
            Debug.LogError("secooooooond taaaaaaaaaaap ");
            isMouseDragging = true;
            initialMousePosition = Input.mousePosition;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            isMouseDragging = false;
        }

        if (isMouseDragging)
        {
            Vector3 currentMousePosition = Input.mousePosition;
            Vector3 mouseDelta = currentMousePosition - initialMousePosition;
            extrusionDistance = mouseDelta.y * 0.01f;
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            isPushing = false;
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            isPushing = true;
        }
    }*/

    Mesh DeformMesh(Mesh mesh, List<int> selectedFaces, float extrusionDistance, bool isPushing)
    {
        // No need to instantiate a new mesh, modify the existing one.
        Vector3[] vertices = mesh.vertices; // use existing vertices
        Vector3[] normals = mesh.normals;
        int[] triangles = mesh.triangles;

        Vector3 extrusionDirection = CalculateExtrusionDirection(selectedFaces, vertices, normals);

        HashSet<int> uniqueVertices = new HashSet<int>(); // To prevent modifying the same vertex multiple times.

        for (int i = 0; i < selectedFaces.Count; i++)
        {
            int faceIndex = selectedFaces[i];
            for (int j = 0; j < 3; j++) // A face has 3 vertices.
            {
                int vertexIndex = triangles[faceIndex * 3 + j];
                // Check if this vertex has already been modified.
                if (!uniqueVertices.Contains(vertexIndex))
                {
                    uniqueVertices.Add(vertexIndex); // Mark this vertex as modified.

                    if (isPushing)
                    {
                        vertices[vertexIndex] += extrusionDirection * extrusionDistance;
                    }
                    else
                    {
                        vertices[vertexIndex] -= extrusionDirection * extrusionDistance;
                    }
                }
            }
        }

        mesh.vertices = vertices; // Assign the modified vertices back to the mesh.
        mesh.RecalculateNormals(); // Recalculate normals to reflect the changes in the mesh.
        mesh.RecalculateBounds(); // Recalculate the bounds of the mesh for optimization.

        return mesh; // Return the modified mesh.
    }


    Vector3 CalculateExtrusionDirection(List<int> selectedFaces, Vector3[] vertices, Vector3[] normals)
    {
        Vector3 averageNormal = Vector3.zero;
        foreach (int faceIndex in selectedFaces)
        {
            int vertexIndex = faceIndex * 3;
            averageNormal += normals[vertexIndex];
        }
        averageNormal.Normalize();

        return averageNormal;
    }

    int FindFaceIndex(Vector3 point, Vector3[] vertices, int[] triangles)
    {
        for (int i = 0; i < triangles.Length; i += 3)
        {
            Vector3 v1 = vertices[triangles[i]];
            Vector3 v2 = vertices[triangles[i + 1]];
            Vector3 v3 = vertices[triangles[i + 2]];

            if (PointInTriangle(point, v1, v2, v3))
            {
                return i / 3;
            }
        }

        return -1;
    }

    bool PointInTriangle(Vector3 point, Vector3 v1, Vector3 v2, Vector3 v3)
    {
        Vector3 v1v2 = v2 - v1;
        Vector3 v1v3 = v3 - v1;
        Vector3 v1p = point - v1;

        float dot00 = Vector3.Dot(v1v2, v1v2);
        float dot01 = Vector3.Dot(v1v2, v1v3);
        float dot02 = Vector3.Dot(v1v2, v1p);
        float dot11 = Vector3.Dot(v1v3, v1v3);
        float dot12 = Vector3.Dot(v1v3, v1p);

        float inverDeno = 1 / (dot00 * dot11 - dot01 * dot01);

        float u = (dot11 * dot02 - dot01 * dot12) * inverDeno;
        if (u < 0 || u > 1)
            return false;

        float v = (dot00 * dot12 - dot01 * dot02) * inverDeno;
        if (v < 0 || v > 1)
            return false;

        return u + v <= 1;
    }

    void UpdateSelectedFaces(int faceIndex, List<int> selectedFaces)
    {
        if (!selectedFaces.Contains(faceIndex))
        {
            selectedFaces.Add(faceIndex);
        }
        else
        {
            selectedFaces.Remove(faceIndex);
        }
    }
 bool WouldIntersectRay(Vector3 newPosition)
    {
        RaycastHit hit;
        if (Physics.Raycast(newPosition, frontFace.forward, out hit, 0.5f))
        {
            if (hit.transform != frontFace)
            {
                return true; // Intersection detected
            }
        }
        return false;
    }
}