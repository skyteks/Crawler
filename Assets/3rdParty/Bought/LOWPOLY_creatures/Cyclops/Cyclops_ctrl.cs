using UnityEngine;
using System.Collections;

public class Cyclops_ctrl : MonoBehaviour {
	
	
	private Animator anim;
	private CharacterController controller;
	private int battle_state=0;
	public float speed = 1.0f;
	public float runSpeed = 3.0f;
	public float turnSpeed = 60.0f;
	public float gravity = 20.0f;
	public float jump_power = 150.0f;
	private Vector3 moveDirection = Vector3.zero;
	private float w_sp = 0.0f;
	private float r_sp = 0.0f;

	private int aaa;

	
	// Use this for initialization
	void Start () 
	{
						
		anim = GetComponent<Animator>();
		controller = GetComponent<CharacterController> ();

		w_sp = speed; //read walk speed
		r_sp = runSpeed; //read run speed
		runSpeed = 1;
	}
	
	// Update is called once per frame
	void Update () 
	{	
		if (Input.GetKey ("1"))  // turn to still state
		{ 		
			anim.SetInteger ("battle", 0);
			battle_state = 0;
			runSpeed = 1;
		}
		if (Input.GetKey ("2")) // turn to battle state
		{ 
			anim.SetInteger ("battle", 1);
			battle_state = 1;
			runSpeed = r_sp;
		}
		
		if (Input.GetKey ("up")) 
		{
			anim.SetInteger ("moving", 1);//walk/run/moving
			Debug.Log (runSpeed);
		}
		else 
		{
			anim.SetInteger ("moving", 0);
		}
		
		
		if (Input.GetKey ("down")) //walkback
		{
			anim.SetInteger ("moving", 2);
			runSpeed = 1;
		}
		if (Input.GetKeyUp ("down")) 
		{
			if (battle_state == 0) runSpeed = 1;
			else if (battle_state >0) runSpeed = r_sp;
		}

		if (Input.GetMouseButtonDown (0)) { //attack
			anim.SetInteger ("moving", 3);
		}
		if (Input.GetMouseButtonDown (1)) { //alt attack1
			anim.SetInteger ("moving", 4);
		}
		if (Input.GetMouseButtonDown (2)) { //alt attack2
			anim.SetInteger ("moving", 6);
		}

		//-----------------------------------------------------JUMP	

		if (Input.GetKeyUp ("space")) { //jump
			anim.SetInteger ("moving", 20);
		}

//-----------------------------------------------------

		if (Input.GetKeyDown ("p")) { // defence_start
			anim.SetInteger ("moving", 8);
		}
		if (Input.GetKeyUp ("p")) { // defence_end
			anim.SetInteger ("moving", 9);
		} 


		if (Input.GetKeyDown ("o")) { //die_1
			anim.SetInteger ("moving", 12);
		}

		if (Input.GetKeyDown ("i")) { //die_2
			anim.SetInteger ("moving", 13);
		}
		
		if (Input.GetKeyDown ("u")) { //hit
			int n = Random.Range (0, 2);
			if (n == 0) {
				anim.SetInteger ("moving", 10);
			} else {
				anim.SetInteger ("moving", 11);
			}
		}

		if (Input.GetKeyDown ("c")) { //roar
			anim.SetInteger ("moving", 16);
		}
		/*

*/	//

		if(controller.isGrounded)
		{
			moveDirection=transform.forward * Input.GetAxis ("Vertical") * speed * runSpeed * (this.transform.localScale.z);
			
		}
		float turn = Input.GetAxis("Horizontal");
		transform.Rotate(0, turn * turnSpeed * Time.deltaTime, 0);
		controller.Move(moveDirection * Time.deltaTime);
		moveDirection.y -= gravity * Time.deltaTime;
	}
}



