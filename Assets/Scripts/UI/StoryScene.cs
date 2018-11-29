using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class StoryScene : MonoBehaviour {

    public Text storyText;
    public Text buttonText;
    public Button startButton;
    public List<string> lines;
    public int sceneToLoad = 2;
    private int lineIdx = 0;

    public void Nextline()
    {
        if (lineIdx < lines.Count)
        {
            storyText.text = lines[lineIdx];
            lineIdx++;

            if (lineIdx == lines.Count)
                buttonText.text = "Start Game";
        }
        else
        {
            startButton.enabled = false;
            buttonText.text = "Loading";
            storyText.enabled = false;
            StartCoroutine(BeginLoadSceneAsync());
            //SceneManager.LoadScene(sceneToLoad);
        }
    }

    private void initLines()
    {
        lines.Add("Many crop rotations ago in a time when there was no time...\n\n" +
                  "The most elite intelligence in the universe, \"The Ancients\" reigned.");
        lines.Add("Weilding a technology far beyond human comprehension, The Ancients manipulated the fabric of reality.\n\n" +
                   "It is referred to by modern starfaring species as Quantum Aether or 'Quaether'.");
        lines.Add("The fate of The Ancients is perhaps the most debated historical enigma.\n\n" +
                  "Some believe they never left and are simply observing the folds of time unravel.");
        lines.Add("As the single most valued substance in the universe, the discovery of new Quather Artifacts draws the attention of any half witted interstellar organic thinker both good, and bad...");

    }

    // Use this for initialization
    void Start () {
        if (lines.Count == 0) {
            initLines();
        }
        Nextline();
    }

    IEnumerator BeginLoadSceneAsync()
    {
        AsyncOperation asyncSceneLoad = SceneManager.LoadSceneAsync(sceneToLoad);

        while(!asyncSceneLoad.isDone)
        {
            yield return null;
        }
    }
}
