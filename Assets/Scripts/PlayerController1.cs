using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController1 : MonoBehaviour {

    private Gun currentRun;

    [SerializeField]
    private float walkSpeed;
    [SerializeField]
    private float runSpeed;
    [SerializeField]
    private float crouchSpeed;
    private float playerSpeed;

    [SerializeField]
    private float jump;

    [SerializeField]
    private float crouchPos;
    private float originPos;
    private float playerCrouchPos;

    private bool isRun = false;
    private bool isGround = true;
    private bool isCrouch = false;

    private CapsuleCollider capsuleCollider;
   

    [SerializeField]
    private Camera theCamera;
    [SerializeField]
    private GameObject holder;

    private CrossHair theCrossHair;
    private Rigidbody myRigid;
    private Animator ani;


    private Transform goal;
    private NavMeshAgent agent;
    private GameObject goalObject;

	void Start () {
        goalObject = GameObject.Find("goalStation");
        goal = goalObject.transform;
        agent = GetComponent<NavMeshAgent>();
        agent.destination = new Vector3(goal.position.x, goal.position.y, goal.position.z);
        capsuleCollider = GetComponent<CapsuleCollider>();
        myRigid = GetComponent<Rigidbody>();
        playerSpeed = walkSpeed;
        originPos = theCamera.transform.localPosition.y;
        playerCrouchPos = originPos;
        theCrossHair = FindObjectOfType<CrossHair>();
        ani = GetComponent<Animator>();
    }
	
	// Update is called once per frame
	void Update () {
        IsGround();
        Move();
        Run();
        Jump();
        Crouch();
       
	}
    private void Move()
    {
        float moveDirX = Input.GetAxisRaw("Horizontal");
        float moveDirZ = Input.GetAxisRaw("Vertical");

        Vector3 moveHorizontal = transform.right * moveDirX;
        Vector3 moveVertical = transform.forward * moveDirZ;

        Vector3 velocity = (moveHorizontal + moveVertical).normalized * playerSpeed;
        myRigid.MovePosition(transform.position + velocity * Time.deltaTime);
    }

    private void Run()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            Running();
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            StopRunning();
        }
    }

    private void Running()
    {
        isRun = true;
        playerSpeed = runSpeed;
        ani.SetBool("isRun", true);
    }

    private void StopRunning()
    {
        isRun = false;
        playerSpeed = walkSpeed;
        ani.SetBool("isRun", false);
    }

    private void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGround)
        {
            Jumping();
        }
    }

    private void Jumping()
    {
        myRigid.velocity = transform.up * jump;
    }

    private void IsGround()
    {
        isGround = Physics.Raycast(transform.position, Vector3.down, capsuleCollider.bounds.extents.y + 0.7f);
    }

    private void Crouch()
    {
        if (Input.GetKey("c")) // 유니티 상에서 leftcontrol을 누르게되면 키가안눌리는 버그가 있어 c로대채
        {                      // 후에 KeyCode.LeftControl로 바꿀예정
            Crouching();
        }
        if (Input.GetKeyUp("c")) 
        {
            StopCrouch();
        }
    }

    private void Crouching()
    {
        isCrouch = true;
        playerSpeed = crouchSpeed;
        playerCrouchPos = crouchPos;
        StartCoroutine(CrouchCoroutine());
    }

    private void StopCrouch()
    {
        isCrouch = false;
        playerSpeed = walkSpeed;
        playerCrouchPos = originPos;
        StartCoroutine(CrouchCoroutine());
    }


    IEnumerator CrouchCoroutine()
    {
        // this.transform.position = new Vector3();
        // yield return null;
        
        int count = 0;
        float posY = holder.transform.localPosition.y;
       // float camera_posY = theCamera.transform.localPosition.y;
        while (posY != playerCrouchPos)
        {
            count++;
            posY = Mathf.Lerp(posY, playerCrouchPos, 0.3f);
            holder.transform.localPosition = new Vector3(0, posY, 0);
            //theCamera.transform.localPosition = new Vector3(0, camera_posY, 0);
            if (count > 10)
            {
                break;
            }
            yield return null;
        }
        holder.transform.localPosition = new Vector3(0, playerCrouchPos, 0);
       // theCamera.transform.localPosition = new Vector3(0, playerCrouchPos, 0);
    }
}
