using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Enemy : MonoBehaviour
{
    public float initialHealth = 300f;

    public GameObject player;

    public GameObject healthCounter;

    public Image healthBarRed;

    public Image healthBar;

    private float currentHealth = 0f;

    private int[] moveList = {20, 30, 40, 50};

    private bool enemyTurn = false;

    private int damageDealt = 0;

    private System.Random rand = new System.Random();

    private float redHealthChange = 0f;

    private bool healthSubtractor = false;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = initialHealth;
        TextMeshProUGUI healthText = healthCounter.GetComponent<TextMeshProUGUI>();
        healthText.SetText(currentHealth.ToString());
        healthBar.preserveAspect = false;

        
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

        if (redHealthChange > 0f & healthSubtractor)
        {
            redHealthChange -= 3;
            float newWidth2 = healthBarRed.rectTransform.rect.width - 3;
            healthBarRed.rectTransform.sizeDelta = new Vector2(newWidth2, 39.9f);
            if (healthBarRed.rectTransform.rect.width < 0f)
            {
                healthBarRed.rectTransform.sizeDelta = new Vector2(0f, 39.9f);
            }
        }
        else
        {
            healthSubtractor = false;
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
        float newWidth = (390f * (currentHealth / initialHealth));
        healthBar.rectTransform.sizeDelta = new Vector2(newWidth, 39.9f);
        
        healthBarRed.rectTransform.sizeDelta = new Vector2((390f * (damage / initialHealth)), 39.9f);
        healthBarRed.rectTransform.localPosition = new Vector2(-195+newWidth, 198.3032f);
        redHealthChange = healthBarRed.rectTransform.rect.width;
        Invoke("HealthSubtracterCall", 0.5f);
    }

    private void ActivateTurn()
    {
        enemyTurn = true;
    }

    private void HealthSubtracterCall()
    {
        healthSubtractor = true;
    }
}
