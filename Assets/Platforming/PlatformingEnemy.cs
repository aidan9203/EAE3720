﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlatformingEnemy : MonoBehaviour
{
	public float jump_force;
	public float move_force;

	public Transform player;
	public float player_engage_distance_in_sight;
	bool player_engaged = false;
	public float player_engage_distance_no_sight;
	public float player_ignore_distance;

	public List<Transform> waypoints;
	public bool loop;
	public bool reverse;

	public int hp;
	public int damage;
	public float knockback;
	public float attack_frequency;
	float attack_timer = 0;

	int waypoint = 0;

	bool facing_right = true;
	bool jumped = false;

	float waypoint_min_distance = 0.5f;

	Transform tf;
	Rigidbody2D rb;

	Vector3 scale_initial;

	public string death_scene = "";

	// Start is called before the first frame update
	void Start()
    {
		tf = GetComponent<Transform>();
		rb = GetComponent<Rigidbody2D>();
		scale_initial = GetComponent<Transform>().localScale;
		waypoint = FindClosestWaypoint(false);
	}

    // Update is called once per frame
    void Update()
    {
		attack_timer += Time.deltaTime;

		Vector2 waypoint_direction;

		Vector2 player_direction = player.position - tf.position;
		float player_distance = Mathf.Abs(player_direction.magnitude);
		
		//Player engagement
		if (player_distance < player_engage_distance_in_sight)
		{
			if (!player_engaged)
			{
				RaycastHit2D hit_player = Physics2D.Raycast(tf.position, player_direction.normalized, player_engage_distance_in_sight, ~LayerMask.GetMask("Enemy"));
				if (hit_player.collider != null && hit_player.collider.gameObject.layer == LayerMask.NameToLayer("Player"))
				{
					if (hit_player.distance > player_engage_distance_no_sight)
					{
						if (facing_right && player_direction.x > 0) { player_engaged = true; }
						else if (!facing_right && player_direction.x < 0) { player_engaged = true; }
					}
					else { player_engaged = true; }
				}
			}
		}
		else if (player_engaged && player_distance > player_ignore_distance) { player_engaged = false; waypoint = FindClosestWaypoint(true); }
		
		//Pick between player and next waypoint
		if (player_engaged) { waypoint_direction = player_direction; }
		else { waypoint_direction = GetNextWaypointDirection(); }

		RaycastHit2D hit_down = Physics2D.Raycast(tf.position, Vector2.down, 0.6f, ~LayerMask.GetMask("Enemy"));

		//Update direction
		if (hit_down.collider != null)
		{
			if (waypoint_direction.x >= 0) { Face(true); }
			else { Face(false); }
		}
		else { jumped = false; }
		int move_direction = (facing_right ? 1 : -1);

		//Detect collisions
		RaycastHit2D hit_forward = Physics2D.Raycast(tf.position, move_direction * Vector2.right, 0.7f, ~LayerMask.GetMask("Enemy"));

		//Obstacle in front of the enemy
		if (hit_forward.collider != null)
		{
			if (hit_forward.collider.gameObject.layer == LayerMask.NameToLayer("Player"))
			{
				//Attacking the player
				if (attack_timer > attack_frequency)
				{
					attack_timer = 0;
					hit_forward.collider.GetComponent<PlatformingMovement>().Damage(damage, (player.position - tf.position).normalized, knockback);
				}
			}
			else if (hit_forward.collider.gameObject.layer == LayerMask.NameToLayer("Platform") && !jumped && hit_down.collider != null)
			{
				rb.AddForce(new Vector2(0, jump_force * 100));
				jumped = true;
			}
		}
		else
		{
			//Apply forward motion
			rb.velocity = new Vector2(100 * move_force * move_direction * Time.deltaTime, rb.velocity.y);
		}
	}





	//Returns the index of the closest waypoint
	private int FindClosestWaypoint(bool direction_matters)
	{
		int min = -1;
		float min_distance = float.MaxValue;
		for (int w = 0; w < waypoints.Count; w++)
		{
			Vector2 direction = waypoints[w].position - tf.position;
			float distance = Mathf.Abs(direction.magnitude);
			if (distance < min_distance && (!direction_matters || (facing_right && direction.x > 0) || (!facing_right && direction.x < 0)))
			{
				min_distance = distance;
				min = w;
			}
		}
		if (waypoints[min].position.x > waypoints[waypoint].position.x)
		{
			if (waypoints[min].position.x > player.position.x)
			{
				reverse = (min > waypoint ? true : false);
			}
			else
			{
				reverse = (min > waypoint ? false : true);
			}
		}
		else
		{
			if (waypoints[min].position.x > player.position.x)
			{
				reverse = (min > waypoint ? false : true);
			}
			else
			{
				reverse = (min > waypoint ? true : false);
			}
		}
		return min;
	}


	/// <summary>
	/// Returns the (unnormalized) direction of the next waypoint as a Vector2, updates the current waypoint if necessary
	/// </summary>
	private Vector2 GetNextWaypointDirection()
	{
		//Find next waypoint
		Vector2 waypoint_direction = waypoints[waypoint].position - tf.position;
		float waypoint_distance = waypoint_direction.magnitude;
		//Move to next waypoint and recalculate if we are close enough
		if (waypoint_distance < waypoint_min_distance)
		{
			if (reverse) { waypoint--; }
			else { waypoint++; }
			if (waypoint == waypoints.Count)
			{
				if (loop) { waypoint = 0; }
				else { reverse = true; waypoint--; }
			}
			if (waypoint == -1)
			{
				if (loop) { waypoint = waypoints.Count - 1; }
				else { reverse = false; waypoint++; }
			}
			waypoint_direction = waypoints[waypoint].position - tf.position;
		}
		return waypoint_direction;
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
		hp -= damage;
		rb.velocity += (10 * amount * new Vector2(direction.x, 0)) + new Vector2(0, 1.0f);
		if (hp <= 0)
		{
			if (death_scene != "") { SceneManager.LoadScene(death_scene); }
			GameObject.Destroy(this.gameObject);
		}
	}
}