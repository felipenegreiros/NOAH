using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacScript : MonoBehaviour
{
    [SerializeField] Animator anim;
    private CharacterController _charController;

    public float inX;
    public float inZ;
    public float kicktime = 0;
    public float punchtime = 0;
    private Vector3 vmovement;
    private Vector3 vvelocity;
    public float moveSpeed;
    private float gravidade;
    private int tempo;
    bool kick;

    [SerializeField] private float forceMagnitude;

    //ragdollvaribles
    // [SerializeField] public MeshCollider Maincollider;
    // [SerializeField] public Collider boxcollider;
    [SerializeField] public Collider Maincollider;
    [SerializeField] public Collider pecollider;
    [SerializeField] public Collider maocollider;
    //[SerializeField] public Rigidbody peRigid;

    [SerializeField] GameObject perna;
    [SerializeField] GameObject mao;
    [SerializeField] GameObject ThisGuyrig;
    [SerializeField] GameObject bala;
    [SerializeField] GameObject Sword;
    [SerializeField] GameObject ps;
    [SerializeField] Transform bulletpoint;

    public KeyCode upKey = KeyCode.W;
    public KeyCode downKey = KeyCode.S;
    public KeyCode leftKey = KeyCode.A;
    public KeyCode rightKey = KeyCode.D;

    public KeyCode kickKey = KeyCode.K;
    public KeyCode runKey = KeyCode.M;
    public KeyCode punchKey = KeyCode.H;
    public KeyCode shootKey = KeyCode.J;

    float hits = 0;
    public Vector3 characPosition;
    public Quaternion characRotation;
    void Start()
    {
        Getragdoolbits();
        RagdollOff();

        GameObject Playeri = GameObject.FindGameObjectWithTag("Player");
        _charController = Playeri.GetComponent<CharacterController>();
        anim = Playeri.GetComponent<Animator>();
        
        moveSpeed = 16f;
        gravidade = 0.5f;

        anim.SetBool("kick", false);
        anim.SetBool("kick2", true);
    }

    Rigidbody[] ragrigid;
    Collider[] ragcollider;
    void Getragdoolbits()
    {
        ragrigid = ThisGuyrig.GetComponentsInChildren<Rigidbody>();
        ragcollider = ThisGuyrig.GetComponentsInChildren<Collider>();
    }
    void RagdollOff()
    {
        foreach (Collider col in ragcollider)
        {
            col.enabled = false;
        }
        foreach (Rigidbody rag in ragrigid)
        {
            rag.isKinematic = true;
        }
    
        anim.enabled = true;
        Maincollider.enabled = true;
        GetComponent<Rigidbody>().isKinematic = false;
    }
    void PeEnable()
    {
       // pecollider.enabled = true;
        perna.tag = "Golpe";
    }
    void PeDisable()
    {
        pecollider.enabled = false;
    }
    void maoEnable()
    {
        mao.tag = "Golpe";
    }
    void RagdollOn()
    {
        anim.enabled = false;

        foreach (Collider col in ragcollider)
        {
            col.enabled = true;
        }
        foreach (Rigidbody rag in ragrigid)
        {
            rag.isKinematic = false;
        }

        Maincollider.enabled = false;
        GetComponent<Rigidbody>().isKinematic = true;

        hits = 0;
    }
       private void OnCollisionEnter(Collision collision)
    {

        if (collision.gameObject.name == "Rosto_AI")
        {
            RagdollOn();
           // Instantiate(ps, characPosition, characRotation);
            Debug.Log("pow");
        }
        if (collision.gameObject.tag == "balah")
        {
            // boxcollider.enabled = true;
            // Maincollider.enabled = false;

            // RagdollOn();

            hits++;
            anim.SetBool("hit", true);
            Invoke("hitfalse", 0.2f);
           // Instantiate(ps, characPosition, characRotation);
            Debug.Log("pow");
        }
        else
        {
           // Maincollider.enabled = true;
           // Invoke("disablebox", 0.7f);
        }

    }

    void hitfalse()
    {
        anim.SetBool("hit", false);
    }
    void disablebox()
    {
       // boxcollider.enabled = false;
    }
    // Update is called once per frame
    void Update()
    {
        if (hits > 4)
        {
            RagdollOn();
        }
       // RagdollOff();
        if (Input.GetKey(upKey))
        {
            RagdollOff();
        }

        inX = Input.GetAxis("Horizontal");
        inZ = Input.GetAxis("Vertical");

        Maist();

        //CORRER
        if (Input.GetKey(runKey))
        {

            anim.SetBool("run", true);
            anim.SetBool("run2", false);
            kicktime = 0;
            inX = inX * 100;

            // Debug.Log("colon");
        }
        if (Input.GetKeyUp(runKey))
        {
            anim.SetBool("run", false);
            anim.SetBool("run2", true);

        }
        //CHUTE
        if (Input.GetKey(kickKey))
        {
            kicktime = 100;
            anim.SetBool("run", false);
            anim.SetBool("run2", true);
            anim.SetBool("kick", true);
            anim.SetBool("lbool", false);
            anim.SetBool("rbool", false);
            kick = true;

        }
        else
        {
            kicktime = kicktime - 5f;
            //isso aq ta so no getkey ai se o cara pressionar nunca vai descer
        }

        if (Input.GetKeyUp(kickKey))
        {
            anim.SetBool("kick", false);
            kick = false;
        }

        if (kicktime < 0)
        {
            kicktime = 0;
        }
        if (kicktime > 20 )
        {
            Invoke("PeEnable", 0.8f);
            pecollider.enabled = true;
           // perna.tag = "balah";
        }
        else
        {
            perna.tag = "Untagged";
        }

        //PUNCH
        if (Input.GetKey(punchKey))
        {
            punchtime = 100;
            anim.SetBool("run", false);
            anim.SetBool("run2", true);
            anim.SetBool("punch", true);
            anim.SetBool("lbool", false);
            anim.SetBool("rbool", false);


        }
        else
        {
            punchtime = punchtime - 4f;
            //isso aq ta so no getkey ai se o cara pressionar nunca vai descer
        }

        if (Input.GetKeyUp(punchKey))
        {
            anim.SetBool("punch", false);

        }

        if (punchtime < 0)
        {
            punchtime = 0;
        }
        if (punchtime > 10)
        {
            Invoke("maoEnable", 0.4f);
            maocollider.enabled = true;
            // perna.tag = "balah";
        }
        else
        {
            mao.tag = "Untagged";
        }

        //WALK
        if (Input.GetKey(upKey))
        {
            anim.SetBool("bool1", true);
            anim.SetBool("bool2", true);
            inX = inX * 100;
           // tempo = 0;
        }

        if (Input.GetKeyUp(upKey)) 
        {
           // anim.SetBool("run", false);
           // anim.SetBool("run2", true);

            anim.SetBool("bool1", false);
            anim.SetBool("bool2", false);
            tempo = 0;
        }
        if (Input.GetKey(leftKey))
        {
            anim.SetBool("lbool", true);
            anim.SetBool("lbool2", false);
        }
        else 
        {
            anim.SetBool("lbool", false);
            anim.SetBool("lbool2", true);
        }
        if (Input.GetKey(rightKey))
        {
            anim.SetBool("rbool", true);
            anim.SetBool("rbool2", false);
        }
        else
        {
            anim.SetBool("rbool", false);
            anim.SetBool("rbool2", true);  
        }
        if (tempo > 160)
        {
           // anim.SetBool("run", true);
           // anim.SetBool("run2", false);
        }

        //AGACHAR
        if (Input.GetKeyDown(downKey))
        {
            anim.SetBool("Agacha", true);
            Maincollider.enabled = false;

        }
        if (Input.GetKeyUp(downKey))
        {
            anim.SetBool("Agacha", false);
            Maincollider.enabled = true;
        }

        //Atirar
        if (Input.GetKeyDown(shootKey))
        {
            anim.SetBool("shoot", true);

            Invoke("atira", 0.4f);
           
        }
        else
        {
            anim.SetBool("shoot", false);
        }
        if (Input.GetKeyUp(shootKey))
        {
            anim.SetBool("shoot", false);
        }

    }

    void atira()
    {
        Rigidbody rb = Instantiate(bala, bulletpoint.position, Quaternion.identity).GetComponent<Rigidbody>();
        rb.AddForce(transform.forward * 4f, ForceMode.Impulse);
        rb.AddForce(transform.up * -0.1f, ForceMode.Impulse);
    }
    private void FixedUpdate()
    {
        //o problema da tremedeira eh o collider e os comandos uparrow com leftright arrow quando executados juntos

        vmovement = _charController.transform.forward * inZ *(2f * Time.deltaTime);
        
        _charController.transform.Rotate(Vector3.up * inX * (2f * Time.deltaTime));

            if (Input.GetKeyDown(upKey) || Input.GetKeyDown(runKey))
            {
                _charController.Move(vmovement * moveSpeed * Time.deltaTime);
            tempo = 0;

        }

    }
    private void Maist()
    {
        tempo++;
    }
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Rigidbody rigidbody = hit.collider.attachedRigidbody;
        if (rigidbody != null)
        {
            Vector3 forceDirection = hit.gameObject.transform.position - transform.position;
            forceDirection.y = 0;
            forceDirection.Normalize();

            rigidbody.AddForceAtPosition(forceDirection * forceMagnitude, transform.position, ForceMode.Impulse);
        }

    }

}
