using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.TestTools;

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
            if (go != null) Object.Destroy(go);
        _created.Clear();
    }

    [UnityTest]
    public IEnumerator Interact_WithRawVegetableInHand_EmptiesHand()
    {
        PlayerHand hand = Spawn("Player").GetComponentInChildren<PlayerHand>();
        ChoppingTable table = Spawn("ChoppingTable").GetComponent<ChoppingTable>();
        yield return null;

        hand.TryPickUp(Spawn("RawVegetable").GetComponent<RawVegetable>());
        table.Interact();

        Assert.IsNull(hand.Held);
    }

    [UnityTest]
    public IEnumerator Interact_WhileChopping_DoesNotTakeNewIngredient()
    {
        PlayerHand firstHand = Spawn("Player").GetComponentInChildren<PlayerHand>();
        ChoppingTable table = Spawn("ChoppingTable").GetComponent<ChoppingTable>();
        yield return null;

        firstHand.TryPickUp(Spawn("RawVegetable").GetComponent<RawVegetable>());
        table.Interact();

        PlayerHand secondHand = Spawn("Player").GetComponentInChildren<PlayerHand>();
        RawVegetable secondVeg = Spawn("RawVegetable").GetComponent<RawVegetable>();
        secondHand.TryPickUp(secondVeg);
        table.Interact();

        Assert.IsNotNull(secondHand.Held);
    }

    [UnityTest]
    public IEnumerator OnlyInteracts_WithRawVegetable()
    {
        PlayerHand hand = Spawn("Player").GetComponentInChildren<PlayerHand>();
        ChoppingTable table = Spawn("ChoppingTable").GetComponent<ChoppingTable>();
        yield return null;

        RawMeat meat = Spawn("RawMeat").GetComponent<RawMeat>();
        hand.TryPickUp(meat);
        table.Interact();

        Assert.AreSame(meat, hand.Held);
    }
}
