using UnityEngine;

public class UIHandler : MonoBehaviour
{
    public MeshManipulator meshManipulator;

    public void OnPushPullButtonPressed()
    {
        float pushDistance = 1.0f; // Set this to the desired push/pull distance
        meshManipulator.PushPullFace(pushDistance);
    }
}
