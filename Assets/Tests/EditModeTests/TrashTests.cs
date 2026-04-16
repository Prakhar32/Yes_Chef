using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

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
            Object.DestroyImmediate(go);
        _created.Clear();
    }

    [Test]
    public void WhenHoldingIngredient_Interact_CollectsIngredient()
    {
        Trash trash = Make<Trash>();
        PlayerHand hand = new PlayerHand();
        hand.TryPickUp(Make<RawVegetable>());

        trash.Interact(hand);

        Assert.IsTrue(hand.Held == null);
    }
}
