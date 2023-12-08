using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Joystick : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    public static Joystick instance = null;

    public bool active = false;

    public Vector3 direction { get { return new Vector3(input.x, 0f, input.y); } }

    [SerializeField] private RectTransform baseRect;
    [SerializeField] private RectTransform backgroundRect;
    [SerializeField] private RectTransform stickRect;

    private Canvas canvas;

    public Vector2 input = Vector2.zero;

    [SerializeField] private float range;
    [SerializeField] private float deadZone;

    public Camera cam;
    private GameObject backgroundGameobject;

    public void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(this);

        canvas = GetComponentInParent<Canvas>();

        backgroundGameobject = backgroundRect.gameObject;
        if(canvas == null ) 
        {
            Debug.LogError("!!! Джойстик находится вне объекта Canvas !!!");
        }

        Vector2 center = Vector2.one / 2;

        backgroundRect.pivot = center;
        stickRect.anchorMin = center;
        stickRect.anchorMax = center;
        stickRect.pivot = center;
        stickRect.anchoredPosition = Vector2.zero;

        backgroundGameobject.SetActive(false);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (active)
        {
        backgroundRect.anchoredPosition = ScreenPointToAnchoredPosition(eventData.position);
        backgroundGameobject.SetActive(true);

        OnDrag(eventData);
        }

    }

    public void OnPointerUp(PointerEventData eventData) 
    {
        backgroundGameobject.SetActive(false);

        input = Vector2.zero;
        stickRect.anchoredPosition = Vector2.zero;
    }

    public void OnDrag(PointerEventData eventData)
    {
        cam = cam ?? eventData.pressEventCamera;

        Vector2 position = RectTransformUtility.WorldToScreenPoint(cam, backgroundRect.position);
        Vector2 radius = backgroundRect.sizeDelta / 2;

        input = (eventData.position - position) / (radius * canvas.scaleFactor);

        if (input.magnitude > deadZone)
        {
            if(input.magnitude > 1)
            {
                input = input.normalized;
            }  
        }
            
        else
            input = Vector2.zero;

        stickRect.anchoredPosition = input * radius * range;
    }

    protected Vector2 ScreenPointToAnchoredPosition(Vector2 screenPosition)
    {
        Vector2 localPoint = Vector2.zero;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(baseRect, screenPosition, cam, out localPoint))
        {
            Vector2 pivotOffset = baseRect.pivot * baseRect.sizeDelta;
            return localPoint - (backgroundRect.anchorMax * baseRect.sizeDelta) + pivotOffset;
        }
        return Vector2.zero;
    }

    public bool IsCentered()
    {
        if (input == Vector2.zero) return true;
        else return false;
    }
}
