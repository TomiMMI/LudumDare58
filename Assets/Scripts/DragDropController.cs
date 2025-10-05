using DG.Tweening;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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
    private Rigidbody2D selectedObject;

    private Vector2 mouseLastWorldPosition;
    private float mouseDownTime;
    private DragState currentState = DragState.None;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        DragDropController.Instance = this;
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
                        selectedObject = temp;
                        this.currentState = DragState.Dragging;
                        mouseLastWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                        selectedObject.gameObject.layer = LayerMask.NameToLayer("Selected Gems");
                        selectedObject.transform.DOScale(1.1f, 0.2f);

                    }
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

            selectedObject.linearVelocity = (moveDelta + ((Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition) - (Vector2)selectedObject.transform.position)) * Time.deltaTime * dragSpeed;
        this.mouseLastWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    private Rigidbody2D GetGemAtMousePosition()
    {
        Ray temp = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D[] hitInfo = Physics2D.GetRayIntersectionAll(temp, Mathf.Infinity);
        foreach (RaycastHit2D hit in hitInfo)
        {
            if (hit.transform != null && hit.transform.GetComponent<Gem>() is Gem gem && gem != null && hit.transform.GetComponent<Rigidbody2D>() is Rigidbody2D rb && rb != null)
            {
                return rb;
            }

        }
        return null;
    }

    private void StopDragging()
    {

        selectedObject.transform.DOScale(1, 0.2f);
        selectedObject.linearVelocity = Vector2.zero;
        selectedObject.angularVelocity = 0f;
        selectedObject.gameObject.layer = LayerMask.NameToLayer("Default");
        selectedObject = null;
    }
}
