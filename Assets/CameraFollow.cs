using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
	public Transform target;

    // Start is called before the first frame update
    void Start()
    {
		transform.position = new Vector3(target.position.x, target.position.y, transform.position.z);
	}

    // Update is called once per frame
    void Update()
    {
		transform.position = Vector3.Lerp(transform.position, new Vector3(target.position.x, target.position.y, transform.position.z), 0.05f);
    }
}
