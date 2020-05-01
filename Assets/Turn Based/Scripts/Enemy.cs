using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class Enemy : MonoBehaviour
{
    public float initialHealth = 300f;

    public GameObject player;

    //public GameObject healthCounter;

    public Image healthBarRed;

    public Image healthBar;

    private float currentHealth = 0f;

    private int[] moveList = {30, 50, 70};

    private string[] moveNameList = { "The enemy spews bile", "Your wounds fester", "The enemy infects you"};

    private bool enemyTurn = false;

    private int damageDealt = 0;

    private System.Random rand = new System.Random();

    private float redHealthChange = 0f;

    private bool healthSubtractor = false;

    public GameObject deathPanelHolder;

    private CanvasGroup deathPanel = null;

    public TextMeshProUGUI deathText;

    private bool deathMode = false;

    private bool ded = false;

    public AudioClip sound;

    private Button button { get { return GetComponent<Button>(); } }
    private AudioSource source { get { return GetComponent<AudioSource>(); } }

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = initialHealth;
        //TextMeshProUGUI healthText = healthCounter.GetComponent<TextMeshProUGUI>();
        //healthText.SetText(currentHealth.ToString());
        healthBar.preserveAspect = false;
        deathPanel = deathPanelHolder.GetComponent<CanvasGroup>();
        gameObject.AddComponent<AudioSource>();
        source.clip = sound;
        source.playOnAwake = false;

    }

    // Update is called once per frame
    void Update()
    {
        /*if (currentHealth <= 0)
        {
            Destroy(this.gameObject);
        }*/

         if (deathMode)
        {
            if (deathPanel.alpha < 1)
            {
                deathPanel.alpha += 0.02f;
            }

            else if (ded == false)
            {
                ded = true;
                deathText.SetText("YOU WIN! <br> <br> Aidan <br> Bryetan <br> Parimal <br> Wyatt");
                Invoke("gameQuitter", 6f);
            }
            
        }
        
        if (enemyTurn)
        {
            int moveIndex = rand.Next(0, moveList.Length);
            damageDealt = moveList[moveIndex];
            string moveName = moveNameList[moveIndex];
            ArrayList stuff = new ArrayList();
            stuff.Add(moveName);
            stuff.Add(damageDealt);
            stuff.Add(2);
            player.SendMessage("BattleLogArray", stuff);
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
            damage = (int) currentHealth;
            currentHealth = 0;
        }
        else
        {
            currentHealth -= damage;
        }
        PlaySound();
        //TextMeshProUGUI healthText = healthCounter.GetComponent<TextMeshProUGUI>();
        //healthText.SetText(currentHealth.ToString());
        float newWidth = (390f * (currentHealth / initialHealth));
        healthBar.rectTransform.sizeDelta = new Vector2(newWidth, 39.9f);
        
        healthBarRed.rectTransform.sizeDelta = new Vector2((390f * (damage / initialHealth)), 39.9f);
        healthBarRed.rectTransform.localPosition = new Vector2(-195+newWidth, 198.3032f);
        redHealthChange = healthBarRed.rectTransform.rect.width;
        Invoke("HealthSubtracterCall", 0.5f);
    }

    private void ActivateTurn()
    {
        if (currentHealth <= 0)
        {
            Invoke("deathScreen", 2f);
        }

        else
        {
            Invoke("turnStart", 1.5f);
        }
        
    }

    private void HealthSubtracterCall()
    {
        healthSubtractor = true;
    }

    private void turnStart()
    {
        enemyTurn = true;
    }

    private void deathScreen()
    {
        deathMode = true;
    }

    private void gameQuitter()
    {
        Application.Quit();
    }

    private void PlaySound()
    {
        source.PlayOneShot(sound);
    }
}
