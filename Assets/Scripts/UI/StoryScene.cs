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

    private int line = 0;

    public void Nextline()
    {
        if (line < lines.Count)
        {
            storyText.text = lines[line];
            line++;

            if (line == lines.Count)
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

	// Use this for initialization
	void Start () {
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
