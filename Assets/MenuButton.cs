using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuButton : MonoBehaviour
{
    public string start_scene;
    bool queue_start;

    public GameObject fade;

    // Start is called before the first frame update
    void Start()
    {
        queue_start = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (queue_start)
        {
            fade.GetComponent<Image>().color = new Vector4(0, 0, 0, fade.GetComponent<Image>().color.a + 5 * Time.deltaTime);
            if (fade.GetComponent<Image>().color.a >= 1)
            {
                SceneManager.LoadScene(start_scene);
            }
        }
    }

    public void StartButton()
    {
        queue_start = true;
    }
}
