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


    // Use this for initialization
    void Awake () {
        chickenIcon = this.transform.Find("Icon").gameObject.GetComponent<Image>();
        selectedImage = this.transform.Find("Selected").gameObject.GetComponent<Image>();

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


}
