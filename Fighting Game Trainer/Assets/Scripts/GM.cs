using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class GM : MonoBehaviour
{
    public float maxTimer;
    public float timer;

    public float inputTimer;

    public int excersise;
    public int failTracker;

    public bool inExplonation;
    public bool excersiseComplete;

    public GameObject excersiseTracker;

    public VideoPlayer VP;
    public VideoClip[] videoClips;

    public GameObject character1;
    public GameObject character2;

    public GameObject mainCanvas;
    public Text timerTXT;
    public Text promptTXT;

    public GameObject explonationCanvas;
    public Text titleTXT;
    public Text descTXT;

    public CharacterBase character1Script;
    public CharacterBase character2Script;


    public void ResetScene(int type)
    {
        GameObject[] Fireballs = GameObject.FindGameObjectsWithTag("Fireball");
        if (Fireballs.Length >= 1)
        {
            for (int i=0; i < Fireballs.Length; i++)
            {
                Destroy(Fireballs[i]);
            }
        }

        if (type == 0)
        {
            failTracker += 1;
            if (failTracker >= 3)
            {
                mainCanvas.SetActive(false);
                character1Script.canPlay = false;
                character2Script.canPlay = false;
                inputTimer = 1f;
                inExplonation = true;
                VP.gameObject.SetActive(true);
                failTracker = 0;
            }
            if (character2.GetComponent<AI>().excersise == 1)
            {
                if (!character2.GetComponent<AI>().Solution1)
                {
                    promptTXT.text = "Try jumping over the fireballs";
                    if (inExplonation)
                    {
                        VP.clip = videoClips[0];
                        VP.Play();
                    }
                }
                else if (!character2.GetComponent<AI>().Solution2)
                {
                    promptTXT.text = "Try using back special";
                    if (inExplonation)
                    {
                        VP.clip = videoClips[1];
                        VP.Play();
                    }
                }
            
                else if (!character2.GetComponent<AI>().Solution3)
                {
                    promptTXT.text = "Try beating them with your own fireballs";
                    if (inExplonation)
                    {
                        VP.clip = videoClips[2];
                        VP.Play();
                    }
                }
            }
            else if (character2.GetComponent<AI>().excersise == 2)
            {
                if (!character2.GetComponent<AI>().Solution1)
                {
                    promptTXT.text = "Try jumping to them and hitting them";
                    if (inExplonation)
                    {
                        VP.clip = videoClips[3];
                        VP.Play();
                    }
                }
                else if (!character2.GetComponent<AI>().Solution2)
                {
                    promptTXT.text = "Try using Forward Special";
                    if (inExplonation)
                    {
                        VP.clip = videoClips[4];
                        VP.Play();
                    }
                }
                else if (!character2.GetComponent<AI>().Solution3)
                {
                    promptTXT.text = "Try using a heavy attack to hit them first";
                    if (inExplonation)
                    {
                        VP.clip = videoClips[5];
                        VP.Play();
                    }
                }
            }
        }

        character2.GetComponent<AI>().ResetAttempts();

        character1.transform.position = new Vector3(-19.9f,2.69f,8.08f);
        character1.transform.rotation = Quaternion.Euler(0, 0, 0);
        character1.GetComponent<Rigidbody>().velocity = Vector3.zero;

        character2.transform.position = new Vector3(20.01f, 2.69f, 8.08f);
        character2.transform.rotation = Quaternion.Euler(0, 180, 0);
        character2.GetComponent<Rigidbody>().velocity = Vector3.zero;
        timer = maxTimer;
    }

    public void ChangePrompt(string promptText)
    {
        promptTXT.text = promptText;
    }

    public void ExcersiseOver()
    {
        character1Script.canPlay = false;
        character2Script.canPlay = false;
        mainCanvas.active = false;
        explonationCanvas.active = true;
        inputTimer = 1f;
        excersiseComplete = true;
        titleTXT.text = "Congratulations!";
        descTXT.text = "You found every way to counter this interaction!";
    }

    // Start is called before the first frame update
    void Start()
    {
        /*
        VP.gameObject.SetActive(true);
        VP.Play();
        */
        excersiseTracker = GameObject.FindGameObjectWithTag("ExcersiseTracker");
        excersise = excersiseTracker.GetComponent<ExcersiseTracker>().Excersise;

        character1Script = character1.GetComponent<CharacterBase>();
        character2Script = character2.GetComponent<CharacterBase>();
        character1Script.canPlay = false;
        character2Script.canPlay = false;
        mainCanvas.active = false;
        if (excersise == 1)
        {
            titleTXT.text = "Fireball War";
            descTXT.text = "Find a way to get around the enemies fireballs and hit them. There are 3 possible solutions to this excersise";
        }
        else if (excersise == 2)
        {
            titleTXT.text = "Anti-Air Training";
            descTXT.text = "Find a way to hit the enemy out of the sky before they hit you. There are 3 possible solutions to this excersise";
        }
        explonationCanvas.active = true;
        inExplonation = true;
    }

    

    // Update is called once per frame
    void Update()
    {
        if (inExplonation)
        {
            inputTimer -= Time.deltaTime;
            if (Input.anyKey && inputTimer <= 0)
            {
                mainCanvas.active = true;
                explonationCanvas.active = false;
                character1Script.canPlay = true;
                character2Script.canPlay = true;
                VP.gameObject.SetActive(false);
                inExplonation = false;
            }
        }
        else if (excersiseComplete)
        {
            inputTimer -= Time.deltaTime;
            if (Input.anyKey && inputTimer <= 0)
            {
                Destroy(excersiseTracker);
                SceneManager.LoadScene(0);
                excersiseComplete = false;
                
            }
        }
        else
        {

        }
        float p1X = character1.transform.position.x;
        float p2X = character2.transform.position.x;

        if (p1X > p2X)
        {
            character1Script.isFacingLeft = true;
            character2Script.isFacingLeft = false;
        }
        else if (p2X > p1X)
        {
            character1Script.isFacingLeft = false;
            character2Script.isFacingLeft = true;
        }
    }
}
