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
    private Dictionary<int, List<DayDialog>> dialogForDays;

    public void Start()
    {
        dialogForDays = new Dictionary<int, List<DayDialog>>();
    }

    /// <summary>
    /// A hacky way to ensure that we are getting the latest dialog for each day since we have to retrieve it game day
    /// </summary>
    private void getDialogForDay()
    {
        if (speaker != Speaker.turret && !dialogForDays.ContainsKey(References.GetEnemySpawnerManager().nightNumber))
        {
            dialogForDays.Add(References.GetEnemySpawnerManager().nightNumber, Json2Dialog.getDialogListForDay(speaker, References.GetEnemySpawnerManager().nightNumber));
            dialog = dialogForDays[References.GetEnemySpawnerManager().nightNumber];
        }
    }

    public bool ActivateDialog()
    {
        getDialogForDay();
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
        getDialogForDay();
        var dialogForDay = dialog.Find(obj => obj.day == References.GetEnemySpawnerManager().nightNumber);
        if (dialogForDay == null)
            return false;

        if (dialogForDay.dialogRead)
            return false;

        return true;
         
    }
}
