using UnityEngine;
using UnityEngine.Events;

public class ScoreSystem : MonoBehaviour
{
    [Header("Point settings")]
    [SerializeField] int scoreWinNumber = 100;
    [SerializeField] int lowestScore = 0;
    [SerializeField] int startingScore = 0;
    public int currentScore {get; private set;}
    [SerializeField] bool clampUpperScore = false;
    [SerializeField] bool clampLowerScore = true;

    public UnityEvent onScoreGain, onScoreLoss, onScoreWinNumber, onScoreZero;
    public UnityEvent<int> updateScoreUI;

    private float upClamp;
    private float lowClamp;

    private void Start()
    {
        currentScore = startingScore;
    }

    /// <summary>
    /// This method changes the points of a target. It has diffent outcomes for if scoreChange is positive or negative.
    /// Can be used as a health script
    /// </summary>
    public void AdjustPoints(int scoreChange) {
        
        if (scoreChange != 0) {

            currentScore += scoreChange;
            upClamp = (clampUpperScore ? scoreWinNumber : Mathf.Infinity);
            lowClamp = (clampLowerScore ? lowestScore : -Mathf.Infinity);
            Mathf.Clamp(currentScore, lowClamp, upClamp);

            if (currentScore <= 0) {
                onScoreZero.Invoke();
            } else if (currentScore >= scoreWinNumber) {
                onScoreWinNumber.Invoke();
            } else {
                if (scoreChange > 0) {
                    onScoreGain.Invoke();                   
                } else {
                    onScoreLoss.Invoke();
                }
            }
            updateScoreUI.Invoke(currentScore);
        }

        
        
    }
    
    
}
