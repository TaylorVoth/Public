using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Moose : Wild {


    public List<Unit> pastVictims = new List<Unit>();
    float currentTime;
    float chargingMax = 1f;
    float chargingSpeed = 500;
    bool charging;
    [SerializeField]
    float timer;
    float duration = 2;
    float cooldownTime = 10;
	// Use this for initialization
	protected override void Awake()
    {
        base.Awake();

        //starts the moose ready to charge
        timer = cooldownTime + 1;
	}
	
	// Update is called once per frame
	protected override void Update ()
    {
        base.Update();
        timer += Time.deltaTime;
        if(!charging && timer > cooldownTime)
        {

            RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, Vector2.left, 25);
            for(int i = 0; i < hits.Length; i++)
            {
                if((!hits[i].collider.name.StartsWith("moo") && (hits[i].collider.GetComponent<Unit>())))
                {
                    Debug.Log("I see " + hits[i].collider.name);
                    Charge();
                }
            }
        }
        if(charging)
        {
            if((timer <= duration || timer > cooldownTime) && (chargingSpeed * myRigidbody.velocity.x) < chargingMax)
            {
                myRigidbody.AddForce(Vector2.left * chargingSpeed);
                //myRigidbody.velocity += Vector2.left * chargingSpeed * Time.deltaTime;
            }
            
            else if(timer > duration)
            {
                charging = false;
                
            }
        }
	}
    void Charge()
    {
        wandering = false;
        timer = 0;
        charging = true;
        pastVictims.Clear();

    }
    void Attack()
    {
        
        if(timer <= currentTime + 1)
        {

            RaycastHit2D[] victims = Physics2D.CircleCastAll(new Vector2(transform.position.x - 2,transform.position.y + 0.5f), 2.5f, Vector2.zero);

            for (int x = 0; x < victims.Length; ++x)
            {
                if (victims[x].collider.GetComponent<Unit>())
                {

                    Unit victim = victims[x].collider.GetComponent<Unit>();
                    UnitControl victimControl = victims[x].collider.GetComponent<UnitControl>();
                    
                    if (!pastVictims.Contains(victim) && victims[x].collider.gameObject != gameObject)
                    {
                        Debug.Log("hit " + victim.name);

                        pastVictims.Add(victim);
                        victim.health -= 1;

                        //Hit Effects
                        victimControl.inMeteor = 1;

                        //if victim is to my left
                        if (victim.transform.position.x < transform.position.x)
                        {
                            //knock 'em left
                            victim.myRigidbody.velocity = new Vector2(-30, 10);
                        }
                        //if victim is to my right
                        else if (victim.transform.position.x > transform.position.x)
                        {
                            //knock 'em right
                            victim.myRigidbody.velocity = new Vector2(30, 10);
                        }

                        Debug.Log(victims[x].collider.name + "'s health is now: " + victim.health + "/" + victim.maxHealth);
                    }

                }

            }
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.GetComponent<Unit>())
        {
            myRigidbody.velocity = Vector2.zero;
            currentTime = timer;
            Unit victim = collision.collider.GetComponent<Unit>();
            charging = false;
            Attack();
            wandering = true;
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(new Vector2(transform.position.x - 2, transform.position.y + 0.5f), 2.5f);
    }
}
