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
    public GameObject cameraObject;
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
    bool tutorialCollided;



    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        
        tutorialCollided = false;
        shootTutorialText.SetActive(false);
        
        rigidBody = GetComponent<Rigidbody2D>(); 
        animator = GetComponent<Animator>();   
        audioSource = GetComponent<AudioSource>();
        
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
        if(gameObject == null) return; //IF THE PLAYER DOESN'T EXISTS (IF SOMETHING DESTROYED PLAYER), DO NOT EJECUTE THIS SCRIPT  

        playerHealthText.text = "PlayerHealth: " + playerHealth.ToString(); //SHOWS THE PLAYER'S LIFE ON TEXT

        shootTimer -= Time.deltaTime;  //SHOOW COOLDOWN

        horizontal = Input.GetAxisRaw("Horizontal"); //A and D or LEFT and RIGHT ARROWS HAVE VALUES BETWEEN -1 and 1

        if(horizontal < 0.0f) 
        {
            transform.localScale = new Vector3(-0.3f, 0.3f, 0.3f); //TO CHANGE THE DIRECTION OF THE CHARACTER
           
        } else if(horizontal > 0.0f)
        {
            transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
            
        } 

<<<<<<< HEAD
        if(tutorialCollided)
        {
            StartCoroutine(shootTutorial());
        }


        animator.SetBool("running", horizontal != 0.0f); //TO ANIMATE THE PLAYER
=======
            animator.SetBool("running", horizontal != 0.0f);
>>>>>>> 5433a905378139cc4e709de4635f915446280c57
        

        /*------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        --- *JUMPING and THROWING*
        -------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        ---*/

        if(Input.GetKeyDown(KeyCode.W) && canJump) //IF THE PLAYER CAN JUMP IN THAT MOMENT AND PRESS THE *W* BUTTON, JUMPS
        {
            jump();
        }
        if(Input.GetKeyDown(KeyCode.Space) && numberStones > 0 && shootTimer <= 0f) 
        {
            StartCoroutine(Shoot()); //IF THE PLAYER PRESS THE *SPACE*, THE COOLDOWN IS 0, AND HAVE STONES, SHOOTS
        }

        Debug.DrawRay(transform.position, Vector3.down * rayLength, Color.red); //*ray to debugging the player jump*

        
        if(Physics2D.Raycast(transform.position, Vector2.down, rayLength)) //IF THE PLAYER IS IN THE FLOOR, CAN JUMP
        {
           canJump = true;
        } else if(!Physics2D.Raycast(transform.position, Vector2.down, rayLength)) canJump = false;

               

    }

    private void FixedUpdate()
    {
        rigidBody.velocity = new Vector2(horizontal * speed * Time.deltaTime, rigidBody.velocity.y); //THE LINE THAT MAKES THE PLAYER MOVE
    }

    private void jump()
    {
        rigidBody.AddForce(Vector2.up * jumpForce); //THE LINE THAT ADDS A FORCE (Xd) TO JUMP
        audioSource.PlayOneShot(jumpSound);
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

        numberStones--;

        Debug.Log("number of stones: " + numberStones);
        
        shootTimer = initialShootTimer;
        
        GameObject newStone = Instantiate(stonePrefab, stoneOrigin.position, Quaternion.identity);
        
        newStone.GetComponent<Rigidbody2D>().velocity = new Vector2(stoneSpeed * bDirection() * Time.fixedDeltaTime, 0f);
        
        yield return new WaitForSeconds(stoneCooldown);
        
        Destroy(newStone);
           
        
    }

    IEnumerator playerHit()
    {
        playerHealth--;
        transform.position = new Vector3(-11.8699999f,-1.54999995f,0f); 
        cameraObject.transform.position = new Vector3(-5.01999998f, 0f, -10f);
        Debug.Log("Player health: " + playerHealth);
        if(playerHealth == 0) Destroy(gameObject);
        yield return new WaitForSeconds(0);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "projectile" || other.gameObject.tag == "dieZone")
        {
            StartCoroutine(playerHit());
        } 

        if(other.gameObject.tag == "shootTutorial")
        {
            tutorialCollided = true;
        }        

        if(other.gameObject.tag == "stonesBag")
        {
            numberStones++;
            Debug.Log("number stones: " + numberStones);
        }

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
