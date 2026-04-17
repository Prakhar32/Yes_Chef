using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class TrashTests
{
    private readonly List<GameObject> _created = new List<GameObject>();

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

    [UnityTest]
    public IEnumerator WhenHoldingIngredient_Interact_CollectsIngredient()
    {
        Trash trash = Make<Trash>();
        PlayerHand hand = new PlayerHand();
        hand.TryPickUp(Make<RawVegetable>());

        trash.Interact(hand);

        yield return null;

        Assert.IsNull(hand.Held);
    }
}
