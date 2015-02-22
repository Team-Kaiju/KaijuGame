using UnityEngine;
using System.Collections;

public class BasicPlayer : MonoBehaviour
{
	public GameObject rArmCol;
	public GameObject cam;
	public float moveSpeed = 10F;
	public float jumpForce = 10F;
	public float decelFactor = 0.01F;
	int punchTick = 0;
	Vector3 punchPos;

	// Use this for initialization
	void Start()
	{
		punchPos = rArmCol.transform.localPosition;
	}
	
	// Update is called once per frame
	void Update()
	{
		float vSpeed = Input.GetAxis ("Vertical");
		float hSpeed = Input.GetAxis("Horizontal");

		Vector3 forward = cam.transform.TransformDirection(Vector3.forward);
		forward.y = 0;
		forward = forward.normalized;
		Vector3 right = new Vector3(forward.z, 0, -forward.x);
		Vector3 moveDirection = (hSpeed * right + vSpeed * forward).normalized;

		moveDirection *= moveSpeed;
		this.rigidbody.velocity = Vector3.Lerp(this.rigidbody.velocity, new Vector3(moveDirection.x, this.rigidbody.velocity.y, moveDirection.z), decelFactor);

		if(moveDirection.magnitude > 0.1F && !Input.GetButton("Fire2"))
		{
			this.transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(moveDirection), 0.1F);
		} else if(Input.GetButton("Fire2"))
		{
			this.transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(forward), 0.1F);
		}
		
		if (Input.GetButtonDown ("Jump"))
		{
			this.rigidbody.AddForce(new Vector3(0F, jumpForce, 0F), ForceMode.Impulse);
		}

		// Simple punch script. Will be replaced later when a proper model with animations is implemented
		if(punchTick > 0)
		{
			rArmCol.transform.localPosition = punchPos + new Vector3(0F, 0F, Mathf.Sin(Mathf.Deg2Rad * punchTick/10 * 180) * 30F);
			rArmCol.renderer.enabled = true;
			rArmCol.collider.enabled = true;
			punchTick--;
		} else if(Input.GetButtonDown ("Fire1"))
		{
			punchTick = 10;
		} else
		{
			rArmCol.transform.localPosition = punchPos;
			//rArmCol.renderer.enabled = false;
			rArmCol.collider.enabled = false;
		}
	}
}
