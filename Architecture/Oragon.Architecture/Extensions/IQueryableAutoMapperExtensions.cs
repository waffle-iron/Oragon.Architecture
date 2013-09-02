using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;

namespace Oragon.Architecture.Extensions
{
	public static class IQueryableAutoMapperExtensions
	{
		public static IQueryable<TDestination> AutoMapperConvertAll<TSource, TDestination>(this IQueryable<TSource> source)
		{
			IQueryable<TDestination> returnValue = source.Select(it =>
				AutoMapper.Mapper.Map<TSource, TDestination>(it)
			);
			return returnValue;
		}

		public static List<TDestination> AutoMapperConvertAll<TSource, TDestination>(this IList<TSource> source)
		{
			List<TDestination> returnValue = source.Select(it =>
				AutoMapper.Mapper.Map<TSource, TDestination>(it)
			).ToList();
			return returnValue;
		}
	}
}
