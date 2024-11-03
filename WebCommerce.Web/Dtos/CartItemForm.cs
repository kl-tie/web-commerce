namespace WebCommerce.Web.Web.Dtos;

public record CartItemForm
{
    public Guid UserAccountId { get; set; }

    public Guid ProductId { get; set; }

    public int Qty { get; set; }
}
