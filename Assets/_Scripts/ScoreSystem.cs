using UnityEngine;
using TMPro;

public class ScoreSystem : MonoBehaviour
{
    public static ScoreSystem instance;

    private int score = 0;
    private int totalPoints = 0;

    // Elapsed time in seconds (fractional)
    private float elapsedTime = 0f;

    // Assign these in the Inspector (or leave one null to auto-find on the same GameObject)
    public TMP_Text scoreText;
    public TMP_Text timerText;

    void Start()
    {
        if (instance != null)
        {
            Destroy(this);
            return;
        }

        instance = this;

        if (scoreText == null)
            scoreText = GetComponent<TMP_Text>();

        ResetScore();
    }

    public void ResetScore()
    {
        score = 0;
        if (scoreText != null)
            scoreText.text = score.ToString();

        totalPoints = GameObject.FindGameObjectsWithTag("Point").Length;
        elapsedTime = 0f;
        UpdateTimerText();
    }

    public void AddScore(int points)
    {
        score += points;
        if (scoreText != null)
            scoreText.text = score.ToString();
    }

    public int GetScore()
    {
        return score;
    }

    public float GetScorePercentage()
    {
        if (totalPoints == 0)
            return 0f;
        return (float)score / totalPoints;
    }

    // Set timer (seconds)
    public void SetTimer(int time)
    {
        elapsedTime = time;
        UpdateTimerText();
    }

    void Update()
    {
        // Advance timer each frame
        elapsedTime += Time.deltaTime;
        UpdateTimerText();
    }

    private void UpdateTimerText()
    {
        if (timerText == null)
            return;

        int seconds = (int)elapsedTime;
        int milliseconds = (int)((elapsedTime - seconds) * 1000f);
        timerText.text = $"{seconds}s {milliseconds:000}ms";
    }
}
