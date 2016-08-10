namespace EnergyTrading.Contracts.Errors
{
    public class HandlerResponseDetails
    {
        public ErrorMessage Message { get; set; }
        public ErrorHandler Handler { get; set; }
    }
}