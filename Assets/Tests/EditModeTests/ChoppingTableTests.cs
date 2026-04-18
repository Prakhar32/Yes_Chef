using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class ChoppingTableTests
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
    public void Interact_WithRawVegetableInHand_EmptiesHand()
    {
        ChoppingTable table = Spawn("ChoppingTable").GetComponent<ChoppingTable>();
        PlayerHand hand = Spawn("Player").GetComponentInChildren<PlayerHand>();
        hand.TryPickUp(Spawn("RawVegetable").GetComponent<RawVegetable>());

        table.Interact(hand);

        Assert.IsNull(hand.Held);
    }

    [Test]
    public void Interact_WhileChopping_DoesNotTakeNewIngredient()
    {
        ChoppingTable table = Spawn("ChoppingTable").GetComponent<ChoppingTable>();
        PlayerHand firstHand = Spawn("Player").GetComponentInChildren<PlayerHand>();
        firstHand.TryPickUp(Spawn("RawVegetable").GetComponent<RawVegetable>());
        table.Interact(firstHand);

        PlayerHand secondHand = Spawn("Player").GetComponentInChildren<PlayerHand>();
        RawVegetable secondVeg = Spawn("RawVegetable").GetComponent<RawVegetable>();
        secondHand.TryPickUp(secondVeg);
        table.Interact(secondHand);

        Assert.IsNotNull(secondHand.Held);
    }

    [Test]
    public void OnlyInteracts_WithRawVegetable()
    {
        ChoppingTable table = Spawn("ChoppingTable").GetComponent<ChoppingTable>();
        PlayerHand hand = Spawn("Player").GetComponentInChildren<PlayerHand>();
        RawMeat meat = Spawn("RawMeat").GetComponent<RawMeat>();
        hand.TryPickUp(meat);

        table.Interact(hand);

        Assert.AreSame(meat, hand.Held);
    }
}
