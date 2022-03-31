using UnityEngine;
public class SafeArea : MonoBehaviour
{
    [SerializeField] private bool isGameCanvas;
    private RectTransform Panel;
    private ScreenOrientation screenOrientation;
    private void Awake()
    {
        Panel = GetComponent<RectTransform>();
        UpdateArea();
        screenOrientation = Screen.orientation;
    }
    private void UpdateArea()
    {
        var safeArea = Screen.safeArea;
        if (safeArea != Rect.zero)
        {
            Vector2 anchorMin = new Vector2(safeArea.position.x, 0f);
            Vector2 anchorMax = new Vector2(Screen.width , 1f);
            anchorMin.x /= Screen.width;
            anchorMax.x /= Screen.width;
            Panel.anchorMin = anchorMin;
            Panel.anchorMax = isGameCanvas ? anchorMax - anchorMin : anchorMax;
        }
    }
    private void Update()
    {
        if (isGameCanvas) return;
        var currentOrientation = Screen.orientation;
        if (screenOrientation != currentOrientation)
        {
            screenOrientation = currentOrientation;
            UpdateArea();
        }
    }

}