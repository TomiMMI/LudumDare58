using UnityEngine;
using System;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;

public class SuicidalText : MonoBehaviour
{
    private String text;

[SerializeField]
    private TMP_Text textMeshPro;

    private Vector2 movementVector;
    private Vector2 sizeVector;
    private float timer = 0f;
    private float lifetime;


    public void SetupText(String text, Vector2 movementVector, Vector2 sizeVector, float time)
    {
        this.textMeshPro.text = text;
        this.movementVector = movementVector;
        this.sizeVector = sizeVector;
        this.lifetime = time;

    }
    public void SetupText(String text, Vector2 movementVector, float time)
    {
        this.textMeshPro.text = text;
        this.movementVector = movementVector;
        this.lifetime = time;

    }
    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= lifetime)
        {
            Destroy(gameObject);
        }
        else
        {
            transform.position += (Vector3)(movementVector * Time.deltaTime);
            if (sizeVector != null)
            {
                transform.localScale += (Vector3)(sizeVector * Time.deltaTime);
            }
        }
    }
}