using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UIPlatformer {
	public class FlatPlayer : MonoBehaviour {
		private Camera mainCam;
		private Vector3 baseCamOffs;

		public Text scoreOBJ;

		public float speed = 2, jumpForce = 2;
		public KeyCode jump;

		private Rigidbody rb;

		private bool canJump = true;

		public GameObject lossScreen;

		public static float XPOS {
			get{try {return self.transform.position.x; }
			catch(System.Exception){return 0;};}
		}

		private float currentX = 0;

		private int lastTick;

		private static FlatPlayer self;

		public static int SCORE = 0;
		private bool moving = false;

		private SpriteRenderer sprite;

		public GameObject[] destroyOnDeath;

		void Start () {
			lossScreen.SetActive(false);
			self = this;
			sprite = GetComponent<SpriteRenderer>();
			mainCam = Camera.main;
			baseCamOffs = mainCam.transform.position;
			rb = GetComponent<Rigidbody>();

			StartCoroutine(AddPoints());
		}

		private IEnumerator AddPoints() {
			while(true) {
				if(moving) SCORE++;
				yield return new WaitForSeconds(0.5f);
			}
		}

		void Update() {
			Movement();
		}
		
		void FixedUpdate () {
			Vector3 camDest =  transform.position + baseCamOffs;
			camDest = new Vector3(Mathf.Clamp(camDest.x, currentX, camDest.x), camDest.y, camDest.z);
			mainCam.transform.position = Vector3.Lerp(mainCam.transform.position, camDest, Time.deltaTime * speed);
			moving = false;

			scoreOBJ.text = SCORE.ToString();
			if(transform.position.y < -25) Die();
		}

		protected void Die() {
			lossScreen.SetActive(true);
			foreach(GameObject i in destroyOnDeath) Destroy(i);
			Destroy(gameObject);
		}

		protected void Movement() {
			moving = false;
			if(Input.GetKeyDown(jump) || Input.GetKeyDown(KeyCode.Space)) Jump();

			float dir = 0;
			if(Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) {
				dir = 1;
				sprite.flipX = false;
			}
			if(Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) {
				dir = -1;
				sprite.flipX = true;
			}
			if(currentX < XPOS) {
				currentX = XPOS;
				moving = true;
			}

			if(Camera.main.WorldToViewportPoint(rb.position + new Vector3(speed * dir, 0, 0)).x > 0.1f) rb.position += new Vector3(speed * dir, 0, 0) * Time.deltaTime * 10;
			else moving = false;
			transform.localRotation = Quaternion.Euler(Mathf.Sin(Time.time * speed) * 2f, 0, Mathf.Cos(Time.time * speed * 8) * 4.5f);
		}

		protected void Jump() {
			if(!canJump) return;
			canJump = false;
			rb.AddForce(new Vector3(0, 1, 0) * jumpForce * 1000);
		}

		void OnCollisionEnter(Collision col) {
			if(col.gameObject.tag == "Platform") canJump = true;
		}

		public static bool IsMoving() {
			return self.moving;
		}
	}
}
