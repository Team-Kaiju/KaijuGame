using UnityEngine;
using System.Collections;

public class ExitPortal : MonoBehaviour
{
    public float spinSpeed = 25F;

    Material mat;
    BasicPlayer player;

	// Use this for initialization
	void Start ()
    {
        player = GameObject.FindObjectOfType<BasicPlayer>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        transform.Rotate(Vector3.up, Time.deltaTime * spinSpeed);
	}

    void OnTriggerEnter(Collider col)
    {
        if(col.gameObject == player.gameObject)
        {
            GameManager manager = GameObject.FindObjectOfType<GameManager>();

            if(manager != null)
            {
                manager.GameOver();
            }
        }
    }
}
