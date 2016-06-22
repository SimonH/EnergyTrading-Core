namespace EnergyTrading.Data.EntityFramework
{
    using System.Data.Entity;

    /// <summary>
    /// Database initialize that does nothing
    /// </summary>
    /// <remarks>
    /// Don't want EF to try and 'fix' my production database unless I want to
    /// </remarks>
    /// <typeparam name="TContext"></typeparam>
    public class NullDatabaseInitializer<TContext> : IDatabaseInitializer<TContext>
        where TContext : DbContext 
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public void InitializeDatabase(TContext context)
        {
        }
    }
}