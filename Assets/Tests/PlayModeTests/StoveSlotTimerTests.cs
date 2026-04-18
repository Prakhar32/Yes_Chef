using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.TestTools;

public class StoveSlotTimerTests
{
    private readonly List<GameObject> _created = new List<GameObject>();
    private static readonly WaitForSeconds WaitForCook = new WaitForSeconds(6.1f);

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
    public IEnumerator AfterCookDuration_CookedMeatSpawnsAtSlotPosition()
    {
        PlayerHand hand = Spawn("Player").GetComponentInChildren<PlayerHand>();
        StoveSlot slot = Spawn("Stove").GetComponentInChildren<StoveSlot>();
        yield return null;

        slot.transform.position = new Vector3(1f, 0f, 2f);
        hand.TryPickUp(Spawn("RawMeat").GetComponent<RawMeat>());
        slot.Interact();

        yield return WaitForCook;

        CookedMeat cooked = Object.FindFirstObjectByType<CookedMeat>();
        _created.Add(cooked.gameObject);
        Assert.AreEqual(new Vector3(1f, 0f, 2f), cooked.transform.position);
    }

    [UnityTest]
    public IEnumerator WhenMeatIsCooked_Interact_WithEmptyHand_PutsItInHand()
    {
        PlayerHand hand = Spawn("Player").GetComponentInChildren<PlayerHand>();
        StoveSlot slot = Spawn("Stove").GetComponentInChildren<StoveSlot>();
        yield return null;

        hand.TryPickUp(Spawn("RawMeat").GetComponent<RawMeat>());
        slot.Interact();

        yield return WaitForCook;

        slot.Interact();

        Assert.IsInstanceOf<CookedMeat>(hand.Held);
    }
}
