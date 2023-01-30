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
            var propertyName =
                 (property.Body
                          .GetType()
                          .GetProperty("Member")?
                          .GetValue(property.Body) as MemberInfo)?.Name;

            typeof(TEntity).GetProperty(propertyName)?.SetValue(entity, value);
            return this;
        }

        public TEntity Build()
        {
            return (TEntity)entity;
        }
    }
}
