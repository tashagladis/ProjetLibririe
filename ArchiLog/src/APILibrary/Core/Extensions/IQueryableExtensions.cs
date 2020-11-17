using APILibrary.Core.Attributs;
using APILibrary.Core.Models;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace APILibrary.Core.Extensions
{
    public static class IQueryableExtensions
    {
        public static object SelectObject(object value, string[] fields)
        {
            var expo = new ExpandoObject() as IDictionary<string, object>;
            var collectionType = value.GetType();

            foreach (var field in fields)
            {
                var prop = collectionType.GetProperty(field, BindingFlags.Public |
                    BindingFlags.IgnoreCase | BindingFlags.Instance);
                if (prop != null)
                {
                    var isPresentAttribute = prop.CustomAttributes
                         .Any(x => x.AttributeType == typeof(NotJsonAttribute));
                    if (!isPresentAttribute)
                        expo.Add(prop.Name, prop.GetValue(value));
                }
                else
                {
                    throw new Exception($"Property {field} does not exist.");
                }
            }
            return expo;
        }

        public static IQueryable<dynamic> SortDynamic<T>(this IQueryable<T> query, string[] asc) where T : ModelBase
        {
            var parameter = Expression.Parameter(typeof(T), "x");

            var membersExpression = asc.Select(y => Expression.Property(parameter, y));

            var membersAssignment = membersExpression.Select(z => Expression.Bind(z.Member, z));

            var body = Expression.MemberInit(Expression.New(typeof(T)), membersAssignment);


            var lambda = Expression.Lambda<Func<T, dynamic>>(body, parameter);

            return query.Select(lambda);
        }

        
        public static IQueryable<T> SelectModel<T>(this IQueryable<T> query, string[] fields) where T : ModelBase
        {
            var parameter = Expression.Parameter(typeof(T), "x");
            // Recuperer les parametres de TMODEL

            var membersExpression = fields.Select(y => Expression.Property(parameter, y));
            // On selectionne les paramtres de de fiedls present dans TModel

            var membersAssignment = membersExpression.Select(z => Expression.Bind(z.Member, z));
            //On assigne ses parametres comme des membres

            var body = Expression.MemberInit(Expression.New(typeof(T)), membersAssignment);
            // créé un T et lui assigne des membres

            var lambda = Expression.Lambda<Func<T, T>>(body, parameter);

            return query.Select(lambda);
        }


    }
}
