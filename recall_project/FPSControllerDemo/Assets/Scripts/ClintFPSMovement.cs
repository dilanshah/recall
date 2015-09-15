using UnityEngine;
using System.Collections;

public class ClintFPSMovement : MonoBehaviour {
	//for movement
	public float moveSpeed;
	public float turnAngle;

	//for gravitational jumping
	public float jumpSpeed;
	public float maxJump;

	//Bools to denote accending => max => landing => ground
	private bool jumping; 
	private bool landing;

	//Must know when to stop accent.
	public Transform playerObject; //Input player character
	private float originalHeight;	//Needed to receive starting y position

	//velocity multiplied up to the difference between these, 0 on apex, then down
	public float maxFrames;  //max # of seconds in jump. 60 frames = 1 second jump
	private float frameCount;  

	
	// Use this for initialization
	void Start () {
		moveSpeed = 10f;
		turnAngle = 180f;
		maxJump = 20f;
		jumping = false;
		landing = false;
		frameCount = 0;
		maxFrames = 30;
		jumpSpeed = 0.5f;
		originalHeight = playerObject.position.y;

		//Used to initially "freeze" motion. 
		//If this were not here, character would begin falling immediately if there was no rigibody obj underneath
		GetComponent<Rigidbody>().isKinematic = true;  //Toggling Kinematics just stops at this point
		GetComponent<Rigidbody>().isKinematic = false;
		
	}
	
	// Update is called once per frame
	void Update () {

		//Left analog joystick up/down OR w&s keybd controls forward/backward direction (+/- z)
		GetComponent<Transform>().Translate 
			(0f, 0f, moveSpeed * Input.GetAxis("Vertical") * Time.deltaTime);

		//Left analog joystick left right OR a&d kbd controls rotate left/right (rotate around Y)
		GetComponent<Transform>().Rotate  //Controls rotation around Y
			(0f,turnAngle * Input.GetAxis("Horizontal") * Time.deltaTime, 0f);

		//Joystick 3 OR space controls jump (Y up/down)
		if (Input.GetButton("Jump") && !jumping && !landing){  //Handle only 1 button jump till complete
			jumping = true;
		}

		if(jumping){
			frameCount++;

			GetComponent<Rigidbody>().velocity = new Vector3(0f, jumpSpeed * (maxFrames - frameCount), 0f);
			//GetComponent<Rigidbody>().AddForce(0f, (5 * maxFrames - frameCount) * Time.deltaTime, 0f);
			if(frameCount >= maxFrames){
				GetComponent<Rigidbody>().useGravity = true;  //to start downward direction
				jumping = false;
				landing = true;
			}
		}
		if (landing){
			GetComponent<Rigidbody>().useGravity = false;
			if(frameCount <= 0){
				frameCount = 0;
			}
			else{
				frameCount--;
			}

			GetComponent<Rigidbody>().velocity = new Vector3(0f, jumpSpeed * -(maxFrames - frameCount), 0f);
			//GetComponent<Rigidbody>().AddForce(0f, -(20 * maxFrames - frameCount) * Time.deltaTime, 0f);
	
			if(transform.position.y <= originalHeight){//Used to stop descent and keep at ground level
				GetComponent<Rigidbody>().isKinematic = true;  //Toggling Kinematics just stops at this point
				GetComponent<Rigidbody>().isKinematic = false; 
				landing = false;
			}


		}
	




			
	}
}
