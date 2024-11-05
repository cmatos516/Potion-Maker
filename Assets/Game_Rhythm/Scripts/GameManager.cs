using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    // Audio and Gameplay
    public AudioSource theMusic;
    public bool startPlaying;
    public BeatScroller theBS;
    public static GameManager instance;

    // Scoring
    public int currentScore;
    public int scorePerNote = 100;
    public int scorePerGoodNote = 125;
    public int scorePerPerfectNote = 150;

    public int currentMultiplier;
    public int multiplierTracker;
    public int[] multiplierThresholds;

    public Text scoreText;
    public Text multiText;

    // Note Stats
    public float totalNotes;
    public float normalHits;
    public float goodHits;
    public float perfectHits;
    public float missedHits;

    // Results Screen
    public GameObject resultsScreen;
    public Text percentHitText, normalsText, goodsText, perfectsText, missesText, rankText, finalScoreText;

    // Pipe Game Variables
    public GameObject PipesHolder;
    public GameObject[] Pipes;
    [SerializeField] int totalPipes = 0;
    int correctedPipes = 0;

    // Start is called before the first frame update
    private void Start()
    {
        // Singleton instance setup
        instance = this;

        // Initialize score and multiplier
        scoreText.text = "Score: 0";
        currentMultiplier = 1;

        // Count total notes in game
        totalNotes = FindObjectsOfType<NoteObject>().Length;

        // Initialize Pipes
        totalPipes = PipesHolder.transform.childCount;
        Pipes = new GameObject[totalPipes];

        for (int i = 0; i < Pipes.Length; i++)
        {
            Pipes[i] = PipesHolder.transform.GetChild(i).gameObject;
        }
    }

    // Update is called once per frame
    private void Update()
    {
        // Start gameplay on any key press
        if (!startPlaying)
        {
            if (Input.anyKeyDown)
            {
                startPlaying = true;
                theBS.hasStarted = true;
                theMusic.Play();
            }
        }
        // Display results when music stops
        else if (startPlaying && !theMusic.isPlaying && !resultsScreen.activeInHierarchy)
        {
            ShowResults();
        }
    }

    // Display Results Screen
    private void ShowResults()
    {
        resultsScreen.SetActive(true);

        normalsText.text = normalHits.ToString();
        goodsText.text = goodHits.ToString();
        perfectsText.text = perfectHits.ToString();
        missesText.text = missedHits.ToString();

        float totalHit = normalHits + goodHits + perfectHits;
        float percentHit = (totalHit / totalNotes) * 100f;
        percentHitText.text = percentHit.ToString("F1") + "%";

        rankText.text = GetRank(percentHit);
        finalScoreText.text = currentScore.ToString();
    }

    // Determine Rank
    private string GetRank(float percentHit)
    {
        if (percentHit > 95) return "S";
        if (percentHit > 85) return "A";
        if (percentHit > 70) return "B";
        if (percentHit > 55) return "C";
        if (percentHit > 40) return "D";
        return "F";
    }

    // Note hit logic
    public void NoteHit()
    {
        Debug.Log("Hit On Time");

        if (currentMultiplier - 1 < multiplierThresholds.Length)
        {
            multiplierTracker++;

            if (multiplierThresholds[currentMultiplier - 1] <= multiplierTracker)
            {
                multiplierTracker = 0;
                currentMultiplier++;
            }
        }

        multiText.text = "Multiplier: x" + currentMultiplier;
        scoreText.text = "Score: " + currentScore;
    }

    public void NormalHit()
    {
        currentScore += scorePerNote * currentMultiplier;
        NoteHit();
        normalHits++;
    }

    public void GoodHit()
    {
        currentScore += scorePerGoodNote * currentMultiplier;
        NoteHit();
        goodHits++;
    }

    public void PerfectHit()
    {
        currentScore += scorePerPerfectNote * currentMultiplier;
        NoteHit();
        perfectHits++;
    }

    public void NoteMissed()
    {
        Debug.Log("Missed Note");
        currentMultiplier = 1;
        multiplierTracker = 0;
        multiText.text = "Multiplier: x" + currentMultiplier;
        missedHits++;
    }

    // Pipe Game Mechanics
    public void correctMove()
    {
        correctedPipes += 1;
        Debug.Log("Correct Move");

        if (correctedPipes == totalPipes)
        {
            Debug.Log("Excellent!");
        }
    }

    public void wrongMove()
    {
        correctedPipes = Mathf.Max(0, correctedPipes - 1);
        Debug.Log("Wrong Move");
    }
}
