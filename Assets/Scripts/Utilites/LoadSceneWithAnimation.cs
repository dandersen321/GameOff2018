using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneWithAnimation : MonoBehaviour {

    public Animator transitionAnimation;
    public int sceneToLoadIndex = 1;

    public void StartGame()
    {
        StartCoroutine(LoadScene());
    }

    IEnumerator LoadScene()
    {
        transitionAnimation.SetTrigger("StartGame");
        yield return new WaitForSeconds(2.5f);
        SceneManager.LoadScene(sceneToLoadIndex);
    }
	
}
