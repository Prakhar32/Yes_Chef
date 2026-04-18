using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.TestTools;

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
            if (go != null) Object.Destroy(go);
        _created.Clear();
    }

    [UnityTest]
    public IEnumerator Interact_WithRawMeatInHand_EmptiesHand()
    {
        PlayerHand hand = Spawn("Player").GetComponentInChildren<PlayerHand>();
        StoveSlot slot = Spawn("Stove").GetComponentInChildren<StoveSlot>();
        yield return null;

        hand.TryPickUp(Spawn("RawMeat").GetComponent<RawMeat>());
        slot.Interact();

        Assert.IsNull(hand.Held);
    }

    [UnityTest]
    public IEnumerator Interact_WhileCooking_DoesNotTakeNewIngredient()
    {
        PlayerHand firstHand = Spawn("Player").GetComponentInChildren<PlayerHand>();
        StoveSlot slot = Spawn("Stove").GetComponentInChildren<StoveSlot>();
        yield return null;

        firstHand.TryPickUp(Spawn("RawMeat").GetComponent<RawMeat>());
        slot.Interact();

        PlayerHand secondHand = Spawn("Player").GetComponentInChildren<PlayerHand>();
        secondHand.TryPickUp(Spawn("RawMeat").GetComponent<RawMeat>());
        slot.Interact();

        Assert.IsNotNull(secondHand.Held);
    }

    [UnityTest]
    public IEnumerator Interact_WithNonRawMeatInHand_DoesNotTakeIngredient()
    {
        PlayerHand hand = Spawn("Player").GetComponentInChildren<PlayerHand>();
        StoveSlot slot = Spawn("Stove").GetComponentInChildren<StoveSlot>();
        yield return null;

        RawVegetable veg = Spawn("RawVegetable").GetComponent<RawVegetable>();
        hand.TryPickUp(veg);
        slot.Interact();

        Assert.AreSame(veg, hand.Held);
    }
}
