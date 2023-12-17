namespace Shipfinity.Shared.Exceptions
{
    public class SellerNotFoundException : Exception
    {
        public SellerNotFoundException(string id) :base($"Seller with id: {id} not found")
        {}
    }
}
