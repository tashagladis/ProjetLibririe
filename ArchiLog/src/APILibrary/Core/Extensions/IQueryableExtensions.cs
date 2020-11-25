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



        public static IQueryable<T> SelectModel<T>(this IQueryable<T> query, string[] fields) where T : ModelBase
        {
            var parameter = Expression.Parameter(typeof(T), "x");
            // Recuperer les parametres de T

            var membersExpression = fields.Select(y => Expression.Property(parameter, y));
            // On selectionne les paramtres de de fiedls present dans T

            var membersAssignment = membersExpression.Select(z => Expression.Bind(z.Member, z));
            //On assigne ses parametres comme des membres

            var body = Expression.MemberInit(Expression.New(typeof(T)), membersAssignment);
            // créé un T et lui assigne des membres

            var lambda = Expression.Lambda<Func<T, T>>(body, parameter);

            return query.Select(lambda);

            // cette liste renvoi une liste de colonnes fields et les autres colonnes apparaissent aussi avec la valeur null
        }

        public static IQueryable<dynamic> SelectDynamic<T>(this IQueryable<T> query, string[] fields) where T : ModelBase
        {
            var parameter = Expression.Parameter(typeof(T), "x");

            var membersExpression = fields.Select(y => Expression.Property(parameter, y));

            var membersAssignment = membersExpression.Select(z => Expression.Bind(z.Member, z));

            var body = Expression.MemberInit(Expression.New(typeof(T)), membersAssignment);

            var lambda = Expression.Lambda<Func<T, dynamic>>(body, parameter);

            return query.Select(lambda);
        }


        public static IQueryable<T> SelectColonnesAsc<T>(IQueryable<T> query, string[] field) where T : ModelBase
        {

            //lamba 2
            var parameter = Expression.Parameter(typeof(T));
            var property = Expression.Property(parameter, field[0]);
            // Créer une propriétée
            var body = Expression.Convert(property, typeof(object));

            var lambda = Expression.Lambda<Func<T, object>>(body, parameter);

            // on créé la lambda2 pour la deuxième colonne du tableau
            var parameter2 = Expression.Parameter(typeof(T));
            var property2 = Expression.Property(parameter2, field[1]);
            var body2 = Expression.Convert(property2, typeof(object));

            var lambda2 = Expression.Lambda<Func<T, object>>(body2, parameter2);


            return query.OrderBy(lambda).ThenBy(lambda2);
            // thenBy Pour la deuxième colonne




        }



        public static IQueryable<T> SelectColonnesDesc<T>(IQueryable<T> query, string[] field) where T : ModelBase
        {

            //lambda1
            var parameter = Expression.Parameter(typeof(T));
            var property = Expression.Property(parameter, field[0]);
            var body = Expression.Convert(property, typeof(object));

            var lambda = Expression.Lambda<Func<T, object>>(body, parameter);

            // on créé la lambda2 pour  
            var parameter2 = Expression.Parameter(typeof(T));
            var property2 = Expression.Property(parameter, field[1]);
            var body2 = Expression.Convert(property2, typeof(object));

            var lambda2 = Expression.Lambda<Func<T, object>>(body2, parameter2);

            // appel de mes deux lambdas

            return query.OrderByDescending(lambda).ThenBy(lambda2);
            //thenBy Pour la deuxième colonne


        }


        public static IQueryable<T> SelectColonnesName<T>(IQueryable<T> query, string field, string value) where T : ModelBase
        {
                // J'extrait ma chaine de caractère pour vérifier pour testé sa valeur
                  //var valueOut = value.StartsWith("*") ? value.Substring(1) : value;
                  //valueOut = valueOut.EndsWith("*") ? value.Substring(0, valueOut.Length - 1) : valueOut;
                //: value.EndsWith("*") ? value.Substring(0, value.Length - 1)
                 var valueOut = value.Replace("*", "");

                 var parameter = Expression.Parameter(typeof(T));
                // Accéder a la propriété de type field
                var property = Expression.Property(parameter, field);
                Expression val = Expression.Constant(valueOut);
                //transformer la valeur value en Expression
                var body = Expression.Equal(property, val);

                var lambda = Expression.Lambda<Func<T, bool>>(body, parameter);
                return query.Where(lambda);

        }

        public static IQueryable<T> SelectColonnesGender<T>(IQueryable<T> query, string field, string[] value) where T : ModelBase
        {

            // lambada 1
            var parameter = Expression.Parameter(typeof(T));
            var property = Expression.Property(parameter, field);
            // Accéder a la propriété de type field


            // Expression PropOb = Expression.Convert(property, typeof(object));

            Expression val = Expression.Constant(value[0]);
            //transformer la valeur value en Expression

            var body = Expression.Equal(property, val);


            // lambada 2
            var property2 = Expression.Property(parameter, field);
            // Accéder a la propriété de type field

            // Expression PropOb = Expression.Convert(property, typeof(object));

            Expression val2 = Expression.Constant(value[1]);
            //transformer la valeur value en Expression

            var body2 = Expression.Equal(property2, val2);


            //Recupère le resultat de nos deux expressions lambda
            BinaryExpression bodies = Expression.Or(body,body2);

            var lambdaAll = Expression.Lambda<Func<T, bool>>(bodies, parameter);

            return query.Where(lambdaAll);


        }




    }
}
