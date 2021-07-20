using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 游戏者战斗中信息栏
/// </summary>
public class GamerStatusUI : MonoBehaviour
{
    //inspecter
    [SerializeField]
    Image faceImage;
    [SerializeField]
    Text HPText;
    [SerializeField]
    Text lestCardsText;
    [SerializeField]
    Text molText;
    [SerializeField]
    Text skillText;

    [Header("Animation")]
    public Animator drawCardAnimator;

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
            UpdateUI();
        }
    }
    protected void UpdateUI()
    {
        if (gamer != null)
        {
            faceImage.sprite = gamer.character.FaceIcon;
            HPText.text = gamer.HP.ToString();
            molText.text = gamer.Mol.ToString();
            gamer.OnHPChange.AddListener(() => HPText.text = gamer.HP.ToString());
            gamer.OnMolChange.AddListener(() => molText.text = gamer.Mol.ToString());
        }
        else
        {
            faceImage.sprite = null;
            HPText.text = "--";
        }
    }
    //data
    public Deck Deck => gamer.deck;

    private void Start()
    {
        Deck.onCardCountChange.AddListener(() => lestCardsText.text = Deck.CardCount.ToString());
        lestCardsText.text = Deck.CardCount.ToString();
    }
    public int CardDrawProcess => drawCardAnimator.GetInteger("CardDrawProcess");
    SubstanceCard currentDrawingCard;
    /// <summary>
    /// 最后抽到的卡牌
    /// </summary>
    public SubstanceCard LastDrawingCard => currentDrawingCard;
    public void DrawCard()
    {
        if (Deck.CardCount > 0)
        {
            currentDrawingCard = Deck.DrawRandomCard();
            Transform duplicatedCardTf = MatchManager.HandCards.FindCard(card => card.GetComponent<SubstanceCard>().Substance.Equals(currentDrawingCard.Substance));
            if (duplicatedCardTf == null)
                MatchManager.HandCards.Add(currentDrawingCard.gameObject);
            else
                duplicatedCardTf.GetComponent<SubstanceCard>().UnionSameCard(LastDrawingCard);
        }
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
}
