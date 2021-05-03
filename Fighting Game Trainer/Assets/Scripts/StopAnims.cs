using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopAnims : MonoBehaviour
{
    //HB = HitBox
    public GameObject leftHandHB;
    public GameObject rightHandHB;
    public GameObject rightLegHB;
    public GameObject leftLegHB;

    public GameObject fireball;

    public GameObject character;

    public Transform front;

    public CharacterBase characterScript;

    public Animator playerANIM;
    public void StopLight()
    {
        leftHandHB.SetActive(false);
        playerANIM.SetBool("Light", false);
    }
    public void StopMedium()
    {
        rightHandHB.SetActive(false);
        playerANIM.SetBool("Medium", false);
    }
    public void StopHeavy()
    {
        rightLegHB.SetActive(false);
        playerANIM.SetBool("Heavy", false);
    }

    public void Stopjm()
    {
        leftLegHB.SetActive(false);
        rightLegHB.SetActive(false);
        playerANIM.SetBool("Medium", false);
    }
    public void Stopjh()
    {
        leftLegHB.SetActive(false);
        playerANIM.SetBool("Heavy", false);
    }

    public void StopNSpecial()
    {
        playerANIM.SetBool("NSpecial", false);
        characterScript.inSpecial = false;
    }
    public void StopFSpecial()
    {
        playerANIM.SetBool("FSpecial", false);
        characterScript.inSpecial = false;
        characterScript.isJumping = true;
        characterScript.hurtbox.enabled = true;
        rightHandHB.SetActive(false);
    }
    public void StopBSpecial()
    {
        playerANIM.SetBool("BSpecial", false);
        characterScript.inSpecial = false;
        rightLegHB.SetActive(false);
    }

    public void ActivateJMHitBox()
    {
        leftLegHB.SetActive(true);
    }

    public void ActivateLightHitbox()
    {
        leftHandHB.SetActive(true);
    }
    public void ActivateMediumHitbox()
    {
        rightHandHB.SetActive(true);
    }
    public void ActivateHeavyHitbox()
    {
        rightLegHB.SetActive(true);
    }

    public void SpawnFireball()
    {
        GameObject newFireBall = Instantiate(fireball, front.position, Quaternion.identity);
        newFireBall.GetComponent<HitDetection>().attacker = character;
        if (characterScript.isFacingLeft == true)
        {
            newFireBall.GetComponent<Rigidbody>().velocity += new Vector3(-10, 0, 0);
        }
        else
        {
            newFireBall.GetComponent<Rigidbody>().velocity += new Vector3(10, 0, 0);
        }
    }

}
