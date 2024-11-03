namespace WebCommerce.Web.Web.Dtos;

public record CartItemDetail
{
    public Guid UserAccountId { get; set; }

    public string FullName { get; set; } = string.Empty;

    public Guid ProductId { get; set; }

    public string ProductName { get; set; } = string.Empty;

    public decimal Price { get; set; }

    public int Qty { get; set; }

    public decimal SubTotal { get; set; }
}
