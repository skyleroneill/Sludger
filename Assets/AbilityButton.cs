using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class AbilityButton : MonoBehaviour, ISelectHandler
{
    [System.Serializable]
    public class OnSelectEvent : UnityEvent { }
    [SerializeField]
    private OnSelectEvent onSelect = new OnSelectEvent();
    public OnSelectEvent onSelectEvent { get { return onSelect; } set { onSelect = value; } }

    public void OnSelect(BaseEventData eventData)
    {
        SelectEventTriggered();
    }

    public void SelectEventTriggered()
    {
        onSelectEvent.Invoke();
    }
}
