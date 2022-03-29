using UnityEngine;
public class ScreenBordersCollidersSpawner : MonoBehaviour
{
    [SerializeField] private bool registerCollision;
    private void Start()
    {
        var camera = Camera.main;
        var safeArea = Screen.safeArea;
        Vector2 lDCorner = camera.ScreenToWorldPoint(new Vector3(0f, safeArea.height, camera.nearClipPlane));
        Vector2 rUCorner = camera.ScreenToWorldPoint(new Vector3(safeArea.width, 0f, camera.nearClipPlane));
        Vector2[] colliderpoints;
        EdgeCollider2D upperEdge = new GameObject("upperEdge").AddComponent<EdgeCollider2D>();
        colliderpoints = upperEdge.points;
        colliderpoints[0] = new Vector2(lDCorner.x, lDCorner.y);
        colliderpoints[1] = new Vector2(rUCorner.x, lDCorner.y);
        upperEdge.points = colliderpoints;
        EdgeCollider2D lowerEdge = new GameObject("lowerEdge").AddComponent<EdgeCollider2D>();
        colliderpoints = lowerEdge.points;
        colliderpoints[0] = new Vector2(lDCorner.x, rUCorner.y);
        colliderpoints[1] = new Vector2(rUCorner.x, rUCorner.y);
        lowerEdge.points = colliderpoints;
        var leftEdgeGO = new GameObject("leftEdge");
        if (registerCollision)
        {
            leftEdgeGO.AddComponent<Border>().side = ScreenSide.Left;
            leftEdgeGO.tag = "Border";
        }
        var leftEdge = leftEdgeGO.AddComponent<EdgeCollider2D>();
        if (registerCollision)
        {
            leftEdge.isTrigger = true;
        }
        colliderpoints = leftEdge.points;
        colliderpoints[0] = new Vector2(lDCorner.x, lDCorner.y);
        colliderpoints[1] = new Vector2(lDCorner.x, rUCorner.y);
        leftEdge.points = colliderpoints;
        var rightEdgeGO = new GameObject("rightEdge");
        if (registerCollision)
        {
            rightEdgeGO.AddComponent<Border>().side = ScreenSide.Right;
            rightEdgeGO.tag = "Border";
        }
        var rightEdge = rightEdgeGO.AddComponent<EdgeCollider2D>();
        if (registerCollision)
        {
            rightEdge.isTrigger = true;
        }
        colliderpoints = rightEdge.points;
        colliderpoints[0] = new Vector2(rUCorner.x, rUCorner.y);
        colliderpoints[1] = new Vector2(rUCorner.x, lDCorner.y);
        rightEdge.points = colliderpoints;
    }
}