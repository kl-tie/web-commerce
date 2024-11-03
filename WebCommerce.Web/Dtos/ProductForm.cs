namespace WebCommerce.Web.Web.Dtos;

public record ProductForm
{
    public string Name { get; set; } = string.Empty;

    public decimal Price { get; set; }

    public int Qty { get; set; }
}
