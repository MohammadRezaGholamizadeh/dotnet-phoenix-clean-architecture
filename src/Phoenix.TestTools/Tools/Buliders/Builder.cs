using Phoenix.DataSources.Infrastructures.DBContexts;
using System.Linq.Expressions;
using System.Reflection;

namespace Phoenix.TestTools.Tools.Buliders
{
    public class Builder<TEntity> where TEntity : class
    {
        public object? entity =
            Activator.CreateInstance(typeof(TEntity));
        public Builder<TEntity> With(
               Expression<Func<TEntity, object?>> property,
               object? value = null)
        {
            var propertyBodyType = property.Body.GetType();
            var propertyName = string.Empty;
            if (propertyBodyType.GetProperty("Operand") != null)
            {
                var operand =
                 property.Body
                         .GetType()
                         .GetProperty("Operand")?
                         .GetValue(property.Body);
                propertyName =
                    (operand?.GetType().GetProperty("Member")?
                     .GetValue(operand) as MemberInfo)?.Name;
            }
            else
            {
                propertyName =
                    (property.Body
                             .GetType()
                             .GetProperty("Member")?
                             .GetValue(property.Body) as MemberInfo)?.Name;
            }

            typeof(TEntity).GetProperty(propertyName)?.SetValue(entity, value);
            return this;
        }

        public TEntity Build()
        {
            return (TEntity)entity;
        }

        public async Task<TEntity> BuildAndSaveInDataBase(
            EFDataContext context)
        {
            await context.Add((TEntity)entity)
                         .Context.SaveChangesAsync();
            return (TEntity)entity;
        }
    }
}
