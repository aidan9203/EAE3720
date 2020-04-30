using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class Player : MonoBehaviour
{
    public GameObject ammoCountText;

    public GameObject grenadeCountText;

    //public GameObject healthCounter;

    public Button button1;

    public Button button2;

    public Button button3;

    public Button button4;

    public float initialHealth = 300f;

    public GameObject enemy;

    private int ammoCount = 16;

    private int grenadeCount = 2;

    private float currentHealth = 0;

    private bool playerTurn = true;

    private int damageDealt = 0;

    private System.Random rand = new System.Random();

    private  Button[] buttonList = null;

    private Button selectedButton = null;

    private Button subSelectedButton = null;

    private int selectedButtonIndex = -1;

    private ButtonSubmenu submenu = null;

    private Image subButtonImage = null;

    bool actionMenu = false;

    public Image healthBar;

    public Image healthBarRed;

    private float redHealthChange = 0f;

    private bool healthSubtractor = false;

    private ArrayList battleLog = new ArrayList();

    public TextMeshProUGUI text1;
    public TextMeshProUGUI text2;
    public TextMeshProUGUI text3;
    public TextMeshProUGUI text4;
    public TextMeshProUGUI text5;
    public TextMeshProUGUI text6;
    public TextMeshProUGUI text7;
    public TextMeshProUGUI text8;
    public TextMeshProUGUI text9;
    public TextMeshProUGUI text10;

    private int textPointer = 0;

    public GameObject battlePanel;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = initialHealth;
        //TextMeshProUGUI healthText = healthCounter.GetComponent<TextMeshProUGUI>();
        //healthText.SetText(currentHealth.ToString());
        Button[] buttonListCreator ={button1, button2, button3, button4};
        buttonList = buttonListCreator;
        selectedButton = button1;
        selectedButton.Select();
        selectedButtonIndex = 0;
        actionMenu = true;
        battleLog.Add(text1);
        battleLog.Add(text2);
        battleLog.Add(text3);
        battleLog.Add(text4);
        battleLog.Add(text5);
        battleLog.Add(text6);
        battleLog.Add(text7);
        battleLog.Add(text8);
        battleLog.Add(text9);
        battleLog.Add(text10);

    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(EventSystem.current.currentSelectedGameObject);

        
        if (currentHealth <= 0)
        {
            Destroy(this.gameObject);
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

        if (actionMenu & playerTurn)
        {
            if ((Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S)) && (selectedButtonIndex <= 2))
            {
                selectedButtonIndex += 1;
                selectedButton = buttonList[selectedButtonIndex];
                selectedButton.Select();
            }

            else if ((Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W)) && (selectedButtonIndex >= 1))
            {
                selectedButtonIndex -= 1;
                selectedButton = buttonList[selectedButtonIndex];
                selectedButton.Select();
            }

            else if (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.Return))
            {
                selectedButton.onClick.Invoke();
                actionMenu = false;
                submenu = selectedButton.GetComponent<ButtonSubmenu>();
                subSelectedButton = submenu.buttonTL;
                EventSystem.current.SetSelectedGameObject(null);
                subSelectedButton.Select();
                Image buttonImage = subSelectedButton.GetComponent<Image>();
                buttonImage.color = new Color(1f, 1f, 1f, 1f);

            }
        }
        //Image buttonImage = selectedButton.GetComponent<Image>();
        //buttonImage.color = (Color.gray);
        else if (playerTurn)
        {
            if ((Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S)))
            {
                if (subSelectedButton.Equals(submenu.buttonTL))
                {
                    if (submenu.buttonBL != null)
                    {
                        subButtonImage = subSelectedButton.GetComponent<Image>();
                        subButtonImage.color = new Color(1f, 1f, 1f, 0f);
                        subSelectedButton = submenu.buttonBL;
                    }
                }

                else if (subSelectedButton.Equals(submenu.buttonTR))
                {
                    if (submenu.buttonBR != null)
                    {
                        subButtonImage = subSelectedButton.GetComponent<Image>();
                        subButtonImage.color = new Color(1f, 1f, 1f, 0f);
                        subSelectedButton = submenu.buttonBR;
                    }
                }
            }

            if ((Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W)))
            {
                if (subSelectedButton.Equals(submenu.buttonBL))
                {
                    if (submenu.buttonTL != null)
                    {
                        subButtonImage = subSelectedButton.GetComponent<Image>();
                        subButtonImage.color = new Color(1f, 1f, 1f, 0f);
                        subSelectedButton = submenu.buttonTL;
                    }
                }
                else if (subSelectedButton.Equals(submenu.buttonBR))
                {
                    if (submenu.buttonTR != null)
                    {
                        subButtonImage = subSelectedButton.GetComponent<Image>();
                        subButtonImage.color = new Color(1f, 1f, 1f, 0f);
                        subSelectedButton = submenu.buttonTR;
                    }
                }
            }

            if ((Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A)))
            {
                if (subSelectedButton.Equals(submenu.buttonBR))
                {
                    if (submenu.buttonBL != null)
                    {
                        subButtonImage = subSelectedButton.GetComponent<Image>();
                        subButtonImage.color = new Color(1f, 1f, 1f, 0f);
                        subSelectedButton = submenu.buttonBL;
                    }
                }

                else if (subSelectedButton.Equals(submenu.buttonTR))
                {
                    if (submenu.buttonTL != null)
                    {
                        subButtonImage = subSelectedButton.GetComponent<Image>();
                        subButtonImage.color = new Color(1f, 1f, 1f, 0f);
                        subSelectedButton = submenu.buttonTL;
                    }
                }
            }

            if ((Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D)))
            {
                if (subSelectedButton.Equals(submenu.buttonBL))
                {
                    if (submenu.buttonBR != null)
                    {
                        subButtonImage = subSelectedButton.GetComponent<Image>();
                        subButtonImage.color = new Color(1f, 1f, 1f, 0f);
                        subSelectedButton = submenu.buttonBR;
                    }
                }

                else if (subSelectedButton.Equals(submenu.buttonTL))
                {
                    if (submenu.buttonTR != null)
                    {
                        subButtonImage = subSelectedButton.GetComponent<Image>();
                        subButtonImage.color = new Color(1f, 1f, 1f, 0f);
                        subSelectedButton = submenu.buttonTR;
                    }
                }
            }

            subButtonImage = subSelectedButton.GetComponent<Image>();
            subButtonImage.color = new Color(1f, 1f, 1f, 1f);
            subSelectedButton.Select();

            if (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.Return))
            {
                subSelectedButton.onClick.Invoke();
                subButtonImage = subSelectedButton.GetComponent<Image>();
                subButtonImage.color = new Color(1f, 1f, 1f, 0f);
                submenu.panel.SetActive(false);
                actionMenu = true;
                selectedButton.Select();
            }
            
            if (Input.GetKeyDown(KeyCode.X))
            {
                subButtonImage = subSelectedButton.GetComponent<Image>();
                subButtonImage.color = new Color(1f, 1f, 1f, 0f);
                submenu.panel.SetActive(false);
                actionMenu = true;
                selectedButton.Select();
            }


        }



    }

    public void punchAttack()
    {
        if (playerTurn)
        {
            damageDealt = 30;
            BattleLogger("Punch", damageDealt, 1);
            enemy.SendMessage("TakeDamage", damageDealt);
            enemy.SendMessage("ActivateTurn");
            playerTurn = false;
            buttonDisable();

        }
    }

    public void kickAttack()
    {
        if (playerTurn)
        {
            damageDealt = 50;
            BattleLogger("Kick", damageDealt, 1);
            enemy.SendMessage("TakeDamage", damageDealt);
            enemy.SendMessage("ActivateTurn");
            playerTurn = false;
            buttonDisable();
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

        float newWidth = (390f * (currentHealth / initialHealth));
        healthBar.rectTransform.sizeDelta = new Vector2(newWidth, 39.9f);

        healthBarRed.rectTransform.sizeDelta = new Vector2((390f * ((float) damage / initialHealth)), 39.9f);
        healthBarRed.rectTransform.localPosition = new Vector2((5.357254f + (newWidth/109.59f)), -4.684f);
        redHealthChange = healthBarRed.rectTransform.rect.width;
        Invoke("HealthSubtracterCall", 0.5f);

        //TextMeshProUGUI healthText = healthCounter.GetComponent<TextMeshProUGUI>();
        //healthText.SetText(currentHealth.ToString());
    }

    private void ActivateTurn()
    {
        Invoke("turnStart", 1.5f);

    }

    //private IEnumerator SelectButtonLater(Button button)
    //{
    //    yield return null;
    //    EventSystem.current.SetSelectedGameObject(null);
    //    EventSystem.current.SetSelectedGameObject(button.gameObject);
    //}

    private void HealthSubtracterCall()
    {
        healthSubtractor = true;
    }

    private void BattleLogger(string move, int dmg, int actor)
    {
        if (textPointer == 0)
        {
            battlePanel.SetActive(true);
        }
        
        if (actor == 1)
        {
            if (textPointer <= 9)
            {
                TextMeshProUGUI battletext = (TextMeshProUGUI)battleLog[textPointer];
                battletext.SetText("You used " + move + "! It did " + dmg + " damage!");
                textPointer += 1;
            }

            else
            {
                int tempCounter = 0;
                while (tempCounter <= 8)
                {
                    TextMeshProUGUI battletexttemp = (TextMeshProUGUI)battleLog[tempCounter];
                    TextMeshProUGUI battletexttemp1 = (TextMeshProUGUI)battleLog[tempCounter + 1];
                    battletexttemp.SetText(battletexttemp1.text);
                    tempCounter += 1;
                }
                TextMeshProUGUI battletext = (TextMeshProUGUI)battleLog[9];
                battletext.SetText("You used " + move + "! It did " + dmg + " damage!");
            }
        }

        if (actor == 2)
        {
            if (textPointer <= 9)
            {
                TextMeshProUGUI battletext = (TextMeshProUGUI)battleLog[textPointer];
                battletext.SetText("The enemy used " + move + "! It did " + dmg + " damage!");
                textPointer += 1;
            }

            else
            {
                int tempCounter = 0;
                while (tempCounter <= 8)
                {
                    TextMeshProUGUI battletexttemp = (TextMeshProUGUI)battleLog[tempCounter];
                    TextMeshProUGUI battletexttemp1 = (TextMeshProUGUI)battleLog[tempCounter + 1];
                    battletexttemp.SetText(battletexttemp1.text);
                    tempCounter += 1;
                }
                TextMeshProUGUI battletext = (TextMeshProUGUI)battleLog[9];
                battletext.SetText("The enemy used " + move + "! It did " + dmg + " damage!");
            }
        }
    }

    private void BattleLogArray(ArrayList stuff)
    {
        string move = (string)stuff[0];
        int dmg = (int)stuff[1];
        int actor = (int)stuff[2];
        BattleLogger(move, dmg, actor);
    }

    private void turnStart ()
    {
        playerTurn = true;
        button1.interactable = true;
        button2.interactable = true;
        button3.interactable = true;
        button4.interactable = true;
    }

    private void buttonDisable()
    {
        button1.interactable = false;
        button2.interactable = false;
        button3.interactable = false;
        button4.interactable = false;
    }



}
