using System.Collections.Generic;
using Pixelplacement;
using UnityEngine;
using UnityEngine.UI;

public class TargetReticle : MonoBehaviour
{
    [SerializeField]
    private RectTransform rectTransform;
    [SerializeField]
    private Image image;

    [Header("Settings")]
    [SerializeField]
    private Vector2 minSize;
    [SerializeField]
    private Vector2 scalePadding;

    private Bounds bounds;
    private RectTransform canvas;
    private Vector2 currentPadding;
    private GameObject currentTarget;
    private GameObject previousTarget;

    private void Start()
    {
        canvas = GetComponentInParent<Canvas>().GetComponent<RectTransform>();
        currentPadding = scalePadding;
    }

    private void Update()
    {
        if (image.enabled && currentTarget != null)
        {
            RecalculateBounds(currentTarget);
        }
    }

    public void SetTarget(GameObject target)
    {
        previousTarget = currentTarget;
        currentTarget = target;
        
        if (currentTarget != null && currentTarget != previousTarget)
        {
            OnTargetChanged();
        }
        else if (currentTarget == null && previousTarget != null)
        {
            OnTargetLost();
        }
    }

    private void OnTargetChanged()
    {
        RecalculateBounds(currentTarget);
        Vector2 startScale = canvas.sizeDelta * 2f;
        Vector2 endScale = scalePadding;

        rectTransform.sizeDelta = startScale;
        
        Tween.Value(startScale, endScale, OnTweenUpdateScale, .25f, 0f, Tween.EaseOutStrong, Tween.LoopType.None, () => image.enabled = true);
    }

    private void OnTargetLost()
    {
        Vector2 startScale = rectTransform.sizeDelta;
        Vector2 endScale = canvas.sizeDelta * 2f;
        
        Tween.Size(rectTransform, startScale, endScale, .15f, 0f, Tween.EaseInStrong, Tween.LoopType.None, null,
            () =>
            {
                image.enabled = false;
            });
    }

    private void OnTweenUpdateScale(Vector2 newScale)
    {
        currentPadding = newScale;
    }
    
    private Vector2 ToCanvasPosition(Vector3 worldPoint)
    {
        return Camera.main.WorldToViewportPoint(worldPoint) * canvas.sizeDelta;
    }

    private void RecalculateBounds(GameObject newSelection)
    {
        Bounds selectionBounds = CalculateSelectionBounds(newSelection);

        IEnumerable<Vector2> boundPoints = new List<Vector2>
        {
            ToCanvasPosition(selectionBounds.min),
            ToCanvasPosition(selectionBounds.max),
            ToCanvasPosition(new Vector3(selectionBounds.min.x, selectionBounds.min.y, selectionBounds.max.z)),
            ToCanvasPosition(new Vector3(selectionBounds.min.x, selectionBounds.max.y, selectionBounds.min.z)),
            ToCanvasPosition(new Vector3(selectionBounds.max.x, selectionBounds.min.y, selectionBounds.min.z)),
            ToCanvasPosition(new Vector3(selectionBounds.min.x, selectionBounds.max.y, selectionBounds.max.z)),
            ToCanvasPosition(new Vector3(selectionBounds.max.x, selectionBounds.min.y, selectionBounds.max.z)),
            ToCanvasPosition(new Vector3(selectionBounds.max.x, selectionBounds.max.y, selectionBounds.min.z))
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

        bounds = selectionBounds;

        Vector2 trueScale = max - min;
        rectTransform.sizeDelta = Vector2.Max(trueScale + currentPadding, minSize);
        rectTransform.anchoredPosition = min + trueScale / 2f;
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

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(bounds.center, bounds.size);
    }
}
