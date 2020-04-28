using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public Sprite[] sprites = new Sprite[8];
    float sprite_timer;
    int sprite_index;
    SpriteRenderer sprite;

    public int damage;
    public float knockback;
    public float velocity;

    // Start is called before the first frame update
    void Start()
    {
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Projectile"), LayerMask.NameToLayer("Enemy"));
        GetComponent<Rigidbody2D>().velocity = transform.up * velocity;
        sprite_timer = 0;
        sprite_index = 0;
        sprite = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = Quaternion.AngleAxis(Mathf.Rad2Deg * -Mathf.Atan2(GetComponent<Rigidbody2D>().velocity.x, GetComponent<Rigidbody2D>().velocity.y), transform.forward);
        sprite_timer += Time.deltaTime;
        if (sprite_timer > 0.1f && sprite_index < sprites.Length)
        {
            sprite_timer = 0;
            sprite.sprite = sprites[++sprite_index];
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.collider.tag == "Player")
        {
            other.collider.GetComponent<PlatformingMovement>().Damage(damage, other.transform.position - transform.position, knockback);
        }
        GameObject.Destroy(this.gameObject);
    }
}
