using UnityEngine;
using System.Collections;

public class moveplayer : MonoBehaviour {


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        this.GetComponent<Transform>().rotation = this.GetComponentInParent<Transform>().rotation;
        if (Input.GetKey(KeyCode.W))
        {
            this.GetComponent<Rigidbody2D>().AddRelativeForce(Vector2.up);
        }
        if (Input.GetKey(KeyCode.A))
        {
            this.GetComponent<Rigidbody2D>().AddRelativeForce(Vector2.left);
        }
        if (Input.GetKey(KeyCode.D))
        {
            this.GetComponent<Rigidbody2D>().AddRelativeForce(Vector2.right);
        }
        if (Input.GetKey(KeyCode.S))
        {
            this.GetComponent<Rigidbody2D>().AddRelativeForce(Vector2.down);
        }
        
    }
}
