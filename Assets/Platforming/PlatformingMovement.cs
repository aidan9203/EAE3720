using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlatformingMovement : MonoBehaviour
{
	public float jump_force;
	public float move_force;
	public float move_force_air;
	public float max_velocity;

	public GameObject sword;

    public int hp;
	public int damage;
	public float knockback;
	public float attack_frequency;
	public float block_frequency;

	float attack_timer = 10;
	float block_timer = 10;

	public string death_scene;


	bool facing_right = true;
	bool jumped = false;

	Transform tf;
	Rigidbody2D rb;

	Vector3 scale_initial;

    // Start is called before the first frame update
    void Start()
    {
		tf = GetComponent<Transform>();
		rb = GetComponent<Rigidbody2D>();
		scale_initial = GetComponent<Transform>().localScale;
    }

    // Update is called once per frame
    void Update()
    {
		if (hp <= 0) { SceneManager.LoadScene(death_scene); }

		GameObject.Find("Health").GetComponent<Text>().text = hp + " HP";
		attack_timer += Time.deltaTime;
		block_timer += Time.deltaTime;
		if (block_timer > 0.8f * block_frequency && attack_timer > attack_frequency / 2.0f) { sword.transform.localEulerAngles = new Vector3(0, 0, 45); }

		//Read inputs
		float input_horizontal = Input.GetAxisRaw("Horizontal");
		float input_jump = Input.GetAxisRaw("Jump");
		if (jumped && input_jump <= 0) { jumped = false; }

		//Detect floors and walls
		RaycastHit2D hit_down = Physics2D.Raycast(tf.position, Vector2.down, 0.6f, ~LayerMask.GetMask("Player"));
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
		if (Input.GetAxisRaw("Attack/Block") > 0 && attack_timer > attack_frequency && block_timer > block_frequency)
		{
			attack_timer = 0;
			sword.transform.localEulerAngles = new Vector3(0, 0, 75);
			if (facing_right && hit_right_upper.collider != null && hit_right_upper.collider.gameObject.layer == LayerMask.NameToLayer("Enemy"))
			{
				hit_right_upper.collider.GetComponent<PlatformingEnemy>().Damage(damage, (hit_right_upper.collider.GetComponent<Transform>().position - tf.position).normalized, knockback);
			}
			else if (facing_right && hit_right_lower.collider != null && hit_right_lower.collider.gameObject.layer == LayerMask.NameToLayer("Enemy"))
			{
				hit_right_lower.collider.GetComponent<PlatformingEnemy>().Damage(damage, (hit_right_lower.collider.GetComponent<Transform>().position - tf.position).normalized, knockback);
			}
			else if (!facing_right && hit_left_upper.collider != null && hit_left_upper.collider.gameObject.layer == LayerMask.NameToLayer("Enemy"))
			{
				hit_left_upper.collider.GetComponent<PlatformingEnemy>().Damage(damage, (hit_left_upper.collider.GetComponent<Transform>().position - tf.position).normalized, knockback);
			}
			else if (!facing_right && hit_left_lower.collider != null && hit_left_lower.collider.gameObject.layer == LayerMask.NameToLayer("Enemy"))
			{
				hit_left_lower.collider.GetComponent<PlatformingEnemy>().Damage(damage, (hit_left_lower.collider.GetComponent<Transform>().position - tf.position).normalized, knockback);
			}
		}
		else if (Input.GetAxisRaw("Attack/Block") < 0 && block_timer > block_frequency && attack_timer > attack_frequency)
		{
			block_timer = 0;
			sword.transform.localEulerAngles = new Vector3(0, 0, 15);
		}

		//Apply movement
		if (hit_down.collider != null && Mathf.Abs(rb.velocity.magnitude) < max_velocity)
		{
			rb.velocity = new Vector2(100 * input_horizontal * move_force * Time.deltaTime, rb.velocity.y);
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
			else if (hit_right_upper.collider != null && hit_right_upper.collider == null)
			{
				rb.AddForce(new Vector2(-jump_force * 40, jump_force * 60));
			}
			else if (hit_right_lower.collider != null && hit_right_lower.collider == null)
			{
				rb.AddForce(new Vector2(-jump_force * 40, jump_force * 60));
			}
			else if (hit_left_upper.collider != null && hit_left_upper.collider == null)
			{
				rb.AddForce(new Vector2(jump_force * 40, jump_force * 60));
			}
			else if (hit_left_lower.collider != null && hit_left_lower.collider == null)
			{
				rb.AddForce(new Vector2(jump_force * 40, jump_force * 60));
			}
		}
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
		if (block_timer < 0.8f * block_frequency && ((direction.x < 0 && facing_right) || (direction.x > 0 && !facing_right)))
		{
			rb.velocity += (3 * amount * new Vector2(direction.x, 0));
		}
		else
		{
			hp -= damage;
			rb.velocity += (10 * amount * new Vector2(direction.x, 0));
		}
	}
}
