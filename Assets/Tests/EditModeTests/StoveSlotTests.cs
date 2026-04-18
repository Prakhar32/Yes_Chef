using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class StoveSlotTests
{
    private readonly List<GameObject> _created = new List<GameObject>();

    private GameObject Spawn(string key)
    {
        var handle = Addressables.LoadAssetAsync<GameObject>(key);
        handle.WaitForCompletion();
        GameObject instance = Object.Instantiate(handle.Result);
        Addressables.Release(handle);
        _created.Add(instance);
        return instance;
    }

    [TearDown]
    public void TearDown()
    {
        foreach (GameObject go in _created)
            if (go != null) Object.DestroyImmediate(go);
        _created.Clear();
    }

    [Test]
    public void Interact_WithRawMeatInHand_EmptiesHand()
    {
        StoveSlot slot = Spawn("Stove").GetComponentInChildren<StoveSlot>();
        PlayerHand hand = Spawn("Player").GetComponentInChildren<PlayerHand>();
        hand.TryPickUp(Spawn("RawMeat").GetComponent<RawMeat>());

        slot.Interact(hand);

        Assert.IsNull(hand.Held);
    }

    [Test]
    public void Interact_WhileCooking_DoesNotTakeNewIngredient()
    {
        StoveSlot slot = Spawn("Stove").GetComponentInChildren<StoveSlot>();
        PlayerHand firstHand = Spawn("Player").GetComponentInChildren<PlayerHand>();
        firstHand.TryPickUp(Spawn("RawMeat").GetComponent<RawMeat>());
        slot.Interact(firstHand);

        PlayerHand secondHand = Spawn("Player").GetComponentInChildren<PlayerHand>();
        secondHand.TryPickUp(Spawn("RawMeat").GetComponent<RawMeat>());
        slot.Interact(secondHand);

        Assert.IsNotNull(secondHand.Held);
    }

    [Test]
    public void Interact_WithNonRawMeatInHand_DoesNotTakeIngredient()
    {
        StoveSlot slot = Spawn("Stove").GetComponentInChildren<StoveSlot>();
        PlayerHand hand = Spawn("Player").GetComponentInChildren<PlayerHand>();
        RawVegetable veg = Spawn("RawVegetable").GetComponent<RawVegetable>();
        hand.TryPickUp(veg);

        slot.Interact(hand);

        Assert.AreSame(veg, hand.Held);
    }
}
