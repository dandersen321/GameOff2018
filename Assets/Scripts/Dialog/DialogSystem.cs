using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogSystem : MonoBehaviour {

    public Text nameText;
    public Text dialogText;
    public Text continueText;
    public bool IsActive { private set; get; }

    public Animator animator;

    private Queue<Line> lines;

    public void Awake()
    {
        lines = new Queue<Line>();
    }

    public void StartDialog(DayDialog dialog)
    {
        IsActive = true;
        animator.SetBool("IsOpen", true);
        lines.Clear();

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
        }
    }

    public void Update()
    {
        if (IsActive && Input.GetKeyDown(KeyCode.Space))
        {
            NextLine();
        }
    }
}
