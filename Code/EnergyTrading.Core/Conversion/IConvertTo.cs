namespace EnergyTrading.Conversion
{
    public interface IConvertTo<out T> where T : new()
    {
        T Convert();
    }
}