using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class buddyMovement : MonoBehaviour
{
    //-----MOVEMENT--------//
    [Header("PLAYER MOVEMENT")]
    public float speed = 1f;
    private Rigidbody2D rigidBody;
    private float horizontal;
    public GameObject belowCast;
    public GameObject  cameraObject;
    public bool canMove; 
   //----------------------------//
   //-----------JUMP-------------//
    public float jumpForce = 1f;
    private bool canJump;
    private float rayLength;
   //---------------------//
   //-----ANIMATION-------//
    private Animator animator;
   //------SOUNDS---------//
    private AudioSource audioSource;
    [Space(height:20)]
    [Header("SOUNDS")]
    public AudioClip jumpSound;
    public AudioClip throwSound;
   //----------------------//
   //-------STONE----------//
   [Space(height:20)]
   [Header("STONE SHOOTING")]
    public GameObject stonePrefab;
    public Transform stoneOrigin;
    public float stoneSpeed, stoneCooldown; 
    private int numberStones;
    private float shootTimer; 
    public float initialShootTimer = 1f;
    //---------------------//
    [Space(height:20)]
    public int playerHealth = 5;
    public GameObject buddy;
    //------GAMEPLAY------//
    [Space(height:20)]
    [Header("GAMEPLAY")]
    public GameObject shootTutorialText;
    public TextMeshProUGUI playerHealthText;
    public GameObject shootTutorialCollider;
    public GameObject winText;
    public GameObject winZone;
    bool tutorialCollided;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        
        canMove = true;

        tutorialCollided = false;
        shootTutorialText.SetActive(false);
        
        rigidBody = GetComponent<Rigidbody2D>(); 
        animator = GetComponent<Animator>();   
        audioSource = GetComponent<AudioSource>();
        
        winText.SetActive(false);

        numberStones = 0;
        Debug.Log("number of stones: " + numberStones);   
        shootTimer = 0f;       
        
        rayLength = 0.65f;
        if(playerHealth <= 0)
        {
            playerHealth = 1;
        }               
        Debug.Log("Player's health is: " + playerHealth);
    }

    
    void Update()
    {
        canMove = true;

        if(gameObject == null) return; //IF THE PLAYER DOESN'T EXISTS (IF SOMETHING DESTROYED PLAYER), DO NOT EJECUTE THIS SCRIPT  

        playerHealthText.text = "PlayerHealth: " + playerHealth.ToString(); //SHOWS THE PLAYER'S LIFE ON TEXT

        shootTimer -= Time.deltaTime;  //SHOOT COOLDOWN

        horizontal = Input.GetAxisRaw("Horizontal"); //A and D or LEFT and RIGHT ARROWS HAVE VALUES BETWEEN -1 and 1

        if(horizontal < 0.0f) 
        {
            transform.localScale = new Vector3(-0.3f, 0.3f, 0.3f); //TO CHANGE THE DIRECTION OF THE CHARACTER
           
        } else if(horizontal > 0.0f)
        {
            transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
            
        } 

        if(tutorialCollided)
        {
            StartCoroutine(shootTutorial());
        }


        animator.SetBool("running", horizontal != 0.0f); //TO ANIMATE THE PLAYER

        animator.SetBool("running", horizontal != 0.0f);
        

        /*------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        --- *JUMPING and THROWING* 
        -------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        ---*/

        if(Input.GetKeyDown(KeyCode.W) && canJump) //If the player presses the W button and can, **JUMPS**
        {
            jump();
        }
        if(Input.GetKeyDown(KeyCode.Space) && numberStones > 0 && shootTimer <= 0f) 
        {
            StartCoroutine(Shoot()); //If the player presses the SPACE button, have stones (finding stones bags), and the shootingCooldown is 0, **SHOOTS**
        }

        Debug.DrawRay(transform.position, Vector3.down * rayLength, Color.red); //*ray to debugging the player jump*

        
        if(Physics2D.Raycast(transform.position, Vector2.down, rayLength)) //If the player is on a floor, CAN JUMP (one of the conditions to jump is true)
        {
           canJump = true;
        } else if(!Physics2D.Raycast(transform.position, Vector2.down, rayLength)) canJump = false; //If not, can't jump
        
        if(Physics2D.Raycast(belowCast.transform.position, Vector2.left, 0.67f))
        {
            canMove = true;
        } else canMove = false;

        
    }

    private void FixedUpdate()
    {
        if(canMove) rigidBody.velocity = new Vector2(horizontal * speed * Time.deltaTime, rigidBody.velocity.y); //THE LINE THAT MAKES THE PLAYER MOVE
    }

    private void jump()
    {
        rigidBody.AddForce(Vector2.up * jumpForce); //The line of the PLAYER_JUMP
        audioSource.PlayOneShot(jumpSound); //**and its sound**
    }

    IEnumerator Shoot()
    {
        int bDirection()
        {
            if(transform.localScale.x < 0f)
            {
                return -1;
            } 
            else if(transform.localScale.x > 0f) 
            {
                return +1;
            }

            return 0;
        } //DETECT THE PLAYER DIRECTION

        audioSource.PlayOneShot(throwSound);

        numberStones--; //Substract one stone 

        Debug.Log("number of stones: " + numberStones);
        
        shootTimer = initialShootTimer; //cooldown is Active  until the value is 0
        
        GameObject newStone = Instantiate(stonePrefab, stoneOrigin.position, Quaternion.identity); //Instantiate (as the function says xd) a stone at the player position
        
        newStone.GetComponent<Rigidbody2D>().velocity = new Vector2(stoneSpeed * bDirection() * Time.fixedDeltaTime, 0f); //Moves the instantiated stone to the actual direction of the player
        
        yield return new WaitForSeconds(stoneCooldown);
        
        Destroy(newStone); //after the stone cooldown, the instantiated stone have been destroyed

           
        
    }

    IEnumerator playerHit()
    {
        playerHealth--; 
        transform.position = new Vector3(-11.8699999f,-1.54999995f,0f);  //the position of the player is equal to the last checkpoint he was on
        cameraObject.transform.position = new Vector3(-5.01999998f, 0f, -10f); //*** the same to the camera***
        Debug.Log("Player health: " + playerHealth);
        if(playerHealth == 0) Destroy(gameObject);
        yield return new WaitForSeconds(0);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "projectile" || other.gameObject.tag == "dieZone")
        {
            StartCoroutine(playerHit()); //When the player touchs a projectile or he falls, its health substracts one
        } 

        if(other.gameObject.tag == "shootTutorial")
        {
            tutorialCollided = true; //the initial tutorial
        }        

        if(other.gameObject.tag == "stonesBag")
        {
            numberStones++;
            Debug.Log("number stones: " + numberStones);
        }

        if(other.gameObject.tag == "caveZone")
        {
            cameraObject.transform.position = new Vector3(46.3899994f, -14.0699997f, -10f);
        }

        if(other.gameObject.tag == "winZone")
        {
            StartCoroutine(win());
        }
    }
    
    IEnumerator win()
    {
        winText.SetActive(true);

        Destroy(winZone);

        yield return new WaitForSeconds(3);

        Application.Quit();   
    }

    IEnumerator shootTutorial()
    {
        shootTutorialText.SetActive(true);

        Destroy(shootTutorialCollider);

        tutorialCollided = false;

        yield return new WaitForSeconds(3);

        if(Input.GetKeyDown(KeyCode.Space))
        {
            Destroy(shootTutorialText);
        }

        Destroy(shootTutorialText);
    }
    


}
