using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI : MonoBehaviour
{
    public int excersise;
    public CharacterBase character;
    public Animator actions;

    public CharacterBase Player;

    public Rigidbody rb;

    public GM gameMaster;

    public bool fall;
    public float jumpVel;

    public bool excersiseActive;
    public float pauseBetweenActions;

    public int Solution1Attempts;
    public int Solution2Attempts;
    public int Solution3Attempts;

    public int Solution1Success;
    public int Solution2Success;
    public int Solution3Success;

    public bool Solution1;
    public bool Solution2;
    public bool Solution3;

    public bool inDp;

    void Start()
    {
        excersise = GameObject.FindGameObjectWithTag("ExcersiseTracker").GetComponent<ExcersiseTracker>().Excersise;   
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Player" && character.isBlocking)
        {
            actions.SetBool("Medium", true);
        }
    }
    public void Update()
    {
        if (!character.canPlay) { }
        else
        {
            pauseBetweenActions -= Time.deltaTime;
            switch (excersise)
            {
                //Idle
                case 0:
                    break;
                //Fireball Spam
                case 1:
                    if (excersiseActive == false)
                    {
                        actions.SetBool("Grounded", true);
                        actions.SetBool("FSpecial", false);
                        character.isBlocking = false;
                        
                        if (Solution3)
                        {
                            pauseBetweenActions = 0.25f;
                        }
                        else
                        {
                            pauseBetweenActions = 1f;
                        }
                        actions.SetBool("NSpecial", true);
                        excersiseActive = true;
                    }
                    if (excersiseActive == true && pauseBetweenActions > 0)
                    {

                        
                        if (Player.playerANIM.GetBool("BSpecial"))
                        {
                            Debug.Log("Tatsu");
                            if (Solution2)
                            {
                                character.isBlocking = true;
                            }

                            Solution2Attempts += 1;
                        }
                        else if (Player.isJumping == true)
                        {
                            Debug.Log("Jumping");
                            Solution1Attempts += 1;
                            character.isBlocking = false;
                        }
                        else if (Player.playerANIM.GetBool("NSpecial"))
                        {
                            Solution3Attempts += 1;
                            Debug.Log("Fireball War");
                            character.isBlocking = false;
                        }
                    }
                    else if (excersiseActive == true && pauseBetweenActions <= 0)
                    {
                        if (!Solution1 && !Solution2)
                        {
                            actions.SetBool("NSpecial", true);
                            if (Solution3)
                            {
                                pauseBetweenActions = 0.05f;
                            }
                            else
                            {
                                pauseBetweenActions = 1f;
                            }
                        }
                        else
                        {
                            float Dist = transform.position.x - Player.transform.position.x;
                            if (Dist < 10)
                            {
                                if (Player.isJumping && Solution1)
                                {
                                    actions.SetBool("NSpecial", false);
                                    character.DP();
                                }
                                else if (Solution2 && !Player.isJumping)
                                {
                                    Debug.Log("Attack");
                                    actions.SetBool("NSpecial", false);
                                    character.isBlocking = true;
                                }
                            }
                            else
                            {
                                actions.SetBool("Grounded", true);
                                actions.SetBool("NSpecial", true);
                                if (Solution3)
                                {
                                    pauseBetweenActions = 0.05f;
                                }
                                else
                                {
                                    pauseBetweenActions = 1f;
                                }
                            }
                        }
                    }

                    break;
                //Jump In
                case 2:
                    if (excersiseActive == false)
                    {
                        rb.velocity = new Vector3(-15, 15, 0);
                        excersiseActive = true;
                        actions.SetBool("Jumping", true);
                        jumpVel = 0.75f;
                    }
                    else if (excersiseActive == true && pauseBetweenActions >= 0)
                    {
                        pauseBetweenActions -= Time.deltaTime;
                        transform.position += Vector3.left * 0.4f;
                        transform.position += Vector3.up * jumpVel;

                        if (transform.position.y <= 2.7f)
                        {
                            gameMaster.ResetScene(0);
                        }
                        jumpVel -= Time.deltaTime;
                        float Dist = transform.position.x - Player.transform.position.x;
                        if (Dist < 10)
                        {
                            actions.SetBool("Heavy", true);
                        }
                        else
                        {
                            actions.SetBool("Heavy", false);
                        }

                        if (Player.isJumping == true)
                        {
                            Debug.Log("Air To Air");
                            Solution1Attempts += 1;
                        }
                        else if (Player.playerANIM.GetBool("FSpecial"))
                        {
                            Debug.Log("DP");
                            Solution2Attempts += 1;
                        }
                        else if (Player.playerANIM.GetBool("Heavy"))
                        {
                            Solution3Attempts += 1;
                            Debug.Log("Neautral Anti Air");
                        }
                    }
                    else if (excersiseActive == true && pauseBetweenActions <= 0)
                    {
                        pauseBetweenActions = 5f;
                        if (character.isGrounded)
                        {
                            character.rigidBody.velocity = new Vector3(-15, 15, 0);
                        }
                    }
                    break;
                default:
                    break;
            }
        }
    }

    public void ResetAttempts()
    {
        Solution1Attempts = 0;
        Solution2Attempts = 0;
        Solution3Attempts = 0;

        excersiseActive = false;
    }

    public void CallForReset()
    {
        actions.Play("Idle", -1, 0.1f);
        int biggest = Mathf.Max(Solution1Attempts, Solution2Attempts, Solution3Attempts);
        if (biggest == Solution1Attempts && !Solution1)
        {
            Solution1Success += 1;
            gameMaster.ChangePrompt("You found the First solution, try to do this " + (3 - Solution1Success) + " More times");
            if (Solution1)
            {
                gameMaster.ChangePrompt("You've aready done this soltuion, try to find something else");
            }
            else if (Solution1Success >= 3)
            {
                gameMaster.ChangePrompt("You've completed the first solution, congrats!");
                Solution1 = true;
            }
        }
        else if (biggest == Solution2Attempts && !Solution2)
        {
            Solution2Success += 1;
            Debug.Log("The player Used Back Special");
            gameMaster.ChangePrompt("You found the Second solution, try to do this " + (3 - Solution2Success) + " More times");
            if (Solution2)
            {
                gameMaster.ChangePrompt("You've aready done this soltuion, try to find something else");
            }
            if (Solution2Success >= 3)
            {
                gameMaster.ChangePrompt("You've completed the Second solution, congrats!");
                Solution2 = true;
            }
        }
        else if (biggest == Solution3Attempts && !Solution3)
        {
            Solution3Success += 1;
            Debug.Log("The player won the fireball war");
            gameMaster.ChangePrompt("You found the Third solution, try to do this " + (3 - Solution3Success) + " More times");
            if (Solution3)
            {
                gameMaster.ChangePrompt("You've aready done this soltuion, try to find something else");
            }
            if (Solution3Success >= 3)
            {
                gameMaster.ChangePrompt("You've completed the Third solution, congrats!");
                Solution3 = true;
            }
        }
        else
        {
            Debug.Log(Solution1Attempts + "," + Solution2Attempts + "," + Solution3Attempts);
        }

        if (Solution1 && Solution2 && Solution3)
        {
            gameMaster.ExcersiseOver();
        }

        excersiseActive = false;

        gameMaster.ResetScene(1);
    }
}
