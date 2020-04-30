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

    public GameObject healthCounter;

    public Button button1;

    public Button button2;

    public Button button3;

    public Button button4;

    public int initialHealth = 300;

    public GameObject enemy;

    private int ammoCount = 16;

    private int grenadeCount = 2;

    private int currentHealth = 0;

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

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = initialHealth;
        TextMeshProUGUI healthText = healthCounter.GetComponent<TextMeshProUGUI>();
        healthText.SetText(currentHealth.ToString());
        Button[] buttonListCreator ={button1, button2, button3, button4};
        buttonList = buttonListCreator;
        selectedButton = button1;
        selectedButton.Select();
        selectedButtonIndex = 0;
        actionMenu = true;

    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(EventSystem.current.currentSelectedGameObject);

        if (currentHealth <= 0)
        {
            Destroy(this.gameObject);
        }
        
        if (actionMenu)
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
        else
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

    //private IEnumerator SelectButtonLater(Button button)
    //{
    //    yield return null;
    //    EventSystem.current.SetSelectedGameObject(null);
    //    EventSystem.current.SetSelectedGameObject(button.gameObject);
    //}
}
