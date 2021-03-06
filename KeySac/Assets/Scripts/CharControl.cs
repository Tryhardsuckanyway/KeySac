﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharControl : MonoBehaviour
{
    Animator animator;
    StateMachine gameManager;
	public Material trailmat;
    float moveSpeed = 4f; //Movement speed
    //float maxSpeed = 4f; //Maximum speed of player
    float turnSpeed = 50f; //Turning/rotation speed; adjustable

    //float acceleration = 2f;
    //float deceleration = 2f;
    private int health;
    private int damage;
    public Rigidbody rb;

    public Text healthUI;

    private GameObject stateMachine;
    //Prefab for bullets player shoots
    public GameObject bulletPrefab;

    //Spawn point for player bullets
    public Transform bulletSpawn;

    //AudioSource
    public AudioSource audioSource;

	// Use this for initialization
	void Start () {
		health = 1;
		gameManager = FindObjectOfType<StateMachine> ();
		animator = GetComponent<Animator> ();
        rb = GetComponent<Rigidbody>();
		setStats ();
        //here for testing
        //gameManager.trade("q");

        //Set Health UI
        SetHealthUIText();
		audioSource = FindObjectOfType<AudioSource> ();
    }
	
	void Update () {
		if (health <= 0) {
			gameManager.OnDeath ();
			//Destroy (this.gameObject);
		}
		if (Input.anyKey) { //Only execute if a key is being pressed
				Move ();
			}
	}

    void Move()
    {
		
        bool[] Keys = gameManager.getKeys();
        if (Input.GetKey(KeyCode.W) && Keys[1])
        {
            transform.position += transform.forward * Time.deltaTime * moveSpeed;
            //rb.velocity  = transform.forward * moveSpeed * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.S) && Keys[4])
        {
            transform.position += -transform.forward * Time.deltaTime * moveSpeed;
            //rb.AddRelativeForce(-transform.forward * moveSpeed * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.A) && Keys[3])
        {
            transform.position += -transform.right * Time.deltaTime * moveSpeed;
        }
        else if (Input.GetKey(KeyCode.D) && Keys[5])
        {
            transform.position += transform.right * Time.deltaTime * moveSpeed;
        }

        if (Input.GetKey(KeyCode.Q) && Keys[0])
        {
            transform.Rotate(Vector3.up, -turnSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.E) && Keys[2])
        {
            transform.Rotate(Vector3.up, turnSpeed * Time.deltaTime);
        }

        if (Input.GetKeyDown(KeyCode.Space) && Keys[6])
        {
			if (animator.GetCurrentAnimatorStateInfo (0).IsName ("Idle")) {
				StartCoroutine ("Fire");
			}
        }
    }

    IEnumerator Fire()
    {
            //Start firing animation
            animator.SetTrigger("Fire");

            //Wait for point in animation to fire bullet
            yield return new WaitForSeconds(0.9f);

            audioSource.Play();

            //Create bullet
            var bullet = (GameObject)Instantiate(bulletPrefab, bulletSpawn.position, bulletSpawn.rotation);
            //Add velocity to bullet
            bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * 60;
            //Bullet trail add
            bullet.AddComponent<TrailRenderer>();
			bullet.GetComponent<TrailRenderer> ().material = trailmat;

    }
    
	//Sets bullet damage
	void SetDamage(){
		BulletScript.SetDamage (damage);
	}

	//Updates stats from StateMachine
	public void setStats(){
		health = gameManager.getHealth ();
		damage = gameManager.getDamage ();
		SetDamage ();
		moveSpeed = gameManager.getSpeed ();
		turnSpeed = gameManager.getTurn ();
	}

	private void OnTriggerEnter(Collider other){
		//Controls collision with enemy
		if ((other.tag == "Enemy" ) || (other.tag == "Patrol")) {
			health -= 1;
			Destroy(other.gameObject);
		}
		if (other.tag == "Finish") {
			gameManager.onLevelFinish ();
		}
	}

    public int getHealth() {
        return this.health;
    }

    void SetHealthUIText()
    {
        healthUI.text = "Health: " + health.ToString();
    }
}
