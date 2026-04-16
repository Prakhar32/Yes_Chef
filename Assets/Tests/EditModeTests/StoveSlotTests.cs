using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using YesChef.Domain;

public class StoveSlotTests
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
    public void Interact_WithRawMeatInHand_EmptiesHand()
    {
        StoveSlot slot = Make<StoveSlot>();
        PlayerHand hand = new PlayerHand();
        hand.TryPickUp(Make<RawMeat>());

        slot.Interact(hand);

        Assert.IsTrue(hand.Held == null);
    }

    [Test]
    public void Interact_WhileCooking_DoesNotTakeNewIngredient()
    {
        StoveSlot slot = Make<StoveSlot>();
        PlayerHand firstHand = new PlayerHand();
        firstHand.TryPickUp(Make<RawMeat>());
        slot.Interact(firstHand);

        PlayerHand secondHand = new PlayerHand();
        secondHand.TryPickUp(Make<RawMeat>());
        slot.Interact(secondHand);

        Assert.IsTrue(secondHand.Held != null);
    }

    [Test]
    public void Interact_WithNonRawMeatInHand_DoesNotTakeIngredient()
    {
        StoveSlot slot = Make<StoveSlot>();
        PlayerHand hand = new PlayerHand();
        RawVegetable veg = Make<RawVegetable>();
        hand.TryPickUp(veg);

        slot.Interact(hand);

        Assert.AreSame(veg, hand.Held);
    }

}
