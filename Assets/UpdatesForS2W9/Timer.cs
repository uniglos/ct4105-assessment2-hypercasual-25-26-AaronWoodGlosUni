using System.Collections;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Timer
/// ---------------------------------------------------------------------------
/// PURPOSE
/// A reusable timer component which can:
/// - wait an optional delay before beginning
/// - run for a supplied duration
/// - run for (intervalCount * regularIntervalTime)
/// - raise UnityEvents while running
/// - raise a percentage-complete UnityEvent<float>
/// - raise a repeating "interval" event during the run
///
/// WHY THIS REWRITE EXISTS
/// The older version worked in simple cases, but had several structural risks:
///
/// 1. It started coroutines without checking whether the GameObject/component
///    was active and enabled.
/// 2. It did not stop existing coroutines before starting a new one, so
///    overlapping timers were possible.
/// 3. It did not stop timer activity during disable/teardown.
/// 4. It could divide by zero in the percent-complete calculation if duration
///    was zero.
/// 5. It used a modulo-based interval check that can behave inconsistently when
///    frame timing fluctuates.
///
/// In your project, a key issue was:
/// "Coroutine couldn't be started because the game object is inactive"
///
/// So this rewrite focuses first on SAFE LIFECYCLE BEHAVIOUR while preserving
/// the existing inspector event names.
///
/// LEGACY FIELD NAMES PRESERVED
/// - TimerStart
/// - WhileTimer
/// - TimerComplete
/// - RegularTimeIntervalEvent
/// - whileTimerPercentComplete
///
/// PUBLIC METHOD NAMES PRESERVED
/// - SetTimerByDuration(float duration)
/// - SetTimerByIntervalCount(int intervalCount)
/// </summary>
public class Timer : MonoBehaviour
{
    [Header("Timing Settings")]

    [Tooltip("Optional delay before the timer begins counting.")]
    [SerializeField] private float waitTimeBeforeStart = 0f;

    [Tooltip("Base interval length used by SetTimerByIntervalCount().")]
    [SerializeField] private float regularIntervalTime = 1f;

    [Header("Events (legacy names preserved for inspector compatibility)")]

    [Tooltip("Invoked once when a timer successfully starts.")]
    public UnityEvent TimerStart;

    [Tooltip("Invoked every frame while the timer is running.")]
    public UnityEvent WhileTimer;

    [Tooltip("Invoked once when the timer completes normally.")]
    public UnityEvent TimerComplete;

    [Tooltip("Invoked at each regular time interval while the timer is running.")]
    public UnityEvent RegularTimeIntervalEvent;

    [Tooltip("Invoked every frame with progress from 0 to 1.")]
    public UnityEvent<float> whileTimerPercentComplete;

    // ------------------------------------------------------------------------
    // INTERNAL STATE
    // ------------------------------------------------------------------------

    /// <summary>
    /// Reference to the currently running timer coroutine.
    /// Keeping this lets us stop an old timer cleanly before starting a new one.
    /// </summary>
    private Coroutine activeTimerCoroutine;

    /// <summary>
    /// The duration of the timer currently in progress.
    /// Mainly useful for diagnostics / future expansion.
    /// </summary>
    private float currentTimerDuration = 0f;

    // ------------------------------------------------------------------------
    // UNITY LIFECYCLE
    // ------------------------------------------------------------------------

    /// <summary>
    /// When this component or its GameObject is disabled, stop any running
    /// coroutine immediately.
    ///
    /// This is especially important during scene changes, menu returns, and
    /// teardown states.
    /// </summary>
    private void OnDisable()
    {
        StopActiveTimer(resetState: true);
    }

    /// <summary>
    /// Also clean up if the object is destroyed directly.
    /// In many cases OnDisable will already have happened first, but this keeps
    /// the behaviour explicit and safe.
    /// </summary>
    private void OnDestroy()
    {
        StopActiveTimer(resetState: true);
    }

    // ------------------------------------------------------------------------
    // PUBLIC API
    // ------------------------------------------------------------------------

    /// <summary>
    /// Starts a timer using an explicit duration in seconds.
    ///
    /// If another timer is already running on this component, it is stopped
    /// first so only one timer is active at a time.
    /// </summary>
    /// <param name="duration">How long the timer should run, in seconds.</param>
    public void SetTimerByDuration(float duration)
    {
        // Do not attempt to start coroutines on an inactive or disabled object.
        if (!CanStartTimer())
        {
            return;
        }

        // Negative durations are not meaningful for this timer.
        // Clamp to zero so behaviour is deterministic.
        duration = Mathf.Max(0f, duration);

        StartFreshTimer(duration);
    }

    /// <summary>
    /// Starts a timer where the total duration is:
    ///
    ///     intervalCount * regularIntervalTime
    ///
    /// This preserves the original public API used elsewhere in the project.
    /// </summary>
    /// <param name="intervalCount">How many intervals the timer should run for.</param>
    public void SetTimerByIntervalCount(int intervalCount)
    {
        // Do not attempt to start coroutines on an inactive or disabled object.
        if (!CanStartTimer())
        {
            return;
        }

        // Negative interval counts are not meaningful.
        intervalCount = Mathf.Max(0, intervalCount);

        // regularIntervalTime should also never be negative.
        float safeInterval = Mathf.Max(0f, regularIntervalTime);
        float duration = intervalCount * safeInterval;

        StartFreshTimer(duration);
    }

