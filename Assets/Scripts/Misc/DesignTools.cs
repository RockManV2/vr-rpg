
#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

public class DesignTools : MonoBehaviour
{

    [MenuItem("GameObject Tools/Snap to ground %g")]
    private static void SnapToGround()
    {
        foreach (GameObject gameObject in Selection.gameObjects)
        {
            Physics.Raycast(gameObject.transform.position, Vector3.down, out var hit);

            if (hit.transform == null) return;
        
            Vector3 pointPosition = hit.point;
            Quaternion pointQuaternion = Quaternion.FromToRotation(Vector3.up, hit.normal);

            gameObject.transform.position = pointPosition;
            gameObject.transform.rotation = pointQuaternion;
        }
    }
    
    [MenuItem("GameObject Tools/Move to cursor #g")]
    private static void MoveToCursor()
    {
        Vector2 mousePos = Event.current.mousePosition;
        mousePos.y = Camera.current.pixelHeight - mousePos.y;
        Ray ray = Camera.current.ScreenPointToRay(mousePos);

        Physics.Raycast(ray, out var hit);

        GameObject gameObject = Selection.gameObjects[0];
        
        gameObject.transform.position =  hit.point;
    }
    
}
#endif