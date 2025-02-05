using Bogus;
using JornadaMilhasV1.Gerencidor;
using JornadaMilhasV1.Modelos;

namespace JornadaMilhas.Test
{
    public class GerenciadorDeOfertasRecuperaMaiorDesconto
    {

        [Fact]
        public void ReturnNullOfertaWhenListIsEmpty()
        {
            //Arrange
            var ofertas = new List<OfertaViagem>();
            var gerenciador = new GerenciadorDeOfertas(ofertas);

            Func<OfertaViagem, bool> filter = o => o.Rota.Destino.Equals("São Paulo");

            //Act
            var oferta = gerenciador.RecoveryGreaterDiscount(filter);

            //Assert
            Assert.Null(oferta);
        }

        [Fact]
        public void ReturnSpecifcOfertaWhenDestinyIsSaoPauloAndDicountIs40()
        {
            //Arrange
            var fakerPeriodo = new Faker<Periodo>()
                .CustomInstantiator(f =>
                {
                    DateTime initialDate = f.Date.Soon();
                    return new Periodo(initialDate, initialDate.AddDays(30));
                });

            var rota = new Rota("Rio de Janeiro", "São Paulo");

            var fakerOferta = new Faker<OfertaViagem>()
                .CustomInstantiator(f => new OfertaViagem(
                    rota,
                    fakerPeriodo.Generate(),
                    100 * f.Random.Int(1, 100)
                    )
                )
                .RuleFor(o => o.Discount, f => 40)
                .RuleFor(o => o.Active, f => true);

            var selectedOferta = new OfertaViagem(rota, fakerPeriodo.Generate(), 80)
            {
                Discount = 40,
                Active = true
            };

            var inactiveOferta = new OfertaViagem(rota, fakerPeriodo.Generate(), 70)
            {
                Discount = 40,
                Active = false
            };

            var ofertas = fakerOferta.Generate(200);
            ofertas.Add(selectedOferta);
            var gerenciador = new GerenciadorDeOfertas(ofertas);

            Func<OfertaViagem, bool> filter = o => o.Rota.Destino.Equals("São Paulo") && o.Active;

            var expectedPrice = 40;

            //Act
            var oferta = gerenciador.RecoveryGreaterDiscount(filter);

            //Assert
            Assert.NotNull(oferta);
            Assert.Equal(expectedPrice, oferta.Preco, 0.001);
        }
    }
}
