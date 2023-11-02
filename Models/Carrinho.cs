namespace PWEBTP.Models
{
    public class Carrinho
    {
        public List<CarrinhoItem> items { get; set; } = new List<CarrinhoItem>();

        public void AddItem(Carro carro, int qtd)
        {
            var item = items.FirstOrDefault(i => i.CarroId == carro.Id);

            if (item != null)
            {
                item.Quantidade += qtd;
                return;
            }

            items.Add(new CarrinhoItem
            {
                CarroId = carro.Id,
                CarroNome = carro.Name,
                PrecoUnit = carro.Preco,
                Quantidade = qtd
            });
        }

        public void RemoveItem(Carro curso) => items.RemoveAll(i => i.CarroId == curso.Id);
        public int TotalItems() => items.Sum(i => i.Quantidade);
        public decimal Total() => items.Sum(i => i.PrecoUnit * i.Quantidade);
        public void Clear() => items.Clear();
    }
}
