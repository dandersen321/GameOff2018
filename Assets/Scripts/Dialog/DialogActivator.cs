using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Speaker {
    alien, immortal
}

/// <summary>
/// The DialogActivator sits on any game object that needs dialog.
/// When the time is right simply call the Activate dialog. 
/// If dialog is already active then the call is ignored and will
/// need to be called again.
/// </summary>
public class DialogActivator : MonoBehaviour {

    public List<DayDialog> dialog;
    public Speaker speaker;

    // For debugging
    public int currentDay = 1;

    public void Start()
    {
        dialog = Json2Dialog.getDialogListForDay(speaker, currentDay);
    }

    public bool ActivateDialog()
    {
        var dialogForDay = dialog.Find(obj => obj.day == currentDay);

        if (dialogForDay != null && !DialogSystem.Instance.IsActive && !dialogForDay.dialogRead)
        {
            DialogSystem.Instance.StartDialog(dialogForDay);
            dialogForDay.dialogRead = true;
            return true;
        }

        return false;
    }
}
