using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitDetection : MonoBehaviour
{
    public GameObject attacker;
    public int damage;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject != attacker && other.gameObject.tag == "Character")
        {
            CharacterBase character = other.gameObject.GetComponent<CharacterBase>();
            if (!character.isBlocking)
            {
                Debug.Log("Got Em");
                character.reduceHP(damage);
                if (gameObject.tag == "Fireball")
                {
                    Destroy(gameObject);
                }
            }
            else
            {

            }
        }
        if (other.gameObject.tag == "Fireball" && gameObject.tag == "Fireball")
        {
            Destroy(other);
            Destroy(gameObject);
        }
    }
}
