using Baz.Mapper.Pattern;
using Baz.Model.Entity;
using Baz.ProcessResult;
using Baz.Repository.Pattern;
using Baz.Service.Base;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace Baz.Service
{
    /// <summary>
    /// Kişi telefon bilgilerinin yöneten interface
    /// </summary>
    public interface IPostaciBekleyenIslemlerAyrintilarService : IService<PostaciBekleyenIslemlerAyrintilar>
    {
        
    }

    /// <summary>
    ///  Kişi telefon bilgilerinin yöneten servis sınıfı
    /// </summary>
    public class PostaciBekleyenIslemlerAyrintilarService : Service<PostaciBekleyenIslemlerAyrintilar>, IPostaciBekleyenIslemlerAyrintilarService
    {
        /// <summary>
        /// Kişi telefon bilgilerinin yöneten servis sınıfının yapıcı metodu
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="dataMapper"></param>
        /// <param name="serviceProvider"></param>
        /// <param name="logger"></param>
        public PostaciBekleyenIslemlerAyrintilarService(IRepository<PostaciBekleyenIslemlerAyrintilar> repository, IDataMapper dataMapper, IServiceProvider serviceProvider, ILogger<PostaciBekleyenIslemlerGenelService> logger) : base(repository, dataMapper, serviceProvider, logger)
        {
        }

        
    }
}