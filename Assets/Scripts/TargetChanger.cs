using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Toggle))]
public class TargetChanger : MonoBehaviour {

    Toggle targetToggle;
    public Image targetCursor;
    public int thisIndex;

    static int _targetIndex = 0;
    public static int TargetIndex
    {
        get
        {
            return _targetIndex;
        }
    }

    void Start()
    {
        _targetIndex = 0;
        targetToggle = GetComponent<Toggle>();
    }

    void Update () {
        if (targetToggle.isOn)
        {
            targetCursor.enabled = true;
            //_targetIndex = thisIndex;
        }
        else
        {
            targetCursor.enabled = false;
        }
	}
}
