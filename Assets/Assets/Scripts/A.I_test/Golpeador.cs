using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Golpeador : MonoBehaviour
{
    public NavMeshAgent agent;
    public GameObject Projectil;
    public Transform player;
    [SerializeField] Animator Ani;
    [SerializeField] Transform bulletpoint;
    [SerializeField] GameObject esse;
    [SerializeField] GameObject Particles;
    [SerializeField] Collider arma;
    [SerializeField] Rigidbody armarig;


    public LayerMask whatIsGround, whatIsPlayer;

    public Vector3 walkpoint;
    bool walkPointSet;
    public float walkPointRange;

    public float timeBetweenAttacks;
    float hits = 0;
    bool alreadyattacked;

    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;
    bool armable;

    private void Awake()
    {
        player = GameObject.Find("Noah").transform;
        agent = GetComponent<NavMeshAgent>();

        Ani.SetBool("Walk2", true);

    }

    private void Update()
    {
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);


        if (!playerInSightRange && !playerInAttackRange) Patroling();
        if (playerInSightRange && !playerInAttackRange) ChasePlayer();
        if (playerInSightRange && playerInAttackRange) AttackPlayer();

        if(armable == true)
        {

        }

    }

    void aramableOn()
    {
        armable = true;
        arma.enabled = true;
       // armarig.isKinematic = true;
    }
    void aramableOff()
    {
        armable = false;
        arma.enabled = false;
       // armarig.isKinematic = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Golpe")
        {
            arma.enabled = false;
            Ani.SetBool("shooting2", false);
            Ani.SetBool("Walk2", false);
            Ani.SetBool("defeat", true);

            hits++;

           // Invoke("hitanimfalse", 0.5f);
            //thisRB.AddForce(transform.forward * -9f, ForceMode.Impulse);
           // if (hits > 2)
          //  {
                Invoke("Ps", 1.3f);
                Destroy(esse, 1.5f);
          //  }
            //mudar a tag so na hr do chute
        }
    }

    void hitanimfalse()
    {
        Ani.SetBool("defeat", false);
    }
    private void Ps()
    {
        Instantiate(Particles, transform.position, Quaternion.identity);
    }

    private void Patroling()
    {
        Ani.SetBool("shooting2", false);
        Ani.SetBool("Walk2", true);

        // Debug.Log("patrol");
        arma.enabled = false;
       // armarig.isKinematic = false;

        if (!walkPointSet)
        {
            SearchWalkPoint();
        }
        if (walkPointSet)
        {
            agent.SetDestination(walkpoint);
        }

        Vector3 distanceToWalkPoint = transform.position - walkpoint;

        if (distanceToWalkPoint.magnitude < 1f)
        {
            walkPointSet = false;
        }
    }
    private void SearchWalkPoint()
    {
        Ani.SetBool("shooting2", false);
        Ani.SetBool("Walk2", true);

        arma.enabled = false;
       // armarig.isKinematic = false;

        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkpoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);
        //nao parece ser nada desse void

        if (Physics.Raycast(walkpoint, -transform.up, 2f, whatIsGround))
        {
            walkPointSet = true;
        }
    }

    private void ChasePlayer()
    {
       
         Ani.SetBool("Walk2", true);
        Ani.SetBool("shooting2", false);

        agent.SetDestination(player.transform.position);

        arma.enabled = false;
       // armarig.isKinematic = false;
        // transform.LookAt(player);
    }
    private void AttackPlayer()
    {
        //agent.SetDestination(transform.position);
        Ani.SetBool("shooting2", true);

        agent.SetDestination(player.transform.position / 2);

       // arma.enabled = true;
       // armarig.isKinematic = true;

        //a treta � com a rota��o, verificar o q esta mechendo com a rota��o
        //possivel conflito entre "LookAt & SetDestination"
        transform.LookAt(new Vector3(player.position.x, transform.position.y, player.position.z));

        if (!alreadyattacked)
        {
            Rigidbody rb = Instantiate(Projectil, bulletpoint.position, Quaternion.identity).GetComponent<Rigidbody>();
            rb.AddForce(transform.forward * 3f, ForceMode.Impulse);
            rb.AddForce(transform.up * -0.5f, ForceMode.Impulse);


            alreadyattacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
      
    }

    private void ResetAttack()
    {
       
        alreadyattacked = false;
    }
}


