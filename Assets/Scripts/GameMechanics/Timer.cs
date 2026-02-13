using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Timer : MonoBehaviour {

    [SerializeField] float waitTimeBeforeStart=0;
    [SerializeField] float regularIntervalTime;

    public UnityEvent TimerStart, WhileTimer, TimerComplete, RegularTimeIntervalEvent;
    public UnityEvent<float> whileTimerPercentComplete;

    public void SetTimerByDuration(float duration) {
        TimerStart.Invoke();
        StartCoroutine(EventTimer(duration));
    }

    public void SetTimerByIntervalCount(int intervalCount) {
        TimerStart.Invoke();
        StartCoroutine(EventTimer(intervalCount * regularIntervalTime));
    }

    IEnumerator EventTimer(float Duration) {
        float elapsedTime = 0;

        yield return new WaitForSeconds(waitTimeBeforeStart);

        while (elapsedTime < Duration) {
            WhileTimer.Invoke(); whileTimerPercentComplete.Invoke(Mathf.Clamp01(elapsedTime/Duration));
            elapsedTime += Time.deltaTime; 

            //Regular time interval event
            if (elapsedTime%regularIntervalTime < Time.deltaTime ) { RegularTimeIntervalEvent.Invoke(); }

            yield return null;
        }

        TimerComplete.Invoke();
    }
}
