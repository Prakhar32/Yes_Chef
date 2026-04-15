using NUnit.Framework;
using YesChef.Domain;

public class OrderFactoryTests
{
    [Test]
    public void CreateRandom_ReturnsUndeliveredOrder()
    {
        Order order = OrderFactory.CreateRandom();
        Assert.AreNotEqual(0, order.Requirements.Count);
    }
}
