using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Player : MonoBehaviour
{
    public GameObject ammoCountText;

    public GameObject grenadeCountText;

    public GameObject healthCounter;

    public int initialHealth = 300;

    public GameObject enemy;

    private int ammoCount = 16;

    private int grenadeCount = 2;

    private int currentHealth = 0;

    private bool playerTurn = true;

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
    }

    public void punchAttack()
    {
        if (playerTurn)
        {
            damageDealt = 30;
            enemy.SendMessage("TakeDamage", damageDealt);
            enemy.SendMessage("ActivateTurn");
            playerTurn = false;
        }
    }

    public void kickAttack()
    {
        if (playerTurn)
        {
            damageDealt = 50;
            enemy.SendMessage("TakeDamage", damageDealt);
            enemy.SendMessage("ActivateTurn");
            playerTurn = false;
        }
    }

    public void shootAttack()
    {
        if (playerTurn && ammoCount >= 1)
        {
            damageDealt = 80;
            ammoCount -= 1;
            TextMeshProUGUI ammoText = ammoCountText.GetComponent <TextMeshProUGUI>();
            ammoText.SetText("AMMO: " + ammoCount.ToString());
            enemy.SendMessage("TakeDamage", damageDealt);
            enemy.SendMessage("ActivateTurn");
            playerTurn = false;
        }
    }

    public void sprayAttack()
    {
        if (playerTurn && ammoCount >= 5)
        {
            damageDealt = 0;
            int hits = rand.Next(1, 6);
            int count = 0;
            while (count <= hits)
            {
                damageDealt += 50;
                count += 1;
            }
            ammoCount -= 5;
            TextMeshProUGUI ammoText = ammoCountText.GetComponent<TextMeshProUGUI>();
            ammoText.SetText("AMMO: " + ammoCount.ToString());
            enemy.SendMessage("TakeDamage", damageDealt);
            enemy.SendMessage("ActivateTurn");
            playerTurn = false;
        }
    }

    public void grenadeAttack()
    {
        if (playerTurn && grenadeCount >= 1)
        {
            damageDealt = 100;
            grenadeCount -= 1;
            TextMeshProUGUI grenadeText = grenadeCountText.GetComponent<TextMeshProUGUI>();
            grenadeText.SetText(ammoCount.ToString());
            enemy.SendMessage("TakeDamage", damageDealt);
            enemy.SendMessage("ActivateTurn");
            playerTurn = false;
        }
    }


    private void TakeDamage(int damage)
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
        playerTurn = true;
    }
}
