using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
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

    bool isResetting = false;

    int currentDay = 1;

    public TextMeshProUGUI clockText;

    public AudioSource audioSource;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(ResetTime());
    }

    // Update is called once per frame
    void Update()
    {
        if(!isResetting && spawnLoop.shouldReset)
        {
            isResetting = true;
            StartCoroutine(ResetTime());
        }
        else if(!isResetting)
        {
            float LerpAngle = Mathf.Lerp(m_StartClockZRotation, m_EndClockZRotation, spawnLoop.GetLerpedTime());
            m_Clockimage.transform.localEulerAngles = new Vector3(m_Clockimage.transform.localEulerAngles.x, m_Clockimage.transform.localEulerAngles.y, LerpAngle);

        }
    }
    public IEnumerator ResetTime()
    {
        m_Clockimage.transform.DORotate(new Vector3(0,0, m_StartClockZRotation+ 365*5), 3, RotateMode.FastBeyond360);
        Camera.main.DOShakePosition(3,0.05f, 25);
        yield return new WaitForSeconds(3);
        if(audioSource != null)
        {
            audioSource.Play();
        }
        clockText.text = "DAY " + currentDay;
        currentDay++;
        PlayerStats.Instance.currentDay += 1;
        isResetting = false;
        spawnLoop.ResetDay();
    }
}
