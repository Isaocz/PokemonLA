using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ScrollChild : MonoBehaviour, IDragHandler , IBeginDragHandler , IEndDragHandler , IScrollHandler
{
    private ScrollRect upperscroll;

    private void Awake()
    {
        Transform parent = transform.parent;
        
        if (parent)
        {
            upperscroll = parent.GetComponentInParent<ScrollRect>();
            while (upperscroll == null && parent.parent != null) {
                parent = parent.parent;
                upperscroll = parent.GetComponentInParent<ScrollRect>();
            }
            
        }
    }

    /// <summary>
    /// ¿ªÊ¼ÍÏ×§
    /// </summary>
    /// <param name="eventData"></param>

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (upperscroll != null)
        {
            upperscroll.OnBeginDrag(eventData);
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (upperscroll != null)
        {
            upperscroll.OnDrag(eventData);
        }


    }

    /// <summary>
    /// ½áÊøÍÏ×§
    /// </summary>
    /// <param name="eventData"></param>
    public void OnEndDrag(PointerEventData eventData)
    {
        if (upperscroll != null)
        {
            upperscroll.OnEndDrag(eventData);
        }
    }

    public void OnScroll(PointerEventData eventData)
    {
        if (upperscroll != null)
        {
            upperscroll.OnScroll(eventData);
        }
    }


}
