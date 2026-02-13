using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SettingsManager : MonoBehaviour
{
    [SerializeField] UnityEvent onActivatePauseMenu, onDeactivatePauseMenu, onSetGameSpeed;

    /// <summary>
    /// Set Game states and speeds
    /// </summary>
    public void PauseGame() {
        onActivatePauseMenu.Invoke();
        Time.timeScale = 0.0f;
    }
    public void ContinueGame() {
        onDeactivatePauseMenu.Invoke();
        Time.timeScale = 1.0f;
    }
    public void SetCustomGameSpeed(float speed) {
        onSetGameSpeed.Invoke();
        Time.timeScale = Mathf.Clamp01(speed);
    }
}
