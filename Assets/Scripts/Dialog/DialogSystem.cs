using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogSystem : MonoBehaviour {

    public static DialogSystem Instance { get; private set; }

    public Text nameText;
    public Text dialogText;
    public Text continueText;
    public bool IsActive { private set; get; }

    public Animator animator;

    private Queue<Line> lines;
    private DialogActivator activeDialogActivator;

    public void Awake()
    {
        lines = new Queue<Line>();
        if (Instance == null)
            Instance = this;
    }

    public void StartDialog(DayDialog dialog, DialogActivator activeDialogActivator)
    {
        this.activeDialogActivator = activeDialogActivator;
        IsActive = true;
        animator.SetBool("IsOpen", true);
        lines.Clear();
        References.GetPlayerMovementController().beginMenu();

        foreach (var line in dialog.lines)
        {
            lines.Enqueue(line);            
        }

        // Get the first line of dialog
        NextLine();
    }

    public void NextLine()
    {
        if (lines.Count > 0)
        {
            AudioPlayer.Instance.PlayeRandomVoice();

            if (lines.Count == 1)
                continueText.text = "Done";

            Line line = lines.Dequeue();
            nameText.text = line.name;
            dialogText.text = line.line;
        }
        else
        {
            IsActive = false;
            animator.SetBool("IsOpen", false);
            continueText.text = "...";
            References.GetPlayerMovementController().closeMenu();
            activeDialogActivator.onDialogFinished();
        }
    }

    public void Update()
    {
        if (IsActive && (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.E)))
        {
            NextLine();
        }
    }
}
