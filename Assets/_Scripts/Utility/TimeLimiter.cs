using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeLimiter
{
    private float m_timeInterval;
    private float m_lastTime;

    public TimeLimiter(float timeInterval)
    {
        m_timeInterval = timeInterval;
    }

    public bool IsEnoughTimePassed()
    {
        if (Time.time - m_lastTime > m_timeInterval)
        {
            m_lastTime = Time.time;
            return true;
        }
        else return false;
    }
}
