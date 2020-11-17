using APILibrary.Core.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Reflection;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;


namespace APILibrary.Core.Attributs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //pourqoui l'heritage de controleur api
    public abstract class ControllerBaseAPI<T, TContext> : ControllerBase where T : ModelBase where TContext: DbContext
    {

        protected readonly TContext _context;

        public ControllerBaseAPI(TContext context)
        {
            this._context = context;
            //A QUOI SERT LE DBCONTEXT
        }

        //?fields=email,phone



        //[ProducesResponseType((int)HttpStatusCode.OK)]
        //[HttpGet]
        //public virtual async Task<ActionResult<IEnumerable<T>>> GetAllAsync()
        //{
        //    var results = await _context.Set<T>().ToListAsync();
        //    return Ok(ToJsonList(results));
        //}

        //[ProducesResponseType((int)HttpStatusCode.OK)]
        //[ProducesResponseType((int)HttpStatusCode.NotFound)]
        //[HttpGet("{id}")]
        //public virtual async Task<ActionResult<T>> GetById([FromRoute] int id)
        //{
        //    T result = await _context.Set<T>().FindAsync(id);
        //    if (result != null)
        //    {
        //        return Ok(result);
        //    }
        //    else
        //    {
        //        return NotFound(new { Message = $"ID {id} not found" });
        //    }
        //}


        [ProducesResponseType((int)HttpStatusCode.OK)]
        [HttpGet]
        public virtual async Task<ActionResult<IEnumerable<dynamic>>> GetAllAsync([FromQuery] string fields)
        {
            //solutiuon 2
            var tab = fields.Split(',');
            var parameter = Expression.Parameter(typeof(T), "x");

            var membersExpression = tab.Select(y => Expression.Property(parameter, y));

            var membersAssignment = membersExpression.Select(z => Expression.Bind(z.Member, z));

            var body = Expression.MemberInit(Expression.New(typeof(T)), membersAssignment);

            var lambda = Expression.Lambda<Func<T, dynamic>>(body, parameter);

            var query = _context.Set<T>().AsQueryable();

            var results = await query.Select(lambda).ToListAsync();

            //return result;

            //var results = await _context.Set<T>().ToListAsync();
            //solution 1
            if (!string.IsNullOrWhiteSpace(fields))
            {

                var tabFields = fields.Split(',');
                var tabNew = results.Select((x) =>
                {
                    var expo = new ExpandoObject() as IDictionary<string, object>;
                    var collectionType = typeof(T);

                    foreach (var field in tabFields)
                    {
                        var prop = collectionType.GetProperty(field, BindingFlags.Public |
                            BindingFlags.IgnoreCase | BindingFlags.Instance);
                        if (prop != null)
                        {
                            //solution 1B
                            /*var isPresentAttribute = prop.CustomAttributes
                                .Any(x => x.AttributeType == typeof(NotJsonAttribute));
                            if(!isPresentAttribute)*/
                            expo.Add(prop.Name, prop.GetValue(x));
                        }
                        else
                        {
                            throw new Exception($"Property {field} does not exist.");
                        }
                    }


                    return expo;
                });
                //solution 1A
                return Ok(ToJsonList(tabNew));
            }
            //fin solution 1

            return Ok(ToJsonList(results));
        }

        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [HttpGet("{id}")]
        public virtual async Task<ActionResult<T>> GetById([FromRoute] int id, [FromQuery] string fields)
        {
            //solution 2: optimisation de la requete SQL
            var tab = new List<string>(fields.Split(','));
            if (!tab.Contains("id"))
                tab.Add("id");

            var parameter = Expression.Parameter(typeof(T), "x");

            var membersExpression = tab.Select(y => Expression.Property(parameter, y));

            var membersAssignment = membersExpression.Select(z => Expression.Bind(z.Member, z));

            var body = Expression.MemberInit(Expression.New(typeof(T)), membersAssignment);

            var lambda = Expression.Lambda<Func<T, T>>(body, parameter);

            var query = _context.Set<T>().AsQueryable();

            var result = query.Select(lambda).SingleOrDefault(x => x.ID == id);


            //return resultat;


            //solution 1 : 
            //T result = await _context.Set<T>().FindAsync(id);
            if (result != null)
            {

                if (!string.IsNullOrWhiteSpace(fields))
                {
                    var tabFields = fields.Split(',');

                    var expo = new ExpandoObject() as IDictionary<string, object>;
                    var modelType = typeof(T);

                    foreach (var field in tabFields)
                    {
                        var prop = modelType.GetProperty(field, BindingFlags.Public |
                            BindingFlags.IgnoreCase | BindingFlags.Instance);
                        if (prop != null)
                        {
                            //solution 1B
                            /*var isPresentAttribute = prop.CustomAttributes
                                .Any(x => x.AttributeType == typeof(NotJsonAttribute));
                            if(!isPresentAttribute)*/
                            expo.Add(prop.Name, prop.GetValue(result));
                        }
                        else
                        {
                            throw new Exception($"Property {field} does not exist.");
                        }
                    }
                    //solution 1B
                    return Ok(ToJson(expo));
                }
                else
                    return Ok(ToJson(result));
            }
            else
            {
                return NotFound(new { Message = $"ID {id} not found" });
            }
        }





        [ProducesResponseType((int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [HttpPost]
        public virtual async Task<ActionResult<T>> CreateItem([FromBody] T item)
        {

            if (ModelState.IsValid)
            {
                _context.Add(item);
                await _context.SaveChangesAsync();

                return item;
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [HttpPut("{id}")]
        public virtual async Task<ActionResult<T>> UpdateItem([FromRoute] int id, [FromBody] T item)
        {
            if (id != item.ID)
            {
                return BadRequest();
            }

            bool result = await _context.Set<T>().AnyAsync(x => x.ID == id);
            if (!result)
                return NotFound(new { Message = $"ID {id} not found" });

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update<T>(item);
                    await _context.SaveChangesAsync();
                    return Ok(item);
                }
                catch (Exception e)
                {
                    return BadRequest(new { e.Message });
                }
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [HttpDelete("{id}")]
        public virtual async Task<ActionResult<T>> DeleteItem([FromRoute] int id)
        {
            T item = await _context.Set<T>().FindAsync(id);
            if (item == null)
                return NotFound();

            _context.Remove<T>(item);

            try
            {
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (Exception e)
            {
                return BadRequest(new { e.Message });
            }

            /*int result = await _context.SaveChangesAsync();
            if(result != 0)
            {
                return NoContent();
            }
            else
            {
                return BadRequest();
            }*/
        }


        protected IEnumerable<dynamic> ToJsonList(IEnumerable<dynamic> tab)
        {
            var tabNew = tab.Select((x) =>
            {
                return ToJson(x);
            });
            return tabNew;
        }

        protected dynamic ToJson(dynamic item)
        {
            var expo = new ExpandoObject() as IDictionary<string, object>;

            var collectionType = typeof(T);

            IDictionary<string, object> dico = item as IDictionary<string, object>;
            if (dico != null)
            {
                foreach (var propDyn in dico)
                {
                    var propInTModel = collectionType.GetProperty(propDyn.Key, BindingFlags.Public |
                            BindingFlags.IgnoreCase | BindingFlags.Instance);

                    var isPresentAttribute = propInTModel.CustomAttributes
                    .Any(x => x.AttributeType == typeof(NotJsonAttribute));

                    if (!isPresentAttribute)
                        expo.Add(propDyn.Key, propDyn.Value);
                }
            }
            else
            {
                foreach (var prop in collectionType.GetProperties())
                {
                    var isPresentAttribute = prop.CustomAttributes
                    .Any(x => x.AttributeType == typeof(NotJsonAttribute));

                    if (!isPresentAttribute)
                        expo.Add(prop.Name, prop.GetValue(item));
                }
            }
            return expo;
        }
    }
}
