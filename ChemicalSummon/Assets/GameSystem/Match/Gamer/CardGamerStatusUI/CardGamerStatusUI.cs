using TMPro;
using UnityEngine;
using UnityEngine.UI;

[DisallowMultipleComponent]
public class CardGamerStatusUI : GamerStatusUI
{
    public TextMeshProUGUI gamerNameText;
    public TextMeshProUGUI lestCardsText;
    public Image faceIcon;
    public SwitchSprite drawableSign;

    [Header("Animation")]
    public Animator drawCardAnimator;
    //data
    public Gamer gamer;
    Deck deck;
    public Deck Deck => deck;

    private void Start()
    {
        MatchManager.instance.onTurnStart.AddListener(OnTurnStart);
        deck = new Deck(); //TODO: copy contents from PlayerSetting
        deck.onCardCountChange.AddListener(UpdateCardCountText);
        gamerNameText.text = gamer.gamerInfo.character.Name;
        faceIcon.sprite = gamer.gamerInfo.character.FaceIcon;
        GaugeValueRangeMax = gamer.InitialHP;
        GaugeValue = gamer.hp;
        UpdateCardCountText(); //inital input
    }
    private void Update()
    {
        GaugeValue = gamer.hp;
    }
    void UpdateCardCountText()
    {
        lestCardsText.text = Deck.CardCount.ToString();
    }
    public int CardDrawProcess => drawCardAnimator.GetInteger("CardDrawProcess");
    SubstanceCard currentDrawingCard;
    /// <summary>
    /// 刚刚抽到的卡牌
    /// </summary>
    public SubstanceCard CurrentDrawingCard => currentDrawingCard;
    /// <summary>
    /// 回合开始时事件
    /// </summary>
    public void OnTurnStart()
    {
        if(deck.CardCount > 0)
        {
            currentDrawingCard = deck.DrawRandomCard();
            lestCardsText.text = Deck.CardCount.ToString();
            Transform drawCardAnimatorTransform = drawCardAnimator.transform;
            if (drawCardAnimatorTransform.childCount > 0)
            {
                foreach(Transform eachChildTransform in drawCardAnimatorTransform)
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
    }
    public void OnCardStock()
    {
        if(currentDrawingCard != null)
        {
            GameObject duplicatedCard = MatchManager.HandCards.cards.Find(card => card.GetComponent<SubstanceCard>().Substance.Equals(currentDrawingCard.Substance));
            if(duplicatedCard == null)
                MatchManager.HandCards.Add(currentDrawingCard.gameObject);
            else
                duplicatedCard.GetComponent<SubstanceCard>().UnionSameCard(CurrentDrawingCard);
            currentDrawingCard = null;
        }
    }
    /// <summary>
    /// 动画 - 抽卡
    /// </summary>
    public void NextDrawProcess()
    {
        int nextProcess = CardDrawProcess + 1;
        switch(nextProcess)
        {
            case 1:
                drawableSign.Toggle();
                break;
            case 2:
                drawableSign.Toggle();
                break;
            case 3:
                drawableSign.Toggle();
                nextProcess = 1;
                break;
        }
        drawCardAnimator.SetInteger("CardDrawProcess", nextProcess);
    }
}
