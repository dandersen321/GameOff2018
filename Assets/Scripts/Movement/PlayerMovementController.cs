﻿using System.Collections;
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

    //private Vector3 preCatapultPosition;
    private Vector3 startingFarmPosition;
    public bool turrentMode = false;

    private Turrent turrent;

    public LayerMask itemMask;
    private float maxAimDistance = 5;

    private GameObject playerTurrentObj;
    private GameObject farmTurrentObj;

    public ChickenType farmingActiveSeed;
    private bool inMenu;
    public bool inShopMenu;

    [HideInInspector]
    public bool lost = false;
    public GameObject looseScreen;

    public Animator animator;

    private float stepSoundTimer = 0.5f;
    private float stepSoundLastPlayed = 0;

    private GameObject InitText;

    public GameObject farmChickenPrefab;
    public GameObject redDot;

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
        startingFarmPosition = GameObject.Find("PlayerStartingPosition").transform.position;
        redDot = GameObject.Find("AimReticule");
        //switchToThirdPersonCamera();
        switchToThirdPersonCamera();
        playerTurrentObj.SetActive(false);

        animator = GetComponentInChildren<Animator>();

        InitText = GameObject.Find("InitText");
    }

    void bodyMove()
    {
        Vector3 targetDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        targetDirection = transform.TransformDirection(targetDirection);

        animator.SetFloat("speedPercent", Mathf.Abs(Input.GetAxis("Vertical")), .1f, Time.deltaTime);

        if (targetDirection != Vector3.zero)
        {
            bodyController.moveInDirection(targetDirection);

            if(stepSoundLastPlayed <= 0)
            {
                AudioPlayer.Instance.PlayRandomFootStep();
                stepSoundLastPlayed = stepSoundTimer;
            }

        }

        bool isGrounded = bodyController.controller.isGrounded;
        if (lastJumpTimer.Expired() && Input.GetButtonDown("Jump"))
        {
            // No jumping for now. If we find we need it, then I will create an animation.
            //bodyController.jump();
            lastJumpTimer.Start(1f);
        }
        
        stepSoundLastPlayed -= Time.deltaTime;
    }

    void cameraMove()
    {
        var md = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));

        md = Vector2.Scale(md, new Vector2(mouseXSensitivity * mouseSmoothing, mouseYSensitivity * mouseSmoothing));
        smoothV.x = Mathf.Lerp(smoothV.x, md.x, 1f / mouseSmoothing);
        smoothV.y = Mathf.Lerp(smoothV.y, md.y, 1f / mouseSmoothing);
        mouseLook += smoothV;

        mouseLook.y = Mathf.Clamp(mouseLook.y, -45, 45);

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
                //References.GetEnemySpawnerManager().endNightMode();

                //endTurrentMode();
                //turrent.deactiveTurrentMode();
                //this.transform.position = preCatapultPosition;
                //switchToThirdPersonCamera();
                //turrentMode = false;
            }
        }
        else
        {
            //checkFarmingKeyPress();

            if (!inMenu && !inShopMenu && !InitText.activeSelf)
            {
                bodyMove();
            }
            else
                animator.SetFloat("speedPercent", 0f, .1f, Time.deltaTime);
        }

        //Debug.Log("InMenu: " + inMenu.ToString());
        if (!inMenu && !inShopMenu && !InitText.activeSelf)
        {
            cameraMove();
        }

    }

    public void beginMenu()
    {
        inMenu = true;
        unlockCursor();
    }

    public void beginShopMenu()
    {
        inShopMenu = true;
        unlockCursor();
    }

    public void closeMenu()
    {
        //throw new KeyNotFoundException("hmm");
        //Debug.Log("Closing Menu");
        inMenu = false;
        lockCursor();
        if (References.activeToolTip != null)
        {
            References.activeToolTip.HideToolTipInfo();
        }
    }

    public void closeShopMenu()
    {
        inShopMenu = false;
        lockCursor();
        if (References.activeToolTip != null)
        {
            References.activeToolTip.HideToolTipInfo();
        }
    }

    public void checkFarmingKeyPress()
    {
        if (inShopMenu && !DialogSystem.Instance.IsActive && (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Escape)))
        {
            References.GetSeedShopActivator().hideSeedShop();
            References.GetUpgradeShopActivator().hideUpgradeShop();
        }
        else if ((Input.GetKeyDown(KeyCode.E) || Input.GetMouseButtonDown(0)) && (!inShopMenu && !DialogSystem.Instance.IsActive))
        {
            Item targetedItem = getTargetedItem();
            if (targetedItem != null)
            {
                Debug.Log("Found item " + targetedItem.gameObject.name);

                if(InitText.activeSelf)
                {
                    InitText.SetActive(false);
                }

                targetedItem.use();
                
            }

        }

        

        ChickenType chickenType = References.getInventoryManager().getChickenKeyPressed();
        if(chickenType)
        {
            //if (farmingActiveSeed == chickenType)
            //{
            //    deselectChickenSeed();
            //}
            //else
            {
                //if (farmingActiveSeed)
                //{
                //    deselectChickenSeed();
                //}
                if (chickenType.seedCount > 0 || chickenType.name == ChickenTypeEnum.normalName)
                {
                    selectChickenSeed(chickenType);
                }
            }
        }
    }

    public void deselectChickenSeed()
    {
        if (farmingActiveSeed != null && farmingActiveSeed.name != ChickenTypeEnum.normalName)
        {
            farmingActiveSeed.startSeed.SetActive(false);
        }
        farmingActiveSeed = null;
    }

    public void selectChickenSeed(ChickenType chickenType)
    {
        
        deselectChickenSeed();
        if (chickenType.name != ChickenTypeEnum.normalName)
        {
            chickenType.startSeed.SetActive(true);
        }
        farmingActiveSeed = chickenType;
    }

    public Item getTargetedItem()
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
            if (hit.gameObject.GetComponent<Item>() == null)
                continue;

            if (!hit.gameObject.GetComponent<Item>().isUsable(farmingActiveSeed))
                continue;

            //DirtPatch dirtPatch = hit.GetComponent<DirtPatch>();
            //if(dirtPatch != null)
            //{
                
            //}

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
        //if (Input.GetKeyDown(KeyCode.Escape))
        //{
        //    unlockCursor();
        //}
        //else
        if (Input.GetMouseButton(0) && !EventSystem.current.IsPointerOverGameObject())
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

    public IEnumerator startTurrentMode()
    {
        bodyController.frozen = true;
        turrentMode = true;
        //preCatapultPosition = this.transform.position;

        yield return new WaitForSeconds(1);
        
        this.transform.position = turrent.gameObject.transform.position + new Vector3(0, 3, 0);

        switchToFirstPersonCamera();
        
        playerTurrentObj.SetActive(true);
        farmTurrentObj.SetActive(false);
        References.GetEnemySpawnerManager().StartNight();
    }

    public IEnumerator endTurrentMode()
    {

        turrent.deactiveTurrentMode();

        yield return new WaitForSeconds(1);
        

        //if (preCatapultPosition != Vector3.zero)
        //    this.transform.position = preCatapultPosition;
        this.transform.position = startingFarmPosition;
        this.transform.eulerAngles = new Vector3(0, 100, 0);
        bodyController.frozen = false;

        
        turrentMode = false;
        playerTurrentObj.SetActive(false);
        farmTurrentObj.SetActive(true);


        switchToThirdPersonCamera();
        References.getChickenUIManager().showDayUI();
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

    public void Lose()
    {
        lost = true;
        beginMenu();
        looseScreen.SetActive(true);
        Debug.Log("You Lose!!!");
    }

    public void retryLastNight()
    {
        lost = false;
        looseScreen.SetActive(false);
        closeMenu();

        //TODO: the rest of the stuff

        References.GetEnemySpawnerManager().resetNight();
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
