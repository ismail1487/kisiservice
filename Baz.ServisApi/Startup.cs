using AutoMapper;
using Baz.AOP.Logger.ExceptionLog;
using Baz.AOP.Logger.Http;
using Baz.KisiServisApi.Handlers;
using Baz.Model.Entity;
using Baz.Model.Entity.ViewModel;
using Baz.Model.Pattern;
using Baz.RequestManager;
using Baz.RequestManager.Abstracts;
using Baz.Service;
using Baz.SharedSession;
using Decor;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using Baz.AletKutusu;
using Baz.Service.Base;

namespace Baz.KisiServisApi
{
    /// <summary>
    /// Uygulamayı ayağa kaldıran class
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class Startup
    {
        /// <summary>
        /// Uygulamayı ayağa kaldıran servisin yapıcı methodudur.
        /// </summary>
        /// <param name="env"></param>
        public Startup(IWebHostEnvironment env)
        {
            Configuration = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables().Build();
        }

        /// <summary>
        /// Uygulamayı yapılandıran özellik
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            SetCoreURL(Configuration.GetValue<string>("CoreUrl"));
            services.AddHttpContextAccessor();
            services.AddControllers(c => { c.Filters.Add(typeof(ModelValidationFilter), int.MinValue); });
            
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Baz.KisiServisApi", Version = "v1" });
                c.OperationFilter<DefaultHeaderParameter>();
            }); 
            services.AddDbContext<Repository.Pattern.IDataContext, Repository.Pattern.Entity.DataContext>(conf => conf.UseSqlServer(Configuration.GetConnectionString("Connection")));
            services.AddSingleton<Baz.Mapper.Pattern.IDataMapper>(new Baz.Mapper.Pattern.Entity.DataMapper(GenerateConfiguratedMapper()));
            services.AddTransient<IRequestHelper, RequestHelper>(provider =>
            {
                return new RequestHelper("", new RequestManagerHeaderHelperForHttp(provider).SetDefaultHeader());
            });
            //////////////////////////////////////////SESSION SERVER AYARLARI/////////////////////////////////////////////////
            //Distributed session iþlemleri için session serverýn network baðlantýlarýný yapýlandýrýr.
            services.AddDistributedSqlServerCache(p =>
            {
                p.ConnectionString = Configuration.GetConnectionString("SessionConnection");
                p.SchemaName = "dbo";
                p.TableName = "SQLSessions";
            });
            services.AddSession(options =>
            {
                options.Cookie.HttpOnly = true;
                options.Cookie.Path = "/";
                options.Cookie.Name = "Test.Session";
                options.IdleTimeout = TimeSpan.FromMinutes(60);
            });
            services.AddSession();
            //Http desteði olmadan paylaþýmlý session iþlemleri yapan servisi kayýt eder.
            services.AddTransient<ISharedSession, BaseSharedSession>();
            //Http desteði olan iþlemler için paylaþýmlý session nesnesinin kaydýný yapar.
            //BaseSharedSessionForHttpRequest iþlemleri için öncelikle BaseSharedSession servisi kayýt edilmelidir.
            services.AddTransient<ILoginUser, LoginUserManager>();
            services.AddTransient<ISharedSessionForHttpRequest, BaseSharedSessionForHttpRequest>();
            //////////////////////////////////////////////////////////////////////////////////////
            services.AddScoped<IYetkiMerkeziService, YetkiMerkeziService>();
            services.AddScoped<ISistemLoginSifreYenilemeAktivasyonHareketleriService, SistemLoginSifreYenilemeAktivasyonHareketleriService>();
            services.AddScoped<IPostaciBekleyenIslemlerGenelService, PostaciBekleyenIslemlerGenelService>();
            services.AddScoped<IPostaciBekleyenIslemlerAyrintilarService, PostaciBekleyenIslemlerAyrintilarService>();
            services.AddScoped<IParamOrganizasyonBirimleriService, ParamOrganizasyonBirimleriService>();
            services.AddScoped<IMesajSeriGenelTanimlarService, MesajSeriGenelTanimlarService>();
            services.AddScoped<IMesajAkisIcerikService, MesajAkisIcerikService>();
            services.AddScoped<IMedyaKutuphanesiService, MedyaKutuphanesiService>();
            //services.AddScoped<IKurumOrganizasyonBirimTanimlariService, KurumOrganizasyonBirimTanimlariService>();
            services.AddScoped<IKurumlarService, KurumlarService>();
            services.AddScoped<IKurumlarKisilerService, KurumlarKisilerService>();
            //services.AddScoped<IKureselParametrelerService, KureselParametrelerService>();
            services.AddScoped<IKisiTelefonBilgileriService, KisiTelefonBilgileriService>();
            services.AddScoped<IKisiService, KisiService>();
            services.AddScoped<IKisiIliskiService, KisiIliskiService>();
            services.AddScoped<IKisiHassasBilgilerService, KisiHassasBilgilerService>();
            services.AddScoped<IKisiGrupService, KisiGrupService>();
            services.AddScoped<IKisiEgitimBilgileriService, KisiEgitimBilgileriService>();
            services.AddScoped<IKisiAdresBilgileriService, KisiAdresBilgileriService>();
            services.AddScoped<IHedefKitleTasarimService, HedefKitleTasarimService>();
            services.AddScoped<IErisimYetkilendirmeTanimlariService, ErisimYetkilendirmeTanimlariService>();

            /////////////////////////////////////////////////////////////////////////////////////
            services.AddScoped<Repository.Pattern.IUnitOfWork, Repository.Pattern.Entity.UnitOfWork>();
            services.AddScoped(typeof(Repository.Pattern.IRepository<>), typeof(Repository.Pattern.Entity.Repository<>));
            services.AddScoped(typeof(Service.Base.IService<>), typeof(Service.Base.Service<>));
            services.AddTransient<ILoginUser, LoginUserManager>();
            services.AddScoped<IKureselParametrelerService, KureselParametrelerService>();
            services.AddScoped<IKurumOrganizasyonBirimTanimlariService, KurumOrganizasyonBirimTanimlariService>();
            var types = typeof(Service.Base.IService<>).Assembly.GetTypes();
            var interfaces = types.Where(p => p.IsInterface && p.GetInterface("IService`1") != null).ToList();

            //Exception loglarýný iþleyen Baz.AOP.Logger.ExceptionLog servisinin kaydýný yapar
            services.AddAOPExceptionLogging();
            //Http iþlemleri için loglama yapan BaseHttpLogger servisinin kaydýný yapar.

            foreach (var item in interfaces)
            {
                var serviceTypes = types.Where(p => p.GetInterface(item.Name) != null && !p.IsInterface).ToList();
                serviceTypes.ForEach(p => services.AddScoped(item, p).Decorated());
            }
            services.AddResponseCompression();
        }

        /// <summary>
        /// Bu yöntem çalışma zamanı tarafından çağrılır. HTTP istek ardışık düzenini yapılandırmak için bu yöntemi kullanın.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        /// <param name="lifetime"></param>
        /// <param name="cache"></param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IHostApplicationLifetime lifetime,
            IDistributedCache cache)
        {
            // Configure the Localization middleware
            app.UseRequestLocalization();

            app.UseMiddleware<ExceptionHandlerMiddleware>();
            ////////////////////////////////// SESSION SERVER AYARLARI/////////////////////////////////////
            app.UseSession();
            lifetime.ApplicationStarted.Register(() =>
            {
                var currentTimeUTC = DateTime.UtcNow.ToString();
                byte[] encodedCurrentTimeUTC = Encoding.UTF8.GetBytes(currentTimeUTC);
                var options = new DistributedCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromSeconds(20));
                cache.Set("cachedTimeUTC", encodedCurrentTimeUTC, options);
            });
            /////////////////////////////////////////////////////////////////////////////////////
            app.UseSwagger();/**/
            app.UseSwaggerUI(c =>
            {
                string swaggerJsonBasePath = string.IsNullOrWhiteSpace(c.RoutePrefix) ? "." : "..";
                c.SwaggerEndpoint($"{swaggerJsonBasePath}/swagger/v1/swagger.json", "Baz.KisiServisApi v1");
            });
            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseMiddleware<AuthMiddleware>();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private Profile GenerateConfiguratedMapper()
        {
            var mapper = Baz.Mapper.Pattern.Entity.DataMapperProfile.GenerateProfile();
            mapper.CreateMap<BasicKisiModel, KisiTemelBilgiler>();
            mapper.CreateMap<KisiTemelBilgiler, BasicKisiModel>();
            mapper.CreateMap<BasicKisiModel, KisiHassasBilgiler>();
            mapper.CreateMap<KisiHassasBilgiler, BasicKisiModel>();
            return mapper;
        }

        private static void SetCoreURL(string url)
        {
            Model.Entity.Constants.LocalPortlar.CoreUrl = url;
        }
    }
}