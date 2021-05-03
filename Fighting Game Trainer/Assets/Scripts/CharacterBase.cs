using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterBase : MonoBehaviour
{
    //Core Variables
    public int HP;
    public float speed;
    public int superCharge;
    public int player1Or2;
    public float stun; //May not be used
    public float groundDistance;

    public LayerMask groundMask;

    public bool isCrouching;
    public bool isJumping;
    public bool isGrounded;
    public bool isBlocking;
    public bool inSpecial;
    public bool isFacingLeft;

    public bool canPlay;

    public Rigidbody rigidBody;

    public Transform groundCheck;

    public Animator playerANIM;

    public CapsuleCollider hurtbox;

    float x;
    float y;
    float l;
    float m;
    float h;
    float s;

    string horizontal;
    string vertical;
    string light;
    string medium;
    string heavy;
    string special;



    // Start is called before the first frame update
    void Start()
    {
        setPlayer1Or2(player1Or2);
        setHP(100);
    }

    // Update is called once per frame
    void Update()
    {
        if (!canPlay)
        {
            setPlayer1Or2(3);
        }
        else
        {
            setPlayer1Or2(player1Or2);
        }
        if (isFacingLeft)
        {
            transform.rotation = Quaternion.Euler(new Vector3(0, 180, 0));
        }
        else
        {
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
        }
        if (player1Or2 != 3)
        {
            x = Input.GetAxis(horizontal);
            y = Input.GetAxis(vertical);
            l = Input.GetAxis(light);
            m = Input.GetAxis(medium);
            h = Input.GetAxis(heavy);
            s = Input.GetAxis(special);
        }
        

        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        if (x < 0 && isGrounded)
        {
            isBlocking = true;
        }
        else
        {
            isBlocking = false;
        }
        if (isGrounded)
        {
            isJumping = false;
            playerANIM.SetBool("Jumping", false);
            playerANIM.SetBool("Grounded", true);
        }

        if (y < 0 && isGrounded)
        {
            isCrouching = true;
            rigidBody.velocity = Vector3.zero;
            playerANIM.SetBool("Walking", false);
            playerANIM.SetBool("Crouching", true);
        }
        else
        {
            isCrouching = false;
            playerANIM.SetBool("Crouching", false);
        }
        if (y > 0 && isGrounded == true && !isCrouching)
        {
            playerANIM.SetBool("Jumping", true);
            playerANIM.SetBool("Grounded", false);
            if (x > 0)
            {
                rigidBody.velocity = new Vector3(x * speed - 5, 20, 0);
            }
            else if (x < 0)
            {
                rigidBody.velocity = new Vector3(x * speed + 5, 20, 0);
            }
            else
            {
                rigidBody.velocity = new Vector3(0, 20, 0);
            }

            isGrounded = false;
            isJumping = true;
        }

        if (!isCrouching && !isJumping && !inSpecial)
        {
            rigidBody.velocity = new Vector3(x, 0, 0) * speed;
            if (x != 0)
            {
                playerANIM.SetBool("Walking", true);
            }
            else
            {
                playerANIM.SetBool("Walking", false);
            }
        }

        if (l > 0)
        {
            playerANIM.SetBool("Light", true);
        }
        if (m > 0)
        {
            playerANIM.SetBool("Medium", true);
        }
        if (h > 0)
        {
            playerANIM.SetBool("Heavy", true);
        }
        if (s > 0 && isGrounded && !inSpecial)
        {
            if (x > 0)
            {
                DP();
            }
            else if (x < 0)
            {
                rigidBody.velocity = new Vector3(20, 0, 0);
                playerANIM.SetBool("BSpecial", true);
                inSpecial = true;
            }
            else
            {
                if (isGrounded)
                {
                    playerANIM.SetBool("NSpecial", true);
                    inSpecial = true;
                }
            }
        }
    }

    //Getters and Changers
    public int getHP()
    {
        return HP;
    }

    public void setHP(int startingHP)
    {
        HP = startingHP;
    }

    public void reduceHP(int damage)
    {
        HP -= damage;
        if (player1Or2 == 3)
        {
            GetComponent<AI>().CallForReset();
        }
        if (player1Or2 == 1)
        {
            GameObject.FindGameObjectWithTag("GM").GetComponent<GM>().ResetScene(0);
        }
        if (HP <= 0)
        {
            Debug.Log("KO");
        }
    }

    public int returnPlayer1Or2()
    {
        return player1Or2;
        //0 = Undefined
        //1 = Player 1
        //2 = Player 2
        //3 = AI;
    }

    public void setPlayer1Or2(int player)
    {
        if (player == 1)
        {
            horizontal = "Horizontal";
            vertical = "Vertical";
            light = "Light";
            medium = "Medium";
            heavy = "Heavy";
            special = "Special";
        }
        else if (player == 2)
        {
            horizontal = "Horizontal2";
            vertical = "Vertical2";
        }
        else if (player == 3)
        {
            horizontal = "";
            vertical = "";
            light = "";
            medium = "";
            heavy = "";
            special = "";
        }
    }

    public void DP()
    {
        rigidBody.velocity = new Vector3(0, 20, 0);
        playerANIM.SetBool("FSpecial", true);
        hurtbox.enabled = false;
        isGrounded = false;
        isJumping = true;
        playerANIM.SetBool("Grounded", false);
        inSpecial = true;
    }

    public float getSpeed()
    {
        return speed;
    }

    public void setSpeed(float newSpeed)
    {
        speed = newSpeed;
    }

    public int getSuperCharge()
    {
        return superCharge;
    }

    public void changeSuperCharge(int change)
    {
        superCharge += change;
        if (superCharge < 0)
        {
            superCharge = 0;
        }
        else if (superCharge > 100)
        {
            superCharge = 100;
        }
    }
}
