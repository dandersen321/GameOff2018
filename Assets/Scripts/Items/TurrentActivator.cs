using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TurrentActivator : Item
{
    public override void use()
    {
        if (References.GetTurrent().overrideDialogCheck)
        {
            References.GetTurrent().activateTurrentMode();
            return;
        }

        var alienDialogAvailable = References.GetTurrent().alienDialog.DialogAvailable();
        var imortalDialogAvailable = References.GetTurrent().imortalDialog.DialogAvailable();
        if (!imortalDialogAvailable && !alienDialogAvailable)
            References.GetTurrent().activateTurrentMode();
        else
            GetComponent<DialogActivator>().ActivateDialog();
    }

}

