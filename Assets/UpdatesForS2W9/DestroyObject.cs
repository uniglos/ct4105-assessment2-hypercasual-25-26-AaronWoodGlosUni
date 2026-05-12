using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// DestroyObject
/// ---------------------------------------------------------------------------
/// PURPOSE
/// A helper component used to destroy objects and optionally notify other
/// systems that the destruction happened.
///
/// WHY THIS VERSION EXISTS
/// The original version invoked UnityEvents every single time OnDestroy()
/// fired. That is unsafe, because OnDestroy() is called not only during
/// normal gameplay destruction, but also during:
///
/// - scene unload
/// - returning to another scene/menu
/// - application quit
/// - teardown/cleanup states
///
/// In your case, that likely caused downstream systems (such as generators
/// and timers) to wake up while the minigame was already shutting down.
///
/// IMPORTANT DESIGN CHANGE
/// This rewrite keeps the OLD FIELD NAMES:
///
///     onDestroy
///     onDestroyObjectPassSelf
///
/// ...so that existing inspector wiring has the best chance of remaining
/// intact, while changing the internal logic to be much safer.
///
/// NEW RULE
/// Events are only fired when destruction was intentionally requested through
/// this script during live gameplay.
/// </summary>
public class DestroyObject : MonoBehaviour
{
    [Header("Legacy event fields kept for inspector compatibility")]

    [Tooltip("Invoked when this script intentionally destroys an object during live gameplay.")]
    [SerializeField] private UnityEvent onDestroy;

    [Tooltip("Invoked when this script intentionally destroys an object during live gameplay, passing the destroyed GameObject.")]
    [SerializeField] private UnityEvent<GameObject> onDestroyObjectPassSelf;

    // ------------------------------------------------------------------------
    // INTERNAL STATE
    // ------------------------------------------------------------------------

    /// <summary>
    /// Static flag shared by all instances.
    /// True once the application is quitting.
    ///
    /// Why static?
    /// Because when Unity begins shutdown, many objects may be destroyed in
    /// quick succession. A static flag lets every instance know this is not
    /// normal gameplay destruction.
    /// </summary>
    private static bool isApplicationQuitting = false;

    /// <summary>
    /// True only when THIS script explicitly requested destruction.
    ///
    /// This prevents passive scene teardown from being treated as a gameplay
    /// event.
    /// </summary>
    private bool intentionalDestroyRequested = false;

    /// <summary>
    /// Stores the object that was requested for destruction.
    ///
    /// In the common case of DestroySelf(), this will be this.gameObject.
    /// In DestroyObj(target), this will be the target passed in.
    /// </summary>
    private GameObject requestedDestroyedObject = null;

    // ------------------------------------------------------------------------
    // UNITY LIFECYCLE
    // ------------------------------------------------------------------------

    /// <summary>
    /// Called when the application is quitting.
    /// We set the shared shutdown flag so OnDestroy() knows not to fire
    /// gameplay events.
    /// </summary>
    private void OnApplicationQuit()
    {
        isApplicationQuitting = true;
    }

    /// <summary>
    /// Called by Unity when THIS GameObject is being destroyed.
    ///
    /// CRITICAL SAFETY LOGIC
    /// We do NOT automatically assume this means "an enemy died" or
    /// "a generated item was removed" or any other gameplay meaning.
    ///
    /// We only fire the UnityEvents if:
    /// - the app is not quitting
    /// - this script explicitly requested the destruction
    /// </summary>
    private void OnDestroy()
    {
        // During application quit, Unity destroys objects as part of global
        // shutdown. Do not treat that as gameplay destruction.
        if (isApplicationQuitting)
        {
            return;
        }

        // If no intentional destroy was requested, then this object is being
        // destroyed for some passive reason such as scene unload.
        // Do not broadcast gameplay events.
        if (!intentionalDestroyRequested)
        {
            return;
        }

        // Null-safe invoke protects against missing event assignments.
        onDestroy?.Invoke();

        // Pass the originally requested destroyed object.
        onDestroyObjectPassSelf?.Invoke(requestedDestroyedObject);
    }

    // ------------------------------------------------------------------------
    // PUBLIC API
    // ------------------------------------------------------------------------

    /// <summary>
    /// Destroys the supplied GameObject.
    ///
    /// IMPORTANT CAVEAT
    /// This method can destroy any GameObject reference you pass in.
    /// However, THIS component only receives OnDestroy() when THIS GameObject
    /// is destroyed.
    ///
    /// So:
    /// - if destroyingObject == this.gameObject, the event flow is coherent
    /// - if destroyingObject is some OTHER object, this component will NOT
    ///   receive OnDestroy() unless this object also gets destroyed
    ///
    /// That means this method is fine as a generic destroy helper, but if you
    /// specifically rely on this script's OnDestroy-based events, DestroySelf()
    /// is the more reliable pattern.
    /// </summary>
    /// <param name="destroyingObject">The GameObject to destroy.</param>
    public void DestroyObj(GameObject destroyingObject)
    {
        // Safety guard: ignore null references rather than throwing confusing
        // downstream errors.
        if (destroyingObject == null)
        {
            Debug.LogWarning($"[{nameof(DestroyObject)}] DestroyObj called with a null target on '{name}'.");
            return;
        }

        // Mark that an intentional gameplay-driven destruction was requested.
        intentionalDestroyRequested = true;
        requestedDestroyedObject = destroyingObject;

        // Request Unity to destroy the target object.
        Destroy(destroyingObject);
    }

    /// <summary>
    /// Destroys this GameObject.
    ///
    /// This is the cleanest and safest usage pattern for this component,
    /// because this component is attached to the same object that is being
    /// destroyed, so OnDestroy() will definitely run on this instance.
    /// </summary>
    public void DestroySelf()
    {
        // Mark this as an intentional gameplay destruction.
        intentionalDestroyRequested = true;
        requestedDestroyedObject = gameObject;

        // Request Unity to destroy this GameObject.
        Destroy(gameObject);
    }

    // ------------------------------------------------------------------------
    // OPTIONAL HELPER
    // ------------------------------------------------------------------------

    /// <summary>
    /// Resets the internal destruction request flags.
    ///
    /// This is not normally needed, but it is useful if you ever expand the
    /// script and want a way to clear pending destruction state manually.
    /// </summary>
    public void ResetDestroyState()
    {
        intentionalDestroyRequested = false;
        requestedDestroyedObject = null;
    }
}