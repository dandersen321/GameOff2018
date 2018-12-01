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
        lines.Add("Victory is yours...");
        lines.Add("Congrats!");

        lines.Add("Farmer trundles over to the rubble that was the alien mothership to discover that a lone member of the high council barely survived the crash.");
        lines.Add("In hurried breaths he tells Farmer that Neil A. was exiled centuries ago after he tried to play God by creating an immortal human.");
        lines.Add("Such an act was a complete infraction of everything their race aimed to be.");
        lines.Add("Neil A.'s punishment was exile to Earth, the planet strategists agreed was least most likely to ever be exposed to an Artifact, removal of his hands and spending eternity in the company of the Immortal he created.");

        lines.Add("A sharp shriek behind Farmer caught his attention.");
        lines.Add("Amidst a tussle of dust and hay emerged the immortal, who threw the trenchcoat to the ground in defiance.");
        lines.Add("As his dying act, the high council member channels the Artifact's Quather and makes the Immortal a mortal human again.");
        lines.Add("The now mortal starts to make a run for one of the chicken coops... Farmer turns to head back to his farm.");
        lines.Add("It's time to seed the crops and feed the chickens.");
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
