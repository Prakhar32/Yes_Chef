using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using YesChef.Domain;

public class ChoppingTableTests
{
    private readonly List<GameObject> _created = new List<GameObject>();

    private T Make<T>() where T : MonoBehaviour
    {
        GameObject go = new GameObject();
        _created.Add(go);
        return go.AddComponent<T>();
    }

    [TearDown]
    public void TearDown()
    {
        foreach (GameObject go in _created)
            Object.DestroyImmediate(go);
        _created.Clear();
    }

    [Test]
    public void Interact_WithRawVegetableInHand_EmptiesHand()
    {
        ChoppingTable table = Make<ChoppingTable>();
        PlayerHand hand = new PlayerHand();
        hand.TryPickUp(Make<RawVegetable>());

        table.Interact(hand);

        Assert.IsNull(hand.Held);
    }

    [Test]
    public void Interact_WhileChopping_DoesNotTakeNewIngredient()
    {
        ChoppingTable table = Make<ChoppingTable>();
        PlayerHand firstHand = new PlayerHand();
        firstHand.TryPickUp(Make<RawVegetable>());
        table.Interact(firstHand); // table is now chopping

        PlayerHand secondHand = new PlayerHand();
        RawVegetable secondVeg = Make<RawVegetable>();
        secondHand.TryPickUp(secondVeg);
        table.Interact(secondHand);

        Assert.IsNotNull(secondHand.Held);
    }

    [Test]
    public void OnlyInteracts_WithRawVegetable()
    {
        ChoppingTable table = Make<ChoppingTable>();
        PlayerHand hand = new PlayerHand();
        RawMeat meat = Make<RawMeat>();
        hand.TryPickUp(meat);

        table.Interact(hand);

        Assert.AreEqual(meat, hand.Held);
    }

    [Test]
    public void DoesNotInteracts_WithEmptyHand()
    {
        ChoppingTable table = Make<ChoppingTable>();
        PlayerHand hand = new PlayerHand();

        table.Interact(hand);

        Assert.IsFalse(table.IsReady);
    }
}
