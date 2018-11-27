using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Turrent : MonoBehaviour
{

    public delegate void TurrentActiveAction(bool active);
    public event TurrentActiveAction OnTurrentActiveChange;

    public GameObject bulletSpawner;
    public GameObject storedBulletObject;
    private BulletFactory factory;
    public LayerMask aimMask;
    public ParticleSystem onHitParticle;
    private float maxAimDistance = 500;
    private float defaulAimtDistance = 50;
    private float minAimDistance = 2;

    private ChickenType activeChickenType { get { return References.getChickenUIManager().getActiveChicken(); } }
    private ChickenUIManager chickenUIManager;
    //private Timer nextFireTimer = new Timer();
    public bool turrentModeActive;
    

    void Start()
    {
        factory = new BulletFactory();
        //bulletSpawner = GameObject.Find("BulletSpawnPosition");
        chickenUIManager = References.getChickenUIManager();
    }

    // update the state of the gun
    void Update()
    {
        if (!turrentModeActive)
            return;

        if (EventSystem.current == null || !EventSystem.current.IsPointerOverGameObject())
        {
            if (Input.GetButton("Fire1") && chickenUIManager.getSelectedChickenSlot().rechargeTimer.Expired())
            {
                fireBullet(getTargetPosition());
            }
        }
    }

    GameObject getTargetedObject()
    {
        //// rayOrigin is the center of the camera's screen (0.5f/0.5f) at the player (0)
        RaycastHit hit;
        if (getRaycastFromCamera(out hit))
        {
            return hit.transform.gameObject;
        }
        return null;
    }

    Camera getPlayerCamera()
    {
        return References.GetPlayerMovementController().gameObject.GetComponentInChildren<Camera>();
    }

    Vector3 getCameraWorldPoint()
    {
        return getPlayerCamera().ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0));
    }

    bool getRaycastFromCamera(out RaycastHit hit)
    {
        Vector3 rayOrigin = getCameraWorldPoint();
        if (Physics.Raycast(rayOrigin, getPlayerCamera().transform.forward, out hit, maxAimDistance, aimMask))
        {
            return true;
        }
        return false;
    }

    // get the position of the gun's target
    Vector3 getTargetPosition()
    {
        Vector3 targetPosition;
        RaycastHit hit;
        if (getRaycastFromCamera(out hit))
        {
            float distance = Vector3.Distance(bulletSpawner.transform.position, hit.point);
            //Debug.Log("Distance: " + distance.ToString());
            if (distance < minAimDistance)
            {
                // ensure we don't get odd gun physics trying to shoot something in front player's face
                targetPosition = bulletSpawner.transform.position + (getPlayerCamera().transform.forward * minAimDistance);
            }
            else
            {
                targetPosition = hit.point;
            }
        }
        else
        {
            // shoot out X units from the player camera to where the player is looking
            targetPosition = getCameraWorldPoint() + (getPlayerCamera().transform.forward * defaulAimtDistance);
        }

        return targetPosition;

    }

    // fire a bullet at the given position
    void fireBullet(Vector3 targetPosition)
    {
        if (activeChickenType.name != ChickenTypeEnum.normalName && activeChickenType.chickenCount <= 0)
            return;
        Debug.Log("Firing " + activeChickenType.name);
        factory.createBullet(storedBulletObject, bulletSpawner.transform.position, targetPosition, activeChickenType, onHitParticle: onHitParticle);
        activeChickenType.chickenCount -= 1;
        chickenUIManager.getSelectedChickenSlot().updateFired();
    }

    public void activateTurrentMode()
    {
        turrentModeActive = true;
        if(OnTurrentActiveChange != null)
            OnTurrentActiveChange(turrentModeActive);
        References.getChickenUIManager().showChickenSlots();
        References.GetPlayerMovementController().startTurrentMode();
        References.GetEnemySpawnerManager().StartNight();
        //activeChickenType = References.getInventoryManager().chickenInventories[0];
    }

    public void deactiveTurrentMode()
    {
        turrentModeActive = false;
        if(OnTurrentActiveChange != null)
            OnTurrentActiveChange(turrentModeActive);
        
        //References.getChickenUIManager().hideTurrentHUD();
    }

}

