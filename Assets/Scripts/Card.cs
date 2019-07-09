using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour {

    // 選択済みカード
    public static Queue<SelectedCard> selectedCards = null;

    public Image foreImage;
    Image backImage;
    SelectedCard cardInfo;

    private void Awake()
    {
        backImage = GetComponent<Image>();
    }

    public void Set(SelectedCard cardInfo)
    {
        if (cardInfo.Card == Character.Card.Buster)
            backImage.color = Color.red;
        else if (cardInfo.Card == Character.Card.Arts)
            backImage.color = Color.blue;
        else if (cardInfo.Card == Character.Card.Quick)
            backImage.color = Color.green;
        else
        {
            backImage.color = Color.black;
        }

        foreImage.sprite = cardInfo.Sprite;
        this.cardInfo = cardInfo;
    }

    public void Selected()
    {
        selectedCards.Enqueue(cardInfo);
    }
}
