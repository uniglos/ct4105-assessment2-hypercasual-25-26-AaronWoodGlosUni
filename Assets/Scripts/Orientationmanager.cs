using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OrientationManager : MonoBehaviour
{
    public enum Orientation
    {
        Portrait,
        Landscape
    }

    [Header("Set the desired orientation for this scene")]
    public Orientation desiredOrientation = Orientation.Portrait;

    private void Awake()
    {
        ApplyOrientation();
    }

    private void Start()
    {
        // Safety net: some devices need a slight delay to apply correctly
        Invoke(nameof(ForceApply), 0.5f);
    }

    void ApplyOrientation()
    {
        switch (desiredOrientation)
        {
            case Orientation.Portrait:
                Screen.orientation = ScreenOrientation.Portrait;
                break;

            case Orientation.Landscape:
                Screen.orientation = ScreenOrientation.LandscapeLeft; // Change to LandscapeRight if you prefer
                break;
        }
    }

    void ForceApply()
    {
        ApplyOrientation(); // Reapply in case Unity didn't handle it properly
    }
}