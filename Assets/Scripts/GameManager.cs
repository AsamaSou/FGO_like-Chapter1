using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SelectedCard
{
    private int atk;
    public int Atk
    {
        get
        {
            return atk;
        }
    }
    Character.Card card;
    public Character.Card Card
    {
        get
        {
            return card;
        }
    }
    Character.クラス charaClass;
    public Character.クラス CharaClass
    {
        get
        {
            return charaClass;
        }
    }
    Sprite sprite;
    public Sprite Sprite
    {
        get
        {
            return sprite;
        }
    }

    public SelectedCard(int atk, Character.Card card, Character.クラス charaClass, Sprite sprite)
    {
        this.atk = atk;
        this.card = card;
        this.charaClass = charaClass;
        this.sprite = sprite;
    }
}

public class GameManager : MonoBehaviour {

    [SerializeField]
    List<Character> servant;
    [SerializeField]
    List<Character> enemies;

    public CanvasGroup cardPanel;
    public CanvasGroup clearPanel;

    delegate void TurnAction();

    Queue<Character> characters = null;
    List<SelectedCard> stackCards = new List<SelectedCard>();
    Queue<SelectedCard> enemySelectedCards = new Queue<SelectedCard>();

    // 展開カード枚数
    const int VIEW_MAX_CARD = 5;

    // 展開カード
    SelectedCard[] selectableCards = new SelectedCard[VIEW_MAX_CARD];

    // 攻撃対象エネミー
    KeyValuePair<int, Character> targetEnemy;

    public Image[] viewCardBacks = new Image[5];
    public Image[] viewCardFores = new Image[5];

    public Card[] choiceCards = new Card[5];
    public Text wave;

    // Use this for initialization
    void Start () {
        wave.text = "Wave "+(totalBattleNum+1).ToString()+"/2";
        clearPanel.alpha = 0;
        clearPanel.blocksRaycasts = false;
        clearPanel.interactable = false;
        gameover.alpha = 0;
        gameover.blocksRaycasts = false;
        gameover.interactable = false;
        targetEnemy = new KeyValuePair<int, Character>(0, enemies[0]);
        SetCard();
    }

    public void AttackButton()
    {
        cardPanel.alpha = 1;
        cardPanel.blocksRaycasts = true;
        cardPanel.interactable = true;
        StartCoroutine(WaitCardSelect());
    }

    void SetCard()
    {
        Card.selectedCards = new Queue<SelectedCard>();

        //カードが空なら追加
        if(stackCards.Count == 0)
        {
            print("カード追加");
            for (int i = 0; i < 3; i++)
            {
                int atk = servant[i].atk;
                Character.クラス servantClass = servant[i].servantClass;
                Sprite sprite = servant[i].sprite;
                for (int j = 0; j < servant[i].cards.Length; j++) {
                    stackCards.Add(new SelectedCard(
                        atk,
                        servant[i].cards[j],
                        servantClass,
                        sprite
                    ));

                }
            }
        }

        foreach (Character enemy in enemies)
        {
            enemySelectedCards.Enqueue(new SelectedCard(
                enemy.atk,
                enemy.cards[Random.Range(0, enemy.cards.Length)],
                enemy.servantClass,
                enemy.sprite
            ));
        }

        for (int i = 0; i < VIEW_MAX_CARD; i++)
        {
            int carryCardKey = Random.Range(0, stackCards.Count);
            selectableCards[i] = stackCards[carryCardKey];
            stackCards.RemoveAt(carryCardKey);

            if (selectableCards[i].Card == Character.Card.Buster)
                viewCardBacks[i].color = Color.red;
            else if (selectableCards[i].Card == Character.Card.Arts)
                viewCardBacks[i].color = Color.blue;
            else if (selectableCards[i].Card == Character.Card.Quick)
                viewCardBacks[i].color = Color.green;
            else
            {
                viewCardBacks[i].color = Color.black;
            }
            
            viewCardFores[i].sprite = selectableCards[i].Sprite;

            choiceCards[i].Set(selectableCards[i]);
        }
    }

    private bool _isSelectComplete = false;
    public bool IsSelectComplete
    {
        set
        {
            _isSelectComplete = value;
        }
    }

    IEnumerator WaitCardSelect()
    {
        // 選択最大枚数
        int SELECT_MAX_CARD = 3;
        while (Card.selectedCards.Count != SELECT_MAX_CARD)
        {
            print(Card.selectedCards.Count);
            yield return null;
        }
        cardPanel.alpha = 0;
        cardPanel.blocksRaycasts = false;
        cardPanel.interactable = false;
        StartCoroutine(BattleAction());
    }

    IEnumerator BattleAction()
    {
        try
        {
            int targetKey = TargetChanger.TargetIndex <= enemies.Count ? TargetChanger.TargetIndex : enemies.Count;
            targetEnemy = new KeyValuePair<int, Character>(targetKey, enemies[targetKey]);
        }
        catch
        {
            targetEnemy = new KeyValuePair<int, Character>(0, enemies[0]);
        }
        while(Card.selectedCards.Count != 0)
        {
            if(targetEnemy.Value == null)
            {
                targetEnemy = new KeyValuePair<int, Character> ( 0 , enemies[0] );
            }
            int damage = DamageCalculator.DamageCalc(Card.selectedCards.Dequeue(), targetEnemy.Value);
            targetEnemy.Value.hp -= damage;
            targetEnemy.Value.Damage(damage);
            if (targetEnemy.Value.hp <= 0)
            {
                Destroy(targetEnemy.Value.transform.gameObject);
                enemies.RemoveAt(targetEnemy.Key);

                if (enemies.Count == 0)
                {
                    StartCoroutine(BattleFinish());
                    yield break;
                }
            }
            yield return new WaitForSeconds(1);
        }

        while (enemySelectedCards.Count != 0)
        {
            int targetKey = Random.Range(0, servant.Count);

            targetEnemy = new KeyValuePair<int, Character>(targetKey, servant[targetKey]);
            int damage = DamageCalculator.DamageCalc(enemySelectedCards.Dequeue(), targetEnemy.Value);
            targetEnemy.Value.hp -= damage;
            targetEnemy.Value.Damage(damage);
            if (targetEnemy.Value.hp <= 0)
            {
                Destroy(targetEnemy.Value.transform.gameObject);
                servant.RemoveAt(targetEnemy.Key);

                if (servant.Count == 0)
                {
                    StartCoroutine(GameOver());
                    // gameover
                    yield break;
                }
            }

            yield return new WaitForSeconds(1);
        }

        SetCard();
    }

    static int totalBattleNum=0;

    IEnumerator BattleFinish()
    {
        totalBattleNum++;
        if (totalBattleNum == 2)
        {
            clearPanel.alpha = 1;
            clearPanel.blocksRaycasts = true;
            clearPanel.interactable = true;
            yield return new WaitForSeconds(5);
            totalBattleNum = 0;
            SceneManager.LoadScene("GameClear");
        }
        else
        {
            yield return new WaitForSeconds(2);
            SceneManager.LoadScene("Main");
            
        }
    }

    public CanvasGroup gameover;

    IEnumerator GameOver()
    {
        gameover.alpha = 1;
        gameover.blocksRaycasts = true;
        gameover.interactable = true;
        yield return new WaitForSeconds(3);
        SceneManager.LoadScene("Main");
    }
}
