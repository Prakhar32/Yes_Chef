using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using YesChef.Domain;

public class OrderTests
{
    private readonly List<GameObject> _created = new List<GameObject>();

    private T Make<T>() where T : MonoBehaviour
    {
        var go = new GameObject();
        _created.Add(go);
        return go.AddComponent<T>();
    }

    [TearDown]
    public void TearDown()
    {
        foreach (var go in _created)
            Object.DestroyImmediate(go);
        _created.Clear();
    }

    [Test]
    public void DeliveredIngredient_SatisfiesMatchingRequirement()
    {
        Order order = new Order(new[] { typeof(ChoppedVegetable), typeof(RawCheese) });
        Order updated = order.Receive(Make<ChoppedVegetable>());
        Assert.AreEqual(1, updated.Pending.Count);
    }

    [Test]
    public void UnpreparedIngredient_DoesNotSatisfyRequirement()
    {
        Order order = new Order(new[] { typeof(ChoppedVegetable) });
        Order updated = order.Receive(Make<RawVegetable>());
        Assert.AreEqual(1, updated.Pending.Count);
    }

    [Test]
    public void IngredientNotOnOrder_DoesNotAffectRemainingRequirements()
    {
        Order order = new Order(new[] { typeof(RawCheese) });
        Order updated = order.Receive(Make<CookedMeat>());
        Assert.AreEqual(1, updated.Pending.Count);
    }

    [Test]
    public void DuplicateRequirements_EachNeedSeparateDelivery()
    {
        Order order = new Order(new[] { typeof(RawCheese), typeof(RawCheese) });
        Order afterFirst = order.Receive(Make<RawCheese>());
        Assert.AreNotEqual(0, afterFirst.Pending.Count);
        Order afterSecond = afterFirst.Receive(Make<RawCheese>());
        Assert.AreEqual(0, afterSecond.Pending.Count);
    }

    [Test]
    public void Score_AccumulatesIngredientScoreValues_OnEachReceive()
    {
        Order order = new Order(new[] { typeof(ChoppedVegetable), typeof(RawCheese) });
        Order afterFirst = order.Receive(Make<ChoppedVegetable>());
        Order afterSecond = afterFirst.Receive(Make<RawCheese>());
        Assert.AreEqual(30, afterSecond.Score);
    }

    [Test]
    public void IsComplete_WhenAllRequirementsFulfilled()
    {
        Order order = new Order(new[] { typeof(RawCheese) });
        Order completed = order.Receive(Make<RawCheese>());
        Assert.IsTrue(completed.IsComplete);
    }
}
