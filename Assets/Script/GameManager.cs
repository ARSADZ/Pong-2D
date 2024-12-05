using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.AI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Game Settings")]
    public int player1Score;
    public int player2Score;
    public float timer;
    public bool isOver;
    public bool goldenGoal;
    public bool isSpawnPowerUp;
    public GameObject ballSpawned;


    [Header("Prefab")]
    public GameObject BallPrefab;
    public GameObject[] powerUp;

    [Header("Panels")]
    public GameObject PausePanel;
    public GameObject GameOverPanel;
    
    [Header("InGame UI")]
    public TextMeshProUGUI timerTxt;
    public TextMeshProUGUI Player1ScoreTxt;
    public TextMeshProUGUI Player2ScoreTxt;
    public GameObject goldenGoalIUI;

    [Header("Game Over")]
    public GameObject player1WinUI;
    public GameObject player2WinUI;
    public GameObject youWin;
    public GameObject youLose;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else{
            instance = this;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1;
        PausePanel.SetActive(false);
        GameOverPanel.SetActive(false);

        player2WinUI.SetActive(false);
        player1WinUI.SetActive(false);
        youWin.SetActive(false);
        youLose.SetActive(false);
        goldenGoalIUI.SetActive(false);

        timer = GameData.instance.gameTimer;
        isOver = false;
        goldenGoal = false;
        SpawnBall();
    }

    // Update is called once per frame
    private void Update()
    {
        Player1ScoreTxt.text = player1Score.ToString();
        Player2ScoreTxt.text = player2Score.ToString();
        if (timer > 0f)
        {
            timer -= Time.deltaTime;
            float minutes = Mathf.FloorToInt(timer / 60);
            float seconds = Mathf.FloorToInt(timer % 60);
            timerTxt.text = string.Format("{0:00}:{1:00}", minutes, seconds);

            if (seconds % 20 == 0 && !isSpawnPowerUp)
            {
                StartCoroutine("SpawnPowerUp");
            }
        }
        if (timer <= 0f && !isOver)
        {
            timerTxt.text = "0:00";
            if (player1Score == player2Score)
            {
                if (!goldenGoal)
                {
                    goldenGoal = true;
                    Ball[] ball = FindObjectsOfType<Ball>();
                    for (int i = 0; 1 < ball.Length; i++)
                    {
                        Destroy(ball[i].gameObject);
                    }
                    goldenGoalIUI.SetActive(true);
                    SpawnBall();
                }
            }
            else
            {
                GameOver();
            }
        }
    }

    public IEnumerator SpawnPowerUp()
    {
        isSpawnPowerUp = true;
        Debug.Log("PowerUp");
        int rand = Random.Range(0, powerUp.Length);
        Instantiate(powerUp[rand], new Vector3(Random.Range(-3.2f, 3.2f), Random.Range(-2.35f, 2.25f), 0), Quaternion.identity);
        yield return new WaitForSeconds(1);
        isSpawnPowerUp= false;
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
        PausePanel.SetActive(true);
        SoundManager.instance.UIClickSfx();
    }
    public void ResumeGame()
    {
        PausePanel.SetActive(false);
        Time.timeScale = 1;
        SoundManager.instance.UIClickSfx();
    }
    public void BackToMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Main Menu");
        SoundManager.instance.UIClickSfx();
    }
    public void Restart()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Gameplay");
        SoundManager.instance.UIClickSfx();
    }
    public void SpawnBall()
    {
        Debug.Log("Muncul Bola");
        StartCoroutine("DelaySpawn");
        SoundManager.instance.UIClickSfx();
    }
    public void GameOver()
    {
        SoundManager.instance.GameOverSfx();
        isOver = true;
        Debug.Log("Game Over");
        Time.timeScale = 0;

        GameOverPanel.SetActive(true);

        if (!GameData.instance.isSinglePlayer)
        {
            if (player1Score > player2Score)
            {
                player1WinUI.SetActive(true);
            }
            if (player2Score > player1Score)
            {
                player2WinUI.SetActive(true);
            }
        }
        else
        {
            if (player1Score > player2Score)
            {
                youWin.SetActive(true);
            }
            if (player2Score > player1Score)
            {
                youLose.SetActive(true);
            }
        }
    }
    private IEnumerator DelaySpawn()
    {
        yield return new WaitForSeconds(3);
        if (ballSpawned == null)
        {
            ballSpawned = Instantiate(BallPrefab, Vector3.zero, Quaternion.identity);
        }
    }
}
