using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.TestTools;

public class TrashTests
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
    public IEnumerator WhenHoldingIngredient_Interact_CollectsIngredient()
    {
        Trash trash = Spawn("Trash").GetComponent<Trash>();
        PlayerHand hand = Spawn("Player").GetComponentInChildren<PlayerHand>();
        hand.TryPickUp(Spawn("RawVegetable").GetComponent<RawVegetable>());

        trash.Interact(hand);

        yield return null;

        Assert.IsNull(hand.Held);
    }
}
