using DG.Tweening;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class DragDropController : MonoBehaviour
{
    public enum DragState
    {
        None,
        Selecting,
        Dragging
    }
    public static DragDropController Instance { get; private set; }

    [SerializeField]
    private float timeToDrag = 0.2f;
    [SerializeField]
    private float dragSpeed = 100f;

        [SerializeField]
    private List<Rigidbody2D> selectedObjects;

    private Vector2 mouseLastWorldPosition;
    private float mouseDownTime;
    private DragState currentState = DragState.None;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        DragDropController.Instance = this;
        selectedObjects = new List<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (currentState)
        {
            case DragState.None:
                if (Input.GetMouseButtonDown(0))
                {
                    Rigidbody2D temp = this.GetGemAtMousePosition();
                    if (temp != null)
                    {
                        if (this.selectedObjects.Contains(temp))
                        {
                            this.currentState = DragState.Selecting;
                        }
                        else
                        {
                            this.UpdateSelectionList(temp);
                        }

                    }
                }
                break;
            case DragState.Selecting:
                mouseDownTime += Time.deltaTime;
                if (Input.GetMouseButtonUp(0))
                {
                    this.UpdateSelectionList(GetGemAtMousePosition());
                    mouseDownTime = 0;
                    this.currentState = DragState.None;
                }
                else if (mouseDownTime >= timeToDrag)
                {
                    mouseDownTime = 0;
                    mouseLastWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

                    foreach (Rigidbody2D obj in selectedObjects)
                    {
                        if(obj == null)
                        {
                            selectedObjects.Remove(obj);
                        }
                        obj.gameObject.layer = LayerMask.NameToLayer("Selected Gems");

                        obj.bodyType = RigidbodyType2D.Dynamic;
                    }
                    this.currentState = DragState.Dragging;
                }
                break;
            case DragState.Dragging:
                if (Input.GetMouseButtonUp(0))
                {
                    this.currentState = DragState.None;
                    this.StopDragging();
                }
                else
                {
                    this.Drag();
                }
                break;
        }
    }

    private void Drag()
    {
        Vector2 moveDelta = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition) - mouseLastWorldPosition;
        foreach (Rigidbody2D obj in selectedObjects.ToList())
        {
            if(obj == null)
            {
                selectedObjects.Remove(obj);
                continue;
            }
            obj.linearVelocity = (moveDelta + ((Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition) - (Vector2)obj.transform.position)) * Time.deltaTime * dragSpeed;
        }
        this.mouseLastWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }


    private bool UpdateSelectionList(Rigidbody2D newSelection)
    {
        if (selectedObjects.Contains(newSelection))
        {
            newSelection.transform.DOScale(1,0.2f);
            selectedObjects.Remove(newSelection);
            return false;
        }
        else
        {
            newSelection.transform.DOScale(1.1f,0.2f);
            selectedObjects.Add(newSelection);
            return true;
        }
    }

    private Rigidbody2D GetGemAtMousePosition()
    {
        Ray temp = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D[] hitInfo = Physics2D.GetRayIntersectionAll(temp, Mathf.Infinity);
        foreach(RaycastHit2D hit in hitInfo)
        {
            if (hit.transform != null && hit.transform.GetComponent<Gem>() is Gem gem && gem != null &&  hit.transform.GetComponent<Rigidbody2D>() is Rigidbody2D rb && rb != null)
            {
                return rb;
            }

        }
        return null;
    }

    private void StopDragging()
    {
        foreach (Rigidbody2D rb in selectedObjects)
        {
            rb.transform.DOScale(1, 0.2f);
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;
            rb.gameObject.layer = LayerMask.NameToLayer("Default");
        }
        this.selectedObjects.Clear();
    }
}
