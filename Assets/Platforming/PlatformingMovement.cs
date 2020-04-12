using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlatformingMovement : MonoBehaviour
{
	public float jump_force;
	public float move_force;
	public float move_force_air;
	public float max_velocity;

	public GameObject sword;
	Animation sword_animations;

	public int max_hp;
    public int hp;
	public int damage;
	public float knockback;
	public float attack_frequency;
	public float block_frequency;

	float attack_timer = 10;
	float block_timer = 10;
	bool blocked = false;

	public string death_scene;


	bool facing_right = true;
	bool jumped = false;

	Transform tf;
	Rigidbody2D rb;
	SpriteRenderer sprite;

	public Sprite[] sprites = new Sprite[4];
	int frame = 0;
	float frame_timer = 0;

	Vector3 scale_initial;

	const float health_size = 0.03f;
	Texture2D health_texture;
	GUIStyle health_style;
	Texture2D health_background_texture;
	GUIStyle health_background_style;

	public bool sword_enabled;

	AudioSource[] sounds;

	// Start is called before the first frame update
	void Start()
    {
		transform.GetChild(0).GetChild(0).gameObject.SetActive(sword_enabled);

		tf = GetComponent<Transform>();
		rb = GetComponent<Rigidbody2D>();
		scale_initial = GetComponent<Transform>().localScale;
		sprite = tf.GetChild(0).GetComponent<SpriteRenderer>();

		health_texture = new Texture2D(1, 1);
		health_style = new GUIStyle();
		health_background_texture = new Texture2D(1, 1);
		health_background_style = new GUIStyle();
		health_background_texture.SetPixel(0, 0, Color.black);
		health_background_texture.Apply();
		health_background_style.normal.background = health_background_texture;

		sounds = GetComponents<AudioSource>();
		sword_animations = sword.GetComponent<Animation>();
	}

    // Update is called once per frame
    void Update()
    {
		sprite.color = new Color(Mathf.Min(255, sprite.color.r) + 10 * Time.deltaTime, Mathf.Min(255, sprite.color.g + 10 * Time.deltaTime), Mathf.Min(255, sprite.color.b + 10 * Time.deltaTime));
		if (hp <= 0)
		{
			Controller.died = true;
			SceneManager.LoadScene(death_scene);
		}

		attack_timer += Time.deltaTime;
		block_timer += Time.deltaTime;
		if (blocked && block_timer > 0.75f * block_frequency)
		{
			blocked = false;
			sword_animations.Play("Unblock");
		}

		//Read inputs
		float input_horizontal = Input.GetAxisRaw("Horizontal");
		float input_jump = Input.GetAxisRaw("Jump");
		if (jumped && input_jump <= 0) { jumped = false; }

		//Detect floors and walls
		RaycastHit2D hit_down = Physics2D.Raycast(tf.position, Vector2.down, 0.7f, ~LayerMask.GetMask("Player"));
		RaycastHit2D hit_left_upper = Physics2D.Raycast(tf.position + new Vector3(0, 0.4f, 0), Vector2.left, 0.7f, ~LayerMask.GetMask("Player"));
		RaycastHit2D hit_right_upper = Physics2D.Raycast(tf.position + new Vector3(0, 0.4f, 0), Vector2.right, 0.7f, ~LayerMask.GetMask("Player"));
		RaycastHit2D hit_left_lower = Physics2D.Raycast(tf.position + new Vector3(0, -0.4f, 0), Vector2.left, 0.7f, ~LayerMask.GetMask("Player"));
		RaycastHit2D hit_right_lower = Physics2D.Raycast(tf.position + new Vector3(0, -0.4f, 0), Vector2.right, 0.7f, ~LayerMask.GetMask("Player"));

		//Change player direction
		float mouse_dir = Camera.main.ScreenToWorldPoint(Input.mousePosition).x - (tf.position.x);
		if (facing_right && mouse_dir < 0)
		{
			Face(false);
		}
		else if (!facing_right && mouse_dir > 0)
		{
			Face(true);
		}

		//Attacking
		if (sword_enabled && Input.GetAxisRaw("Attack/Block") > 0 && attack_timer > attack_frequency && block_timer > block_frequency)
		{
			attack_timer = 0;
			sword_animations.Play("Attack");
			sounds[1].pitch = Random.Range(0.8f, 1.2f);
			sounds[1].Play();
			if (facing_right)
			{
				if (hit_right_upper.collider != null && hit_right_upper.collider.gameObject.layer == LayerMask.NameToLayer("Enemy"))
				{
					hit_right_upper.collider.GetComponent<PlatformingEnemy>().Damage(damage, (hit_right_upper.collider.GetComponent<Transform>().position - tf.position).normalized, knockback);
				}
				else if (hit_right_lower.collider != null && hit_right_lower.collider.gameObject.layer == LayerMask.NameToLayer("Enemy"))
				{
					hit_right_lower.collider.GetComponent<PlatformingEnemy>().Damage(damage, (hit_right_lower.collider.GetComponent<Transform>().position - tf.position).normalized, knockback);
				}
			}
			else
			{
				if (hit_left_upper.collider != null && hit_left_upper.collider.gameObject.layer == LayerMask.NameToLayer("Enemy"))
				{
					hit_left_upper.collider.GetComponent<PlatformingEnemy>().Damage(damage, (hit_left_upper.collider.GetComponent<Transform>().position - tf.position).normalized, knockback);
				}
				else if (hit_left_lower.collider != null && hit_left_lower.collider.gameObject.layer == LayerMask.NameToLayer("Enemy"))
				{
					hit_left_lower.collider.GetComponent<PlatformingEnemy>().Damage(damage, (hit_left_lower.collider.GetComponent<Transform>().position - tf.position).normalized, knockback);
				}
			}
		}
		else if (sword_enabled && Input.GetAxisRaw("Attack/Block") < 0 && block_timer > block_frequency && attack_timer > attack_frequency)
		{
			block_timer = 0;
			blocked = true;
			sword_animations.Play("Block");
		}

		//Apply movement
		if (hit_down.collider != null && Mathf.Abs(rb.velocity.magnitude) < max_velocity)
		{
			rb.velocity = new Vector2(input_horizontal * move_force, rb.velocity.y);
		}
		else
		{
			rb.velocity = new Vector2(Mathf.Max(Mathf.Min(rb.velocity.x + 100 * input_horizontal * move_force_air * Time.deltaTime, max_velocity), -max_velocity), rb.velocity.y);
		}


		//Apply jump force
		if (input_jump > 0 && !jumped)
		{
			jumped = true;
			if (hit_down.collider != null)
			{
				rb.AddForce(new Vector2(0, jump_force * 100));
			}
			else if (hit_right_upper.collider != null || hit_right_lower.collider != null)
			{
				rb.AddForce(new Vector2(-jump_force * 40, jump_force * 60));
			}
			else if (hit_left_upper.collider != null || hit_left_lower.collider != null)
			{
				rb.AddForce(new Vector2(jump_force * 40, jump_force * 60));
			}
		}


		//Animation
		frame_timer += Time.deltaTime;
		if (Mathf.Abs(rb.velocity.x) > 0.1f)
		{
			if (frame_timer > 0.3f / Mathf.Abs(rb.velocity.x))
			{
				frame_timer = 0;
				frame = (frame + 1) % 4;
				if (frame % 2 == 0 && hit_down.collider != null) { sounds[0].pitch = Random.Range(0.8f, 1.2f); sounds[0].Play(); }
			}
		}
		else
		{
			if (frame == 1) { frame = 2; }
			else if (frame == 3) { frame = 0; }
		}
		sprite.sprite = sprites[frame];
	}


	public void OnGUI()
	{
		Color color = Color.green;
		if (hp <= max_hp * 0.4f) { color = Color.red; }
		else if (hp <= max_hp * 0.7f) { color = Color.yellow; }
		health_texture.SetPixel(0, 0, color);
		health_texture.Apply();
		health_style.normal.background = health_texture;
		GUI.Box(new Rect(Screen.width * 0.02f, Screen.height * 0.03f, Screen.width * health_size * max_hp, Screen.height * health_size), GUIContent.none, health_background_style);
		GUI.Box(new Rect(Screen.width * 0.02f, Screen.height * 0.03f, Screen.width * health_size * hp, Screen.height * health_size), GUIContent.none, health_style);
	}


	private void Face(bool right)
	{
		if (right)
		{
			facing_right = true;
			tf.localScale = new Vector3(scale_initial.x, scale_initial.y, scale_initial.z);
		}
		else
		{
			facing_right = false;
			tf.localScale = new Vector3(-scale_initial.x, scale_initial.y, scale_initial.z);
		}
	}


	public void Damage(int damage, Vector2 direction, float amount)
	{
		if (sword_enabled && block_timer < 0.75f * block_frequency && ((direction.x < 0 && facing_right) || (direction.x > 0 && !facing_right)))
		{
			rb.velocity += (3 * amount * new Vector2(direction.x, 0));
			sounds[2].pitch = Random.Range(0.8f, 1.2f);
			sounds[2].Play();
		}
		else
		{
			hp -= damage;
			sprite.color = new Color(255, 0, 0);
			rb.velocity += (10 * amount * new Vector2(direction.x, 0));
		}
	}

	public void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.tag == "Sword")
		{
			sword_enabled = true;
			GameObject.Destroy(collision.gameObject);
			transform.GetChild(0).GetChild(0).gameObject.SetActive(true);
		}
	}
}
