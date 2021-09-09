using System;

/// <summary>
/// 战斗前卡组(静态卡组)
/// </summary>
[Serializable]
public class Deck
{
    public Deck()
    {
        substances = new StackedElementList<Substance>();
    }
    public Deck(StackedElementList<Substance> initialSubstances)
    {
        substances = new StackedElementList<Substance>(initialSubstances);
    }
    public Deck(Deck sampleDeck)
    {
        substances = new StackedElementList<Substance>(sampleDeck.Substances);
    }
    public StackedElementList<Substance> substances;
    public StackedElementList<Substance> Substances => substances;
    public bool IsEmpty => Substances.IsEmpty;
    public int CardCount => Substances.CountStack();

    public void Add(Substance substance)
    {
        Substances.Add(substance);
    }
    public bool Remove(Substance substance)
    {
        return Substances.Remove(substance);
    }
    public int CountCard(Substance substance)
    {
        return Substances.CountStack(substance);
    }
}
