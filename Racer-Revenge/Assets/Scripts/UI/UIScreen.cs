using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIScreen : MonoBehaviour
{
    public virtual void Open()
    {
        gameObject.SetActive(true);
    }
    public virtual void Close()
    {
        gameObject.SetActive(false);
    }
    protected void AddPointerDownTrigger(Button targetButton, UnityAction<BaseEventData> onDownEvent)
    {
        EventTrigger trigger = targetButton.gameObject.AddComponent<EventTrigger>();
        var pointerDown = new EventTrigger.Entry();
        pointerDown.eventID = EventTriggerType.PointerDown;
        pointerDown.callback.AddListener(onDownEvent);
        trigger.triggers.Add(pointerDown);
    }
}
