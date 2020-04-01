using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Controller : MonoBehaviour
{
    static Vector3 startPos;
    static Vector3 lastPos;
    public static bool died;

    List<string> objects = new List<string>();
    string lastObject;

    // Start is called before the first frame update
    void Start()
    {
        if (GameObject.FindGameObjectsWithTag("GameController").Length > 1) { GameObject.Destroy(this.gameObject); }
        DontDestroyOnLoad(this.gameObject);
        SceneManager.sceneLoaded += LevelLoadedCallback;
        startPos = GameObject.Find("Player").transform.position;
        lastPos = startPos;
        died = false;
    }

    // Update is called once per frame
    void Update()
    {
        
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
                foreach(string o in objects)
                {
                    GameObject.Find(o).SetActive(true);
                }
            }
            else
            {
                GameObject.Find("Player").transform.position = lastPos;
                GameObject.Find(lastObject).SetActive(false);
                objects.Add(lastObject);
                lastObject = "";
            }
        }
    }

    public void LoadLevel(GameObject trigger, string name)
    {
        lastPos = GameObject.Find("Player").transform.position;
        lastObject = trigger.name;
        SceneManager.LoadScene(name);
    }
}
