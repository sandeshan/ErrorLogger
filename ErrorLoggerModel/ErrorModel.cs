namespace ErrorLoggerModel
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class ErrorModel : DbContext
    {
        public ErrorModel()
            : base("ErrorModel")
        {
            Database.SetInitializer<ErrorModel>(new ErrorLogsDBInitializer());
        }

        #region Properties used to build the DB

        public DbSet<logType> LogTypes { get; set; }
        public DbSet<ErrorLog> ErrorLogs { get; set; }
        public DbSet<DebugLevel> DebugLevels { get; set; }
        public DbSet<Application> Applications { get; set; }
        public DbSet<UserType> UserTypes { get; set; }
        public DbSet<User> Users { get; set; }

        #endregion

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