    /// <summary>
    /// Optional public helper to stop the current timer manually.
    /// This was not in the original script, but it is a useful addition for
    /// debugging and menu/game-state control.
    /// </summary>
    public void StopTimer()
    {
        StopActiveTimer(resetState: true);
    }

    // ------------------------------------------------------------------------
    // INTERNAL CONTROL
    // ------------------------------------------------------------------------

    /// <summary>
    /// Returns true only if the component is in a valid state to start a timer.
    /// This prevents the "Coroutine couldn't be started because the game object
    /// is inactive" error.
    /// </summary>
    private bool CanStartTimer()
    {
        // "isActiveAndEnabled" covers both:
        // - GameObject active in hierarchy
        // - MonoBehaviour enabled
        if (!isActiveAndEnabled)
        {
            return false;
        }

        return true;
    }

    /// <summary>
    /// Stops any previous timer, stores the new duration, fires TimerStart, and
    /// begins the new coroutine.
    /// </summary>
    private void StartFreshTimer(float duration)
    {
        // Ensure only one timer runs at once.
        StopActiveTimer(resetState: false);

        currentTimerDuration = duration;

        // Safe invoke in case no listeners are assigned.
        TimerStart?.Invoke();

        // Start the timer coroutine and store the reference.
        activeTimerCoroutine = StartCoroutine(EventTimer(duration));
    }

    /// <summary>
    /// Stops the currently running timer coroutine, if any.
    /// </summary>
    /// <param name="resetState">
    /// If true, clears internal tracking fields back to idle values.
    /// </param>
    private void StopActiveTimer(bool resetState)
    {
        if (activeTimerCoroutine != null)
        {
            StopCoroutine(activeTimerCoroutine);
            activeTimerCoroutine = null;
        }

        if (resetState)
        {
            currentTimerDuration = 0f;
        }
    }

    // ------------------------------------------------------------------------
    // CORE TIMER COROUTINE
    // ------------------------------------------------------------------------

    /// <summary>
    /// Main timer coroutine.
    ///
    /// FLOW
    /// 1. optional initial wait
    /// 2. handle zero-duration case cleanly
    /// 3. run frame-by-frame until elapsed time reaches duration
    /// 4. invoke per-frame events and interval events
    /// 5. invoke TimerComplete at the end
    /// </summary>
    private IEnumerator EventTimer(float duration)
    {
        float elapsedTime = 0f;

        // Tracks when the next interval event should fire.
        // Using a threshold like this is usually more stable than checking
        // "elapsed % interval".
        float nextIntervalTriggerTime = Mathf.Max(0f, regularIntervalTime);

        // ------------------------------------------------------------
        // OPTIONAL START DELAY
        // ------------------------------------------------------------
        if (waitTimeBeforeStart > 0f)
        {
            yield return new WaitForSeconds(waitTimeBeforeStart);
        }

        // The object may have been disabled during the wait.
        if (!isActiveAndEnabled)
        {
            CleanupAfterInterruptedTimer();
            yield break;
        }

        // ------------------------------------------------------------
        // ZERO-DURATION CASE
        // ------------------------------------------------------------
        // The old script could calculate elapsed / duration, which is unsafe if
        // duration is zero. Here we complete immediately in a controlled way.
        if (duration <= 0f)
        {
            WhileTimer?.Invoke();
            whileTimerPercentComplete?.Invoke(1f);

            TimerComplete?.Invoke();

            CleanupAfterCompletedTimer();
            yield break;
        }

        // ------------------------------------------------------------
        // MAIN LOOP
        // ------------------------------------------------------------
        while (elapsedTime < duration)
        {
            // If the object becomes inactive while running, stop cleanly.
            if (!isActiveAndEnabled)
            {
                CleanupAfterInterruptedTimer();
                yield break;
            }

            // Invoke per-frame timer events.
            WhileTimer?.Invoke();
            whileTimerPercentComplete?.Invoke(Mathf.Clamp01(elapsedTime / duration));

            // Advance elapsed time.
            elapsedTime += Time.deltaTime;

            // Fire the interval event when enough time has passed.
            //
            // Example:
            // If regularIntervalTime = 1 second, this should fire around
            // 1s, 2s, 3s, etc.
            if (regularIntervalTime > 0f)
            {
                while (elapsedTime >= nextIntervalTriggerTime && nextIntervalTriggerTime <= duration)
                {
                    RegularTimeIntervalEvent?.Invoke();
                    nextIntervalTriggerTime += regularIntervalTime;
                }
            }

            yield return null;
        }

        // Ensure the final progress update reaches 1 exactly.
        WhileTimer?.Invoke();
        whileTimerPercentComplete?.Invoke(1f);

        // Final completion event.
        TimerComplete?.Invoke();

        CleanupAfterCompletedTimer();
    }

    // ------------------------------------------------------------------------
    // CLEANUP HELPERS
    // ------------------------------------------------------------------------

    /// <summary>
    /// Called when a timer reaches its normal end.
    /// </summary>
    private void CleanupAfterCompletedTimer()
    {
        activeTimerCoroutine = null;
        currentTimerDuration = 0f;
    }

    /// <summary>
    /// Called when a timer is interrupted because the object was disabled,
    /// destroyed, or otherwise became invalid while the coroutine was running.
    /// </summary>
    private void CleanupAfterInterruptedTimer()
    {
        activeTimerCoroutine = null;
        currentTimerDuration = 0f;
    }
}