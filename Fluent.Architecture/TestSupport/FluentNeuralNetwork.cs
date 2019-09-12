using Fluent.Architecture.Core.Attributes;
using Fluent.Architecture.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Fluent.Architecture.Core.TestSupport
{
    public class FluentNeuralNetwork
    {
        public List<FluentNode> SortedAggregations { get; } = new List<FluentNode>();

        public Dictionary<Type, FluentNode> DictionaryOfAggregations { get; } = new Dictionary<Type, FluentNode>();

        public List<FluentNode> ExplainToTheTree(List<Type> types, bool setValues)
        {
            types.ForEach(x => ExplainToTheTree(x));

            if (setValues)
            {
                foreach (var item in SortedAggregations)
                {
                    Setvalue(item);
                }
            }

            return SortedAggregations;
        }

        private void Setvalue(FluentNode node)
        {
            foreach (var internalNode in node.ReferencePointers)
            {
                if (node.Instance == null)
                {
                    node.Instance = Setvalue(node.EntityType);
                }
                else
                {
                    return;
                }

                Setvalue(internalNode);
            }

            if (node.Instance == null)
            {
                node.Instance = Setvalue(node.EntityType);
            }
        }

        private object Setvalue(Type type)
        {
            var entity = type.GetExampleValue();
            AddAggegations(entity);
            return entity;
        }

        private void AddAggegations(object entity)
        {
            var list = entity
                            .GetType()
                            .GetProperties()
                            .Select(p =>
                                new
                                {
                                    property = p,
                                    isList = p.PropertyType.Name == "List`1",
                                    type = p.PropertyType.Name == "List`1" ? p.PropertyType.GenericTypeArguments[0] : p.PropertyType,
                                    attr = p.GetCustomAttribute<FluentAggregationAttribute>(true)
                                })
                            .Where(x => x.attr != null)
                            .ToList();

            foreach (var item in list)
            {
                var aggregation = SortedAggregations.Single(x => x.EntityType == item.type);
                object value = aggregation.Instance;

                var externalKeys = item.attr.ExternalKeys;
                var localKeys = item.attr.LocalKeys;

                for (int i = 0; i < externalKeys.Length; i++)
                {
                    var externalKey = externalKeys[i];
                    var localKey = localKeys[i];

                    if (value != null)
                    {
                        var externalValue = item.type.GetProperty(externalKey).GetValue(value);
                        entity.GetType().GetProperty(localKey).SetValue(entity, externalValue);
                    }
                }

                {
                    //Set complex object
                    if (item.isList)
                    {
                        var typeList = typeof(List<>).MakeGenericType(item.type);
                        if (value == null)
                        {
                            value = Activator.CreateInstance(typeList);
                        }
                        else
                        {
                            value = Activator.CreateInstance(typeList, value);
                        }
                    }

                    item.property.SetValue(entity, value);
                }
            }
        }

        private void ExplainToTheTree(Type type, FluentNode parent = null)
        {
            var node = GetTreeNode(type);
            if (node == null)
            {
                node = new FluentNode { EntityType = type };
                DictionaryOfAggregations.Add(type, node);

                var properties = node.EntityType.GetProperties().Where(x => x.GetCustomAttribute<FluentAggregationAttribute>(true) != null).ToList();
                var types = properties.Select(x => x.PropertyType).ToList();
                if (types.Count == 0)
                {
                    node.IsPrimitive = true;
                }
                else
                {
                    types.ForEach(x => ExplainToTheTree(x, node));
                    node.ReferencePointers.Add(node);
                }

                AddAggregation(node);
            }

            if (parent != null)
            {
                parent.ReferencePointers.Add(node);
            }
        }

        private void AddAggregation(FluentNode node)
        {
            Console.WriteLine($"{node.EntityType.Name} Mapped");
            SortedAggregations.Add(node);
        }

        private FluentNode GetTreeNode(Type entityType)
        {
            DictionaryOfAggregations.TryGetValue(entityType, out FluentNode value);
            return value;
        }
    }
}
