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
            Vector2 anchorMin = safeArea.position;
            Vector2 anchorMax = safeArea.position + safeArea.size;
            anchorMin.x /= Screen.width;
            anchorMin.y /= Screen.height;
            anchorMax.x /= Screen.width;
            anchorMax.y /= Screen.height;
            Panel.anchorMin = anchorMin;
            Panel.anchorMax = isGameCanvas ?  anchorMax - anchorMin : anchorMax;
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