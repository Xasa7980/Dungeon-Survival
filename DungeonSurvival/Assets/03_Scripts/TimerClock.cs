using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerClock
{
    public float timerMax;
    public float timer;

    public TimerClock( float timerMax, float timer )
    {
        this.timerMax = timerMax;
        this.timer = timer;
    }

    public IEnumerator TimerWithActionAtStart(Action startAction = default, Action endAction = default)
    {
        startAction?.Invoke();
        yield return new WaitForSeconds( timerMax );
        endAction?.Invoke();
    }
    public IEnumerator Timer( Action startAction = default, Action endAction = default )
    {
        timer = 0;
        startAction?.Invoke();
        while(timer <=  timerMax)
        {
            timer += Time.deltaTime;
            yield return null;
        }
        endAction?.Invoke();
    }
}
