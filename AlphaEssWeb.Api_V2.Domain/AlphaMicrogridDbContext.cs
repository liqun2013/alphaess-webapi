namespace AlphaEssWeb.Api_V2.Domain
{
	using System.Data.Entity;

	public partial class AlphaMicrogridDbContext : DbContext
	{
		public AlphaMicrogridDbContext() : base("name=AlphaMicrogridDbContext")
		{
		}

		public virtual DbSet<ElectricDispatchingControlRecord> ElectricDispatchingControlRecord { get; set; }
		public virtual DbSet<EMSNode> EMSNode { get; set; }
		public virtual DbSet<MeterRecord> MeterRecord { get; set; }
		public virtual DbSet<MicrogridAndSN> MicrogridAndSN { get; set; }
		public virtual DbSet<MicrogridInfo> MicrogridInfo { get; set; }
		public virtual DbSet<MicrogridSummary> MicrogridSummary { get; set; }
		public virtual DbSet<SchedulingStrategy> SchedulingStrategy { get; set; }

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			modelBuilder.Entity<ElectricDispatchingControlRecord>()
					.Property(e => e.ControlPower)
					.HasPrecision(18, 4);

			modelBuilder.Entity<EMSNode>()
					.Property(e => e.Ppv)
					.HasPrecision(18, 4);

			modelBuilder.Entity<EMSNode>()
					.Property(e => e.PrealL1)
					.HasPrecision(18, 4);

			modelBuilder.Entity<EMSNode>()
					.Property(e => e.PrealL2)
					.HasPrecision(18, 4);

			modelBuilder.Entity<EMSNode>()
					.Property(e => e.PrealL3)
					.HasPrecision(18, 4);

			modelBuilder.Entity<EMSNode>()
					.Property(e => e.Pbat)
					.HasPrecision(18, 4);

			modelBuilder.Entity<EMSNode>()
					.Property(e => e.SOC)
					.HasPrecision(18, 4);

			modelBuilder.Entity<EMSNode>()
					.Property(e => e.Pcharge)
					.HasPrecision(18, 4);

			modelBuilder.Entity<EMSNode>()
					.Property(e => e.Pdischarge)
					.HasPrecision(18, 4);

			modelBuilder.Entity<MeterRecord>()
					.Property(e => e.PMeterL1)
					.HasPrecision(18, 4);

			modelBuilder.Entity<MeterRecord>()
					.Property(e => e.PMeterL2)
					.HasPrecision(18, 4);

			modelBuilder.Entity<MeterRecord>()
					.Property(e => e.PMeterL3)
					.HasPrecision(18, 4);

			modelBuilder.Entity<MeterRecord>()
					.Property(e => e.Einput)
					.HasPrecision(18, 4);

			modelBuilder.Entity<MeterRecord>()
					.Property(e => e.Eoutput)
					.HasPrecision(18, 4);

			modelBuilder.Entity<MicrogridInfo>()
					.Property(e => e.MaxGridPower)
					.HasPrecision(18, 4);

			modelBuilder.Entity<MicrogridInfo>()
					.Property(e => e.MaxPEPower)
					.HasPrecision(18, 4);

			modelBuilder.Entity<MicrogridInfo>()
					.Property(e => e.PVPower)
					.HasPrecision(18, 4);

			modelBuilder.Entity<MicrogridInfo>()
					.Property(e => e.WPInstalledPower)
					.HasPrecision(18, 4);

			modelBuilder.Entity<MicrogridInfo>()
					.Property(e => e.DEInstalledPower)
					.HasPrecision(18, 4);

			modelBuilder.Entity<MicrogridInfo>()
					.Property(e => e.OPGInstalledPower)
					.HasPrecision(18, 4);

			modelBuilder.Entity<MicrogridInfo>()
					.Property(e => e.SystemTotalPower)
					.HasPrecision(18, 4);

			modelBuilder.Entity<MicrogridInfo>()
					.Property(e => e.TotalInstalledCapacity)
					.HasPrecision(18, 4);

			modelBuilder.Entity<MicrogridInfo>()
					.Property(e => e.SystemTimeSpan)
					.HasPrecision(18, 0);

			modelBuilder.Entity<MicrogridSummary>()
					.Property(e => e.BuyingPower)
					.HasPrecision(18, 4);

			modelBuilder.Entity<MicrogridSummary>()
					.Property(e => e.SellingPower)
					.HasPrecision(18, 4);

			modelBuilder.Entity<MicrogridSummary>()
					.Property(e => e.LoadPower)
					.HasPrecision(18, 4);

			modelBuilder.Entity<MicrogridSummary>()
					.Property(e => e.GeneratedOutput)
					.HasPrecision(18, 4);

			modelBuilder.Entity<MicrogridSummary>()
					.Property(e => e.RemainingCapacity)
					.HasPrecision(18, 4);

			modelBuilder.Entity<MicrogridSummary>()
					.Property(e => e.SOC)
					.HasPrecision(18, 4);

			modelBuilder.Entity<MicrogridSummary>()
					.Property(e => e.Pdischarge)
					.HasPrecision(18, 4);

			modelBuilder.Entity<MicrogridSummary>()
					.Property(e => e.Pcharge)
					.HasPrecision(18, 4);

			modelBuilder.Entity<MicrogridSummary>()
					.Property(e => e.BuyElectricPower)
					.HasPrecision(18, 4);

			modelBuilder.Entity<MicrogridSummary>()
					.Property(e => e.SellElectricPower)
					.HasPrecision(18, 4);

			modelBuilder.Entity<MicrogridSummary>()
					.Property(e => e.PowerGeneration)
					.HasPrecision(18, 4);

			modelBuilder.Entity<MicrogridSummary>()
					.Property(e => e.ConsumptionPower)
					.HasPrecision(18, 4);

			modelBuilder.Entity<SchedulingStrategy>()
					.Property(e => e.PGridMax)
					.HasPrecision(18, 4);

			modelBuilder.Entity<SchedulingStrategy>()
					.Property(e => e.POutputMax)
					.HasPrecision(18, 4);

			modelBuilder.Entity<SchedulingStrategy>()
					.Property(e => e.DieselPOutputMax)
					.HasPrecision(18, 4);

			modelBuilder.Entity<SchedulingStrategy>()
					.Property(e => e.DieselStopPower)
					.HasPrecision(18, 4);

			modelBuilder.Entity<SchedulingStrategy>()
					.Property(e => e.Power1)
					.HasPrecision(18, 4);

			modelBuilder.Entity<SchedulingStrategy>()
					.Property(e => e.Power2)
					.HasPrecision(18, 4);

			modelBuilder.Entity<SchedulingStrategy>()
					.Property(e => e.Power3)
					.HasPrecision(18, 4);

			modelBuilder.Entity<SchedulingStrategy>()
					.Property(e => e.Power4)
					.HasPrecision(18, 4);

			modelBuilder.Entity<SchedulingStrategy>()
					.Property(e => e.Power5)
					.HasPrecision(18, 4);
		}
	}
}
