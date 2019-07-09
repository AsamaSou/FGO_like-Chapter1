using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AutoSceneChange : MonoBehaviour {

    string sceneName = "Title";

	// Use this for initialization
	IEnumerator Start () {
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene(sceneName);
	}


}
