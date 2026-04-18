using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.TestTools;

public class RefrigeratorTests
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
    public IEnumerator Interact_WhileHoldingIngredient_DoesNotOpenMenu()
    {
        PlayerHand hand = Spawn("Player").GetComponentInChildren<PlayerHand>();
        Refrigerator fridge = Spawn("Fridge").GetComponent<Refrigerator>();
        yield return null;

        bool menuOpened = false;
        fridge.refrigiratorOpen += () => menuOpened = true;

        hand.TryPickUp(Spawn("RawVegetable").GetComponent<RawVegetable>());
        fridge.Interact();

        Assert.IsFalse(menuOpened);
    }
}
