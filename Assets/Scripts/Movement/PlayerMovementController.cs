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

    private Vector3 preCatapultPosition;
    public bool turrentMode = false;

    private Turrent turrent;

    public LayerMask itemMask;
    private float maxAimDistance = 5;

    private GameObject playerTurrentObj;
    private GameObject farmTurrentObj;

    public ChickenType farmingActiveSeed;
    public bool inMenu = false;

    void Start()
    {
        turrent = References.GetTurrent();
        playerView = GameObject.Find("PlayerView");
        bodyController = GetComponent<BodyController>();
        lockCursor();

        firstPersonCamera = GameObject.Find("FirstPersonCamera");
        thirdPersonCamera = GameObject.Find("ThirdPersonCamera");
        playerTurrentObj = GameObject.Find("PlayerTurrent");
        farmTurrentObj = GameObject.Find("Turrent");
        //switchToThirdPersonCamera();
        switchToThirdPersonCamera();
        playerTurrentObj.SetActive(false);
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
        //playerView.transform.eulerAngles = new Vector3(playerView.transform.eulerAngles.y, Mathf.Clamp(transform.eulerAngles.y, -90, 90), playerView.transform.eulerAngles.z);

        //Debug.Log(playerView.transform.eulerAngles);
        //Vector3 eulerAngles = playerView.transform.eulerAngles;
        //if(eulerAngles.x > 45 && eulerAngles.x < 180)
        //{
        //    eulerAngles.x = 45;
        //}

        //if(eulerAngles.x < 270 && eulerAngles.x > 180)
        //{
        //    eulerAngles.x = 270;
        //}

        //playerView.transform.eulerAngles = eulerAngles;

        //float minRotation = -45;
        //float maxRotation = 45;
        //Vector3 currentRotation = transform.localRotation.eulerAngles;
        //currentRotation.x = Mathf.Clamp(currentRotation.x, minRotation, maxRotation);
        //transform.localRotation = Quaternion.Euler(currentRotation);

    }

    private void Update()
    {
        if (turrentMode)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                endTurrentMode();
                //turrent.deactiveTurrentMode();
                //this.transform.position = preCatapultPosition;
                //switchToThirdPersonCamera();
                //turrentMode = false;
            }
        }
        else
        {
            checkFarmingKeyPress();

            if (!inMenu)
            {
                bodyMove();
            }
        }
        if (!inMenu)
        {
            cameraMove();
        }

    }

    public void beginMenu()
    {
        inMenu = true;
        unlockCursor();
    }

    public void closeMenu()
    {
        inMenu = false;
        lockCursor();
    }

    private void checkFarmingKeyPress()
    {
        if (inMenu && (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Escape)))
        {
            References.GetSeedShopActivator().hideSeedShop();
            References.GetUpgradeShopActivator().hideUpgradeShop();
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            Item targetedItem = getTargetedItem();
            if (targetedItem != null)
            {
                Debug.Log("Found item " + targetedItem.gameObject.name);
                targetedItem.use();
            }

        }

        

        ChickenType chickenType = References.getInventoryManager().getChickenKeyPressed();
        if(chickenType && chickenType.name != ChickenTypeEnum.normalName)
        {
            if (farmingActiveSeed == chickenType)
            {
                deselectChickenSeed();
            }
            else
            {
                if (farmingActiveSeed)
                {
                    deselectChickenSeed();
                }
                selectChickenSeed(chickenType);
            }
        }
    }

    private void deselectChickenSeed()
    {
        farmingActiveSeed.startSeed.SetActive(false);
        farmingActiveSeed = null;
    }

    private void selectChickenSeed(ChickenType chickenType)
    {
        chickenType.startSeed.SetActive(true);
        farmingActiveSeed = chickenType;
    }

    Item getTargetedItem()
    {
        GameObject targetedObject = getTargetedObject();
        //Debug.Log("Targeted " + targetedObject.name);
        Item item = targetedObject == null ? null : targetedObject.GetComponent<Item>();
        return item;
    }

    GameObject getTargetedObject()
    {
        //// rayOrigin is the center of the camera's screen (0.5f/0.5f) at the player (0)
        //RaycastHit hit;
        //if (getRaycastFromCamera(out hit))
        //{
        //    return hit.transform.gameObject;
        //}
        //return null;
        float itemRadius = 3f;
        Collider[] colliders = Physics.OverlapSphere(this.transform.position, itemRadius, itemMask);

        GameObject closestTarget = null;
        float? closestSqrMagnitude = null;
        foreach (Collider hit in colliders)
        {
            if (!hit.gameObject.GetComponent<Item>().isUsable())
                continue;
            float sqrMagnitude = (this.transform.position - hit.transform.position).sqrMagnitude;

            if (closestSqrMagnitude == null || sqrMagnitude < closestSqrMagnitude)
            {
                closestTarget = hit.gameObject;
                closestSqrMagnitude = sqrMagnitude;
            }

        }

        return closestTarget;
    }

    Camera getPlayerCamera()
    {
        return GetComponentInChildren<Camera>();
    }

    Vector3 getCameraWorldPoint()
    {
        return getPlayerCamera().ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0));
    }

    bool getRaycastFromCamera(out RaycastHit hit)
    {
        Vector3 rayOrigin = getCameraWorldPoint();
        float radius = 3;
        if (Physics.SphereCast(rayOrigin - new Vector3(0, 0, radius), radius, getPlayerCamera().transform.forward, out hit, maxAimDistance, itemMask))
        {
            return true;
        }
        return false;
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

    public void startTurrentMode()
    {
        preCatapultPosition = this.transform.position;
        this.transform.position = turrent.gameObject.transform.position + new Vector3(0, 3, 0);
        bodyController.frozen = true;
        switchToFirstPersonCamera();
        turrentMode = true;
        playerTurrentObj.SetActive(true);
        farmTurrentObj.SetActive(false);
    }

    public void endTurrentMode()
    {
        if(preCatapultPosition != Vector3.zero)
            this.transform.position = preCatapultPosition;
        bodyController.frozen = false;
        switchToThirdPersonCamera();
        turrentMode = false;
        playerTurrentObj.SetActive(false);
        farmTurrentObj.SetActive(true);
        turrent.deactiveTurrentMode();
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

    //private void useItem(Item item)
    //{
    //    itemInFocus = null;
    //    item.use();
    //}

    //private void OnTriggerEnter(Collider other)
    //{
    //    Item item = other.GetComponent<Item>();
    //    //Debug.Log("Trigger enter");
    //    if (item != null)
    //    {
    //        //Debug.Log("Item focus");
    //        setItemFocus(item);
    //    }
    //}

    //private void setItemFocus(Item item)
    //{
    //    itemInFocus = item;
    //}

}
