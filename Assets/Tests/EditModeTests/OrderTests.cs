using NUnit.Framework;
using YesChef.Domain;

public class OrderTests
{
    [Test]
    public void DeliveredIngredient_SatisfiesMatchingRequirement()
    {
        Order order = new Order(new IIngredient[] { new ChoppedVegetable(), new RawCheese() });
        Order updated = order.Receive(new ChoppedVegetable());
        Assert.AreEqual(1, updated.Requirements.Count);
    }

    [Test]
    public void UnpreparedIngredient_DoesNotSatisfyRequirement()
    {
        Order order = new Order(new IIngredient[] { new ChoppedVegetable() });
        Order updated = order.Receive(new RawVegetable());
        Assert.AreEqual(1, updated.Requirements.Count);
    }

    [Test]
    public void IngredientNotOnOrder_DoesNotAffectRemainingRequirements()
    {
        Order order = new Order(new IIngredient[] { new RawCheese() });
        Order updated = order.Receive(new CookedMeat());
        Assert.AreEqual(1, updated.Requirements.Count);
    }

    [Test]
    public void DuplicateRequirements_EachNeedSeparateDelivery()
    {
        Order order = new Order(new IIngredient[] { new RawCheese(), new RawCheese() });
        Order afterFirst = order.Receive(new RawCheese());
        Assert.AreNotEqual(0, afterFirst.Requirements.Count);
        Order afterSecond = afterFirst.Receive(new RawCheese());
        Assert.AreEqual(0, afterSecond.Requirements.Count);
    }

    [Test]
    public void OrderScore_IsIngredientValueMinusElapsedSeconds_AndCanBeNegative()
    {
        // 20 + 10 = 30
        Order order = new Order(new IIngredient[] { new ChoppedVegetable(), new RawCheese() });
        Assert.AreEqual(25, order.CalculateScore(5.9f));  // 30 - floor(5.9) = 25
        Assert.AreEqual(-70, order.CalculateScore(100f)); // 30 - 100 = -70
    }
}
