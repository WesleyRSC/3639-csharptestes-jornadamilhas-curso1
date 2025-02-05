using JornadaMilhasV1.Modelos;

namespace JornadaMilhas.Test
{
    public class OfertaViagemConstructor
    {
        [Theory]
        [InlineData("", null, "2024-01-01", "2024-01-02", 0, false)]
        [InlineData("OrigemTeste", "DestinoTeste", "2024-02-01", "2024-02-05", 100, true)]
        [InlineData("Vitoria", "Sao paulo", "2024-01-01", "2024-01-02", 0, false)]
        [InlineData("Rio de Janeiro", "Cuiaba", "2024-02-01", "2024-02-05", -100, false)]
        public void ReturnEhValidAccordingwithInputParams(string origin, string destiny, string departureDate, string returnDate, double price, bool expectedResult)
        {
            //Arrange
            Rota rota = new Rota(origin, destiny);
            Periodo peridodo = new Periodo(DateTime.Parse(departureDate), DateTime.Parse(returnDate));

            //Act
            OfertaViagem oferta = new OfertaViagem(rota, peridodo, price);

            //Assert
            Assert.Equal(expectedResult, oferta.EhValido);
        }

        [Fact]
        public void ReturnErrorMessageWhenRouteIsNull()
        {
            Rota rota = null;
            Periodo peridodo = new Periodo(new DateTime(2024, 2, 1), new DateTime(2024, 2, 5));
            double preco = 100.0;

            OfertaViagem oferta = new OfertaViagem(rota, peridodo, preco);

            Assert.Contains("A oferta de viagem não possui rota ou período válidos.", oferta.Erros.Sumario);
            Assert.False(oferta.EhValido);
        }

        [Fact]
        public void ReturnErrorMessageWhenTimeIsInvalid()
        {
            //Arrange
            Rota rota = null;
            Periodo peridodo = new Periodo(new DateTime(2024, 2, 5), new DateTime(2024, 2, 1));
            double preco = 100.0;

            //Act
            OfertaViagem oferta = new OfertaViagem(rota, peridodo, preco);

            //Assert
            Assert.Contains("Erro: Data de ida não pode ser maior que a data de volta.", oferta.Erros.Sumario);
            Assert.False(oferta.EhValido);
        }

        [Fact]
        public void ReturnErrorMessageWhenPriceIsNegative()
        {
            //Arrange
            Rota rota = new Rota("OrigemTeste", "DestinoTeste");
            Periodo peridodo = new Periodo(new DateTime(2024, 2, 1), new DateTime(2024, 2, 5));
            double preco = -100.0;

            //Act
            OfertaViagem oferta = new OfertaViagem(rota, peridodo, preco);

            //Assert
            Assert.Contains("O preço da oferta de viagem deve ser maior que zero.", oferta.Erros.Sumario);
            Assert.False(oferta.EhValido);
        }

        [Fact]
        public void ReturnThreeErrorMessagesWhenRoutePeriodAndPriceAreInvalid()
        {
            //Arrange 
            Rota rota = null;
            Periodo peridodo = new Periodo(new DateTime(2024, 2, 1), new DateTime(2024, 1, 5));
            double preco = -100.0;

            int expectedQuantity = 3;

            //Act
            OfertaViagem oferta = new OfertaViagem(rota, peridodo, preco);

            //Assert
            Assert.Equal(expectedQuantity, oferta.Erros.Count());
        }
    }
}
