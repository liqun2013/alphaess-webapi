namespace AlphaEssWeb.Api_V2.Domain
{
	using System.Data.Entity;

	public partial class AlphaComplaintsProcessingDbContext : DbContext
	{
		public AlphaComplaintsProcessingDbContext() : base("name=AlphaComplaintsProcessingContext")
		{
			Configuration.LazyLoadingEnabled = false;
			Configuration.ProxyCreationEnabled = false;
		}

		public virtual DbSet<Complaints> Complaints { get; set; }
		public virtual DbSet<ComplaintsProcessing> ComplaintsProcessing { get; set; }
		public virtual DbSet<CustomerReviews> CustomerReviews { get; set; }
		//public virtual DbSet<ProductSuggestion> ProductSuggestion { get; set; }
		public virtual DbSet<ComplaintsComment> ComplaintsComment { get; set; }
		public virtual DbSet<CustomerInfo> CustomerInfo { get; set; }
		public virtual DbSet<ComplaintsProcessingAdditionalInfo> ComplaintsProcessingAdditionalInfo { get; set; }

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			modelBuilder.Entity<Complaints>().HasMany(e => e.ComplaintsProcessing).WithRequired(e => e.Complaints).HasForeignKey(e => e.ComplaintId).WillCascadeOnDelete(false);

			modelBuilder.Entity<Complaints>().HasMany(e => e.CustomerReviews).WithRequired(e => e.Complaints).HasForeignKey(e => e.ComplaintId).WillCascadeOnDelete(false);

			modelBuilder.Entity<Complaints>().HasMany(e => e.ComplaintsComment).WithRequired(e => e.Complaints).HasForeignKey(e => e.ComplaintId).WillCascadeOnDelete(false);
		}
	}
}
