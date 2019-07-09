using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneChangeButton : MonoBehaviour {

    public string sceneName="";

    public void SceneChange()
    {
        SceneManager.LoadScene(sceneName);
    }
}
