using UnityEngine;
public class ScreenBordersCollidersSpawner : MonoBehaviour
{
    private const string BORDER_TAG = "Border";
    private void Start()
    {
        var camera = Camera.main;
        Vector2 lDCorner = camera.ViewportToWorldPoint(new Vector3(0f, 0f, camera.nearClipPlane));
        Vector2 rUCorner = camera.ViewportToWorldPoint(new Vector3(1f, 1f, camera.nearClipPlane));
        Vector2[] colliderpoints;
        var upperEdgeGO = new GameObject("upperEdge");
        upperEdgeGO.tag = BORDER_TAG;
        EdgeCollider2D upperEdge = upperEdgeGO.AddComponent<EdgeCollider2D>();
        colliderpoints = upperEdge.points;
        colliderpoints[0] = new Vector2(lDCorner.x, rUCorner.y);
        colliderpoints[1] = new Vector2(rUCorner.x, rUCorner.y);
        upperEdge.points = colliderpoints;
        var lowerEdgeGO = new GameObject("lowerEdge");
        lowerEdgeGO.tag = BORDER_TAG;
        EdgeCollider2D lowerEdge = lowerEdgeGO.AddComponent<EdgeCollider2D>();
        colliderpoints = lowerEdge.points;
        colliderpoints[0] = new Vector2(lDCorner.x, lDCorner.y);
        colliderpoints[1] = new Vector2(rUCorner.x, lDCorner.y);
        lowerEdge.points = colliderpoints;
        var leftEdgeGO = new GameObject("leftEdge");
        leftEdgeGO.AddComponent<Border>().side = ScreenSide.Left;
        leftEdgeGO.tag = BORDER_TAG;
        var leftEdge = leftEdgeGO.AddComponent<EdgeCollider2D>();
        leftEdge.isTrigger = true;
        colliderpoints = leftEdge.points;
        colliderpoints[0] = new Vector2(lDCorner.x, lDCorner.y);
        colliderpoints[1] = new Vector2(lDCorner.x, rUCorner.y);
        leftEdge.points = colliderpoints;
        var rightEdgeGO = new GameObject("rightEdge");
        rightEdgeGO.AddComponent<Border>().side = ScreenSide.Right;
        rightEdgeGO.tag = BORDER_TAG;
        var rightEdge = rightEdgeGO.AddComponent<EdgeCollider2D>();
        rightEdge.isTrigger = true;
        colliderpoints = rightEdge.points;
        colliderpoints[0] = new Vector2(rUCorner.x, rUCorner.y);
        colliderpoints[1] = new Vector2(rUCorner.x, lDCorner.y);
        rightEdge.points = colliderpoints;
    }
}