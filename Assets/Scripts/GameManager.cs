using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.SceneManagement;

public class EnemyInfo
{
    public GameObject enemy;
    public int waveN;
}

public class GameManager : MonoBehaviour
{
    public static GameManager _instance;


    [SerializeField]
    public GameObject enemyTarget;

    [SerializeField]
    public GameObject[] enemisToSpawn;
    [SerializeField]
    public GameObject[] ui;
    [SerializeField]
    public GameObject uiError;

    public GameObject[] uiToDisable;

    [SerializeField]
    public GameObject EnemySpawnPlace;

    public static GameObject buildingReferance;
    public static GameObject playerReferance;

    public static GameObject curentEnemyTarget;

    public static List<int> enemyWaveList = new List<int>();

    public static Dictionary<int, EnemyInfo> enemyDictionary = new Dictionary<int, EnemyInfo>();


    public Text playerScore;

    public static float mainBuildingHealth;
    public static float playerMoney = 200f;

    //public static float levelUpCost = 20f;
    //public static float spawnCost = 20f;

    public bool isFolowPath = false;
    public bool isBuildingMenu = false;
    public bool freeToPress = true;
    public static bool isCheckingPlayerInfo = false;
    public static bool isGameOver = false;
    public static bool checkForNewTarget = false;

    public static float playerMaxAttackPower = 10f;
    public static float playerMaxHealth = 30f;

    public float setMainBuildingHealth = 1000f;

    public float enemyCount = 10f;

    public float timer = 60f;
    public float waitTime;


    public int waveNumber;

    public static float PlayerMoney
    {
        get
        {
            return playerMoney;
        }
        set
        {
            playerMoney = value;
            _instance.SetScore(value);
        }
    }

    public static float MainBuildingHealth
    {
        get
        {
            return mainBuildingHealth;
        }
        set
        {
            mainBuildingHealth = value;
            if (value <= 0)
                _instance.GameOver();
        }
    }

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }

        curentEnemyTarget = enemyTarget;
        PlayerMoney = 200;

        waveNumber = 0;
        enemyWaveList.Add(waveNumber);
    }

    private void Start()
    {
        waitTime = Time.time - 40;


        mainBuildingHealth = setMainBuildingHealth;
    }

    private void Update()
    {
        SpawnEnemies();

        if (Input.GetMouseButtonDown(0))
        {
            
            if (freeToPress)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, 50000f, (1 << 11)))
                {
                    if (hit.transform.GetComponent<Building>())
                    {
                        //DisableUI();
                        buildingReferance = hit.transform.gameObject;
                        hit.transform.GetComponent<Building>().ShowInfo();

                        freeToPress = false;
                        isBuildingMenu = true;
                    }   
                }

            }
        }

    }

    public void SetFreeToPress(bool anable)
    {
        freeToPress = anable;
    }

    public void SetFreeToFollow(bool anable)
    {
        isFolowPath = anable;
    }

    void DisableUI()
    {
        foreach (GameObject item in uiToDisable)
        {
            item.SetActive(false);
        }
    }

    void SetScore(float amount)
    {
        playerScore.text = amount.ToString();
    }

    public static void Deactivate(int indx)
    {
        _instance.ui[indx].SetActive(false);
    }

    public static void ActivateUI(int indx)
    {
        _instance.DisableUI();
        _instance.ui[indx].SetActive(true);

    }
    public static void ActivateErrorUI()
    {
        _instance.uiError.SetActive(true);
    }

    public void BuyWarior()
    {
        buildingReferance.GetComponent<Building>().AddWariorToQueue();
    }

    public void LevelUPWarior()
    {
        buildingReferance.GetComponent<Building>().LevelUP();
    }

    public void ShowPlayerUIInfo()
    {
        DisableUI();

        ui[1].SetActive(true);

        foreach (Text item in ui[1].GetComponentInChildren<Transform>().GetComponentsInChildren<Text>())
        {
            if (item.CompareTag("healthText"))
            {
                item.GetComponentInChildren<Text>().text = "Health " + playerReferance.GetComponent<Player>().currentHealth + "/" + playerReferance.GetComponent<Player>().maxHealth;
            }
            if (item.CompareTag("attackText"))
            {
                item.GetComponentInChildren<Text>().text = "Attack " + playerReferance.GetComponent<Player>().currentAttackPower;
            }
        }
    }



    public void ShowStandartUI()
    {
        DisableUI();
        ui[2].SetActive(true);
    }

    void SpawnEnemies()
    {
        if (Time.time >= timer + waitTime)
        {
            for (int i = 0; i < enemyCount; i++)
            {
                GameObject nextEnemy = enemisToSpawn[Random.Range(0, 2)];
                Vector3 newPosition = new Vector3(EnemySpawnPlace.transform.position.x, EnemySpawnPlace.transform.position.y, EnemySpawnPlace.transform.position.z + (i * 5 * nextEnemy.transform.localScale.z - enemyCount));

                GameObject newEnemy = Instantiate(nextEnemy, newPosition, Quaternion.identity);
                

                enemyDictionary.Add(newEnemy.GetInstanceID(), new EnemyInfo() { enemy = newEnemy, waveN = waveNumber });

            }
            waitTime = Time.time;

            enemyCount++;

            waveNumber++;
            enemyWaveList.Add(waveNumber);

        }
    }

    public void CheckForWaveClearence()
    {
        int count = 0;
        for (int i = enemyWaveList[0]; i < enemyWaveList.Count; i++)
        {
            foreach (var item in enemyDictionary.ToList())
            {
                if (item.Value.waveN == i)
                {
                    count++;
                }
            }
            if (count < 1)
            {
                enemyWaveList.Remove(i);
                PlayerMoney += 100f;
            }
        }

    }

    public void SpawnBuildingCost(float cost)
    {
        if (PlayerMoney >= cost)
        {
            PlayerMoney -= cost;
        }
        else
        {
            ActivateErrorUI();
        }
    }

    public void GameOver()
    {
        isGameOver = true;
        Time.timeScale = 0;
        DisableUI();

        ui[3].SetActive(true);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(0);
    }
}
