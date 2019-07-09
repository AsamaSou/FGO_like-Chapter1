using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Character : MonoBehaviour {

    public enum クラス
    {
        セイバー = 0,
        ランサー = 1,
        アーチャー = 2,
        /*
        ライダー = 3,
        キャスター = 4,
        アサシン = 5,
        */
        バーサーカー = 6
    }

    public enum Card
    {
        Buster,
        Arts,
        Quick
    }

    public string name;
    public int hp, np, atk;
    public クラス servantClass;
    public Card[] cards = new Card[5];
    public Sprite sprite;
    public Slider hpBar;
    public GameObject[] hasObjects;
    public Text damageText;
    public Text hpText;

    
    [Header("キャラデータをここに格納して使ってください")]
    public GameObject characterPrefab;

	// Use this for initialization
	void Awake () {
        if(characterPrefab)
            Init(characterPrefab.GetComponent<Character>());
        damageText.color = new Color(1,1,1,0);
	}

    /// <summary>
    /// 初期化
    /// </summary>
    void Init(Character copyData)
    {
        name = copyData.name;
        hp = copyData.hp;
        hpBar.value = hpBar.maxValue = hp;
        atk = copyData.atk;
        np = 0;
        servantClass = copyData.servantClass;
        cards = copyData.cards;
        sprite = copyData.sprite;
    }

    private void Update()
    {
        hpBar.value = hp;
        hpText.text = hp.ToString();
    }

    Coroutine damageCoroutine = null;

    public void Damage(int damage) {
        if(damageCoroutine != null)
            StopCoroutine(damageCoroutine);
        damageCoroutine = StartCoroutine(DamageCoroutine(damage));
    }


    public IEnumerator DamageCoroutine(int damage)
    {
        damageText.color = new Color(1, 1, 1, 1);
        damageText.text = damage.ToString();
        yield return new WaitForSeconds(1);
        damageText.color = new Color(1,1,1,0);
    }

    private void OnDestroy()
    {
        foreach(GameObject obj in hasObjects)
        {
            Destroy(obj);
        }
    }
}
