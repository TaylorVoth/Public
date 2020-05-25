using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Player : MonoBehaviour {

    public Material black;
    public GameObject master;
    GameMaster masterScript;
    public GameObject corpses;

    Vector3 clickspot;
    Vector3 mousePos;
    [SerializeField]
    float speed = 2.5f;
    float timer;
    bool timerOn;
    bool dashing;
    //public GameObject marker;
    RaycastHit hit;
    public Text timertext;

    SphereCollider myCollider;

    // Use this for initialization


    void Awake () {

        speed = 2.5f;
        masterScript = master.GetComponent<GameMaster>();
        myCollider = GetComponent<SphereCollider>();
        //myCollider.enabled = false;
    }
	
	// Update is called once per frame
	void Update () {
        if(masterScript.gamePlaying)
        {


            timertext.text = timer.ToString("F2");
            mousePos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 6.9f));
            if(!dashing) //if im not dashing
            {

                this.transform.position = Vector3.MoveTowards(this.transform.position, mousePos, speed * Time.deltaTime); //constnatly move towards my mouse
                this.transform.LookAt(mousePos); //always look at the mouse
            }
        
            if(timerOn) //if the timer is on
            {
            
                timer += Time.deltaTime; //count the timer up
            }
            if(Input.GetMouseButtonDown(0) && !dashing) //if i click while not dashing
            {

                clickspot = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 6.9f)); //save the place i clicked
                timerOn = true; //start counting up

                
            }
            if (timer < .8f && timer > 0)  //if less than 0.8f time has passed, but more than 0
            {
                dashing = true; //turn Dashing true for invulnerability and killing
                transform.Translate(Vector3.forward * speed * Time.deltaTime * 2); //dash towards at the basic movespeed (* time.deltatime for frame skips) * 2 cause dashing is twice as fast as moving
}
        
            if(timer > .75f) //if the time passed 0.75 seconds
            {

                dashing = false; //turn off dashing invuln and killing
                timerOn = false; //stop counting with the timer
                timer = 0; //reset the timer to 0 for next time
            }
        }
        //enable hurtbox
    }

    //ALL THE ON KILL STUFF IS HEEEEREE
    private void OnTriggerEnter(Collider other)
    {
        if (dashing)
        {

            Debug.Log("Got 'em");
            //hit.collider.GetComponent<enemyAI>().kill();
            masterScript.enemiesAlive--;
            masterScript.enemiesKilled++;
            other.GetComponent<BoxCollider>().enabled = false;
            other.transform.SetParent(corpses.transform);
            //other.GetComponent<Renderer>().material = black;
            other.transform.position = new Vector3(other.transform.position.x, other.transform.position.y - 1, other.transform.position.z);
            other.GetComponent<enemyAI>().dead = true;
            //other.GetComponent<Material>().color = new Color(0, 0, 0);
            masterScript.currentScore++;
        }
        else
            KillMe();


    }
    void KillMe()
    {
        Debug.Log("BOOMDEAD");
        masterScript.gamePlaying = false;
        masterScript.scoreDisplay.text = "GAME OVER \n Final score: " + masterScript.currentScore + "! \n Press R to restart!";
        masterScript.scoreDisplay.rectTransform.anchoredPosition = new Vector2(0, 600);
    }

    private void OnCollisionEnter(Collision collision)
    {
        
    }
}
