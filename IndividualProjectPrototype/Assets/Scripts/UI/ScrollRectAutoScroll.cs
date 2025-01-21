using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class ScrollRectAutoScroll : MonoBehaviour
{
    public ScrollRect scrollRect;
    public RectTransform contentRect;
    public Scrollbar verticalScrollBar;
    private void Awake()
    {
        scrollRect = GetComponent<ScrollRect>();
        if (scrollRect == null)
        {
            Debug.LogWarning("ScrollRect component is missing on this GameObject. Please add a ScrollRect component.");
            enabled = false; // Disable this script if no ScrollRect is found
            return;
        }
        else
        {
            if (scrollRect.content != null)
            {
                if (contentRect == null)
                {
                    contentRect = scrollRect.content;
                }
            }
            if(scrollRect.verticalScrollbar != null)
            {
                verticalScrollBar = scrollRect.verticalScrollbar;
            }
            else
            {
                Debug.LogWarning("Content component of scrollRect is missing on this GameObject. Please add a ScrollRect component.");
            }
        }


    }
    void Update()
    {
        // Check if there's a selected GameObject and if it's a child of the content
        GameObject selectedObject = EventSystem.current.currentSelectedGameObject;
        if (selectedObject != null && selectedObject.transform.IsChildOf(contentRect))
        {
            ScrollToSelected(selectedObject.GetComponent<RectTransform>());
        }
    }


    private void ScrollToSelected(RectTransform target)
    {
        // Check if the vertical scrollbar is assigned
        if (verticalScrollBar == null) return;

        // Calculate the target's position in the local space of contentRect
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            contentRect,
            target.position,
            scrollRect.GetComponentInParent<Canvas>().worldCamera,
            out localPoint
        );

        // Extract necessary dimensions
        float contentHeight = contentRect.rect.height;
        float viewportHeight = scrollRect.viewport.rect.height;
        float localPosition = localPoint.y;
        float anchoredPosition = Mathf.Abs(contentRect.anchoredPosition.y);
        float targetPositionInContent = localPosition;

        // Calculate the normalized scroll position
        
        float normalizedPosition = Mathf.Clamp01(((targetPositionInContent + Mathf.Sign(targetPositionInContent) * viewportHeight) / (contentHeight - anchoredPosition)));

        

        // Set the scrollbar's value to scroll to the target
        verticalScrollBar.value = normalizedPosition;
    }

}
