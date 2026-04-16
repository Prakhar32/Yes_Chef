using NUnit.Framework;
public class OrderFactoryTests
{
    [Test]
    public void CreateRandom_ReturnsUndeliveredOrder()
    {
        Order order = OrderFactory.CreateRandom();
        Assert.AreNotEqual(0, order.Pending.Count);
    }
}
