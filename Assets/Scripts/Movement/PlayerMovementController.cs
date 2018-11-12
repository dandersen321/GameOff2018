using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerMovementController : MonoBehaviour
{
    public GameObject playerView;
    public BodyController bodyController;

    // First person and third person cameras
    private GameObject firstPersonCamera;
    private GameObject thirdPersonCamera;

    private float mouseXSensitivity = 5.0f;
    private float mouseYSensitivity = 2.0f;
    private float mouseSmoothing = 1.0f;
    private Vector2 smoothV;
    private Vector2 mouseLook;

    private Timer lastJumpTimer = new Timer();

    Item itemInFocus;
    private Vector3 preCatapultPosition;
    public bool turrentMode = false;

    private Turrent turrent;

    void Start()
    {
        turrent = References.GetTurrent();
        playerView = GameObject.Find("PlayerView");
        bodyController = GetComponent<BodyController>();
        lockCursor();

        firstPersonCamera = GameObject.Find("FirstPersonCamera");
        thirdPersonCamera = GameObject.Find("ThirdPersonCamera");
        switchToThirdPersonCamera();
    }

    void bodyMove()
    {
        Vector3 targetDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        targetDirection = transform.TransformDirection(targetDirection);
        

        if (targetDirection != Vector3.zero)
        {
            bodyController.moveInDirection(targetDirection);
        }

        bool isGrounded = bodyController.controller.isGrounded;
        if (lastJumpTimer.Expired() && Input.GetButtonDown("Jump"))
        {
            bodyController.jump();
            lastJumpTimer.Start(1f);
        }
    }

    void cameraMove()
    {
        var md = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));

        md = Vector2.Scale(md, new Vector2(mouseXSensitivity * mouseSmoothing, mouseYSensitivity * mouseSmoothing));
        smoothV.x = Mathf.Lerp(smoothV.x, md.x, 1f / mouseSmoothing);
        smoothV.y = Mathf.Lerp(smoothV.y, md.y, 1f / mouseSmoothing);
        mouseLook += smoothV;

        playerView.transform.localRotation = Quaternion.AngleAxis(-mouseLook.y, Vector3.right);
        this.transform.localRotation = Quaternion.AngleAxis(mouseLook.x, this.transform.up);
    }

    private void Update()
    {
        if (turrentMode)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                turrent.deactiveTurrentMode();
                this.transform.position = preCatapultPosition;
                switchToThirdPersonCamera();
                turrentMode = false;
            }
        }
        else
        {
            if (itemInFocus != null && Input.GetKeyDown(KeyCode.E))
            {
                Debug.Log("start turrent mode");
                itemInFocus.use();
                preCatapultPosition = this.transform.position;
                this.transform.position = turrent.gameObject.transform.position;
                switchToFirstPersonCamera();
                turrentMode = true;
            }
            bodyMove();
        }
        cameraMove();
        
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        float pushPower = 20f;
        Rigidbody body = hit.collider.attachedRigidbody;
        if (body == null || body.isKinematic)
            return;

        if (hit.moveDirection.y < -0.3F)
            return;

        Vector3 pushDir = new Vector3(hit.moveDirection.x, 0, hit.moveDirection.z);
        body.velocity = pushDir * pushPower;
    }

    void setCursorState()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            unlockCursor();
        }
        else if (Input.GetMouseButton(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            lockCursor();
        }
    }

    void lockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void unlockCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }





    public void switchToFirstPersonCamera()
    {
        firstPersonCamera.SetActive(true);
        thirdPersonCamera.SetActive(false);
    }

    public void switchToThirdPersonCamera()
    {
        firstPersonCamera.SetActive(false);
        thirdPersonCamera.SetActive(true);
    }

    private void useItem(Item item)
    {
        itemInFocus = null;
        item.use();
    }

    private void OnTriggerEnter(Collider other)
    {
        Item item = other.GetComponent<Item>();
        //Debug.Log("Trigger enter");
        if (item != null)
        {
            //Debug.Log("Item focus");
            setItemFocus(item);
        }
    }

    private void setItemFocus(Item item)
    {
        itemInFocus = item;
    }

}
