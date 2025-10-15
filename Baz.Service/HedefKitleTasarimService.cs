using Baz.Mapper.Pattern;
using Baz.Model.Entity;
using Baz.Model.Entity.Constants;
using Baz.Model.Entity.ViewModel;
using Baz.Model.Pattern;
using Baz.ProcessResult;
using Baz.Repository.Pattern;
using Baz.RequestManager.Abstracts;
using Baz.Service.Base;
using Microsoft.Extensions.DependencyInjection; // Service eklerken
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using static Baz.Service.Helper.ExpressionUtils;

namespace Baz.Service
{
    /// <summary>
    /// hedef kitle işlemerini yöneten interface
    /// </summary>
    public interface IHedefKitleTasarimService : IService<HedefKitleTanimlamalar>
    {
        /// <summary>
        ///Hedef kitle Idye göre hedef kitleyi döndüren metod
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Result<HedefKitle> SingleOrDefaultForView(int id);

        /// <summary>
        /// hedef kitle asarım idsine göre hedef kitleyi çalıştıran ve key value olarak verileri döndüren metod
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Result<List<KeyValueModel>> RunExpression(int id);
    }

    /// <summary>
    ///  hedef kitle tasarim methodlarının bulunduğu servis sınıfı
    /// </summary>
    public class HedefKitleTasarimService : Base.Service<HedefKitleTanimlamalar>, IHedefKitleTasarimService
    {
        private readonly IRequestHelper _requestHedefKitle;
        private readonly ILoginUser _loginUser;
        private readonly IKisiService _kisiService;

        /// <summary>
        /// hedef kitle tasarim methodlarının bulunduğu servis sınıfının yapıcı metodu
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="dataMapper"></param>
        /// <param name="serviceProvider"></param>
        /// <param name="logger"></param>
        /// <param name="requestHelper"></param>
        /// <param name="loginUser"></param>
        /// <param name="kisiService"></param>
        public HedefKitleTasarimService(IRepository<HedefKitleTanimlamalar> repository, IDataMapper dataMapper, IServiceProvider serviceProvider, ILogger<HedefKitleTasarimService> logger, IRequestHelper requestHelper, ILoginUser loginUser, IKisiService kisiService) : base(repository, dataMapper, serviceProvider, logger)
        {
            _loginUser = loginUser;
            _kisiService = kisiService;
            _requestHedefKitle = requestHelper;
        }

