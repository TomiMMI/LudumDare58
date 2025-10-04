using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class UIHandling : MonoBehaviour
{
    public Transform TransformScreen2;
    public Transform TransformScreen1;

    public Camera Cam;
    public float CameraMoveDuration;
    private void Start()
    {
        Cam = Camera.main;
        
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            _OnLeftScreenRightArrowPressed(null);
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            _OnRightScreenLeftArrowPressed(null);
        }
    }

    public void _OnLeftScreenRightArrowPressed(Button button)
    {
        //MOVE TO SCREEN TWO
        Cam.transform.DOMove( new Vector3(TransformScreen2.position.x, TransformScreen2.position.y,Cam.transform.position.z), CameraMoveDuration).SetEase(Ease.OutQuart);
        //TODO : add checks to remove the current gem from the player drag
    
    }


    public void _OnRightScreenLeftArrowPressed(Button button)
    {
        //MOVE TO SCREEN ONE
        Cam.transform.DOMove(new Vector3(TransformScreen1.position.x, TransformScreen1.position.y, Cam.transform.position.z), CameraMoveDuration).SetEase(Ease.OutQuart);

    }

}
