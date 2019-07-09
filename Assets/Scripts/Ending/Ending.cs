using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Ending : MonoBehaviour {

    public Text getTotalBone;

    public GameObject backToTitleButton;

	// Use this for initialization
	void Start () {
        getTotalBone.text = "これまでひろったほねの数："+PlayerPrefs.GetInt("Bone").ToString()+"個";
        StartCoroutine(BackToTitle());
	}

    IEnumerator BackToTitle()
    {
        yield return new WaitForSeconds(2f);
        backToTitleButton.SetActive(true);
    }

    public void BackToTitleButton()
    {
        SceneManager.LoadScene("Title");
    }
}
