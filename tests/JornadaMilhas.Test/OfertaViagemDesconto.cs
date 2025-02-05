using JornadaMilhasV1.Modelos;

namespace JornadaMilhas.Test
{
    public class OfertaViagemDesconto
    {
        [Fact]
        public void ReturnUpdatedPriceWhenHasDiscount()
        {
            //Arrange
            Rota route = new Rota("OrigemA", "DestinoA");
            Periodo travelTime = new Periodo(new DateTime(2024, 05, 01), new DateTime(2024, 05, 06));
            double originalPrice = 100.00;
            double discount = 20.00;

            double priceWithDiscount = originalPrice - discount;
            OfertaViagem oferta = new OfertaViagem(route, travelTime, originalPrice);

            //Act
            oferta.Discount = discount;

            //Assert
            Assert.Equal(priceWithDiscount, oferta.Preco);
        }

        [Theory]
        [InlineData(120.00, 30)]
        [InlineData(100.00, 30)]
        public void ReturnMaxDiscountWhenDiscountIsEqualsOrGreaterThanPrice(double discount, double priceWithDiscount)
        {
            //Arrange
            Rota route = new Rota("OrigemA", "DestinoA");
            Periodo travelTime = new Periodo(new DateTime(2024, 05, 01), new DateTime(2024, 05, 06));
            double originalPrice = 100.00;

            OfertaViagem oferta = new OfertaViagem(route, travelTime, originalPrice);

            //Act
            oferta.Discount = discount;

            //Assert
            Assert.Equal(priceWithDiscount, oferta.Preco, 0.001);
        }

        [Fact]
        public void ReturnOriginalPriceWhenDiscountValueIsNegative()
        {
            //Arrange
            Rota route = new Rota("OrigemA", "DestinoA");
            Periodo travelTime = new Periodo(new DateTime(2024, 05, 01), new DateTime(2024, 05, 06));
            double originalPrice = 100.00;
            double discount = -120.00;
            OfertaViagem oferta = new OfertaViagem(route, travelTime, originalPrice);

            //Act
            oferta.Discount = discount;

            //Assert
            Assert.Equal(originalPrice, oferta.Preco, 0.001);
        }
    }
}
