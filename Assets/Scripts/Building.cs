using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Building : MonoBehaviour
{

    public List<GameObject> nextWarior = new List<GameObject>();

    public GameObject wariorType;

    public GameObject errorMasage;
    public GameObject uiInfoPanel;

    public Transform wariorSpawnPlace;

    public float levelUpCost = 20f;
    public float spawnCost = 20f;

    public float wariorMaxHelth;
    public float wariorMaxAttack;

    public float timeTillSpawn = 10f;

    public float waitTime;

    private void Start()
    {
        wariorMaxHelth = GameManager.playerMaxHealth;
        wariorMaxAttack = GameManager.playerMaxAttackPower;
    }

    public virtual void Update()
    {
        if (nextWarior.Count > 0)
        {
            if (Time.time >= timeTillSpawn + waitTime)
            {
                GameObject war = Instantiate(nextWarior[0], wariorSpawnPlace.position, Quaternion.identity);
                //war.GetComponent<Player>().SetParameters(wariorMaxAttack, wariorMaxHelth);

                nextWarior.RemoveAt(0);
                waitTime = Time.time;
            }
        }
    }

    public virtual void AddWariorToQueue()
    {
        if(GameManager.PlayerMoney >= spawnCost)
        {
            GameManager.PlayerMoney -= spawnCost;
            
            wariorType.GetComponent<Player>().SetParameters(wariorMaxAttack, wariorMaxHelth);
            nextWarior.Add(wariorType);
            waitTime = Time.time;
        }
        else
        {
            GameManager.ActivateErrorUI();
        }
    }

    public virtual void LevelUP()
    {
        if (GameManager.PlayerMoney >= levelUpCost)
        {
            GameManager.PlayerMoney -= levelUpCost;

            levelUpCost += 10f;
            spawnCost += 5f;


            wariorMaxHelth += 5f;
            wariorMaxAttack += 5f;
        }
        else
        {
            GameManager.ActivateErrorUI();
        }
    }

    public virtual void ShowInfo()
    {
        uiInfoPanel.SetActive(true);
    }

}
