using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class landmine : Trap {

    [SerializeField]
    float maxDuration;
    bool timerGoing;

    bool exploded;

    public List<Unit> pastVictims = new List<Unit>();
    public List<RaycastHit2D> detected = new List<RaycastHit2D>();

    float elapsedDuration = 0;
	// Use this for initialization
	protected override void Awake ()
    {
        base.Awake();
        beenClicked = false;
        pastVictims.Clear();
        maxDuration = 0.5f;
    }

    // Update is called once per frame
    protected override void Update ()
    {
        base.Update();

   

        if(timerGoing)
        {
            elapsedDuration += Time.deltaTime;
        }

        if ((beenClicked || ProximityCheck()) && elapsedDuration < maxDuration)
        {
            RaycastHit2D[] victims = Physics2D.CircleCastAll(transform.position, 1, Vector2.zero);

            for (int x = 0; x < victims.Length; ++x)
            {
                if (victims[x].collider.GetComponent<Unit>())
                {

                    Unit victim = victims[x].collider.GetComponent<Unit>();

                    if (!pastVictims.Contains(victim))
                    {
                        Debug.Log("Hit " + victim.name);
                        pastVictims.Add(victim);
                        victim.health -= 1;
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
                        Debug.Log(victim.name + "'s health is now: " + victim.health + "/" + victim.maxHealth);
                    }

                }

            }
        }
	}
    public override void GotClicked()
    {
        base.GotClicked();
        if(!beenClicked)
        {
            elapsedDuration = 0;
            timerGoing = true;
            beenClicked = true;
        }

    }
    bool ProximityCheck()
    {
        if(!exploded)
        {
            detected.AddRange(Physics2D.CircleCastAll(transform.position, 1, Vector2.zero, 0, 9));
            for(int x = 0; x < detected.Count; x++)
            {
                if(detected[x].collider.GetComponent<Unit>())
                {
                    Debug.Log(detected[x].collider.name + " detected");
                    exploded = true;
                    return true;
                }
            }
        }
        detected.Clear();
        return false;
    }
}
