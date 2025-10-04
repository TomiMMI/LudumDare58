using System;
using System.Collections.Generic;
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
    private List<Transform> selectedObjects;

    private Vector2 mouseLastWorldPosition;
    private float mouseDownTime;
    private DragState currentState = DragState.None;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        DragDropController.Instance = this;
        selectedObjects = new List<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (currentState)
        {
            case DragState.None:
                if (Input.GetMouseButtonDown(0))
                {
                    Transform temp = this.GetGemAtMousePosition();
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

                    foreach (Transform obj in selectedObjects)
                    {
                        obj.gameObject.layer = LayerMask.NameToLayer("Selected Gems");
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
        foreach (Transform obj in selectedObjects)
        {
            obj.GetComponent<Rigidbody2D>().linearVelocity = (moveDelta + ((Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition) - (Vector2)obj.transform.position)) * Time.deltaTime * dragSpeed;
            Debug.Log(obj.GetComponent<Rigidbody2D>().linearVelocity);
        }
        this.mouseLastWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }


    private bool UpdateSelectionList(Transform newSelection)
    {
        if (selectedObjects.Contains(newSelection))
        {
            selectedObjects.Remove(newSelection);
            return false;
        }
        else
        {
            selectedObjects.Add(newSelection);
            return true;
        }
    }

    private Transform GetGemAtMousePosition()
    {
        Ray temp = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hitInfo = Physics2D.GetRayIntersection(temp, Mathf.Infinity);
        if (hitInfo.transform != null)
        {
            return hitInfo.transform;
        }
        return null;
    }

    private void StopDragging()
    {
        foreach (Transform obj in selectedObjects)
        {
            obj.GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;
            obj.GetComponent<Rigidbody2D>().angularVelocity = 0f;
            obj.gameObject.layer = LayerMask.NameToLayer("Default");
        }
        this.selectedObjects.Clear();
    }
}
