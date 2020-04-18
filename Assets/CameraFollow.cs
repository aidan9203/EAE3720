using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
	public Transform target;
    public float offset;

    public float limit_x_lower;
    public float limit_x_upper;
    public float limit_y_lower;
    public float limit_y_upper;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = GetPosition();
	}

    // Update is called once per frame
    void Update()
    {
		transform.position = Vector3.Lerp(transform.position, GetPosition(), 0.05f);
    }

    private Vector3 GetPosition()
    {
        float x = Mathf.Max(Mathf.Min(target.position.x, limit_x_upper), limit_x_lower);
        float y = Mathf.Max(Mathf.Min(target.position.y + offset, limit_y_upper), limit_y_lower);
        return new Vector3(x, y, transform.position.z);
    }
}
