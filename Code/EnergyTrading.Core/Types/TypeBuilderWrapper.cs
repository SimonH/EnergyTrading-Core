using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;

namespace EnergyTrading.Types
{
    public class TypeBuilderWrapper
    {
        private readonly TypeBuilder _builder;
        private readonly IDictionary<string, object> _propertyValues = new Dictionary<string, object>();
        private readonly MethodAttributes getSetAtts = MethodAttributes.Public | MethodAttributes.HideBySig;

        internal TypeBuilderWrapper(TypeBuilder builder)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }
            this._builder = builder;
        }

        private void AddProperty(string propertyName, Type propertyType)
        {
            var propBuilder = _builder.DefineProperty(propertyName, PropertyAttributes.None, propertyType, new[] { propertyType });
            var fieldBuilder = _builder.DefineField($"_{propertyName}", propertyType, FieldAttributes.Private);
            var getMethodBuilder = _builder.DefineMethod($"get_{propertyName}", getSetAtts, propertyType, new Type[] { });
            var getIlGenerator = getMethodBuilder.GetILGenerator();
            getIlGenerator.Emit(OpCodes.Ldarg_0);
            getIlGenerator.Emit(OpCodes.Ldfld, fieldBuilder);
            getIlGenerator.Emit(OpCodes.Ret);

            var setMethodBuilder = _builder.DefineMethod($"set_{propertyName}", getSetAtts, null, new[] { propertyType });
            var setIlGenerator = setMethodBuilder.GetILGenerator();
            setIlGenerator.Emit(OpCodes.Ldarg_0);
            setIlGenerator.Emit(OpCodes.Ldarg_1);
            setIlGenerator.Emit(OpCodes.Stfld, fieldBuilder);
            setIlGenerator.Emit(OpCodes.Ret);

            propBuilder.SetGetMethod(getMethodBuilder);
            propBuilder.SetSetMethod(setMethodBuilder);
        }

        public TypeBuilderWrapper WithProperty(string name, Type type, object value)
        {
            AddProperty(name, type);
            _propertyValues.Add(name, value);
            return this;
        }

        public TypeBuilderWrapper WithProperty<T>(string name)
        {
            return WithProperty<T>(name, default(T));
        }

        public TypeBuilderWrapper WithProperty<T>(string name, T value)
        {
            return WithProperty(name, typeof(T), value);
        }


        public Type Type()
        {
            return _builder.CreateType();
        }

        public object Instance()
        {
            var type = Type();
            var instance = Activator.CreateInstance(type);
            foreach (var pair in _propertyValues)
            {
                type.GetProperty(pair.Key).SetValue(instance, pair.Value);
            }
            return instance;
        }
    }
}