namespace AlphaEssWeb.Api_V2.Domain
{
	using System.Data.Entity;

	public class AlphaEssDb_LogDbContext : DbContext
	{
		public AlphaEssDb_LogDbContext() : base("name=AlphaEssDb_LogDbContext")
		{
		}

		public virtual DbSet<SYS_LOG> SysLog { get; set; }
		public virtual DbSet<UserLog> UserLog { get; set; }
		
		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{

		}
	}
}
