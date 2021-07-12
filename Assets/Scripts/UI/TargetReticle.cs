using System.Collections.Generic;
using UnityEngine;

public class TargetReticle : MonoBehaviour
{
    [SerializeField]
    private RectTransform rectTransform;
    [SerializeField]
    private GameObject debugTarget;

    [Header("Settings")]
    [SerializeField]
    private Vector2 minSize;

    private Vector2 ToScreen(Vector3 worldPoint) => Camera.main.WorldToScreenPoint(worldPoint);

    private void Update()
    {
        RecalculateBounds(debugTarget);
    }

    private void RecalculateBounds(GameObject newSelection)
    {
        Bounds selectionBounds = CalculateSelectionBounds(newSelection);

        IEnumerable<Vector2> boundPoints = new List<Vector2>
        {
            ToScreen(selectionBounds.min),
            ToScreen(selectionBounds.max),
            ToScreen(new Vector3(selectionBounds.min.x, selectionBounds.min.y, selectionBounds.max.z)),
            ToScreen(new Vector3(selectionBounds.min.x, selectionBounds.max.y, selectionBounds.min.z)),
            ToScreen(new Vector3(selectionBounds.max.x, selectionBounds.min.y, selectionBounds.min.z)),
            ToScreen(new Vector3(selectionBounds.min.x, selectionBounds.max.y, selectionBounds.max.z)),
            ToScreen(new Vector3(selectionBounds.max.x, selectionBounds.min.y, selectionBounds.max.z)),
            ToScreen(new Vector3(selectionBounds.max.x, selectionBounds.max.y, selectionBounds.min.z))
        };
        
        Vector2 min = Vector2.positiveInfinity;
        Vector2 max = Vector2.negativeInfinity;
        foreach (Vector2 boundPoint in boundPoints)
        {
            if (boundPoint.x < min.x) min.x = boundPoint.x;
            if (boundPoint.y < min.y) min.y = boundPoint.y;
            if (boundPoint.x > max.x) max.x = boundPoint.x;
            if (boundPoint.y > max.y) max.y = boundPoint.y;
        }
        
        Vector2 center = ToScreen(selectionBounds.center);

        rectTransform.anchoredPosition = center;
        rectTransform.sizeDelta = Vector2.Max(max - min, minSize);
    }

    private Bounds CalculateSelectionBounds(GameObject selection)
    {
        var renderers = selection.GetComponentsInChildren<Renderer>();
        var combinedBounds = renderers[0].bounds;

        for (int i = 1; i < renderers.Length; i++)
        {
            combinedBounds.Encapsulate(renderers[i].bounds);
        }

        return combinedBounds;
    }
}
