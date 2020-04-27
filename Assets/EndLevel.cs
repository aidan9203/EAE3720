using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndLevel : MonoBehaviour
{
    public string home_scene;

    public GameObject fade;
    bool queue_end;

    // Start is called before the first frame update
    void Start()
    {
        queue_end = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (queue_end)
        {
            fade.GetComponent<Image>().color = new Vector4(0, 0, 0, fade.GetComponent<Image>().color.a + 5 * Time.deltaTime);
            if (fade.GetComponent<Image>().color.a >= 1)
            {
                SceneManager.LoadScene(home_scene);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            queue_end = true;
        }
    }
}
