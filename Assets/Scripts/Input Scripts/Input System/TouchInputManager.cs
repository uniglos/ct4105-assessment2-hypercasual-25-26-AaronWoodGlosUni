using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;

[DefaultExecutionOrder(-1)]
public abstract class TouchInputManager : MonoBehaviour {

    public delegate void StartTouchEvent(Vector2 position, float time);
    public event StartTouchEvent OnStartTouch;
    public delegate void EndTouchEvent(Vector2 position, float time);
    public event EndTouchEvent OnEndTouch;
    public delegate void StartSecondTouchEvent(Vector2 position1, Vector2 position2, float time);
    public event StartSecondTouchEvent OnStartSecondTouch;
    public delegate void EndSecondTouchEvent(Vector2 position, Vector2 position2, float time);
    public event EndSecondTouchEvent OnEndSecondTouch;

    public TouchControls touchControls;
    private Vector2 checkPos;
    private Vector2 initialTouchInput;

    private PointerEventData eventData;
    private List<RaycastResult> results;

[Header("Are controls active")]
    [SerializeField] public bool isFunctionActive = true;

    private void Awake() {
        touchControls = new TouchControls();
    }
    private void Start() {
        if (!isFunctionActive) {
            OnDisable();
        }
    }
    public virtual void OnEnable() {
        isFunctionActive = true;
        touchControls.Enable();
        TouchSimulation.Enable();
        EnhancedTouchSupport.Enable();
        touchControls.TouchScreen.TouchPress1.started += ctx => StartTouch(ctx);
        touchControls.TouchScreen.TouchPress1.canceled += ctx => EndTouch(ctx);
        touchControls.TouchScreen.TouchPress2.started += ctx => StartSecondTouch(ctx);
        touchControls.TouchScreen.TouchPress2.canceled += ctx => EndSecondTouch(ctx);
        touchControls.TouchScreen.TouchPress1.canceled += ctx => EndSecondTouch(ctx);
    }
    public virtual void OnDisable() {
        isFunctionActive = false;
        touchControls.Disable();
        TouchSimulation.Disable();
        EnhancedTouchSupport.Disable();
        touchControls.TouchScreen.TouchPress1.started -= ctx => StartTouch(ctx);
        touchControls.TouchScreen.TouchPress1.canceled -= ctx => EndTouch(ctx);
        touchControls.TouchScreen.TouchPress2.started -= ctx => StartSecondTouch(ctx);
        touchControls.TouchScreen.TouchPress2.canceled -= ctx => EndSecondTouch(ctx);
        touchControls.TouchScreen.TouchPress1.canceled -= ctx => EndSecondTouch(ctx);
    }

    private void StartSecondTouch(InputAction.CallbackContext context) {
        if (OnStartSecondTouch != null && !IsPointerOverUIObject(initialTouchInput)) {
            OnStartSecondTouch(touchControls.TouchScreen.TouchPosition1.ReadValue<Vector2>(), touchControls.TouchScreen.TouchPosition2.ReadValue<Vector2>(), (float)context.startTime);
        }
    }
    private void EndSecondTouch(InputAction.CallbackContext context) {
        if (OnEndSecondTouch != null && !IsPointerOverUIObject(initialTouchInput)) {
            OnEndSecondTouch(touchControls.TouchScreen.TouchPosition1.ReadValue<Vector2>(), touchControls.TouchScreen.TouchPosition2.ReadValue<Vector2>(), (float)context.startTime);
        }
    }

    /// <summary>
    /// Basic touch inputs when touching and releasing the screen
    /// </summary>
    private void StartTouch(InputAction.CallbackContext context) {
        if (isFunctionActive) {
            StartCoroutine(StartTouchNextFrame(context));
        }
    }
    private void EndTouch(InputAction.CallbackContext context) {
        if (isFunctionActive) {
            StartCoroutine(EndTouchNextFrame(context));
        }
    }

    private IEnumerator StartTouchNextFrame(InputAction.CallbackContext context) {
        yield return null;
        initialTouchInput = touchControls.TouchScreen.TouchPosition1.ReadValue<Vector2>();
        if (OnStartTouch != null && !IsPointerOverUIObject(initialTouchInput) && isFunctionActive) {
            OnStartTouch(initialTouchInput, (float)context.startTime);
        }
    }

    private IEnumerator EndTouchNextFrame(InputAction.CallbackContext context) {
        yield return null;
        initialTouchInput = touchControls.TouchScreen.TouchPosition1.ReadValue<Vector2>();
        if (OnStartTouch != null && !IsPointerOverUIObject(initialTouchInput)) {
            OnEndTouch(initialTouchInput, (float)context.time);
        }
    }
    private bool IsPointerOverUIObject(Vector2 touchPosition) {
        if (EventSystem.current != null) {
            eventData = new PointerEventData(EventSystem.current) { position = touchPosition };
            results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventData, results);
            return results.Count > 0;
        }
        else {
            return false;
        }
    }
}