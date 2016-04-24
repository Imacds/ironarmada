using UnityEngine;
using System.Collections;

public class imanengine : MonoBehaviour {

    public char letter =  'w';

    public bool move = false;
    

    // Use this for initialization
    void Start () {
        Physics2D.gravity = new Vector2(0, 0);
	}
	
	// Update is called once per frame
	void Update () {
	if(move == true)
        {
this.GetComponent<Rigidbody2D>().AddRelativeForce(Vector2.up*10);

        }
        move = false;
	}
    public void moveforwards(char lettr)
    {
        if (letter == lettr)
        {
            move = true;
        }
    }
}
