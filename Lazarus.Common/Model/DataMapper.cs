using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace SCGL.OMS.FreightJob.Areas.Area.Controllers
{
	public static class DataMapper
	{
		public static TResult ToMap<TResult>(this object objectSource) where TResult : new()
		{
			if (objectSource == null) return default(TResult);
			TResult objectResult = new TResult();
			Type originalType = objectSource.GetType();
			if (originalType.IsGenericType) objectResult = MapList<TResult>(objectSource);
			else Setter(objectSource, objectResult);
			return objectResult;
		}

		public static TResult Map<TInput, TResult>(TInput objectSource) where TResult : new() where TInput : new()
		{
			if (objectSource == null) return default(TResult);
			TResult objectResult = new TResult();
			Type originalType = objectSource.GetType();
			if (originalType.IsGenericType) objectResult = MapList<TResult>(objectSource);
			else Setter(objectSource, objectResult);
			return objectResult;
		}

		public static TResult Map<TResult>(object objectSource) where TResult : new()
		{
			if (objectSource == null) return default(TResult);
			TResult objectResult = new TResult();
			Type originalType = objectSource.GetType();
			if (originalType.IsGenericType) objectResult = MapList<TResult>(objectSource);
			else Setter(objectSource, objectResult);
			return objectResult;
		}

		private static TResult MapList<TResult>(object objectSource) where TResult : new()
		{
			TResult objectResult = new TResult();
			Type originalType = objectSource.GetType();
			Type resultType = objectResult.GetType();
			var _objCount = originalType.GetProperty("Count");
			if (_objCount != null)
			{
				int _count = (int)_objCount.GetValue(objectSource, null);
				for (int i = 0; i < _count; i++)
				{
					object[] _index = { i };
					var _originalItem = originalType.GetProperty("Item").GetValue(objectSource, _index);
					Type _resultObjectItemType = resultType.GetGenericArguments().FirstOrDefault();
					if (_resultObjectItemType == null) throw new Exception($"{resultType.FullName} cannot mapping with {originalType.FullName}");
					var _resulItem = Activator.CreateInstance(_resultObjectItemType);
					Setter(_originalItem, _resulItem);
					resultType.GetMethod("Add").Invoke(objectResult, new[] { _resulItem });
				}
			}
			else
			{
				Setter(objectSource, objectResult);
			}

			return objectResult;
		}

		private static void Setter(object objectSource, object setterResult)
		{
			Type sourceType = objectSource.GetType();
			foreach (var property in sourceType.GetProperties())
			{
				var _resultProperty = setterResult.GetType().GetProperty(property.Name);
				if (_resultProperty == null) continue;
				if (!_resultProperty.CanWrite) continue;
				var _sourceValue = property.GetValue(objectSource);
				if (_sourceValue == null) continue;
				var _resultPropertyType = _resultProperty.PropertyType;
				var _sourcePropertyType = property.PropertyType;
				var _genericSourceNullableType = Nullable.GetUnderlyingType(_sourcePropertyType);
				var _genericResultNullableType = Nullable.GetUnderlyingType(_resultPropertyType);
				if (_resultPropertyType.IsEnum && _sourcePropertyType.IsEnum)
				{
					_resultProperty.SetValue(setterResult, Enum.ToObject(_sourcePropertyType, _sourceValue), null);
					continue;
				}
				else if ((_sourcePropertyType == _resultPropertyType && !_sourcePropertyType.IsGenericType)
					|| (_genericResultNullableType == _sourcePropertyType))//
				{
					_resultProperty.SetValue(setterResult, _sourceValue, null);
					continue;
				}
				else if (_sourcePropertyType.IsGenericType && _resultPropertyType.IsGenericType)
				{
					if (_resultPropertyType.IsInterface && _resultPropertyType.Namespace.Equals(typeof(ICollection<>).Namespace))
					{
						Type _typeParameters = _resultPropertyType.GetGenericArguments().FirstOrDefault();
						if (_typeParameters != null)
						{
							Type _constructed = typeof(List<>).MakeGenericType(_typeParameters);
							var _tmpInstance = Activator.CreateInstance(_constructed);
							var _resultClass = MapInside(_sourceValue, _tmpInstance);
							_resultProperty.SetValue(setterResult, _resultClass, null);
							continue;
						}
					}
					else if (_sourcePropertyType == _resultPropertyType || (_genericResultNullableType != null && _genericSourceNullableType == _genericResultNullableType))
					{
						_resultProperty.SetValue(setterResult, _sourceValue, null);
						continue;
					}
				}
				if (!_resultPropertyType.Namespace.Equals("System"))
				{
					var _tmpIns = Activator.CreateInstance(_resultPropertyType);
					var _resultClass = MapInside(_sourceValue, _tmpIns);
					_resultProperty.SetValue(setterResult, _resultClass, null);
				}
			}
		}

		private static object MapInside(object objectSource, object objectResult)
		{
			Type sourceType = objectSource.GetType();
			Type resultType = objectResult.GetType();
			var getCount = sourceType.GetProperty("Count");
			if (getCount != null)
			{
				int _count = (int)getCount.GetValue(objectSource, null);
				if (resultType.GetGenericTypeDefinition() == typeof(Dictionary<,>) && sourceType.GetGenericTypeDefinition() == resultType.GetGenericTypeDefinition())
				{
					List<object> _dictionaryValues = ((IEnumerable)objectSource).Cast<object>().ToList();
					Type _dictionaryValueType = _dictionaryValues.GetType();
					for (int i = 0; i < _count; i++)
					{
						object[] _index = { i };
						dynamic _dictSouceValue = _dictionaryValueType.GetProperty("Item").GetValue(_dictionaryValues, _index);
						resultType.GetMethod("Add").Invoke(objectResult, new[] { _dictSouceValue.Key, _dictSouceValue.Value });
					}
				}
				else
				{
					List<object> _listObject = ((IEnumerable)objectSource).Cast<object>().ToList();
					foreach (var _souceValueItem in _listObject)
					{
						var _souceValueItemType = _souceValueItem.GetType();
						Type _resultObjectItemType = resultType.GetGenericArguments().FirstOrDefault();
						if (_resultObjectItemType == _souceValueItemType)
						{
							resultType.GetMethod("Add").Invoke(objectResult, new[] { _souceValueItem });
						}
						else if (_resultObjectItemType.IsEnum)
						{
							var _resulItem = Activator.CreateInstance(_resultObjectItemType);
							resultType.GetMethod("Add").Invoke(objectResult, new[] { Enum.ToObject(_resultObjectItemType, _souceValueItem) });
						}
						else
						{
							if (_resultObjectItemType.FullName.Equals("System.Object"))
							{
								var _resulItem = Activator.CreateInstance(_souceValueItemType);
								Setter(_souceValueItem, _resulItem);
								resultType.GetMethod("Add").Invoke(objectResult, new[] { _resulItem });
							}
							else
							{
								var _resulItem = Activator.CreateInstance(_resultObjectItemType);
								Setter(_souceValueItem, _resulItem);
								resultType.GetMethod("Add").Invoke(objectResult, new[] { _resulItem });
							}
						}
					}
				}
			}
			else
			{
				Setter(objectSource, objectResult);
			}

			return objectResult;
		}
		public static string GetEnumDisplayName(this Enum enumType)
		{
			return enumType.GetType().GetMember(enumType.ToString())
						   .First()
						   .GetCustomAttribute<DisplayAttribute>()
						   .Name;
		}

	}
}