using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;

namespace IFramework.AutoMapper
{
    public static class AutoMapperExtension
    {
        /// <summary>   
        /// 集合对集合     
        /// </summary>
        /// <returns></returns>   
        public static TTarget MapTo<TTarget>(this IEnumerable source)
        {
            if (source == null)
                return default(TTarget);

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMissingTypeMaps = true;
                cfg.ValidateInlineMaps = false;
            });
            var mapper = config.CreateMapper();

            return mapper.Map<TTarget>(source);
        }


        /// <summary>   
        /// 对象对对象   
        /// </summary>
        /// <returns></returns>  
        public static TTarget MapTo<TTarget>(this object source)
        {
            if (source == null)
                return default(TTarget);

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMissingTypeMaps = true;
                cfg.ValidateInlineMaps = false;
            });
            var mapper = config.CreateMapper();

            return mapper.Map<TTarget>(source);
        }

    }
}
