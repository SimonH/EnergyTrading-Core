namespace EnergyTrading.serialization
{
    public interface ITextSerializer
    {
        TReturnType Deserialize<TReturnType>(string sourceText, string version, string mapKey);
        string Serialize<T>(T source, string version, string key);
    }
}