using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonSubmenu : MonoBehaviour
{
    public Button buttonTL;

    public Button buttonTR;

    public Button buttonBL;

    public Button buttonBR;

    public GameObject panel;

    public AudioClip selectSound;

    public AudioClip hitSound;

    private Button button {  get { return GetComponent<Button>(); } }
    private AudioSource source1 { get { return GetComponent<AudioSource>(); } }

    private AudioSource source2 { get { return GetComponent<AudioSource>(); } }
    void Start()
    {
        gameObject.AddComponent<AudioSource>();
        source1.clip = selectSound;
        source2.clip = hitSound;
        source1.playOnAwake = false;
        source2.playOnAwake = false;

        button.onClick.AddListener(() => PlaySoundClick());

    }

    // Update is called once per frame

    private void PlaySoundClick()
    {
        source2.PlayOneShot(hitSound);
    }

}   
