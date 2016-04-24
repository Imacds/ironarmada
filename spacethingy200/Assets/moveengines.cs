using UnityEngine;
using System.Collections;

public class moveengines : MonoBehaviour {
    public GameObject commandconsole;
    public bool isdriving = true;
	// Use this for initialization
	void Start () {
        stopdriving();
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyUp(KeyCode.E))
        {
            if (Vector3.Distance(commandconsole.GetComponent<Transform>().position, this.GetComponent<Transform>().position) < 100)
            {
                isdriving = !isdriving;
                if(isdriving == true)
                {
                    startdriving();
                }
                if (isdriving ==false)
                {
                    stopdriving();
                }
            }
        }
        if (isdriving)
        {
            if (Input.GetKey(KeyCode.W))
            {

                foreach (imanengine t in this.GetComponentsInChildren<imanengine>())
                {
                    t.moveforwards('w');
                }
            }
            if (Input.GetKey(KeyCode.S))
            {
                foreach (imanengine t in this.GetComponentsInChildren<imanengine>())
                {
                    t.moveforwards('s');
                }
            }
            if (Input.GetKey(KeyCode.A))
            {
                foreach (imanengine t in this.GetComponentsInChildren<imanengine>())
                {
                    t.moveforwards('a');
                }
            }
            if (Input.GetKey(KeyCode.D))
            {
                foreach (imanengine t in this.GetComponentsInChildren<imanengine>())
                {
                    t.moveforwards('d');
                }
            }
        }

    }
    public void startdriving()
    {
        foreach (turnopaquenshit tur in this.GetComponentsInChildren<turnopaquenshit>())
        {
            tur.govis();
        }


        Rigidbody2D[] allChildren = gameObject.GetComponentsInChildren<Rigidbody2D>();
        foreach (Rigidbody2D child in allChildren)
        {
            if (child.gameObject.tag == "Player")
            {
                child.freezeRotation = false;
                child.gameObject.AddComponent<FixedJoint2D>();
                child.gameObject.GetComponent<FixedJoint2D>().connectedBody = this.GetComponent<Rigidbody2D>();
                child.gameObject.GetComponent<FixedJoint2D>().anchor = new Vector2(0, -0.30f);
            }
        }
    }












        
    public void stopdriving()
    {
        foreach (turnopaquenshit tur in this.GetComponentsInChildren<turnopaquenshit>())
        {
            tur.goinvis();
        }
        Rigidbody2D[] allChildren = gameObject.GetComponentsInChildren<Rigidbody2D>();
        foreach (Rigidbody2D child in allChildren)
        {
            if (child.gameObject.tag == "Player")
            {
                child.freezeRotation = true;
                Destroy(child.gameObject.GetComponent<FixedJoint2D>());
            }
        }
    }
}
