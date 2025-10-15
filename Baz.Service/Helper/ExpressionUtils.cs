using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Baz.Service.Helper
{
    /// <summary>
    /// İfade araçları
    /// </summary>
    public static class ExpressionUtils
    {
        /// <summary>
        ///Karşılaştırma tipleri
        /// </summary>
        public enum ComparisonType
        {
            /// <summary>
            /// VE
            /// </summary>
            AND,

            /// <summary>
            /// VEYA
            /// </summary>
            OR
        }

        /// <summary>
        /// İfade modeli
        /// </summary>
        public class BuildPredicateModel
        {
            /// <summary>
            /// özellik adı
            /// </summary>
            public string PropertyName { get; set; }

            /// <summary>
            ///karşılaştırma
            /// </summary>
            public string Comparison { get; set; }

            /// <summary>
            /// değer
            /// </summary>
            public object Value { get; set; }

            /// <summary>
            /// değer tipi
            /// </summary>
            public Type ValueType { get; set; }
        }

        /// <summary>
        /// Dinamik model
        /// </summary>
        public class BuildPredicateModelForDynamic
        {
            /// <summary>
            /// Ek parametre
            /// </summary>
            public BuildPredicateModel ForEkParametre { get; set; }

            /// <summary>
            /// Değer
            /// </summary>
            public BuildPredicateModel ForDeger { get; set; }
        }

        /// <summary>
        /// Tüm dinamik filtreleri karşılaştırma tipine göre bağlar
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="comparisonType"></param>
        /// <param name="buildPredicateModelList"></param>
        /// <returns></returns>
        private static Expression<Func<T, bool>> BuildPredicateMultiple<T>(ComparisonType comparisonType, List<BuildPredicateModel> buildPredicateModelList)
        {
            List<Expression<Func<T, bool>>> list = new();
            //Tüm dinamik filtreleri karşılaştırma tipine göre bağlar
            foreach (var buildPredicateModel in buildPredicateModelList)
            {
                list.Add(BuildPredicate<T>(buildPredicateModel));
            }
            if (comparisonType == ComparisonType.AND)
            {
                var result = AndAll<T>(list);
                return result;
            }
            else
            {
                var result = OrAll<T>(list);
                return result;
            }
        }

        /// <summary>
        /// Tüm filtreleri karşılaştırma tipine göre bağlar
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="comparisonType"></param>
        /// <param name="buildPredicateModelList"></param>
        /// <returns></returns>
        private static Expression<Func<T, bool>> BuildPredicateMultiple<T>(ComparisonType comparisonType, List<BuildPredicateModelForDynamic> buildPredicateModelList)
        {
            List<Expression<Func<T, bool>>> list = new();

            foreach (var buildPredicateModel in buildPredicateModelList)
            {
                list.Add(BuildPredicate<T>(buildPredicateModel.ForEkParametre));
                list.Add(BuildPredicate<T>(buildPredicateModel.ForDeger));
            }
            if (comparisonType == ComparisonType.AND)
            {
                var result = AndAll<T>(list);
                return result;
            }
            else
            {
                var result = OrAll<T>(list);
                return result;
            }
        }

        /// <summary>
        /// Filtre kaşlılaştırma ifadesini oluşturur
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="buildPredicateModel"></param>
        /// <returns></returns>
        private static Expression<Func<T, bool>> BuildPredicate<T>(BuildPredicateModel buildPredicateModel)
        {
            var parameter = Expression.Parameter(typeof(T));
            var left = buildPredicateModel.PropertyName.Split('.').Aggregate((Expression)parameter, Expression.PropertyOrField);
            var body = MakeComparison(left, buildPredicateModel.Comparison, buildPredicateModel.Value, buildPredicateModel.ValueType);
            return Expression.Lambda<Func<T, bool>>(body, parameter);
        }

        /// <summary>
        /// type değiştirme metodu
        /// </summary>
        /// <param name="value"></param>
        /// <param name="conversion"></param>
        /// <returns></returns>
        public static object ChangeType(object value, Type conversion)
        {
            var t = conversion;

            if (t.IsGenericType && t.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
            {
                if (value == null)
                {
                    return null;
                }

                t = Nullable.GetUnderlyingType(t);
            }

            return Convert.ChangeType(value, t);
        }

        /// <summary>
        /// Karlışaltırma ifadesini oluşturan metod
        /// </summary>
        /// <param name="left"></param>
        /// <param name="comparison"></param>
        /// <param name="value"></param>
        /// <param name="valueType"></param>
        /// <returns></returns>

        private static Expression MakeComparison(Expression left, string comparison, object value, Type valueType)
        {
            if (valueType == null) throw new ArgumentNullException(nameof(valueType));
            var constant = Expression.Constant(ChangeType(value, left.Type), left.Type);
            var zero = Expression.Constant(0);
            MethodCallExpression result = null;
            if (constant.Type == typeof(string))
            {
                var method = constant.Type.GetMethod("CompareTo", new[] { typeof(string) });
                result = Expression.Call(left, method, constant);
            }
            //Karşılaştırma tipine göre ifadeyi belirler
            switch (comparison)
            {
                case "IsEqualTo":
                    {
                        if (constant.Type != typeof(string))
                            return Expression.MakeBinary(ExpressionType.Equal, left, constant);
                        else
                            return Expression.MakeBinary(ExpressionType.Equal, result, zero);
                    }
                case "IsNotEqualTo":
                    {
                        if (constant.Type != typeof(string))
                            return Expression.MakeBinary(ExpressionType.NotEqual, left, constant);
                        else
                            return Expression.MakeBinary(ExpressionType.NotEqual, result, zero);
                    }
                case "IsGreaterThan":
                    {
                        if (constant.Type != typeof(string))
                            return Expression.MakeBinary(ExpressionType.GreaterThan, left, constant);
                        else
                            return Expression.MakeBinary(ExpressionType.GreaterThan, result, zero);
                    }
                case "IsGreaterThanOrEqualTo":
                    {
                        if (constant.Type != typeof(string))
                            return Expression.MakeBinary(ExpressionType.GreaterThanOrEqual, left, constant);
                        else
                            return Expression.MakeBinary(ExpressionType.GreaterThanOrEqual, result, zero);
                    }
                case "IsLessThan":
                    {
                        if (constant.Type != typeof(string))
                            return Expression.MakeBinary(ExpressionType.LessThan, left, constant);
                        else
                            return Expression.MakeBinary(ExpressionType.LessThan, result, zero);
                    }
                case "IsLessThanOrEqualTo":
                    {
                        if (constant.Type != typeof(string))
                            return Expression.MakeBinary(ExpressionType.LessThanOrEqual, left, constant);
                        else
                            return Expression.MakeBinary(ExpressionType.LessThanOrEqual, result, zero);
                    }
                case "DoesNotContain":
                    {
                        if (value is string)
                        {
                            comparison = "Contains";
                            return Expression.Not(Expression.Call(left, comparison, Type.EmptyTypes, constant));
                        }
                        throw new NotSupportedException($"Comparison operator '{comparison}' only supported on string.");
                    }
                case "Contains":
                case "StartsWith":
                case "EndsWith":
                case "IsNull":
                case "IsNotNull":
                case "IsEmpty":
                case "IsNotEmpty":
                case "IsNullOrEmpty":
                    if (value is string)
                    {
                        return Expression.Call(left, comparison, Type.EmptyTypes, constant);
                    }
                    throw new NotSupportedException($"Comparison operator '{comparison}' only supported on string.");
                default:
                    throw new NotSupportedException($"Invalid comparison operator '{comparison}'.");
            }
        }

        /// <summary>
        /// Filtre ifadelerini 've' bağlacıyla bağlar
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expressions"></param>
        /// <returns></returns>

        private static Expression<Func<T, bool>> AndAll<T>(List<Expression<Func<T, bool>>> expressions)
        {
            if (expressions == null)
            {
                throw new ArgumentNullException(nameof(expressions));
            }
            if (!expressions.Any())
            {
                return t => true;
            }

            var combined = expressions
                               .Aggregate((e1, e2) => And(e1, e2));

            return Expression.Lambda<Func<T, bool>>(combined.Body, false, combined.Parameters);
        }

        /// <summary>
        /// Filtre ifadelerini 'veya' bağlacıyla bağlar
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expressions"></param>
        /// <returns></returns>

        private static Expression<Func<T, bool>> OrAll<T>(List<Expression<Func<T, bool>>> expressions)
        {
            if (expressions == null)
            {
                throw new ArgumentNullException(nameof(expressions));
            }
            if (!expressions.Any())
            {
                return t => true;
            }

            var combined = expressions
                               .Aggregate((e1, e2) => Or(e1, e2));

            return Expression.Lambda<Func<T, bool>>(combined.Body, false, combined.Parameters);
        }

        /// <summary>
        /// İki  ifadeyi 'veya' bağlacıyla bağlar
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expression1"></param>
        /// <param name="expression2"></param>
        /// <returns></returns>

        private static Expression<Func<T, bool>> Or<T>(Expression<Func<T, bool>> expression1, Expression<Func<T, bool>> expression2)
        {
            InvocationExpression invocationExpression = Expression.Invoke((Expression)expression2, expression1.Parameters.Cast<Expression>());
            return Expression.Lambda<Func<T, bool>>((Expression)Expression.OrElse(expression1.Body, (Expression)invocationExpression), (IEnumerable<ParameterExpression>)expression1.Parameters);
        }

        /// <summary>
        /// İki bir ifadeyi 've' bağlacıyla bağlar
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expression1"></param>
        /// <param name="expression2"></param>
        /// <returns></returns>

        private static Expression<Func<T, bool>> And<T>(Expression<Func<T, bool>> expression1, Expression<Func<T, bool>> expression2)
        {
            InvocationExpression invocationExpression = Expression.Invoke((Expression)expression2, expression1.Parameters.Cast<Expression>());
            return Expression.Lambda<Func<T, bool>>((Expression)Expression.AndAlso(expression1.Body, (Expression)invocationExpression), (IEnumerable<ParameterExpression>)expression1.Parameters);
        }

        

        /// <summary>
        /// Static ifadeler için where sorgusu oluşturan metod
        /// </summary>
        /// <param name="query"></param>
        /// <param name="comparisonType"></param>
        /// <param name="buildPredicateModelList"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static IQueryable<T> WhereBuilder<T>(this IQueryable<T> query, ComparisonType comparisonType, List<BuildPredicateModel> buildPredicateModelList)
        {
            return query.Where(ExpressionUtils.BuildPredicateMultiple<T>(comparisonType, buildPredicateModelList));
        }

        /// <summary>
        /// Dinamik ifadeler için where sorgusu oluşturan metod
        /// </summary>
        /// <param name="query"></param>
        /// <param name="comparisonType"></param>
        /// <param name="buildPredicateModelList"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static IQueryable<T> WhereBuilder<T>(this IQueryable<T> query, ComparisonType comparisonType, List<BuildPredicateModelForDynamic> buildPredicateModelList)
        {
            return query.Where(ExpressionUtils.BuildPredicateMultiple<T>(comparisonType, buildPredicateModelList));
        }
    }
}