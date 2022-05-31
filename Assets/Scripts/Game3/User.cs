using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class User : MonoBehaviour {
	public float moveSpeed = 3, jumpSpeed = 2;

	private float dir = 1;

	public SpriteRenderer sprite;
	public Bump bump;
	private Rigidbody rb;

	private bool rage = false;
	private float rageDelay = 0;
	private bool canJump = false;

	private bool lockControls = true;

	private Vector3 targetLoc = Vector3.zero;

	private static User self;

	void Start () {
		self = this;
		rb = GetComponent<Rigidbody>();
		bump.enabled = false;
	}
	
	void Update () {
		if(transform.position.y < -150) Die();

		if(!lockControls) {
			float move = 0;
			bool jump = false;
			if(Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) move = moveSpeed;
			if(Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) move = -moveSpeed;
			Move(move, jump);
			if(Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.Space)) Jump();

			ApplyMovement();
		}
		
		if(rage) {
			rb.MovePosition(transform.position + new Vector3(0, -Random.Range(0, 3 + rageDelay), 0) / 10);
			rageDelay += Time.deltaTime;
		} else rageDelay = 0;

		sprite.color = Color.Lerp(Color.white, new Color(1, 0.4f, 0.4f), rageDelay);
	}

	void FixedUpdate() {
		float camTarget = Mathf.Lerp(Camera.main.transform.position.x, transform.position.x, Time.deltaTime * 4);
		Camera.main.transform.position = new Vector3(camTarget, Camera.main.transform.position.y, Camera.main.transform.position.z);
	}

	protected void Die() {
		OffenseSpawner.Offend("Self-Butchery");
		OffenseSpawner.EndGame();
		Destroy(gameObject);
	}

	protected void Move(float speed, bool jump) {
		if(speed > 0) {
			dir = 1;
			sprite.flipX = false;
		}
		else if(speed < 0) {
			dir =  - 1;
			sprite.flipX = true;
		}
		bump.enabled = speed != 0;
		if(speed == 0) {
			sprite.transform.localPosition = Vector3.zero;
			sprite.transform.localScale = Vector3.one;
		}
		targetLoc = new Vector3(speed, 0, 0) / 10;
	}

	protected void Jump() {
		if(!canJump) return;
		targetLoc += new Vector3(0, jumpSpeed, 0);
	}

	protected void ApplyMovement() {
		rb.MovePosition(transform.position + targetLoc);
	}

	void OnCollisionEnter(Collision col) {
		if(col.gameObject != null) canJump = true;
	}

	public static void ReadyPlayer() {
		self.lockControls = false;
		self.rage = false;
	}

	public static void Rage() {
		self.rage = true;
	}

	public static void Bumpy(bool i) {
		self.bump.enabled = i;
	}
}
