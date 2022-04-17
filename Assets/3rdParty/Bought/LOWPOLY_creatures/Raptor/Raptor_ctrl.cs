using UnityEngine;
using System.Collections;

public class Raptor_ctrl : MonoBehaviour {
	
	
	private Animator anim;
	private CharacterController controller;
	private bool battle_state;
	public float speed = 6.0f;
	public float runSpeed = 3.0f;
	public float turnSpeed = 60.0f;
	public float gravity = 20.0f;
	public float jump_power = 150.0f;
	private Vector3 moveDirection = Vector3.zero;
	private float w_sp = 0.0f;
	private float r_sp = 0.0f;

	
	// Use this for initialization
	void Start () 
	{
		anim = GetComponent<Animator>();
		controller = GetComponent<CharacterController> ();
		//battle_state = false;
		w_sp = 1; //read walk speed
		r_sp = runSpeed; //read run speed
	}
	
	// Update is called once per frame
	void Update () 
	{		


		//Debug.Log(Input.GetAxis ("Horizontal"));
	
		if (Input.GetKey ("2")) 
			{ //battle_idle
				anim.SetInteger ("battle", 1);
				battle_state = true;
						
			}
		if (Input.GetKey ("1")) 
			{ 			//idle
				anim.SetInteger ("battle", 0);
				battle_state = false;
			}
		if (Input.GetKey ("up"))
		   {		 //moving
			if (battle_state == false) {
				anim.SetInteger ("moving", 1);//walk
				runSpeed = w_sp;
			} else {
				anim.SetInteger ("moving", 2);//run
				runSpeed = r_sp;
			}
			
			
		} else {
			anim.SetInteger ("moving", 0);
		}

		if (Input.GetMouseButtonDown (0)) { //attack
			anim.SetInteger ("moving", 3);
		}
		if (Input.GetMouseButtonDown (1)) { //alt attack1
			anim.SetInteger ("moving", 4);
		}
//		if (Input.GetMouseButtonDown (2)) { //alt attack2
//			anim.SetInteger ("moving", 6);
//		}
//-----------------------------------------------------JUMP		

		if (Input.GetKeyUp ("space")) { //
			anim.SetInteger ("moving", 20);
		}

//		if (Input.GetKeyDown ("p")) { // defence_start
//			anim.SetInteger ("moving", 8);
//		}
//		if (Input.GetKeyUp ("p")) { // defence_end
//			anim.SetInteger ("moving", 9);
//		} 


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

		//-------------------------------------------------------------------TURNS

		if (Input.GetAxis ("Horizontal") > 0.1f) 
			{
				anim.SetLayerWeight(1,1f);
				anim.SetBool ("turn_right", true);
			} else 
			{
				anim.SetBool ("turn_right", false);
				//anim.SetLayerWeight(1,0f);
			}

		if (Input.GetAxis ("Horizontal") < -0.1f) 
			{
			anim.SetLayerWeight(1,1f);
			anim.SetBool ("turn_left", true);
			} else 
			{
			anim.SetBool ("turn_left", false);
			//anim.SetLayerWeight(1,0f);
			}

		//----------------------------------------------------------------------------------------

		if (controller.isGrounded) {

			moveDirection=transform.forward * Input.GetAxis ("Vertical") * speed * runSpeed * (this.transform.localScale.z);
			if (Input.GetAxis("Vertical") >= 0.1f)
			{
			float turn = Input.GetAxis("Horizontal");
			transform.Rotate(0, turn * turnSpeed * Time.deltaTime, 0);
			}

			if (Input.GetButton ("Jump")) {
				anim.SetInteger ("moving", 7);

			}
			
		}
		moveDirection.y -= gravity * Time.deltaTime;
		controller.Move (moveDirection * Time.deltaTime);
		}

}



