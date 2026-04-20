using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.TestTools;

public class CustomerWindowTests
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

    [UnitySetUp]
    public IEnumerator SetUp()
    {
        Time.timeScale = 10f;
        yield return null;
    }

    [TearDown]
    public void TearDown()
    {
        Time.timeScale = 1f;
        foreach (GameObject go in _created)
            if (go != null) Object.Destroy(go);
        _created.Clear();
    }

    [UnityTest]
    public IEnumerator CorrectIngredient_FulfillsOrder_AndClearsHand()
    {
        PlayerHand hand = Spawn("Player").GetComponentInChildren<PlayerHand>();
        CustomerWindow window = Spawn("CustomerWindow").GetComponent<CustomerWindow>();
        yield return null;

        window.Open(new Order(new[] { typeof(ChoppedVegetable) }));
        hand.TryPickUp(Spawn("ChoppedVegetable").GetComponent<ChoppedVegetable>());
        window.Interact();

        Assert.IsNull(hand.Held);
        Assert.IsTrue(window.IsEmpty);
    }

    [UnityTest]
    public IEnumerator UnpreparedIngredient_DoesNotFulfillOrder_AndStaysInHand()
    {
        PlayerHand hand = Spawn("Player").GetComponentInChildren<PlayerHand>();
        CustomerWindow window = Spawn("CustomerWindow").GetComponent<CustomerWindow>();
        yield return null;

        window.Open(new Order(new[] { typeof(ChoppedVegetable) }));
        RawVegetable raw = Spawn("RawVegetable").GetComponent<RawVegetable>();
        hand.TryPickUp(raw);
        window.Interact();

        Assert.AreSame(raw, hand.Held);
        Assert.IsFalse(window.IsEmpty);
    }

    [UnityTest]
    public IEnumerator WrongIngredient_DoesNotFulfillOrder_AndStaysInHand()
    {
        PlayerHand hand = Spawn("Player").GetComponentInChildren<PlayerHand>();
        CustomerWindow window = Spawn("CustomerWindow").GetComponent<CustomerWindow>();
        yield return null;

        window.Open(new Order(new[] { typeof(RawCheese) }));
        RawMeat meat = Spawn("RawMeat").GetComponent<RawMeat>();
        hand.TryPickUp(meat);
        window.Interact();

        Assert.AreSame(meat, hand.Held);
        Assert.IsFalse(window.IsEmpty);
    }

    [UnityTest]
    public IEnumerator NoActiveOrder_DoesNotClearHand()
    {
        PlayerHand hand = Spawn("Player").GetComponentInChildren<PlayerHand>();
        CustomerWindow window = Spawn("CustomerWindow").GetComponent<CustomerWindow>();
        yield return null;

        RawCheese cheese = Spawn("RawCheese").GetComponent<RawCheese>();
        hand.TryPickUp(cheese);
        window.Interact();

        Assert.AreSame(cheese, hand.Held);
    }

}
