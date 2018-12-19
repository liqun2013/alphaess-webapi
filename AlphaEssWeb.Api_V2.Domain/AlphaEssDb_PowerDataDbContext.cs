namespace AlphaEssWeb.Api_V2.Domain
{
	using System.Data.Entity;

	public class AlphaEssDb_PowerDataDbContext : DbContext
	{
		public AlphaEssDb_PowerDataDbContext() : base("name=AlphaEssDb_PowerDataDbContext")
		{
		}

		public virtual DbSet<Report_Power> Report_Power { get; set; }
		public virtual DbSet<PowerData> PowerData { get; set; }
		
		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			modelBuilder.Entity<PowerData>()
					.Property(e => e.PrealL1)
					.HasPrecision(11, 1);

			modelBuilder.Entity<PowerData>()
					.Property(e => e.PrealL2)
					.HasPrecision(11, 1);

			modelBuilder.Entity<PowerData>()
					.Property(e => e.PrealL3)
					.HasPrecision(11, 1);

		}
	}
}
