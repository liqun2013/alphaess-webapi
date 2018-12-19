using AlphaEss.Api_V2.Infrastructure;
using AlphaEssWeb.Api_V2.Domain;
using AlphaEssWeb.Api_V2.Domain.Services;
using Autofac;
using Autofac.Integration.WebApi;
using System;
using System.Reflection;
using System.Web.Http;

namespace AlphaEssWeb.Api_V2.Config
{
	public class AutofacWebAPI
	{
		public static void Initialize(HttpConfiguration config)
		{
			Initialize(config, RegisterServices(new ContainerBuilder()));
		}
		public static void Initialize(HttpConfiguration config, IContainer container)
		{
			config.DependencyResolver = new AutofacWebApiDependencyResolver(container);
		}
		private static IContainer RegisterServices(ContainerBuilder builder)
		{
			builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

			// EF DbContext
			builder.RegisterType<AlphaEssDbContext>().InstancePerRequest();
			builder.RegisterType<AlphaEssDb_PowerDataDbContext>().InstancePerRequest();
			builder.RegisterType<AlphaEssDb_LogDbContext>().InstancePerRequest();
			builder.RegisterType<AlphaMicrogridDbContext>().InstancePerRequest();
			builder.RegisterType<AlphaComplaintsProcessingDbContext>().InstancePerRequest();
			//builder.RegisterType<RemoteMeterContext>().InstancePerRequest();

			//Register repositories
			builder.RegisterType(typeof(EntityRepository<APP_Version, Guid>)).As(typeof(IEntityRepository<APP_Version, Guid>)).InstancePerRequest();
			builder.RegisterType(typeof(EntityRepository<BASE_COUNTRY, int>)).As(typeof(IEntityRepository<BASE_COUNTRY, int>)).InstancePerRequest();
			builder.RegisterType(typeof(EntityRepository<BASE_ERRCODE, Guid>)).As(typeof(IEntityRepository<BASE_ERRCODE, Guid>)).InstancePerRequest();
			builder.RegisterType(typeof(EntityRepository<SYS_API, Guid>)).As(typeof(IEntityRepository<SYS_API, Guid>)).InstancePerRequest();
			builder.RegisterType(typeof(EntityRepository<SYS_APPVERSION, Guid>)).As(typeof(IEntityRepository<SYS_APPVERSION, Guid>)).InstancePerRequest();
			builder.RegisterType(typeof(EntityRepository<SYS_APPVERSIONDETAIL, Guid>)).As(typeof(IEntityRepository<SYS_APPVERSIONDETAIL, Guid>)).InstancePerRequest();
			builder.RegisterType(typeof(EntityRepository<SYS_APPVERSIONREC, Guid>)).As(typeof(IEntityRepository<SYS_APPVERSIONREC, Guid>)).InstancePerRequest();
			builder.RegisterType(typeof(EntityRepository<SYS_CONFIG, Guid>)).As(typeof(IEntityRepository<SYS_CONFIG, Guid>)).InstancePerRequest();
			builder.RegisterType(typeof(EntityRepository<SYS_EMAILRULE, Guid>)).As(typeof(IEntityRepository<SYS_EMAILRULE, Guid>)).InstancePerRequest();
			builder.RegisterType(typeof(EntityRepository<SYS_EMAILRULEUSER, Guid>)).As(typeof(IEntityRepository<SYS_EMAILRULEUSER, Guid>)).InstancePerRequest();
			builder.RegisterType(typeof(EntityRepository<SYS_LICENSE, Guid>)).As(typeof(IEntityRepository<SYS_LICENSE, Guid>)).InstancePerRequest();
			builder.RegisterType(typeof(EntityLogDbRepository<SYS_LOG, Guid>)).As(typeof(IEntityRepository<SYS_LOG, Guid>)).InstancePerRequest();
			builder.RegisterType(typeof(EntityRepository<SYS_MENU, Guid>)).As(typeof(IEntityRepository<SYS_MENU, Guid>)).InstancePerRequest();
			builder.RegisterType(typeof(EntityRepository<SYS_MSG, Guid>)).As(typeof(IEntityRepository<SYS_MSG, Guid>)).InstancePerRequest();
			builder.RegisterType(typeof(EntityRepository<SYS_MSGUSER, Guid>)).As(typeof(IEntityRepository<SYS_MSGUSER, Guid>)).InstancePerRequest();
			builder.RegisterType(typeof(EntityRepository<SYS_PRODATTACH, Guid>)).As(typeof(IEntityRepository<SYS_PRODATTACH, Guid>)).InstancePerRequest();
			builder.RegisterType(typeof(EntityRepository<SYS_PRODLEAVEMSG, Guid>)).As(typeof(IEntityRepository<SYS_PRODLEAVEMSG, Guid>)).InstancePerRequest();
			builder.RegisterType(typeof(EntityRepository<SYS_PRODUCT, Guid>)).As(typeof(IEntityRepository<SYS_PRODUCT, Guid>)).InstancePerRequest();
			builder.RegisterType(typeof(EntityRepository<SYS_PTYPE, Guid>)).As(typeof(IEntityRepository<SYS_PTYPE, Guid>)).InstancePerRequest();
			builder.RegisterType(typeof(EntityRepository<SYS_ROLE, Guid>)).As(typeof(IEntityRepository<SYS_ROLE, Guid>)).InstancePerRequest();
			builder.RegisterType(typeof(EntityRepository<SYS_PTYPEDETAIL, Guid>)).As(typeof(IEntityRepository<SYS_PTYPEDETAIL, Guid>)).InstancePerRequest();
			builder.RegisterType(typeof(EntityRepository<SYS_ROLEMENU, Guid>)).As(typeof(IEntityRepository<SYS_ROLEMENU, Guid>)).InstancePerRequest();
			builder.RegisterType(typeof(EntityRepository<SYS_ROLEUSER, Guid>)).As(typeof(IEntityRepository<SYS_ROLEUSER, Guid>)).InstancePerRequest();
			builder.RegisterType(typeof(EntityRepository<SYS_SN, Guid>)).As(typeof(IEntityRepository<SYS_SN, Guid>)).InstancePerRequest();
			builder.RegisterType(typeof(EntityRepository<SYS_USER, Guid>)).As(typeof(IEntityRepository<SYS_USER, Guid>)).InstancePerRequest();
			builder.RegisterType(typeof(EntityRepository<SYS_USERAGREEMENT, Guid>)).As(typeof(IEntityRepository<SYS_USERAGREEMENT, Guid>)).InstancePerRequest();
			builder.RegisterType(typeof(EntityRepository<SYS_USERSERVICEAREA, Guid>)).As(typeof(IEntityRepository<SYS_USERSERVICEAREA, Guid>)).InstancePerRequest();
			builder.RegisterType(typeof(EntityRepository<SysWeatherForecast, Guid>)).As(typeof(IEntityRepository<SysWeatherForecast, Guid>)).InstancePerRequest();
			builder.RegisterType(typeof(EntityRepository<Sys_Language, Guid>)).As(typeof(IEntityRepository<Sys_Language, Guid>)).InstancePerRequest();
			builder.RegisterType(typeof(ComplaintsProcessingEntityRepository<CustomerInfo, long>)).As(typeof(IEntityRepository<CustomerInfo, long>)).InstancePerRequest();

			builder.RegisterType(typeof(EntityRepository<Sys_ServicePartnerSn, long>)).As(typeof(IEntityRepository<Sys_ServicePartnerSn, long>)).InstancePerRequest();

			builder.RegisterType(typeof(EntityRepository<Sys_ResellerLicense, long>)).As(typeof(IEntityRepository<Sys_ResellerLicense, long>)).InstancePerRequest();

			builder.RegisterType(typeof(EntityRepository<Report_Energy, long>)).As(typeof(IEntityRepository<Report_Energy, long>)).InstancePerRequest();

			builder.RegisterType(typeof(EntityRepository<VT_CMD, Guid>)).As(typeof(IEntityRepository<VT_CMD, Guid>)).InstancePerRequest();
			builder.RegisterType(typeof(EntityRepository<VT_COLDATA, Guid>)).As(typeof(IEntityRepository<VT_COLDATA, Guid>)).InstancePerRequest();
			//builder.RegisterType(typeof(EntityRepository<VT_COLDATA_DEBUG>)).As(typeof(IEntityRepository<VT_COLDATA_DEBUG>)).InstancePerRequest();
			builder.RegisterType(typeof(EntityRepository<VT_SYSFAULT, Guid>)).As(typeof(IEntityRepository<VT_SYSFAULT, Guid>)).InstancePerRequest();
			builder.RegisterType(typeof(EntityRepository<VT_SYSINSTALL, Guid>)).As(typeof(IEntityRepository<VT_SYSINSTALL, Guid>)).InstancePerRequest();
			builder.RegisterType(typeof(EntityRepository<VT_SYSINSTALLATTACH, Guid>)).As(typeof(IEntityRepository<VT_SYSINSTALLATTACH, Guid>)).InstancePerRequest();
			builder.RegisterType(typeof(EntityRepository<VT_SYSMAINTAIN, Guid>)).As(typeof(IEntityRepository<VT_SYSMAINTAIN, Guid>)).InstancePerRequest();
			builder.RegisterType(typeof(EntityRepository<VT_SYSTEM, Guid>)).As(typeof(IEntityRepository<VT_SYSTEM, Guid>)).InstancePerRequest();
			builder.RegisterType(typeof(EntityRepository<Sys_RemoteDispatch, Guid>)).As(typeof(IEntityRepository<Sys_RemoteDispatch, Guid>)).InstancePerRequest();
			builder.RegisterType(typeof(SystemRepository)).As(typeof(ISystemRepository)).InstancePerRequest();
			builder.RegisterType(typeof(ComplaintsRepository)).As(typeof(IComplaintsRepository)).InstancePerRequest();

			builder.RegisterType(typeof(EntityRepository<Report_Power, long>)).As(typeof(IEntityRepository<Report_Power, long>)).InstancePerRequest();
			builder.RegisterType(typeof(EntityRepository<Report_Income, long>)).As(typeof(IEntityRepository<Report_Income, long>)).InstancePerRequest();
			builder.RegisterType(typeof(EntityPowerDataDbRepository<PowerData, Guid>)).As(typeof(IEntityRepository<PowerData, Guid>)).InstancePerRequest();

			builder.RegisterType(typeof(EntityRepository<Sys_PurchasePrice, Guid>)).As(typeof(IEntityRepository<Sys_PurchasePrice, Guid>)).InstancePerRequest();
			builder.RegisterType(typeof(EntityRepository<Sys_SellPrice, Guid>)).As(typeof(IEntityRepository<Sys_SellPrice, Guid>)).InstancePerRequest();

			builder.RegisterType(typeof(ComplaintsProcessingEntityRepository<Complaints, long>)).As(typeof(IEntityRepository<Complaints, long>)).InstancePerRequest();

			builder.RegisterType(typeof(EntityLogDbRepository<UserLog, Guid>)).As(typeof(IEntityRepository<UserLog, Guid>)).InstancePerRequest();
			builder.RegisterType(typeof(MicrogridEntityRepository<ElectricDispatchingControlRecord, Guid>)).As(typeof(IEntityRepository<ElectricDispatchingControlRecord, Guid>)).InstancePerRequest();
			builder.RegisterType(typeof(MicrogridEntityRepository<EMSNode, Guid>)).As(typeof(IEntityRepository<EMSNode, Guid>)).InstancePerRequest();
			builder.RegisterType(typeof(MicrogridEntityRepository<MeterRecord, Guid>)).As(typeof(IEntityRepository<MeterRecord, Guid>)).InstancePerRequest();
			builder.RegisterType(typeof(MicrogridEntityRepository<MicrogridAndSN, Guid>)).As(typeof(IEntityRepository<MicrogridAndSN, Guid>)).InstancePerRequest();
			builder.RegisterType(typeof(MicrogridEntityRepository<MicrogridInfo, Guid>)).As(typeof(IEntityRepository<MicrogridInfo, Guid>)).InstancePerRequest();
			builder.RegisterType(typeof(MicrogridEntityRepository<MicrogridSummary, Guid>)).As(typeof(IEntityRepository<MicrogridSummary, Guid>)).InstancePerRequest();
			builder.RegisterType(typeof(MicrogridEntityRepository<SchedulingStrategy, Guid>)).As(typeof(IEntityRepository<SchedulingStrategy, Guid>)).InstancePerRequest();
			builder.RegisterType(typeof(EntityRepository<SYS_Country_Region, Guid>)).As(typeof(IEntityRepository<SYS_Country_Region, Guid>)).InstancePerRequest();

			//builder.RegisterType(typeof(EntityMeterRepository<DevInfo, Guid>)).As(typeof(IEntityRepository<DevInfo, Guid>)).InstancePerRequest();
			//builder.RegisterType(typeof(EntityMeterRepository<DevData, Guid>)).As(typeof(IEntityRepository<DevData, Guid>)).InstancePerRequest();

			builder.RegisterType(typeof(EntityRepository<Report_Energy, long>)).As(typeof(IEntityRepository<Report_Energy, long>)).InstancePerRequest();
			builder.RegisterType(typeof(EntityRepository<Report_Power, long>)).As(typeof(IEntityRepository<Report_Power, long>)).InstancePerRequest();
			builder.RegisterType(typeof(EntityRepository<CompanyContactDetail, long>)).As(typeof(IEntityRepository<CompanyContactDetail, long>)).InstancePerRequest();

			// Services
			builder.RegisterType<CryptoService>().As<ICryptoService>().InstancePerRequest();
			builder.RegisterType<MembershipService>().As<IMembershipService>().InstancePerRequest();
			//builder.RegisterType<MembershipService>().As<IMembershipService>().InstancePerRequest();
			builder.RegisterType<ParameterValidateService>().As<IParameterValidateService>().InstancePerRequest();
			builder.RegisterType<EmailService>().As<IEmailService>().InstancePerRequest();
			builder.RegisterType<SystemService>().As<ISystemService>().InstancePerRequest();
			builder.RegisterType<AppClientService>().As<IAppClientService>().InstancePerRequest();
			builder.RegisterType<MicrogridService>().As<IMicrogridService>().InstancePerRequest();
			builder.RegisterType<WeatherForecastService>().As<IWeatherForecastService>().InstancePerRequest();
			builder.RegisterType<ComplaintsService>().As<IComplaintsService>().InstancePerRequest();
			builder.RegisterType<ApiService>().As<IApiService>().InstancePerRequest();
			builder.RegisterType<SysMsgService>().As<ISysMsgService>().InstancePerRequest();
			builder.RegisterType<CompanyContactDetailService>().As<ICompanyContactDetailService>().InstancePerRequest();

			builder.RegisterType<AlphaRemotingService>().As<IAlphaRemotingService>().SingleInstance();


			//builder.RegisterType<AlphaESSV2Service>().As<IAlphaESSV2Service>().InstancePerRequest();
			//builder.RegisterType<ReportInfoService>().As<IReportInfoService>().InstancePerRequest();
			//builder.RegisterType<RunningDataService>().As<IRunningDataService>().InstancePerRequest();
			//builder.RegisterType<PowerDataService>().As<IPowerDataService>().InstancePerRequest();
			//builder.RegisterType<RemoteDispatchService>().As<IRemoteDispatchService>().InstancePerRequest();
			//builder.RegisterType<DevInfoService>().As<IDevInfoService>().InstancePerRequest();
			//builder.RegisterType<DevDataService>().As<IDevDataService>().InstancePerRequest();

			// registration goes here
			return builder.Build();
		}
	}
}
