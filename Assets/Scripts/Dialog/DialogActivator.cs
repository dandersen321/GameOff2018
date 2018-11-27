using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The DialogActivator sits on any game object that needs dialog.
/// When the time is right simply call the Activate dialog. 
/// If dialog is already active then the call is ignored and will
/// need to be called again.
/// </summary>
public class DialogActivator : MonoBehaviour {

    public List<DayDialog> dialog;

    // For debugging
    public int currentDay = 1;

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