        /// <summary>
        /// hedef kitle asarım idsine göre hedef kitleyi çalıştıran ve key value olarak verileri döndüren metod
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Result<List<KeyValueModel>> RunExpression(int id)
        {
            var _kurumIdleri = _kisiService.AmirveyaMusteriTemsilcisiKurumlariIDGetir(_loginUser.KisiID).Value;

            var item = SingleOrDefaultForView(id);

            if (item.IsSuccess && item.Value?.Filters != null && item.Value.Filters.Count > 1)
            {
                //Hedef kitledeki filterlar
                var locig = item.Value.Filters.FirstOrDefault(p => string.IsNullOrEmpty(p.MemberName));
                //Hedef kitledeki karşılaştırma tipleri
                var comparisonType = locig.LocigalOperator == "OR" ? ComparisonType.OR : ComparisonType.AND;

                List<BuildPredicateModel> compositeStatic = new();
                List<BuildPredicateModel> compositeStaticHassasBilgiler = new();
                List<BuildPredicateModelForDynamic> compositeDynamic = new();

                foreach (var filtre in item.Value.Filters)
                {
                    if (!string.IsNullOrEmpty(filtre.MemberName) && !string.IsNullOrEmpty(filtre.LocigalOperator))
                    {
                        //Filter ek parametreyi kapsıyorsa  dinamik belirteçlere, hasasbilgileri kapsıyorsa hassasstatic belirteçlere, kapsamıyorsa temel bilgi static belirteçlere eklenmesi
                        if (filtre.EkParametreId > 0)
                        {
                            compositeDynamic.Add(new BuildPredicateModelForDynamic
                            {
                                ForDeger = new BuildPredicateModel()
                                {
                                    Comparison = filtre.Operator,
                                    PropertyName = filtre.FieldName,
                                    Value = filtre.Value,
                                    ValueType = Type.GetType(filtre.FieldType)
                                },
                                ForEkParametre = new BuildPredicateModel()
                                {
                                    Comparison = "IsEqualTo",
                                    PropertyName = "KisiEkParametreID",
                                    Value = filtre.EkParametreId,
                                    ValueType = typeof(int)
                                }
                            });
                        }
                        else
                        {
                            if (filtre.TableName == "KisiHassasBilgiler")
                            {
                                compositeStaticHassasBilgiler.Add(new BuildPredicateModel
                                {
                                    Comparison = filtre.Operator,
                                    PropertyName = filtre.FieldName,
                                    Value = filtre.Value,
                                    ValueType = Type.GetType(filtre.FieldType)
                                });
                            }
                            else
                            {
                                compositeStatic.Add(new BuildPredicateModel
                                {
                                    Comparison = filtre.Operator,
                                    PropertyName = filtre.FieldName,
                                    Value = filtre.Value,
                                    ValueType = Type.GetType(filtre.FieldType)
                                });
                            }
                        }
                    }
                }
                List<KeyValueModel> staticData = new();
                List<KeyValueModel> staticDataHassasBilgiler = new();
                List<KeyValueModel> dynamicData = new();
                int temelBilgilerCount = compositeStatic.Count;
                int hassasBilgilerCount = compositeStaticHassasBilgiler.Count;
                int dynamicCount = compositeDynamic.Count;
                var _kisiService = _serviceProvider.GetService<IKisiService>();
                var _kisiHassasBilgilerService = _serviceProvider.GetService<IKisiHassasBilgilerService>();
                
                // karşılaştırma tipleri, kurum idleri ve beltireçlere göre filtreleme işlemleri
                if (temelBilgilerCount > 0)
                    staticData = _kisiService.List(comparisonType, compositeStatic, _kurumIdleri).Value;
                if (hassasBilgilerCount > 0)
                    staticDataHassasBilgiler = _kisiHassasBilgilerService.List(comparisonType, compositeStaticHassasBilgiler, _kurumIdleri).Value;
                


                var result = new List<KeyValueModel>();
                //Karşılaştırma tipine göre value filtresi yapılması ve hedef kitlenin filtrelemesine göre sorgunun dönmesi
                if (locig.LocigalOperator == "OR")
                {
                    if (temelBilgilerCount > 0)
                        result.AddRange(staticData);
                    if (hassasBilgilerCount > 0)
                        result.AddRange(staticDataHassasBilgiler);
                    if (dynamicCount > 0)
                        result.AddRange(dynamicData);
                }
                else
                {
                    result.AddRange(staticData);
                    result.AddRange(staticDataHassasBilgiler);
                    result.AddRange(dynamicData);

                    if (temelBilgilerCount > 0)
                        result = result.Where(x => staticData.Any(y => x.Value == y.Value)).ToList();
                    if (hassasBilgilerCount > 0)
                        result = result.Where(x => staticDataHassasBilgiler.Any(y => x.Value == y.Value)).ToList();
                    if (dynamicCount > 0)
                        result = result.Where(x => dynamicData.Any(y => x.Value == y.Value)).ToList();
                }
                return result.GroupBy(x => x.Value).Select(x => x.First()).ToList().ToResult();
            }
            var res = new List<KeyValueModel>().ToResult();
            res.StatusCode = (int)ResultStatusCode.ReadError;
            return res;
        }

        /// <summary>
        ///Hedef kitle Idye göre hedef kitleyi döndüren metod
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Result<HedefKitle> SingleOrDefaultForView(int id)
        {
            if (id == 0)
            {
                return Results.Fail("İşleminiz gerçekleştirilemedi!", ResultStatusCode.ReadError);
            }
            var item = base.SingleOrDefault(id).Value;
            return item != null ? new HedefKitle()
            {
                TabloID = item.TabloID,
                KurumId = item.KurumID,
                KisiId = item.KisiID,
                Tanim = item.Tanim,
                HedefKitleTipi = item.HedefKitleTipi,
                Filters = JsonConvert.DeserializeObject<List<HedefKitleFilter>>(item.HedefKitleFiltre)
            }.ToResult() : Results.Fail("İşleminiz gerçekleştirilemedi!", ResultStatusCode.ReadError);
        }
    }
}