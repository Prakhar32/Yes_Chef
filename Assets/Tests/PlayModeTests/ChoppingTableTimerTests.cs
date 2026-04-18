using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.TestTools;

public class ChoppingTableTimerTests
{
    private readonly List<GameObject> _created = new List<GameObject>();
    private static readonly WaitForSeconds WaitForChop = new WaitForSeconds(2.1f);

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
    public IEnumerator AfterChopDuration_ChoppedVegetableSpawnsAtTablePosition()
    {
        ChoppingTable table = Spawn("ChoppingTable").GetComponent<ChoppingTable>();
        table.transform.position = new Vector3(1f, 2f, 3f);
        PlayerHand hand = Spawn("Player").GetComponentInChildren<PlayerHand>();
        hand.TryPickUp(Spawn("RawVegetable").GetComponent<RawVegetable>());
        table.Interact(hand);

        yield return WaitForChop;

        ChoppedVegetable chopped = Object.FindFirstObjectByType<ChoppedVegetable>();
        _created.Add(chopped.gameObject);
        Assert.AreEqual(new Vector3(1f, 2f, 3f), chopped.transform.position);
    }

    [UnityTest]
    public IEnumerator WhenVegetableIsChopped_Interact_WithEmptyHand_PutsItInHand()
    {
        ChoppingTable table = Spawn("ChoppingTable").GetComponent<ChoppingTable>();
        PlayerHand hand = Spawn("Player").GetComponentInChildren<PlayerHand>();
        hand.TryPickUp(Spawn("RawVegetable").GetComponent<RawVegetable>());
        table.Interact(hand);

        yield return WaitForChop;

        table.Interact(hand);

        Assert.IsInstanceOf<ChoppedVegetable>(hand.Held);
    }
}
