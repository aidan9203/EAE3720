using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Enemy : MonoBehaviour
{
    public int initialHealth = 300;

    public GameObject player;

    public GameObject healthCounter;

    private int currentHealth = 0;

    private int[] moveList = {20, 30, 40, 50};

    private bool enemyTurn = false;

    private int damageDealt = 0;

    private System.Random rand = new System.Random();

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = initialHealth;
        TextMeshProUGUI healthText = healthCounter.GetComponent<TextMeshProUGUI>();
        healthText.SetText(currentHealth.ToString());
    }

    // Update is called once per frame
    void Update()
    {
        if (currentHealth <= 0)
        {
            Destroy(this.gameObject);
        }

        if (enemyTurn)
        {
            damageDealt = moveList[rand.Next(0, moveList.Length)];
            player.SendMessage("TakeDamage", damageDealt);
            player.SendMessage("ActivateTurn");
            enemyTurn = false;
        }
    }

    private void TakeDamage (int damage)
    {
        if (currentHealth - damage <= 0)
        {
            currentHealth = 0;
        }
        else
        {
            currentHealth -= damage;
        }

        TextMeshProUGUI healthText = healthCounter.GetComponent<TextMeshProUGUI>();
        healthText.SetText(currentHealth.ToString());
    }

    private void ActivateTurn()
    {
        enemyTurn = true;
    }
}
