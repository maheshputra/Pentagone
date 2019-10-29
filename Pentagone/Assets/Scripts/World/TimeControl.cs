using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeControl : MonoBehaviour
{
    public static TimeControl instance;
    [Range(0,0.5f)]
    [SerializeField] private float stopTime;

    private void Awake()
    {
        if (instance != null)
            Destroy(gameObject);
        else
            instance = this;
    }

    public void DamagedTime() {
        StartCoroutine(ScaleTime2(0, 1, stopTime));
    }

    /// <summary>
    /// lerping
    /// </summary>
    /// <param name="start"></param>
    /// <param name="end"></param>
    /// <param name="time"></param>
    /// <returns></returns>
    IEnumerator ScaleTime(float start, float end, float time)
    {
        float lastTime = Time.realtimeSinceStartup;
        float timer = 0.0f;

        while (timer < time)
        {
            Time.timeScale = Mathf.Lerp(start, end, timer / time);
            timer += (Time.realtimeSinceStartup - lastTime);
            lastTime = Time.realtimeSinceStartup;
            yield return null;
        }

        Time.timeScale = end;
    }

    /// <summary>
    /// tidak lerping
    /// </summary>
    /// <param name="start"></param>
    /// <param name="end"></param>
    /// <param name="time"></param>
    /// <returns></returns>
    IEnumerator ScaleTime2(float start, float end, float time)
    {
        float lastTime = Time.realtimeSinceStartup;
        float timer = 0.0f;

        Time.timeScale = start;
        while (timer < time)
        {
            timer += (Time.realtimeSinceStartup - lastTime);
            lastTime = Time.realtimeSinceStartup;
            yield return null;
        }

        Time.timeScale = end;
    }

}
