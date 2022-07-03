using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    private const string ROUND = "Round: ";

    public GameObject[] spawns;
    public GameObject enemyPrefab;
    public GameObject endScreen;
    public Text roundNumber;
    public Text roundsSurvived;

    public int enemiesAlive = 0;
    public int round = 0;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (enemiesAlive == 0)
        {
            round++;
            roundNumber.text = ROUND + round.ToString();
            NextWave(round);
        }
    }

    private void NextWave(int round)
    {
        for (var x = 0; x < round * round; x++)
        {
            GameObject spawnPoint = spawns[Random.Range(0, spawns.Length)];

            GameObject enemySpawned = Instantiate(enemyPrefab, spawnPoint.transform.position, Quaternion.identity);
            enemySpawned.GetComponent<EnemyAiController>().gameController = GetComponent<GameController>();

            enemiesAlive++;

            if(x % 3 == 0)
            {
                spawnOnIntervals();
            }
        }
    }

    private void spawnOnIntervals()
    {
        float start = Time.deltaTime;
        while (start + 2 < Time.deltaTime)
        {

        }
    }

    public void Restart()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(1);
    }

    public void EndGame()
    {
        Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.None;
        roundNumber.gameObject.SetActive(false);
        endScreen.SetActive(true);
        roundsSurvived.text = round.ToString();
    }

    public void BackToMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }
}
