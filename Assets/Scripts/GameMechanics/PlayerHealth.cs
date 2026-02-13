using UnityEngine;
using UnityEngine.Events;

public class PlayerHealth : MonoBehaviour {
    [SerializeField] int maxHealth = 100;
    [SerializeField] int currenthealth = 100;
    public UnityEvent onHealthGain, onHealthLose, onHealthZero, onHealthFull;

    /// <summary>
    /// This method changes the health of a target. It has diffent outcomes for if hitPoints is positive or negative.
    /// </summary>
    public void AdjustHealth(int hitPoints) {
        if (hitPoints != 0) {
            currenthealth += hitPoints;
            currenthealth = Mathf.Clamp(currenthealth, 0, maxHealth);
            if (currenthealth <= 0) {
                onHealthZero.Invoke();
            } else if (currenthealth >= maxHealth) {
                onHealthFull.Invoke();
            } else {
                if (hitPoints > 0) {
                    onHealthGain.Invoke();
                } else {
                    onHealthLose.Invoke();
                }
            }
        }
    }
}