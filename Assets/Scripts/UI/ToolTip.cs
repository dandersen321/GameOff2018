using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class ToolTip : MonoBehaviour {

    Button buttonToToolTip;
    public string nameToDisplay;
    public string messageToDisplay;

    GameObject toolTip;
    Text mainText;
    Text nameText;

    Timer toolTipTimer = new Timer();
    bool hovering = false;
    float toolTipTimerWait = 0.5f;


    public void Awake()
    {
        buttonToToolTip = GetComponent<Button>();

        toolTip = GameObject.Find("ToolTipPanel");
        mainText = GameObject.Find("ToolTipMainText").GetComponent<Text>();
        nameText = GameObject.Find("ToolTipNameText").GetComponent<Text>();

        EventTrigger.Entry eventtype = new EventTrigger.Entry();
        eventtype.eventID = EventTriggerType.PointerEnter;
        eventtype.callback.AddListener((eventData) => { OnPointerEnter(); });

        EventTrigger.Entry eventtype2 = new EventTrigger.Entry();
        eventtype2.eventID = EventTriggerType.PointerExit;
        eventtype2.callback.AddListener((eventData) => { OnPointerExit(); });

        buttonToToolTip.gameObject.AddComponent<EventTrigger>();
        buttonToToolTip.gameObject.GetComponent<EventTrigger>().triggers.Add(eventtype);
        buttonToToolTip.gameObject.GetComponent<EventTrigger>().triggers.Add(eventtype2);
    }

    // Use this for initialization
    void Start () {
        HideToolTipInfo();	
	}

    public void Update()
    {
        if (hovering && (Time.timeScale == 0 || toolTipTimer.Expired()) && !toolTip.activeSelf)
        {
            ShowToolTipInfo();
        }
    }


    public void ShowToolTipInfo()
    {
        toolTip.SetActive(true);
        toolTip.transform.position = this.transform.position + new Vector3(350, -250, 0);
        nameText.text = nameToDisplay;
        mainText.text = messageToDisplay;
    }

    public void HideToolTipInfo()
    {
        toolTip.SetActive(false);
    }


    public void OnPointerEnter()
    {
        hovering = true;
        toolTipTimer.Start(toolTipTimerWait);


    }

    public void OnPointerExit()
    {
        //Debug.Log("Exit!");
        hovering = false;
        HideToolTipInfo();
    }
}
