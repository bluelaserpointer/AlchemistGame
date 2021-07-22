using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

/// <summary>
/// 游戏者战斗中信息栏
/// </summary>
public abstract class GamerStatusUI : MonoBehaviour
{
    //inspecter
    [SerializeField]
    Image faceImage;
    [SerializeField]
    StatusPanels statusPanels;
    public StatusPanels StatusPanels => statusPanels;

    [SerializeField]
    UnityEvent onHandCardsChanged;
    //data
    protected Gamer gamer;
    /// <summary>
    /// 游戏者
    /// </summary>
    public Gamer Gamer
    {
        get => gamer;
        set
        {
            if (gamer == value)
                return;
            gamer = value;
            statusPanels.Gamer = gamer;
            UpdateUI();
        }
    }
    /// <summary>
    /// 手牌变化事件
    /// </summary>
    public UnityEvent OnHandCardsChanged => onHandCardsChanged;
    public bool IsMySide => gamer.IsMe;
    public bool IsEnemySide => gamer.IsEnemy;
    protected void UpdateUI()
    {
        if (gamer != null)
        {
            faceImage.sprite = gamer.character.FaceIcon;
        }
        else
        {
            faceImage.sprite = null;
        }
    }
    public Deck Deck => gamer.deck;

    SubstanceCard currentDrawingCard;
    /// <summary>
    /// 最后抽到的卡牌
    /// </summary>
    public SubstanceCard LastDrawingCard => currentDrawingCard;
    public void DrawCard()
    {
        if (Deck.CardCount > 0)
        {
            AddHandCard(currentDrawingCard = Deck.DrawRandomCard());
        }
    }
    public SubstanceCard FindHandCard(Substance substance)
    {
        return gamer.handCards.Find(card => card.Substance.Equals(substance));
    }
    public SubstanceCard FindHandCard(SubstanceCard substanceCard)
    {
        return gamer.handCards.Find(card => card.IsSameSubstance(substanceCard));
    }
    /// <summary>
    /// 加入手牌
    /// </summary>
    /// <param name="substanceCard"></param>
    public virtual void AddHandCard(SubstanceCard substanceCard)
    {
        SubstanceCard duplicatedCard = FindHandCard(substanceCard);
        if (duplicatedCard == null)
            gamer.handCards.Add(substanceCard);
        else
            duplicatedCard.UnionSameCard(LastDrawingCard);
        OnHandCardsChanged.Invoke();
    }
    public void RemoveHandCard(Substance substance)
    {
       RemoveHandCard(FindHandCard(substance));
    }
    public virtual bool RemoveHandCard(SubstanceCard substanceCard)
    {
        if(gamer.handCards.Remove(substanceCard))
        {
            OnHandCardsChanged.Invoke();
            return true;
        }
        return false;
    }
    /// <summary>
    /// 解放卡牌(回收摩尔能量)
    /// </summary>
    /// <param name="substanceCard"></param>
    public void ReleaseCard(SubstanceCard substanceCard)
    {
        Destroy(substanceCard.gameObject);
        gamer.Mol += substanceCard.Mol * substanceCard.CardAmount;
    }
    /// <summary>
    /// 手牌总数
    /// </summary>
    /// <returns></returns>
    public int GetHandCardCount()
    {
        int count = 0;
        foreach(SubstanceCard card in Gamer.handCards)
        {
            count += card.CardAmount;
        }
        return count;
    }
    /// <summary>
    /// 回合开始时事件
    /// </summary>
    public void OnTurnStart()
    {
        DrawCard();
        /* //(deprecated) card draw animation
        if (Deck.CardCount > 0)
        {
            currentDrawingCard = Deck.DrawRandomCard();
            lestCardsText.text = Deck.CardCount.ToString();
            Transform drawCardAnimatorTransform = drawCardAnimator.transform;
            if (drawCardAnimatorTransform.childCount > 0)
            {
                foreach (Transform eachChildTransform in drawCardAnimatorTransform)
                {
                    Destroy(eachChildTransform.gameObject);
                }
            }
            SubstanceCard forDrawAnimationCard = Instantiate(currentDrawingCard, drawCardAnimatorTransform); //clone to card animater
            forDrawAnimationCard.EnableShadow(true);
            //Add click event
            forDrawAnimationCard.GetComponent<CardDrag>().enabled = false;
            Button button = forDrawAnimationCard.gameObject.AddComponent<Button>();
            button.onClick.AddListener(() => {
                NextDrawProcess();
                forDrawAnimationCard.EnableShadow(false);
                MatchManager.CardInfoDisplay.SetCard(currentDrawingCard);
                button.enabled = false;
            });
            //Inovke card draw animation
            drawCardAnimator.SetInteger("CardDrawProcess", 1);
            drawableSign.Lit();
        }
        else
        {
            //TODO: Invoke gameover or ...
        }
        */
    }
    /// <summary>
    /// 获得可消耗为融合素材的所有卡牌(手牌+场地)
    /// </summary>
    /// <returns></returns>
    public List<SubstanceCard> GetConsumableCards()
    {
        List<SubstanceCard> cards = new List<SubstanceCard>();
        cards.AddRange(gamer.handCards);
        cards.AddRange(gamer.Field.Cards);
        return cards;
    }
}
