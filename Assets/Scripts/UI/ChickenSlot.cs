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
        return chickenType.name == ChickenTypeEnum.normalName;
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
        updateDisable(isUsableAsBullet());
        if (isNormalSlot())
            return;
        this.chickenIcon.sprite = chickenType.sprite;
        //imageOverLayText.text = isNormalSlot() ? "-" : chickenType.chickenCount.ToString();
        imageOverLayText.text = chickenType.chickenCount.ToString();
    }

    public void updateSeedCount()
    {
        this.chickenIcon.sprite = chickenType.seedSprite;
        imageOverLayText.text = isNormalSlot() ? "-" : chickenType.seedCount.ToString();
        updateDisable(isUsabledAsPlant());
    }

    public void updateDisable(bool validIcon)
    {
        if (validIcon)
        {
            imageOverLayText.color = Color.black;
            chickenIcon.color = Color.white;
        }
        else
        {
            imageOverLayText.color = Color.red;
            chickenIcon.color = Color.gray;
        }
    }

    public void updateDayTimeChickenUICount()
    {
        if (isNormalSlot())
            return;

        chickenCountDayTimeText.text = chickenType.name + ": " + chickenType.chickenCount;
    }

    public bool isUsableAsBullet()
    {
        return chickenType.name == ChickenTypeEnum.normalName || chickenType.chickenCount > 0;
    }

    public bool isUsabledAsPlant()
    {
        return chickenType.name != ChickenTypeEnum.normalName && chickenType.seedCount > 0;
    }


}
