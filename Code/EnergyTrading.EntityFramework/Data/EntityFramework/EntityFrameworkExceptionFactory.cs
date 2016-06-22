namespace EnergyTrading.Data.EntityFramework
{
    using EnergyTrading.Data.Sql;
    using EnergyTrading.Exceptions;

    /// <summary>
    /// 
    /// </summary>
    public class EntityFrameworkExceptionFactory : ExceptionTranslator
    {
        /// <summary>
        /// 
        /// </summary>
        public EntityFrameworkExceptionFactory()
        {
            this.AddFactory(new SqlExceptionFactory());
        }
    }
}