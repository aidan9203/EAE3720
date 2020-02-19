using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class TopDownMovement : MonoBehaviour
{
    public Sprite[] sprites_north = new Sprite[4];
    public Sprite[] sprites_south = new Sprite[4];
    public Sprite[] sprites_east = new Sprite[4];
    public Sprite[] sprites_west = new Sprite[4];
    public GameObject sprite;

    public float move_speed;

    int frame = 0;
    float frame_timer = 0;

    float scale_horizontal = 0.5f;
    float scale_vertical = 0.5f;

    public Dictionary<Vector2Int, GameObject> grid = new Dictionary<Vector2Int, GameObject>();
    Vector2 grid_position;
    Vector2 grid_position_last;

    int direction = 0;

    Transform tf;

    float move_timer = -1;

    int input_horizontal = 0;
    int input_vertical = 0;

    // Start is called before the first frame update
    void Start()
    {
        tf = GetComponent<Transform>();
        grid_position = WorldToGrid(tf.position);
        grid_position_last = grid_position;

        //Convert world to grid
        GameObject[] objects = GameObject.FindObjectsOfType<GameObject>();
        foreach(GameObject o in objects)
        {
            if (o.layer == LayerMask.NameToLayer("Platform"))
            {
                Vector2Int pos = WorldToGrid(o.transform.position);
                if (!grid.ContainsKey(pos)) { grid.Add(pos, o); }
                o.transform.position = new Vector3(o.transform.position.x, o.transform.position.y, o.transform.position.y);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        frame_timer += Time.deltaTime;
        if (move_timer >= 0) { move_timer += Time.deltaTime; }

        if (move_timer > move_speed)
        {
            move_timer = -1;
            grid_position = grid_position_last + new Vector2(input_horizontal, input_vertical);
            grid_position_last = grid_position;
        }

        if (move_timer < 0)
        {
            input_horizontal = Mathf.RoundToInt(Input.GetAxisRaw("Horizontal"));
            input_vertical = Mathf.RoundToInt(Input.GetAxisRaw("Vertical"));
            if (input_horizontal != 0 && input_vertical != 0) { input_horizontal = 0; input_vertical = 0; }

            if (input_horizontal != 0 || input_vertical != 0)
            {
				move_timer = 0;

				//Set direction
				if (input_horizontal != 0 && input_vertical == 0)
                {
                    direction = 2 + input_horizontal;
                }
                else if (input_horizontal == 0)
                {
                    direction = 1 - input_vertical;
                }

                //Check for obstacles
                if (input_horizontal > 0 && grid.ContainsKey(new Vector2Int(Mathf.RoundToInt(grid_position.x + 1), Mathf.RoundToInt(grid_position.y)))) { input_horizontal = 0; }
                if (input_horizontal < 0 && grid.ContainsKey(new Vector2Int(Mathf.RoundToInt(grid_position.x - 1), Mathf.RoundToInt(grid_position.y)))) { input_horizontal = 0; }
                if (input_vertical > 0 && grid.ContainsKey(new Vector2Int(Mathf.RoundToInt(grid_position.x), Mathf.RoundToInt(grid_position.y + 1)))) { input_vertical = 0; }
                if (input_vertical < 0 && grid.ContainsKey(new Vector2Int(Mathf.RoundToInt(grid_position.x), Mathf.RoundToInt(grid_position.y - 1)))) { input_vertical = 0; }
            }
            else
            {
                if (frame == 1) { frame = 2; }
                else if (frame == 3) { frame = 0; }
            }
        }
        else
        {
            grid_position = grid_position_last + (move_timer / move_speed) * new Vector2(input_horizontal, input_vertical);
            if (frame_timer > move_speed / 2.0f)
            {
                frame_timer = 0;
                frame = (frame + 1) % 4;
            }
        }

        //Set sprite
        switch(direction)
        {
            case 0:
                sprite.GetComponent<SpriteRenderer>().sprite = sprites_north[frame];
                break;
            case 1:
                sprite.GetComponent<SpriteRenderer>().sprite = sprites_west[frame];
                break;
            case 2:
                sprite.GetComponent<SpriteRenderer>().sprite = sprites_south[frame];
                break;
            case 3:
                sprite.GetComponent<SpriteRenderer>().sprite = sprites_east[frame];
                break;
        }

        //Convert grid to world position
        tf.position = GridToWorld(grid_position);
    }

    //Convert grid position to screen position
    public Vector3 GridToWorld(Vector2 gridpos)
    {
        return new Vector3(gridpos.x * scale_horizontal, gridpos.y * scale_vertical, gridpos.y * scale_vertical);
    }

    //Convert screen position to grid position
    public Vector2Int WorldToGrid(Vector2 worldpos)
    {
        return new Vector2Int(Mathf.RoundToInt(worldpos.x / scale_horizontal), Mathf.RoundToInt(worldpos.y / scale_vertical));
    }
}
