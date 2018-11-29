using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class EndScene : MonoBehaviour {

    public Text storyText;
    public Text buttonText;
    public Button startButton;
    public List<string> lines;
    public int sceneToLoad = 0;
    private int lineIdx = 0;

    public void Nextline()
    {
        if (lineIdx < lines.Count)
        {
            storyText.text = lines[lineIdx];
            lineIdx++;

            if (lineIdx == lines.Count)
                buttonText.text = "Done";
        }
        else
        {
            startButton.enabled = false;
            storyText.enabled = false;
            SceneManager.LoadScene(sceneToLoad);
        }
    }

    private void initLines()
    {
        lines.Add("You beat the game...");
        lines.Add("Congrats!");
    }

    // Use this for initialization
    void Start () {
        if (lines.Count == 0) {
            initLines();
        }
        Nextline();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            Nextline();
    }
}
