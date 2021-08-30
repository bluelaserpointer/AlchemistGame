using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(Rigidbody2D))]
public class MovementControl2D : MonoBehaviour
{
	public float maxSpeed = 10f;
	bool facingright = true;
	Animator anim;
	Rigidbody2D rigi;
	// Use this for initialization
	void Start()
	{
		//anim = GetComponent<Animator>();
		rigi = GetComponent<Rigidbody2D>();
	}

	// Update is called once per frame
	void FixedUpdate()
	{
		float xmove = Input.GetAxis("Horizontal");
		float ymove = Input.GetAxis("Vertical");
		//anim.SetFloat("speed", Mathf.Abs(move));
		rigi.velocity = new Vector2(xmove, ymove) * maxSpeed;
		if ((xmove > 0 && !facingright) || (xmove < 0 && facingright))
			Flip();
	}

	void Flip()
	{
		facingright = !facingright;
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}

}
