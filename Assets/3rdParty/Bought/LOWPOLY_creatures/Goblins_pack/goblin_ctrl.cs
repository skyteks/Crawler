using UnityEngine;
using System.Collections;

public class goblin_ctrl : MonoBehaviour {

	private Animator anim;
	private CharacterController controller;
	private bool battle_state;
	public float speed = 6.0f;
	public float runSpeed = 1.0f;
	public float turnSpeed = 60.0f;
	public float gravity = 20.0f;
	public int goblin_class; // 1-warrior 2-shamon 3-archer
	private Vector3 moveDirection = Vector3.zero;



	// Use this for initialization
	void Start () {

		anim = GetComponent<Animator>();
		controller = GetComponent<CharacterController> ();


	}
	
	// Update is called once per frame
	void Update () 
	{


		if (Input.GetKey("2")) //состояние покоя
		{
			anim.SetInteger("battle", 1);
			battle_state = true;
			//rend.GetComponent<SkinnedMeshRenderer>().enabled = false;
	    	//GetComponentsInChildren.gbln_war_eq
			//rend.GetComponent<SkinnedMeshRenderer>().enabled = false;
		}
		if (Input.GetKey("1")) 			//
		{
			anim.SetInteger("battle", 0);
			battle_state = false;
		}
		if (Input.GetKey ("up")) {		 //moving
			if (battle_state == false)
			{
				anim.SetInteger ("moving", 1);//
				runSpeed = 1.0f;
			} else 
			{
				anim.SetInteger ("moving", 2);//
				runSpeed = 2.6f;
			}


		} else {
			anim.SetInteger ("moving", 0);
		}

		if (Input.GetKeyDown("o")) //death1
		{
			anim.SetInteger("moving", 13);
		}
		if (Input.GetKeyDown("i")) //death2
		{
			anim.SetInteger("moving", 14);
		}

		if (Input.GetKeyDown("u")) //hit
		{
			int n = Random.Range(0,2);
			if (n == 0) {
			anim.SetInteger("moving", 15);
			} else {anim.SetInteger("moving", 16);}
		}


		if (Input.GetKeyDown("r")) //RESET
		{
			anim.SetInteger("battle", 2);
		}



		switch (goblin_class) {
		case 1: 
			//print ("Goblin is Warrior");
			if (Input.GetMouseButtonDown (0)) //attack
			{
				anim.SetInteger("moving", 3);
			}
			if (Input.GetMouseButtonDown (1)) //power attack
			{
				anim.SetInteger("moving", 4);
			}
			if (Input.GetKeyDown("p")) //defence_start
			{
				anim.SetInteger("moving", 9);
			}
			if (Input.GetKeyUp("p")) //defence_end
			{
				anim.SetInteger("moving", 10);
			} 

			break;
			
		case 2:
			//print ("Goblin is Shaman");
			if (Input.GetMouseButtonDown (0)) //attack
			{
				anim.SetInteger("moving", 5);
			}
			if (Input.GetMouseButtonDown (1)) //power attack
			{
				anim.SetInteger("moving", 6);
			}
			if (Input.GetMouseButtonDown (2)) //attack
			{
				anim.SetInteger("moving", 3);
			}
			if (Input.GetKeyDown("p")) //defence_start
			{
				anim.SetInteger("moving", 11);
			}
			if (Input.GetKeyUp("p")) //defence_end
			{
				anim.SetInteger("moving", 12);
			} 
		
			break;
			
		case 3: 
			//print ("Goblin is Archer");
			if (Input.GetMouseButtonDown (0)) //attack
			{
				anim.SetInteger("moving", 7);
			}
			if (Input.GetMouseButtonDown (1)) //power attack
			{
				anim.SetInteger("moving", 8);
			}
			if (Input.GetKeyDown("p")) //defence_start
			{
				anim.SetInteger("moving", 11);
			}
			if (Input.GetKeyUp("p")) //defence_end
			{
				anim.SetInteger("moving", 12);
			} 

			break;
			
		}


	




		if(controller.isGrounded)
		{
			moveDirection=transform.forward * Input.GetAxis ("Vertical") * speed * runSpeed * (this.transform.localScale.z);

		}
		float turn = Input.GetAxis("Horizontal");
		transform.Rotate(0, turn * turnSpeed * Time.deltaTime, 0);
		controller.Move(moveDirection * Time.deltaTime);
		moveDirection.y -= gravity * Time.deltaTime;

		/*
		switch (goblin_class) {
		case 1: 
			print ("Goblin is Warrior");
			break;
			
		case 2:
			print ("Goblin is Shaman");
			break;
			
		case 3: 
			print ("Goblin is Archer");
			break;
			
		}*/
	
	}
}












