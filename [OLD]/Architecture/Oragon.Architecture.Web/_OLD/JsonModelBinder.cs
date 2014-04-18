//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Web.Mvc;
//using System.Globalization;

//namespace Oragon.Architecture.Web.Mvc
//{
//	public class JsonModelBinder : DefaultModelBinder
//	{
//		private CultureInfo culture = CultureInfo.GetCultureInfo("pt-br");

//		//public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
//		//{
//		//    object returnvValue = null;
//		//    //ValueProviderResult valueProviderResult = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
//		//    //if (valueProviderResult != null && !string.IsNullOrEmpty(valueProviderResult.AttemptedValue) && valueProviderResult.AttemptedValue.StartsWith("{"))
//		//    //    returnvValue = JsonHelper.Deserialize(valueProviderResult.AttemptedValue, bindingContext.ModelType);
//		//    //else
//		//        returnvValue = base.BindModel(controllerContext, bindingContext);
//		//    return returnvValue;
//		//}

//		public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
//		{
//			var valueProviderResult = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);

//			if (valueProviderResult != null && !string.IsNullOrEmpty(valueProviderResult.AttemptedValue))
//			{
//				// Somente tipos fracionários ou datas
//				//if (bindingContext.ModelType == typeof(DateTime) || bindingContext.ModelType == typeof(DateTime?))
//				//    return DateTime.Parse(valueProviderResult.AttemptedValue, culture);
//				//else 
//				if (bindingContext.ModelType == typeof(decimal) || bindingContext.ModelType == typeof(decimal?))
//					return decimal.Parse(valueProviderResult.AttemptedValue, culture);
//				else if (bindingContext.ModelType == typeof(float) || bindingContext.ModelType == typeof(float?))
//					return float.Parse(valueProviderResult.AttemptedValue, culture);
//				else if (bindingContext.ModelType == typeof(double) || bindingContext.ModelType == typeof(double?))
//					return double.Parse(valueProviderResult.AttemptedValue, culture);
//			}

//			return base.BindModel(controllerContext, bindingContext);
//		}

//	}
//}

