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

    private ChoppingTable MakeTable()
    {
        ChoppingTable table = Make<ChoppingTable>();
        table.choppedVegetablePrefab = Make<ChoppedVegetable>();
        return table;
    }

    [UnityTest]
    public IEnumerator AfterChopDuration_ChoppedVegetableSpawnsAtRawVegetablePosition()
    {
        ChoppingTable table = MakeTable();
        PlayerHand hand = new PlayerHand();
        RawVegetable raw = Make<RawVegetable>();
        raw.transform.position = new Vector3(1f, 2f, 3f);
        hand.TryPickUp(raw);
        table.Interact(hand);

        yield return WaitForChop;

        ChoppedVegetable chopped = Object.FindFirstObjectByType<ChoppedVegetable>();
        Assert.AreEqual(new Vector3(1f, 2f, 3f), chopped.transform.position);
    }

    [UnityTest]
    public IEnumerator WhenVegetableIsChopped_Interact_WithEmptyHand_PutsItInHand()
    {
        ChoppingTable table = MakeTable();
        PlayerHand hand = new PlayerHand();
        hand.TryPickUp(Make<RawVegetable>());
        table.Interact(hand);

        yield return WaitForChop;

        table.Interact(hand);

        Assert.IsInstanceOf<ChoppedVegetable>(hand.Held);
    }

}
