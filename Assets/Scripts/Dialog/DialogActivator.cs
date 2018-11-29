using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Speaker {
    alien, immortal, turret
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

    public void Start()
    {
        if (speaker != Speaker.turret)
            dialog = Json2Dialog.getDialogListForDay(speaker, References.GetEnemySpawnerManager().nightNumber);
    }

    public bool ActivateDialog()
    {
        if (speaker == Speaker.turret)
        {
            DialogSystem.Instance.StartDialog(dialog[0]);
            return true;
        }

        var dialogForDay = dialog.Find(obj => obj.day == References.GetEnemySpawnerManager().nightNumber);

        if (dialogForDay != null && !DialogSystem.Instance.IsActive && !dialogForDay.dialogRead)
        {
            DialogSystem.Instance.StartDialog(dialogForDay);
            dialogForDay.dialogRead = true;
            return true;
        }

        return false;
    }

    public bool DialogAvailable()
    {
        var dialogForDay = dialog.Find(obj => obj.day == References.GetEnemySpawnerManager().nightNumber);
        if (dialogForDay == null)
            return false;

        if (dialogForDay.dialogRead)
            return false;

        return true;
         
    }
}
