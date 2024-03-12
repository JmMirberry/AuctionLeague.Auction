namespace AuctionLeague.Data
{
    public class PlayerSaleValidationResult
    {

        public static PlayerSaleValidationResult Pass()
        {
            return new PlayerSaleValidationResult(true);
        }

        public static PlayerSaleValidationResult Fail(string error)
        {
            return new PlayerSaleValidationResult(false, error);
        }

        private PlayerSaleValidationResult(bool isValid, string error)
        {
            IsValid = isValid; Error = error;
        }

        private PlayerSaleValidationResult(bool isValid)
        {
            IsValid = isValid;
        }
        public readonly bool IsValid;
        public readonly string Error;
    }

}