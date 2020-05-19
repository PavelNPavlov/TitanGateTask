using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using WebsiteWebApI.Data.Contracts;
using WebsiteWebApI.Data.CrudAPIModels.Input;
using WebsiteWebApI.Data.Repository.Contracts;
using WebsiteWebApI.DataModels.BaseModels;

namespace WebsiteWebApI.Data
{
    public class BaseDataService<TCreate, TEdit, TEntityModel, TEntity, TContext> : IBaseDataService<TCreate, TEdit, TEntityModel, TEntity, TContext>
        where TEntity : BaseDbModel
        where TContext : DbContext
        where TEntityModel : class
    {
        protected readonly IGenericRepository<TContext, TEntity> genericRepository;
        protected readonly IMapper mapper;
        protected readonly string IdPropertyName;
        protected readonly ApplicationDbContext modelContext;

        protected const string AscSortingCode = "asc";
        protected const string DescSortingCode = "desc";

        public BaseDataService(IGenericRepository<TContext, TEntity> genericRepository,
            IMapper mapper,
            ApplicationDbContext modelContext)
        {
            this.genericRepository = genericRepository;
            this.mapper = mapper;
            this.modelContext = modelContext;

            var className = typeof(TEntity).Name;
            this.IdPropertyName = genericRepository.GetPrimaryKey();
        }

        public virtual TEntityModel GetItemById(GetInputModel id)
        {
            var item = this.genericRepository.GetByID(id.Id);

            if (item.IsDeleted)
            {
                return null;
            }
            var result = this.mapper.Map<TEntityModel>(item);
            return result;
        }

        public virtual IList<TEntityModel> GetItems(IList<FilterModel> filters, SortModel sortModel, int page = 1, int pageSize = 20)
        {
            var currentItems = this.genericRepository.All();

            if (filters != null)
            {
                foreach (var item in filters)
                {
                    var condition = (Expression<Func<TEntity, bool>>)this.CreateExpressionForFilter(typeof(TEntity), item);
                    currentItems = currentItems.Where(condition);
                }
            }

            var filteredItems = this.mapper.ProjectTo<TEntityModel>(currentItems).ToList();

            IOrderedEnumerable<TEntityModel> orderedList;
            if (sortModel != null)
            {
                orderedList = this.SortEntities(filteredItems, sortModel);
            }
            else
            {
                orderedList = this.SortEntities(filteredItems, new SortModel { Direction = AscSortingCode, Prop = nameof(BaseDbModel.Id) });
            }

            var skipItemsCount = (page - 1) * pageSize;

            var pagedItems = pageSize <= 0 ?
                orderedList : // get all, because no page size is pecified
                orderedList
                    .Skip(skipItemsCount)
                    .Take(pageSize);

            var result = pagedItems.ToList();

            return result;
        }

        protected virtual IOrderedEnumerable<TEntityModel> SortEntities(IList<TEntityModel> items, SortModel sortModel)
        {
            var sortRule = sortModel;
            var propName = sortRule.Prop;
            var propInfo = typeof(TEntityModel).GetProperty(propName);

            IOrderedEnumerable<TEntityModel> orderedItems;

            if (sortModel.Direction == DescSortingCode)
            {
                orderedItems = items.OrderByDescending(x => propInfo.GetValue(x, null));
            }
            else
            {
                orderedItems = items.OrderBy(x => propInfo.GetValue(x, null));
            }


            return orderedItems;
        }

        protected virtual Expression CreateExpressionForFilter(Type TEntityType, FilterModel filter)
        {
                       //Expression<Func<TEntity, bool>> condition = null;
            Expression condition;

            var proppertyDotSeparatedPath = filter.Prop;

            if (filter.Prop.ToUpperInvariant() == "ID")
            {
                proppertyDotSeparatedPath = this.IdPropertyName;
            }

            var propPathToEvaluate = proppertyDotSeparatedPath;

            var paramEntity = Expression.Parameter(TEntityType, TEntityType.Name.Substring(0, 1).ToLowerInvariant() + TEntityType.Name.Substring(1));


            Expression exppressionOfPropertyOrFunction = paramEntity;

            while (propPathToEvaluate.Length > 0)
            {
                var dotIndex = propPathToEvaluate.IndexOf('.', StringComparison.InvariantCulture);
                var propertyName = string.Empty;

                if (dotIndex >= 0)
                {
                    propertyName = propPathToEvaluate.Substring(0, dotIndex);
                    propPathToEvaluate = propPathToEvaluate.Substring(dotIndex + 1);
                }
                else
                {
                    propertyName = propPathToEvaluate;
                    propPathToEvaluate = string.Empty;
                }

                // Add current Propery to the expression
                exppressionOfPropertyOrFunction = Expression.Property((Expression)exppressionOfPropertyOrFunction, propertyName);

                if (exppressionOfPropertyOrFunction.Type.IsGenericType && (exppressionOfPropertyOrFunction.Type.IsValueType == false))
                {
                    // If a property is a Generic Collection then use <current_property_expression>.Any(<the_rest_of_the_filter_property_path>)
                    var subFilter = new FilterModel
                    {
                        Prop = propPathToEvaluate,
                        Value = filter.Value,
                        Comparison = filter.Comparison
                    };

                    Expression subFilterExpression = CreateExpressionForFilter(exppressionOfPropertyOrFunction.Type.GenericTypeArguments[0], subFilter);
                    propPathToEvaluate = "";

                    // find Generic "Any" method with proper number of parameneters
                    var methodAnyGeneric = typeof(Enumerable).GetMethods(BindingFlags.Static | BindingFlags.Public).First(m => m.Name == "Any" && m.GetParameters().Length == 2);
                    var methodAnySpecialized = methodAnyGeneric.MakeGenericMethod(exppressionOfPropertyOrFunction.Type.GenericTypeArguments[0]);

                    // Expression paramEntity.Any(exppressionOfPropertyOrFunction => <subFilterExpression that uses exppressionOfPropertyOrFunction>)
                    var func = Expression.Lambda(
                        Expression.Call(methodAnySpecialized, new Expression[] { exppressionOfPropertyOrFunction, subFilterExpression }), // call to "Any(<sub_filter_expression>)"
                        new ParameterExpression[] { paramEntity }); // object on witch to execute the "object.Any(<sub_filter_expression>)" call
                    exppressionOfPropertyOrFunction = func;
                    filter.Comparison = "Any"; // used to switch to "Any" variant later
                }
            }

            var propType = exppressionOfPropertyOrFunction.Type;

            var entityIdParam = paramEntity; // Expression.Parameter(TEntityType);
            Expression entityIdProperty = exppressionOfPropertyOrFunction;

            MethodInfo containsMethod = null;
            UnaryExpression converted = null;
            Expression constProperty = Expression.Constant(filter.Value);

            if (filter.Comparison != "oneOf" && filter.Comparison != "notOneOf" && filter.Comparison != "Any")
            {
                var converted2 = Expression.Convert(Expression.Constant(filter.Value), propType);
                constProperty = converted2;
                converted = Expression.Convert(constProperty, propType);
                //var newTestType = propType.IsGenericType ? propType.GenericTypeArguments[0] : propType;
                //converted = Expression.Convert(constProperty, newTestType);
            }
            else
            {
                if (propType == typeof(string))
                {
                    constProperty = Expression.Constant(((string)filter.Value).Split("|").ToList());
                    containsMethod = typeof(ICollection<string>).GetMethod("Contains");
                }
                else if (propType == typeof(long))
                {
                    constProperty = Expression.Constant(((string)filter.Value).Split("|").Select(x => long.Parse(x)).ToList());
                    containsMethod = typeof(ICollection<long>).GetMethod("Contains");
                }
                else if (propType == typeof(decimal))
                {
                    constProperty = Expression.Constant(((string)filter.Value).Split("|").Select(x => decimal.Parse(x)).ToList());
                    containsMethod = typeof(ICollection<decimal>).GetMethod("Contains");
                }
                else if (propType == typeof(int))
                {
                    constProperty = Expression.Constant(((string)filter.Value).Split("|").Select(x => int.Parse(x)).ToList());
                    containsMethod = typeof(ICollection<int>).GetMethod("Contains");
                }
                else if (propType == typeof(double))
                {
                    constProperty = Expression.Constant(((string)filter.Value).Split("|").Select(x => double.Parse(x)).ToList());
                    containsMethod = typeof(ICollection<double>).GetMethod("Contains");
                }

            }

            if (filter.Comparison == "equals")
            {
                condition = Expression.Lambda(Expression.Equal(entityIdProperty, converted), entityIdParam);
            }
            else if (filter.Comparison == "contains")
            {
                var pattern = Expression.Constant($"%{filter.Value}%");
                var fucn = Expression.Call(typeof(DbFunctionsExtensions), "Like", Type.EmptyTypes, Expression.Constant(EF.Functions), entityIdProperty, pattern);
                containsMethod = typeof(string).GetMethod("Contains", new[] { typeof(string), typeof(StringComparison) });

                condition = Expression.Lambda(fucn, entityIdParam);
            }
            else if (filter.Comparison == "notContains")
            {
                var pattern = Expression.Constant($"%{filter.Value}%");
                var fucn = Expression.Call(typeof(DbFunctionsExtensions), "Like", Type.EmptyTypes, Expression.Constant(EF.Functions), entityIdProperty, pattern);
                containsMethod = typeof(string).GetMethod("Contains", new[] { typeof(string), typeof(StringComparison) });

                condition = Expression.Lambda(Expression.Not(fucn), entityIdParam);
            }
            else if (filter.Comparison == "not")
            {
                condition = Expression.Lambda(Expression.NotEqual(entityIdProperty, converted), entityIdParam);
            }
            else if (filter.Comparison == "greater")
            {
                condition = Expression.Lambda(Expression.GreaterThan(entityIdProperty, converted), entityIdParam);
            }
            else if (filter.Comparison == "greaterOrEqual")
            {
                condition = Expression.Lambda(Expression.GreaterThanOrEqual(entityIdProperty, converted), entityIdParam);
            }
            else if (filter.Comparison == "smaller")
            {
                condition = Expression.Lambda(Expression.GreaterThanOrEqual(converted, entityIdProperty), entityIdParam);
            }
            else if (filter.Comparison == "smallerOrEqual")
            {
                condition = Expression.Lambda(Expression.GreaterThanOrEqual(converted, entityIdProperty), entityIdParam);
            }
            else if (filter.Comparison == "oneOf")
            {

                condition = Expression.Lambda(Expression.Call(constProperty, containsMethod, entityIdProperty), entityIdParam);
            }
            else if (filter.Comparison == "notOneOf")
            {

                condition = Expression.Lambda(Expression.Not(Expression.Call(constProperty, containsMethod, entityIdProperty)), entityIdParam);
            }
            else if (filter.Comparison == "Any")
            {

                condition = exppressionOfPropertyOrFunction;
            }
            else
            {
                condition = Expression.Lambda(Expression.Constant(true), entityIdParam);
            }

            return condition;
        }

        public virtual TEntityModel CreateItem(TCreate model)
        {
            var item = this.mapper.Map<TEntity>(model);
            this.genericRepository.Create(item);
            //this.genericRepository.SaveChanges();

            var result = this.mapper.Map<TEntityModel>(item);
            return result;
        }

        public virtual void CreateItemWithNoReturn(TCreate model)
        {
            var item = this.mapper.Map<TEntity>(model);
            this.genericRepository.Create(item);
           
            return;
        }

        public virtual TEntityModel EditItem(TEdit model)
        {
            var idProperty = typeof(TEdit).GetProperty("Id");
            var id = Convert.ToDecimal(idProperty.GetValue(model));

            var currentItem = this.genericRepository.GetByID(id);

            this.mapper.Map<TEdit, TEntity>(model, currentItem);

            //var item = this.mapper.Map<TEntity>(model);
            this.genericRepository.Edit(currentItem);
            this.genericRepository.SaveChanges();

            var result = this.mapper.Map<TEntityModel>(currentItem);
            return result;
        }


        public virtual void EditItemNoReturn(TEntity model)        
        {            
            this.genericRepository.Edit(model);
            this.genericRepository.SaveChanges();
        }

        public virtual void DeleteItem(DeleteInputModel item)
        {            
            this.genericRepository.Delete(item.Id);
            this.genericRepository.SaveChanges();
        }

        public void SaveChanges()
        {
            this.genericRepository.SaveChanges();
        }
    }
}
