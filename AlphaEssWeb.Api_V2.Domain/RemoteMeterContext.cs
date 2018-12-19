namespace AlphaEssWeb.Api_V2.Domain
{
	using System.Data.Entity;

	public class RemoteMeterContext : DbContext
	{
		public RemoteMeterContext() : base("name=RemoteMeterContext")
		{
		}

		public virtual DbSet<DevData> DevData { get; set; }
		public virtual DbSet<DevInfo> DevInfo { get; set; }
		public virtual DbSet<SYS_USER> SysUser { get; set; }

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{

		}
	}
}
