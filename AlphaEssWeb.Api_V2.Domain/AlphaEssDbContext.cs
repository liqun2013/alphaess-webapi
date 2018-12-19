namespace AlphaEssWeb.Api_V2.Domain
{
	using System.Data.Entity;

	public class AlphaEssDbContext : DbContext
	{
		public AlphaEssDbContext() : base("name=AlphaEssDbContext")
		{
		}

		public virtual DbSet<APP_Version> APP_Version { get; set; }
		public virtual DbSet<BASE_COUNTRY> BASE_COUNTRY { get; set; }
		public virtual DbSet<BASE_ERRCODE> BASE_ERRCODE { get; set; }
		public virtual DbSet<SYS_API> SYS_API { get; set; }
		public virtual DbSet<SYS_APPVERSION> SYS_APPVERSION { get; set; }
		public virtual DbSet<SYS_APPVERSIONDETAIL> SYS_APPVERSIONDETAIL { get; set; }
		public virtual DbSet<SYS_APPVERSIONREC> SYS_APPVERSIONREC { get; set; }
		public virtual DbSet<Sys_Company> Sys_Company { get; set; }
		public virtual DbSet<SYS_CONFIG> SYS_CONFIG { get; set; }
		public virtual DbSet<SYS_EMAILRULE> SYS_EMAILRULE { get; set; }
		public virtual DbSet<SYS_EMAILRULEUSER> SYS_EMAILRULEUSER { get; set; }
		public virtual DbSet<Sys_Language> Sys_Language { get; set; }
		public virtual DbSet<SYS_LICENSE> SYS_LICENSE { get; set; }
		public virtual DbSet<SYS_LOG> SYS_LOG { get; set; }
		public virtual DbSet<SYS_MENU> SYS_MENU { get; set; }
		public virtual DbSet<SYS_MSG> SYS_MSG { get; set; }
		public virtual DbSet<SYS_MSGUSER> SYS_MSGUSER { get; set; }
		public virtual DbSet<SYS_PRODATTACH> SYS_PRODATTACH { get; set; }
		public virtual DbSet<SYS_PRODLEAVEMSG> SYS_PRODLEAVEMSG { get; set; }
		public virtual DbSet<SYS_PRODUCT> SYS_PRODUCT { get; set; }
		public virtual DbSet<SYS_PTYPE> SYS_PTYPE { get; set; }
		public virtual DbSet<SYS_PTYPEDETAIL> SYS_PTYPEDETAIL { get; set; }
		public virtual DbSet<Sys_PurchasePrice> Sys_PurchasePrice { get; set; }
		public virtual DbSet<SYS_ROLE> SYS_ROLE { get; set; }
		public virtual DbSet<SYS_ROLEMENU> SYS_ROLEMENU { get; set; }
		public virtual DbSet<SYS_ROLEUSER> SYS_ROLEUSER { get; set; }
		public virtual DbSet<Sys_SellPrice> Sys_SellPrice { get; set; }
		public virtual DbSet<SYS_SN> SYS_SN { get; set; }
		public virtual DbSet<SYS_USER> SYS_USER { get; set; }
		public virtual DbSet<SYS_USERAGREEMENT> SYS_USERAGREEMENT { get; set; }
		public virtual DbSet<SYS_USERSERVICEAREA> SYS_USERSERVICEAREA { get; set; }
		public virtual DbSet<VT_CMD> VT_CMD { get; set; }
		public virtual DbSet<VT_COLDATA> VT_COLDATA { get; set; }
		public virtual DbSet<VT_SYSFAULT> VT_SYSFAULT { get; set; }
		public virtual DbSet<VT_SYSINSTALL> VT_SYSINSTALL { get; set; }
		public virtual DbSet<VT_SYSINSTALLATTACH> VT_SYSINSTALLATTACH { get; set; }
		public virtual DbSet<VT_SYSMAINTAIN> VT_SYSMAINTAIN { get; set; }
		public virtual DbSet<VT_SYSTEM> VT_SYSTEM { get; set; }
		public virtual DbSet<Sys_ResellerLicense> Sys_ResellerLicense { get; set; }
		public virtual DbSet<Sys_ServicePartnerSn> Sys_ServicePartnerSn { get; set; }
		public virtual DbSet<Report_Energy> Report_Energy { get; set; }
		public virtual DbSet<Sys_RemoteDispatch> Sys_RemoteDispatch { get; set; }
		public virtual DbSet<Report_Power> Report_Power { get; set; }
		public virtual DbSet<Report_Income> Report_Income { get; set; }
		public virtual DbSet<VUserAndRoles> VUserAndRoles { get; set; }
		public virtual DbSet<SysWeatherForecast> SysWeatherForecast { get; set; }
		public virtual DbSet<SYS_Country_Region> SYS_Country_Region { get; set; }
		public virtual DbSet<CompanyContactDetail> CompanyContactDetail { get; set; }

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			modelBuilder.Entity<SYS_MENU>()
					.HasMany(e => e.SYS_MENU1)
					.WithOptional(e => e.SYS_MENU2)
					.HasForeignKey(e => e.PARENTID);

			modelBuilder.Entity<SYS_MENU>()
					.HasMany(e => e.SYS_ROLEMENU)
					.WithOptional(e => e.SYS_MENU)
					.HasForeignKey(e => e.MENUID);

			modelBuilder.Entity<Sys_PurchasePrice>()
					.Property(e => e.PurchasePriceMaxPrice)
					.HasPrecision(18, 6);

			modelBuilder.Entity<Sys_PurchasePrice>()
					.Property(e => e.PurchasePriceMinPrice)
					.HasPrecision(18, 6);

			modelBuilder.Entity<Sys_PurchasePrice>()
					.Property(e => e.PurchasePrice)
					.HasPrecision(18, 6);

			//modelBuilder.Entity<Sys_PurchasePrice>().Property(e => e.PurchasePriceDate).HasColumnType("date");

			modelBuilder.Entity<SYS_ROLE>()
					.HasMany(e => e.SYS_ROLEMENU)
					.WithOptional(e => e.SYS_ROLE)
					.HasForeignKey(e => e.ROLEID);

			modelBuilder.Entity<SYS_ROLE>()
					.HasMany(e => e.SYS_ROLEUSER)
					.WithOptional(e => e.SYS_ROLE)
					.HasForeignKey(e => e.ROLEID);

			modelBuilder.Entity<Sys_SellPrice>()
					.Property(e => e.SellPricePrice)
					.HasPrecision(18, 6);

			modelBuilder.Entity<SYS_USER>()
					.HasMany(e => e.SYS_MSG)
					.WithOptional(e => e.SYS_USER)
					.HasForeignKey(e => e.CREATE_ACCOUNT)
					.WillCascadeOnDelete();

			modelBuilder.Entity<SYS_USER>()
					.HasMany(e => e.SYS_MSGUSER)
					.WithOptional(e => e.SYS_USER)
					.HasForeignKey(e => e.SENDUSER_ID)
					.WillCascadeOnDelete();

			modelBuilder.Entity<SYS_USER>()
					.HasMany(e => e.SYS_MSGUSER1)
					.WithOptional(e => e.SYS_USER1)
					.HasForeignKey(e => e.USER_ID);

			modelBuilder.Entity<SYS_MSG>().HasMany(e => e.SYS_MSGUSER).WithRequired(x => x.SYS_MSG).HasForeignKey(e => e.MSG_ID);

			modelBuilder.Entity<SYS_USER>()
					.HasMany(e => e.SYS_ROLEUSER)
					.WithOptional(e => e.SYS_USER)
					.HasForeignKey(e => e.USERID);

			modelBuilder.Entity<SYS_USER>()
					.HasMany(e => e.VT_SYSTEM)
					.WithOptional(e => e.SYS_USER)
					.HasForeignKey(e=>e.UserId);

			modelBuilder.Entity<VT_COLDATA>()
					.Property(e => e.Ppv1)
					.HasPrecision(11, 2);

			modelBuilder.Entity<VT_COLDATA>()
					.Property(e => e.Ppv2)
					.HasPrecision(11, 2);

			modelBuilder.Entity<VT_COLDATA>()
					.Property(e => e.Upv1)
					.HasPrecision(11, 1);

			modelBuilder.Entity<VT_COLDATA>()
					.Property(e => e.Upv2)
					.HasPrecision(11, 1);

			modelBuilder.Entity<VT_COLDATA>()
					.Property(e => e.Ua)
					.HasPrecision(11, 1);

			modelBuilder.Entity<VT_COLDATA>()
					.Property(e => e.Ub)
					.HasPrecision(11, 1);

			modelBuilder.Entity<VT_COLDATA>()
					.Property(e => e.Uc)
					.HasPrecision(11, 1);

			modelBuilder.Entity<VT_COLDATA>()
					.Property(e => e.Fac)
					.HasPrecision(11, 2);

			modelBuilder.Entity<VT_COLDATA>()
					.Property(e => e.Ubus)
					.HasPrecision(11, 1);

			modelBuilder.Entity<VT_COLDATA>()
					.Property(e => e.PrealL1)
					.HasPrecision(11, 1);

			modelBuilder.Entity<VT_COLDATA>()
					.Property(e => e.PrealL2)
					.HasPrecision(11, 1);

			modelBuilder.Entity<VT_COLDATA>()
					.Property(e => e.PrealL3)
					.HasPrecision(11, 1);

			modelBuilder.Entity<VT_COLDATA>()
					.Property(e => e.Tinv)
					.HasPrecision(11, 1);

			modelBuilder.Entity<VT_COLDATA>()
					.Property(e => e.PacL1)
					.HasPrecision(11, 1);

			modelBuilder.Entity<VT_COLDATA>()
					.Property(e => e.PacL2)
					.HasPrecision(11, 1);

			modelBuilder.Entity<VT_COLDATA>()
					.Property(e => e.PacL3)
					.HasPrecision(11, 1);

			modelBuilder.Entity<VT_COLDATA>()
					.Property(e => e.EpvTotal)
					.HasPrecision(11, 2);

			modelBuilder.Entity<VT_COLDATA>()
					.Property(e => e.Einput)
					.HasPrecision(11, 2);

			modelBuilder.Entity<VT_COLDATA>()
					.Property(e => e.Eoutput)
					.HasPrecision(11, 2);

			modelBuilder.Entity<VT_COLDATA>()
					.Property(e => e.Echarge)
					.HasPrecision(11, 2);

			modelBuilder.Entity<VT_COLDATA>()
					.Property(e => e.PmeterL1)
					.HasPrecision(11, 2);

			modelBuilder.Entity<VT_COLDATA>()
					.Property(e => e.PmeterL2)
					.HasPrecision(11, 2);

			modelBuilder.Entity<VT_COLDATA>()
					.Property(e => e.PmeterL3)
					.HasPrecision(11, 2);

			modelBuilder.Entity<VT_COLDATA>()
					.Property(e => e.PmeterDc)
					.HasPrecision(11, 1);

			modelBuilder.Entity<VT_COLDATA>()
					.Property(e => e.Pbat)
					.HasPrecision(11, 1);

			modelBuilder.Entity<VT_COLDATA>()
					.Property(e => e.Soc)
					.HasPrecision(11, 1);

			modelBuilder.Entity<VT_COLDATA>()
					.Property(e => e.Batv)
					.HasPrecision(11, 1);

			modelBuilder.Entity<VT_COLDATA>()
					.Property(e => e.Batc)
					.HasPrecision(11, 1);

			modelBuilder.Entity<VT_COLDATA>()
					.Property(e => e.BatC1)
					.HasPrecision(11, 1);

			modelBuilder.Entity<VT_COLDATA>()
					.Property(e => e.BatC2)
					.HasPrecision(11, 1);

			modelBuilder.Entity<VT_COLDATA>()
					.Property(e => e.BatC3)
					.HasPrecision(11, 1);

			modelBuilder.Entity<VT_COLDATA>()
					.Property(e => e.BatC4)
					.HasPrecision(11, 1);

			modelBuilder.Entity<VT_COLDATA>()
					.Property(e => e.BatC5)
					.HasPrecision(11, 1);

			modelBuilder.Entity<VT_COLDATA>()
					.Property(e => e.BatC6)
					.HasPrecision(11, 1);

			modelBuilder.Entity<VT_COLDATA>()
					.Property(e => e.Soc1)
					.HasPrecision(11, 1);

			modelBuilder.Entity<VT_COLDATA>()
					.Property(e => e.Soc2)
					.HasPrecision(11, 1);

			modelBuilder.Entity<VT_COLDATA>()
					.Property(e => e.Soc3)
					.HasPrecision(11, 1);

			modelBuilder.Entity<VT_COLDATA>()
					.Property(e => e.Soc4)
					.HasPrecision(11, 1);

			modelBuilder.Entity<VT_COLDATA>()
					.Property(e => e.Soc5)
					.HasPrecision(11, 1);

			modelBuilder.Entity<VT_COLDATA>()
					.Property(e => e.Soc6)
					.HasPrecision(11, 1);

			modelBuilder.Entity<VT_SYSTEM>()
					.Property(e => e.Sellprice)
					.HasPrecision(18, 6);

			modelBuilder.Entity<VT_SYSTEM>()
					.Property(e => e.Saleprice0)
					.HasPrecision(18, 6);

			modelBuilder.Entity<VT_SYSTEM>()
					.Property(e => e.Saleprice1)
					.HasPrecision(18, 6);

			modelBuilder.Entity<VT_SYSTEM>()
					.Property(e => e.Saleprice2)
					.HasPrecision(18, 6);

			modelBuilder.Entity<VT_SYSTEM>()
					.Property(e => e.Saleprice3)
					.HasPrecision(18, 6);

			modelBuilder.Entity<VT_SYSTEM>()
					.Property(e => e.Saleprice4)
					.HasPrecision(18, 6);

			modelBuilder.Entity<VT_SYSTEM>()
					.Property(e => e.Saleprice5)
					.HasPrecision(18, 6);

			modelBuilder.Entity<VT_SYSTEM>()
					.Property(e => e.Saleprice6)
					.HasPrecision(18, 6);

			modelBuilder.Entity<VT_SYSTEM>()
					.Property(e => e.Saleprice7)
					.HasPrecision(18, 6);
		}
	}
}
