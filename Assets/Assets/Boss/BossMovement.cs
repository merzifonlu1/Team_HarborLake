using Cainos.PixelArtPlatformer_VillageProps;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMovement : MonoBehaviour
{
    [SerializeField] private Transform player;
    private Animator anim;
    private Rigidbody2D rb;

    //----------------Moving------------------
    private bool FacingRight;
    private bool CanMove;
    [SerializeField] private float runspeed = 5f;
    //----------------------------------------

    //--------Pulse Ability--------
    public bool Pulsing;
    private bool CanPulse;
    private bool pulsetimer;
    [SerializeField] private float pulsePower = 54f;
    [SerializeField] private Rigidbody2D playerrb;
    //-----------------------------

    //---------------- Attack1 Ability -----------------
    private bool Attacking1;
    private bool canAttack;
    [SerializeField] private float attack1range = 4.5f;
    private GameObject Boss;
    private GameObject attack1object;
    private PolygonCollider2D attack1coll;
    // -------------------------------------------------

    //------------------ SpinAttack Ability -------------------
    private bool Spining;
    private bool CanSpin;
    private bool spintimer;
    [SerializeField] private float spinattackrange = 12f;
    [SerializeField] private float spinPower = 26f;
    private GameObject spinobject;
    private PolygonCollider2D spincoll;
    //----------------------------------------------------------


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        //----------------Moving------------------
        CanMove = true;
        FacingRight = false;
        //----------------------------------------

        //--------Pulse Ability--------
        Pulsing = false;
        CanPulse = false;
        pulsetimer = true;
        //-----------------------------

        //---------------- Attack1 Ability -----------------
        Attacking1 = false;
        canAttack = true;
        Boss = GameObject.Find("Boss");
        attack1object = Boss.transform.GetChild(0).gameObject;
        attack1coll = attack1object.GetComponent<PolygonCollider2D>();
        attack1coll.enabled = false;
        //--------------------------------------------------

        //------------------ SpinAttack Ability -------------------
        Spining = false;
        CanSpin = false;
        spintimer = true;
        Boss = GameObject.Find("Boss");
        spinobject = Boss.transform.GetChild(1).gameObject;
        spincoll = spinobject.GetComponent<PolygonCollider2D>();
        spincoll.enabled = false;       
        //----------------------------------------------------------
    }


    void Update()
    {
        if (Vector2.Distance(player.position, rb.position) > attack1range)
        {
            CanMove = true;
        }
        else
        {
            CanMove = false;
        }

        if ((Vector2.Distance(player.position, rb.position) > 7f) && (Vector2.Distance(player.position, rb.position) < spinattackrange))
        {
            CanSpin = true;
        }
        else
        {
            CanSpin = false;
        }

        if (Vector2.Distance(player.position, rb.position) < 4f)
        {
            CanPulse = true;
        }
        else
        {
            CanPulse = false;
        }
    }

    private void FixedUpdate()
    {
        if (Attacking1) { return; }
        if (Spining) {return;}

        LookAtPlayer();

        if (CanMove)
        {
            Walk();
        }

        if (!CanMove && canAttack)
        {
            StartCoroutine(Attack1());
        }

        if (CanPulse && pulsetimer)
        {
            StartCoroutine(Pulse());
        }

        if (CanSpin && spintimer)
        {
            StartCoroutine(Spinattack());
        }

    }

    private void Walk()
    {
        anim.SetBool("Walking", true);
        Vector2 target = new Vector2(player.position.x, rb.position.y);
        Vector2 newPos = Vector2.MoveTowards(rb.position, target, runspeed * Time.fixedDeltaTime);
        rb.MovePosition(newPos);
    }

    //---------------- Attack1 Ability -----------------
    private IEnumerator Attack1()
    {

        canAttack = false;
        Attacking1 = true;
        anim.SetBool("Walking", false);        
        anim.SetTrigger("Attack1");
        yield return new WaitForSeconds(0.8f);
        Attacking1 = false;
        yield return new WaitForSeconds(0.3f);
        canAttack = true;
    }  
    void activateatack1Collider(){attack1coll.enabled = true;}void deactivateatack1Collider(){attack1coll.enabled = false;}void quake(){playerrb.velocity = new Vector2(0f,transform.up.y * 5f);}
    //--------------------------------------------------

    //------------------ Pulse Ability -------------------
    public IEnumerator Pulse()
    {
        pulsetimer = false;
        Pulsing = true;
        anim.SetTrigger("Pulse");
        if (FacingRight)
        {
            playerrb.velocity = new Vector2(1 * pulsePower, rb.velocity.y);
        }
        else
        {
            playerrb.velocity = new Vector2(-1 * pulsePower, rb.velocity.y);
        }
        yield return new WaitForSeconds(0.6f);
        Pulsing = false;
        yield return new WaitForSeconds(4f);
        pulsetimer = true;
    }
    //----------------------------------------------------

    //------------------ SpinAttack Ability -------------------
    private IEnumerator Spinattack()
    {
        spintimer = false;
        Spining = true;
        anim.SetTrigger("Spin");
        if (FacingRight)
        {
            rb.velocity = new Vector2(1 * spinPower, rb.velocity.y);
        }
        else
        {
            rb.velocity = new Vector2(-1 * spinPower, rb.velocity.y);
        }
        yield return new WaitForSeconds(0.6f);
        Spining = false;
        yield return new WaitForSeconds(8f);
        spintimer = true;
    }
    void activatespinatackCollider(){spincoll.enabled = true;} void deactivatespinatackCollider() {spincoll.enabled = false;}
    //----------------------------------------------------------

    private void flip()
    {
        transform.Rotate(0f, 180f, 0f);
        FacingRight = !FacingRight;
    }

    public void LookAtPlayer()
    {
        Vector3 flipped = transform.localScale;
        flipped.z = -1f;
        if (transform.position.x > player.position.x && FacingRight && !Attacking1)
        {
            transform.localScale = flipped;
            transform.Rotate(0f, 180f, 0f);
            FacingRight = false;
        }
        else if (transform.position.x < player.position.x && !FacingRight && !Attacking1)
        {
            transform.localScale = flipped;
            transform.Rotate(0f, 180f, 0f);
            FacingRight = true;
        }
    }

}
