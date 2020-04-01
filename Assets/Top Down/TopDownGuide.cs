using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TopDownGuide : MonoBehaviour
{
    public Text dialog_box;
    public GameObject dialog_background;

    public string message;
    public float time;

    static float timer = -1;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (timer > time)
        {
            dialog_box.text = "";
            dialog_background.SetActive(false);
            timer = -1;
        }
        else if (timer >= 0)
        {
            timer += Time.deltaTime;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            dialog_box.text = message;
            dialog_background.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            timer = 0;
        }
    }
}
