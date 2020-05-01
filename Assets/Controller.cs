using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Controller : MonoBehaviour
{
    static Vector3 startPos;
    static Vector3 lastPos;
    public static bool died;

    List<string> objects = new List<string>();
    string lastObject;

    bool queue_end, started;
    string queued_scene = "";

    // Start is called before the first frame update
    void Start()
    {
        if (GameObject.FindGameObjectsWithTag("GameController").Length > 1) { GameObject.Destroy(this.gameObject); }
        DontDestroyOnLoad(this.gameObject);
        SceneManager.sceneLoaded += LevelLoadedCallback;
        startPos = GameObject.Find("Player").transform.position;
        lastPos = startPos;
        died = false;

        queue_end = false;
        started = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (SceneManager.GetActiveScene().name == "Overworld")
        {
            GameObject fade = GameObject.Find("Fade");
            if (started)
            {
                fade.GetComponent<Image>().color = new Vector4(0, 0, 0, fade.GetComponent<Image>().color.a - 5 * Time.deltaTime);
                if (fade.GetComponent<Image>().color.a <= 0)
                {
                    started = false;
                }
            }
            else if (queue_end)
            {
                GameObject.Find("Fade").GetComponent<Image>().color = new Vector4(0, 0, 0, fade.GetComponent<Image>().color.a + 5 * Time.deltaTime);
                if (fade.GetComponent<Image>().color.a >= 1)
                {
                    SceneManager.LoadScene(queued_scene);
                    queued_scene = "";
                    queue_end = false;
                    started = true;
                }
            }
        }
    }


    void LevelLoadedCallback(Scene scene, LoadSceneMode sceneMode)
    {
        if (scene.name == "Overworld")
        {
            if (died)
            {
                GameObject.Find("Player").transform.position = startPos;
                lastPos = startPos;
                died = false;
                foreach (string o in objects)
                {
                    GameObject.Find(o).SetActive(false);
                }
                GameObject.Find(lastObject).SetActive(true);
                lastObject = "";
            }
            else
            {
                GameObject.Find("Player").transform.position = lastPos;
                objects.Add(lastObject);
                lastObject = "";
                foreach (string o in objects)
                {
                    GameObject.Find(o).SetActive(false);
                }
                startPos = lastPos;
            }
        }
    }

    public void LoadLevel(GameObject trigger, string name)
    {
        lastPos = GameObject.Find("Player").transform.position;
        lastObject = trigger.name;
        GameObject.Find(lastObject).SetActive(false);
        queued_scene = name;
        queue_end = true;
    }
}
