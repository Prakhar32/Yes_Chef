using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using YesChef.Domain;

public class ChoppingTableTimerTests
{
    private readonly List<GameObject> _created = new List<GameObject>();
    private static readonly WaitForSeconds WaitForChop = new WaitForSeconds(2.1f);

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
            if (go != null) Object.Destroy(go);
        _created.Clear();
    }

    private ChoppingTable MakeTable()
    {
        ChoppingTable table = Make<ChoppingTable>();
        table.choppedVegetablePrefab = Make<ChoppedVegetable>();
        return table;
    }

    [UnityTest]
    public IEnumerator AfterChopDuration_TableIsReady()
    {
        ChoppingTable table = MakeTable();
        PlayerHand hand = new PlayerHand();
        hand.TryPickUp(Make<RawVegetable>());
        table.Interact(hand);

        yield return WaitForChop;

        Assert.IsTrue(table.IsReady);
    }

    [UnityTest]
    public IEnumerator Interact_WhenReady_WithEmptyHand_PutsChoppedVegetableInHand()
    {
        ChoppingTable table = MakeTable();
        PlayerHand hand = new PlayerHand();
        hand.TryPickUp(Make<RawVegetable>());
        table.Interact(hand);

        yield return WaitForChop;

        table.Interact(hand);

        Assert.IsInstanceOf<ChoppedVegetable>(hand.Held);
    }

    [UnityTest]
    public IEnumerator Interact_WhenReady_WithEmptyHand_ClearsTable()
    {
        ChoppingTable table = MakeTable();
        PlayerHand hand = new PlayerHand();
        hand.TryPickUp(Make<RawVegetable>());
        table.Interact(hand);

        yield return WaitForChop;

        table.Interact(hand);

        Assert.IsFalse(table.IsReady);
    }
}
