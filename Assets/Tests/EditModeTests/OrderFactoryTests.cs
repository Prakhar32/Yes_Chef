using NUnit.Framework;
public class OrderFactoryTests
{
    [Test]
    public void CreateRandom_ReturnsUndeliveredOrder()
    {
        Order order = OrderFactory.CreateRandom();
        Assert.IsFalse(order.IsComplete);
    }
}
