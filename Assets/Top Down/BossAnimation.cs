using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAnimation : MonoBehaviour
{
    public Sprite[] sprites = new Sprite[12];
    SpriteRenderer sprite;
    int frame = 0;
    float frame_timer = 0;

    // Start is called before the first frame update
    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
		//Animation
		frame_timer += Time.deltaTime;
		if (frame_timer > 0.2f)
		{
			frame_timer = 0;
			frame = (frame + 1) % 12;
		}
		sprite.sprite = sprites[frame];
	}
}
