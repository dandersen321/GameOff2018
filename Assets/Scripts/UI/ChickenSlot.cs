using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChickenSlot : MonoBehaviour {

    public ChickenType chickenType;
    public Image chickenIcon;
    public Image chickenIconRecharge;
    public Text imageText;
    public Timer rechargeTimer = new Timer();
    private bool active = false;
    //public bool selected = false;
    private Image selectedImage;
    private Text imageOverLayText;

    private Text chickenCountDayTimeText;

    private bool isNormalSlot()
    {
        return this.gameObject.name == "Chicken1";
    }

    // Use this for initialization
    void Awake () {
        chickenIcon = this.transform.Find("Icon").gameObject.GetComponent<Image>();
        selectedImage = this.transform.Find("Selected").gameObject.GetComponent<Image>();
        imageOverLayText = this.transform.Find("ImageOverLayText").gameObject.GetComponent<Text>();
        //int slotNumber = System.Convert.ToInt32(this.gameObject.name[-1]);

        chickenIconRecharge = chickenIcon.transform.Find("ModuleRechargeRate").GetComponent<Image>();
        imageText = GetComponentInChildren<Text>();


        clearChicken();
    }

    void Update()
    {
        if (!active)
            return;

        chickenIconRecharge.fillAmount = (Mathf.Min(1, rechargeTimer.endTime - Time.time)) / chickenType.cooldown;
    }


    public void setChicken(ChickenType chickenType)
    {
        active = true;
        this.chickenType = chickenType;
        chickenIcon.sprite = chickenType.sprite;
        chickenIcon.enabled = true;
        //chickenIcon.transform.localScale = Vector3.one * 50;
        chickenIconRecharge.enabled = true;
        imageText.text = chickenType.name;

        if (isNormalSlot())
        {

        }
        else
        {
            chickenCountDayTimeText = GameObject.Find(chickenType.name + "ChickensDayTimeText").GetComponent<Text>();
        }

        



        updateChickenUICount();
    }

    public void clearChicken()
    {
        active = false;
        this.chickenType = null;
        chickenIcon.sprite = null;
        chickenIcon.enabled = false;
        chickenIconRecharge.enabled = false;
        imageText.text = "";
        deselectChicken();
    }

    public void selectChicken()
    {
        //selected = true;
        //imageText.text = "Selected!";
        selectedImage.enabled = true;
    }

    public void deselectChicken()
    {
        //selected = false;
        //imageText.text = "DeSelected!";
        selectedImage.enabled = false;
    }

    public void updateFired()
    {
        rechargeTimer.Start(chickenType.cooldown);
        updateChickenUICount();
    }

    public void updateChickenUICount()
    {
        imageOverLayText.text = isNormalSlot() ? "-" : chickenType.chickenCount.ToString();
    }

    public void updateSeedCount()
    {
        imageOverLayText.text = isNormalSlot() ? "-" : chickenType.seedCount.ToString();
    }

    public void updateDayTimeChickenUICount()
    {
        if (isNormalSlot())
            return;

        chickenCountDayTimeText.text = chickenType.name + " Chickens: " + chickenType.chickenCount;
    }


}
