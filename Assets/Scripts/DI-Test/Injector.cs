using System;
using UnityEngine;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;

namespace Sandbox.DI
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Method | AttributeTargets.Property)]
    public sealed class InjectAttribute : Attribute { }

    [AttributeUsage(AttributeTargets.Method)]
    public sealed class ProvideAttribute : Attribute { }

    public interface IDependencyProvider { }

    [DefaultExecutionOrder(-1000)]
    internal class Injector : MonoBehaviour
    {
        public bool logEnabled = false;

        const BindingFlags m_bindingFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
        readonly Dictionary<Type, object> registry = new Dictionary<Type, object>();

        void Awake()
        {
            var monobehaviour = FindMonoBehaviours();

            var providers = monobehaviour.OfType<IDependencyProvider>();
            foreach (var provider in providers)
                RegisterProvider(provider);

            var injectables = FindMonoBehaviours().Where(IsInjectable);
            foreach (var injectable in injectables)
                Inject(injectable);
        }

        void Inject(object instance)
        {
            var type = instance.GetType();

            var injectableFields = type.GetFields(m_bindingFlags)
                .Where(member => Attribute.IsDefined(member, typeof(InjectAttribute)));
            foreach (var injectableField in injectableFields)
            {
                var fieldType = injectableField.FieldType;
                var resolvedInstance = Resolve(fieldType);

                if (resolvedInstance == null)
                    throw new Exception($"Failed to inject {fieldType.Name} into {type.Name}");

                injectableField.SetValue(instance, resolvedInstance);

                if (logEnabled)
                    Debug.Log($"Field Injected {fieldType.Name} into {type.Name}");
            }

            var injectableMethods = type.GetMethods(m_bindingFlags)
                .Where(member => Attribute.IsDefined(member, typeof(InjectAttribute)));
            foreach (var injectableMethod in injectableMethods)
            {
                var requiredParam = injectableMethod.GetParameters()
                    .Select(parameter => parameter.ParameterType).ToArray();

                var resolvedInstances = requiredParam.Select(Resolve).ToArray();
                if (resolvedInstances.Any(resolvedInstance => resolvedInstance == null))
                    throw new Exception($"Failed to inject {type.Name}.{injectableMethod.Name}");

                injectableMethod.Invoke(instance, resolvedInstances);

                if (logEnabled)
                    Debug.Log($"Method Injected {type.Name}.{injectableMethod.Name}");
            }

            var injectableProperties = type.GetProperties(m_bindingFlags)
                .Where(member => Attribute.IsDefined(member, typeof(InjectAttribute)));
            foreach (var injectableProperty in injectableProperties)
            {
                var propertyType = injectableProperty.PropertyType;
                var resolvedInstance = Resolve(propertyType);

                if (resolvedInstance == null)
                    throw new Exception($"Failed to inject {propertyType.Name} into {type.Name}");

                injectableProperty.SetValue(instance, resolvedInstance);

                if (logEnabled)
                    Debug.Log($"Property Injected {propertyType.Name} into {type.Name}");
            }
        }

        object Resolve(Type type)
        {
            registry.TryGetValue(type, out var resolvedInstance);
            return resolvedInstance;
        }

        static bool IsInjectable(MonoBehaviour obj)
        {
            var member = obj.GetType().GetMembers(m_bindingFlags);
            return member.Any(x => Attribute.IsDefined(x, typeof(InjectAttribute)));
        }

        void RegisterProvider(IDependencyProvider provider)
        {
            var methods = provider.GetType().GetMethods(m_bindingFlags);

            foreach (var method in methods)
            {
                if (!Attribute.IsDefined(method, typeof(ProvideAttribute))) continue;

                var returnType = method.ReturnType;
                var providedInstance = method.Invoke(provider, null);
                if (providedInstance != null)
                {
                    registry.Add(returnType, providedInstance);
                    if (logEnabled) Debug.Log($"Registered {returnType.Name} from {provider.GetType().Name}");
                }
                else
                    throw new Exception($"Provider {provider.GetType().Name} returned null for {returnType.Name}");
            }
        }

        static MonoBehaviour[] FindMonoBehaviours() => FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.InstanceID);
    }
}