using System;
using System.Collections.Generic;
using ProtoBuf;
using UnityEngine;

namespace UniSave.Containers
{
    [ProtoContract]
    public sealed class ComponentContainer : Container
    {
        [ProtoMember(1)] public int TypeIndex { get; set; }

        private ComponentContainer(Type componentType)
        {
            Members = new List<MemberContainer>();
            TypeIndex = OptimizationContainer.AddType(componentType);
        }

        public static ComponentContainer CreateContainer(Component component)
        {
            var container = new ComponentContainer(component.GetType());

            GetMembers(component, container, component.GetType());

            return container;
        }

        public static void Unpack(ComponentContainer container, GameObject obj, GameObjectContainer objectContainer)
        {
            var typeName = OptimizationContainer.GetTypeName(container.TypeIndex);

            // Check if component type still exists.
            if (Type.GetType(typeName) == null)
            {
                Debug.LogWarning(String.Format("UniSave: Component type [{0}] doesn't exist anymore. It has either been renamed or have its namespace changed.", OptimizationContainer.GetTypeName(container.TypeIndex)));
                return;
            }

            Component comp = obj.GetComponent(Type.GetType(typeName));

            if (comp == null)
            {
                if (objectContainer.WasInstantiated)
                {
                    comp = obj.AddComponent(Type.GetType(typeName));
                }

                else
                {
                    return;
                }
            }

            foreach (var member in container.Members)
            {
                SetMembers(comp, member);
            }
        }

        // For ProtoBuf deserialization.
        private ComponentContainer()
        { 
        }
	}
}
