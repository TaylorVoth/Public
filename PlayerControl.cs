using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class PlayerControl : MonoBehaviour {


    [SerializeField]
    Transform rightArm;
    GameObject myGun;
    Gun myGunScript;
    Camera cam;
    Rigidbody2D myBody;
    private float speed = 15;
    [SerializeField]
    protected Vector3 mouse;


    // Use this for initialization
    void Awake ()
    {        
        myBody = GetComponent<Rigidbody2D>(); 
        
    }

    // Update is called once per frame
    void Update () {
        if (myGunScript)
            myGunScript.pickupDelay = 1; //set 1 every frame to keep the collider off

        if (Input.GetKeyDown(KeyCode.Q) )
        {
            Debug.Log("Pressed Q");            

            Collider2D nearbyDrops = Physics2D.OverlapCircle(transform.position, 1, LayerMask.GetMask("FloorGun"));

            if(nearbyDrops && nearbyDrops.GetComponent<Gun>().pickupDelay <= 0)
            {
                Debug.Log("saw " + nearbyDrops.name);

                if (myGun)
                {
                    //drop my gun first
                    //TODO change gun position
                    Debug.Log("dropped my " + myGun.name);
                    myGunScript.pickupDelay = 1;
                    myGun.transform.position = transform.position;
                    myGun.transform.SetParent(null);
                    myGun = null;
                }
                myGun = nearbyDrops.gameObject;                
                myGun.transform.SetParent(transform);
                myGun.transform.SetPositionAndRotation(rightArm.position, transform.rotation);      
                myGunScript = myGun.GetComponent<Gun>();
                myGun.GetComponent<BoxCollider2D>().enabled = false;
                
                Debug.Log("picked up " + myGun.name);
            }
        }        
	}
    private void FixedUpdate()
    {
        #region Look at mouse

        // Distance from camera to object.  We need this to get the proper calculation.
        float camDis = Camera.main.transform.position.z - transform.position.z;

        // Get the mouse position in world space. Using camDis for the Z axis.
        mouse = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, camDis));

        float AngleRad = Mathf.Atan2(mouse.y - myBody.position.y, mouse.x - myBody.position.x);
        float angle = (180 / Mathf.PI) * AngleRad;
        myBody.rotation = angle;
        #endregion

        #region Movement

        if(Input.GetKey(KeyCode.LeftShift))
        {
            speed = 50;

        }
        else
        {
            speed = 20;
        }

        myBody.velocity = new Vector2((Input.GetAxis("Horizontal") * speed * Time.deltaTime), myBody.velocity.y);
        myBody.velocity = new Vector2(myBody.velocity.x, (Input.GetAxis("Vertical") * speed * Time.deltaTime));
        #endregion

        
        if(myGun)
        {
            myGunScript.mouse = mouse;
            if(Input.GetKeyDown(KeyCode.Space) || Input.GetKey(KeyCode.Mouse0))
            {
                myGunScript.Shoot();
            }
            if(Input.GetKeyDown(KeyCode.R))
            {
                myGunScript.Reload();
            }
        }

    }
    
}
