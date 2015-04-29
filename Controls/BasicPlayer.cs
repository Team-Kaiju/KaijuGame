using UnityEngine;
using System.Collections;

public class BasicPlayer : ObjDestroyable
{
	public GameObject rArmCol;
	public GameObject cam;
	public float moveSpeed = 10F;
	public float jumpForce = 10F;
	public float decelFactor = 0.01F;
	public float jumpCooldown = 1F;
	float curJmpCool = 0F;
	float punchTick = 0;
	Vector3 punchPos;
	public float gravity = 1F;
	[HideInInspector]
	public bool onGround = false;

    public Animation animObj;
    public string walkAnim;
    public string idleAnim;
    public string attackAnim;
    public string walkAttackAnim;

    Vector3 spawnPoint;
    Quaternion spawnRotation;

	// Use this for initialization
	public override void Start()
	{
		base.Start();

        spawnPoint = transform.position;
        spawnRotation = transform.rotation;

		punchPos = rArmCol.transform.localPosition;
	}
	
	// Update is called once per frame
	public override void Update()
	{
		base.Update();

        if(this.transform.position.y <= 50F)
        {
            this.transform.position = spawnPoint;
            this.transform.rotation = spawnRotation;
            this.GetComponent<Rigidbody>().velocity = Vector3.zero;
            this.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        }

		float vSpeed = Input.GetAxis ("Vertical");
		float hSpeed = Input.GetAxis("Horizontal");

		Vector3 forward = cam.transform.TransformDirection(Vector3.forward);
		forward.y = 0;
		forward = forward.normalized;
		Vector3 right = new Vector3(forward.z, 0, -forward.x);
		Vector3 moveDirection = (hSpeed * right + vSpeed * forward).normalized;

		moveDirection *= moveSpeed * (Input.GetButton("Sprint")? 1.5F : 1F);
		this.GetComponent<Rigidbody>().AddForce(Vector3.down * gravity);
		this.GetComponent<Rigidbody>().velocity = Vector3.Lerp(this.GetComponent<Rigidbody>().velocity, new Vector3(moveDirection.x, this.GetComponent<Rigidbody>().velocity.y, moveDirection.z), decelFactor);
		this.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;

		onGround = Physics.Raycast(transform.position + (Vector3.up * 0.5F), Vector3.down, 3F);

		if(moveDirection.magnitude > 0.1F && !Input.GetButton("Fire2"))
		{
			this.transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(moveDirection), 0.1F);
		} else if(Input.GetButton("Fire2"))
		{
			this.transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(forward), 0.1F);
        }

        if(moveDirection.magnitude > 0.1F)
        {
            if (punchTick > 0 || this.animObj.IsPlaying(attackAnim) || this.animObj.IsPlaying(walkAttackAnim))
            {
                if(walkAttackAnim != null && walkAttackAnim.Length > 0)
                {
                    this.animObj.Play(walkAttackAnim);
                }
            } else
            {
                if(walkAnim != null && walkAnim.Length > 0)
                {
                    this.animObj[walkAnim].speed = Input.GetButton("Sprint")? 1.5F : 1F;
                    this.animObj.Play(walkAnim);
                }
            }
        }
        else if (punchTick > 0 || this.animObj.IsPlaying(attackAnim) || this.animObj.IsPlaying(walkAttackAnim))
        {
            if(attackAnim != null && attackAnim.Length > 0)
            {
                this.animObj[attackAnim].speed = 2F;
                this.animObj.Play(attackAnim);
            }
        }
        else
        {
            if (idleAnim != null && idleAnim.Length > 0)
            {
                this.animObj.Play(idleAnim);
            }
        }
		
		if (Input.GetButtonDown ("Jump") && onGround && curJmpCool <= 0F)
		{
			curJmpCool = jumpCooldown;
			this.GetComponent<Rigidbody>().AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
		} else if(onGround && curJmpCool > 0F)
		{
			curJmpCool -= Time.deltaTime;
		}

        float punchDist = 10F;
        float punchSpeed = 25;

		// Simple punch script. Will be replaced later when a proper model with animations is implemented
        if (punchTick > 0 || this.animObj.IsPlaying(attackAnim) || this.animObj.IsPlaying(walkAttackAnim))
		{
            punchTick = punchTick >= 0 ? punchTick : 0;
            rArmCol.transform.localPosition = punchPos + new Vector3(0F, 0F, Mathf.Clamp(Mathf.Sin(Mathf.Deg2Rad * punchTick / punchSpeed * 360), 0F, 180F) * punchDist);
			//rArmCol.GetComponent<Renderer>().enabled = true;
			rArmCol.GetComponent<Collider>().enabled = true;
			punchTick -= 60 * Time.deltaTime;
		} else if(Input.GetButton ("Fire1"))
		{
			rArmCol.GetComponent<Collider>().enabled = false;
            punchTick = punchSpeed;
		} else
		{
			rArmCol.transform.localPosition = punchPos;
			//rArmCol.renderer.enabled = false;
			rArmCol.GetComponent<Collider>().enabled = false;
		}
	}
}
