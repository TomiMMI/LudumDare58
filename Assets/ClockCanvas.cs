using UnityEngine;
using UnityEngine.UI;

public class ClockCanvas : MonoBehaviour
{
    [SerializeField]
    SpawnLoop spawnLoop;

    [SerializeField]
    private Image m_Clockimage;

    [SerializeField]
    private float m_StartClockZRotation = 95;
    [SerializeField]
    private float m_EndClockZRotation = -95;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        float LerpAngle = Mathf.Lerp(m_StartClockZRotation, m_EndClockZRotation, spawnLoop.GetLerpedTime());
        m_Clockimage.transform.localEulerAngles = new Vector3(m_Clockimage.transform.localEulerAngles.x, m_Clockimage.transform.localEulerAngles.y, LerpAngle);
    }
}
