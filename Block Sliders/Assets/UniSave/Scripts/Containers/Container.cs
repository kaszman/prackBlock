using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using ProtoBuf;
using UniSave.Attributes;
using UnityEngine;
using Object = UnityEngine.Object;

namespace UniSave.Containers
{
    /// <summary>
    /// Base class for all type of containers.
    /// </summary>
    [
    ProtoContract,
    ProtoInclude(100, typeof(GameObjectContainer)),
    ProtoInclude(101, typeof(ComponentContainer)),
    ProtoInclude(102, typeof(MemberContainer))
    ]
	public abstract class Container
    {
        private List<MemberContainer> _members;
        [ProtoMember(1)] public List<MemberContainer> Members { get { return _members ?? (_members = new List<MemberContainer>()); } protected set { _members = value; }}

        protected static void GetMembers(object parentObject, Container parentMember, Type parentComponent)
        {
            //FIELDS
            foreach (var field in parentComponent.GetFields(BindingFlags.Public | BindingFlags.Instance))
            {
                if (field.IsDefined(typeof(ObsoleteAttribute), false))
                {
                    continue;
                }

                if (field.IsDefined(typeof (UniSaveIgnoreDataMemberAttribute), false))
                {
                    //Debug.Log(String.Format("Ignore attribute found on field: {0}. Skipping.", field.Name));
                    continue;
                }

                // Check for custom excludes
                if (!Check(parentComponent, field))
                {
                    continue;
                }

                var obj = field.GetValue(parentObject);

                //Debug.Log("Component: " + parentComponent.Name + " | Field > " + field.Name + ": " + obj);

                CreateMemberContainer(obj, parentMember, MemberType.Field, field.FieldType, field.Name);
            }

            // PROPERTIES
            foreach (var property in parentComponent.GetProperties(BindingFlags.Public | BindingFlags.Instance).Where(p => p.CanRead && p.CanWrite && p.GetIndexParameters().Length == 0))
            {
                if (property.IsDefined(typeof (ObsoleteAttribute), false))
                {
                    continue;
                }

                if (property.IsDefined(typeof(UniSaveIgnoreDataMemberAttribute), false))
                {
                    //Debug.Log(String.Format("Ignore attribute found on property: {0}. Skipping.", property.Name));
                    continue;
                }

                // Check for custom excludes
                if (!Check(parentComponent, property))
                {
                    continue;
                }

                var obj = property.GetGetMethod(true).Invoke(parentObject, null);

                //Debug.Log("Component: " + parentComponent.Name + " | Property > " + property.Name + ": " + obj);

                CreateMemberContainer(obj, parentMember, MemberType.Property, property.PropertyType, property.Name);
            }
        }

        private static bool Check(Type parentComponent, MemberInfo member)
        {
            if (parentComponent != typeof(GameObject) && (member.DeclaringType == typeof(Component) || member.DeclaringType == typeof(Object)))
            {
                //Debug.Log("SKIP: " + parentComponent.Name + " | " + member.Name);
                return false;
            }

            if (parentComponent == typeof(Camera) && (member.Name == "projectionMatrix" || member.Name == "worldToCameraMatrix"))
            {
                return false;
            }

            if (parentComponent == typeof(Renderer) && (member.Name == "material" || member.Name == "sharedMaterial" ||
                member.Name == "sharedMaterials"))
            {
                return false;
            }

            if (parentComponent == typeof(Collider) && member.Name == "sharedMaterial")
            {
                return false;
            }

            if (parentComponent == typeof(Collider2D) && member.Name == "sharedMaterial")
            {
                return false;
            }

            if (parentComponent == typeof(MeshFilter) && member.Name == "sharedMesh")
            {
                return false;
            }

            if (parentComponent == typeof(MeshCollider) && member.Name == "sharedMesh")
            {
                return false;
            }

            if (typeof(SkinnedMeshRenderer).IsAssignableFrom(parentComponent) && (member.Name == "sharedMesh" || member.Name == "bones"))
            {
                return false;
            }

            if (parentComponent == typeof(Transform) && member.Name == "parent")
            {
                return false;
            }

            return true;
        }

        private static void CreateMemberContainer(object obj, Container parentMember, MemberType memberType, Type type, string name)
        {
            if (obj == null || obj.Equals(null))
            {
                parentMember.AddMember(memberType, type, name, "", true);
            }

            else
            {
                // Array check
                if (obj.GetType().IsArray)
                {
                    //Debug.Log(name + " is an array.");

                    var arr = (Array)obj;

                    if (arr.Rank > 1)
                    {
                        Debug.LogWarning("UniSave: Multidimensional arrays are not supported!");
                        return;
                    }

                    var mo = parentMember.AddMember(memberType, type, name, arr.ToString());

                    var i = 0;
                    foreach (var element in arr)
                    {
                        //Debug.Log(element);                   
                        CreateMemberContainer(element, mo, MemberType.Field, element.GetType(), i.ToString(CultureInfo.InvariantCulture));
                        i++;
                    }

                    return;
                }

                // Collection check
                
                if (obj is ICollection)
                {
                    if (obj is IDictionary)
                    {
                        Debug.LogWarning("UniSave: Hashtable/Dictionary is not supported!");
                        return;
                    }

                    if (obj is IList)
                    {
                        //Debug.Log(name + " is a list.");

                        var list = (ICollection) obj;

                        var mo = parentMember.AddMember(memberType, type, name, list.ToString());

                        var i = 0;
                        foreach (var element in list)
                        {
                            //Debug.Log(element);
                            CreateMemberContainer(element, mo, MemberType.Field, element.GetType(), i.ToString(CultureInfo.InvariantCulture));
                            i++;
                        }

                        return;
                    }
                }

                // COMPONENT REFERENCE
                if (typeof(Component).IsAssignableFrom(type))
                {
                    parentMember.AddMember(memberType, type, name, obj.ToString());
                    //Debug.Log(name + " is a component.");
                }

                // ASSET
                else if (!typeof (Component).IsAssignableFrom(type) && typeof (Object).IsAssignableFrom(type))
                {
                    string objectName = obj.ToString();

                    if (obj.ToString().Contains(" (Instance)"))
                    {
                        objectName = obj.ToString().Replace(" (Instance)", string.Empty);
                    }

                    else if (obj.ToString().Contains(" Instance"))
                    {
                        objectName = obj.ToString().Replace(" Instance", string.Empty);
                    }

                    //Debug.Log(objectName);
                    parentMember.AddMember(memberType, type, name, objectName);

                    //Debug.Log(name + " is an asset.");
                }

                else
                {
                    MemberContainer m = parentMember.AddMember(memberType, type, name, obj.ToString());
                    GetMembers(obj, m, type);
                }
            }
        }

        private MemberContainer AddMember(MemberType type, Type objectType, string name, string value, bool isNull = false)
        {
            var member = MemberContainer.CreateContainer(type, objectType, name, value, isNull);

            Members.Add(member);

            return member;
        }

        protected static void SetMembers(object parentObject, MemberContainer member)
        {
            //Debug.Log(parentObject.GetType().Name + " | " + member.Name + ": " + member.Value);

            MemberInfo memberInfo = null;
            Type memberObjectType = null;

            if (member.Type == MemberType.Field)
            {
                var memberName = OptimizationContainer.GetMemberName(member.NameIndex);

                memberInfo = parentObject.GetType().GetField(memberName);

                // Check if field type still exists
                if (memberInfo == null)
                {
                    Debug.LogWarning(String.Format("UniSave: Field [{0}] could not be found on [{1}].", memberName, parentObject.GetType().Name));
                    return;
                }

                memberObjectType = ((FieldInfo) memberInfo).FieldType;

                if (!OptimizationContainer.TypesAreIdentical(memberObjectType, member.ObjectTypeIndex))
                {
                    Debug.LogWarning(String.Format("UniSave: The data type of field [{0}] on [{1}] was changed.", memberName, parentObject.GetType().Name));
                    return;
                }
            }

            else if (member.Type == MemberType.Property)
            {
                var memberName = OptimizationContainer.GetMemberName(member.NameIndex);

                memberInfo = parentObject.GetType().GetProperty(memberName);

                // Check if property type still exists
                if (memberInfo == null)
                {
                    Debug.LogWarning(String.Format("UniSave: Property [{0}] could not be found on [{1}].", memberName, parentObject.GetType().Name));
                    return;
                }

                memberObjectType = ((PropertyInfo) memberInfo).PropertyType;

                if (!OptimizationContainer.TypesAreIdentical(memberObjectType, member.ObjectTypeIndex))
                {
                    Debug.LogWarning(String.Format("UniSave: The data type of property [{0}] on [{1}] was changed.", memberName, parentObject.GetType().Name));
                    return;
                }
            }

            if (memberInfo != null)
            {
                if (member.IsNull)
                {
                    //Debug.Log(member.Name + " is Null.");
                    SetValue(parentObject, memberInfo, null);
                    return;
                }

                if (memberObjectType.IsArray)
                {
                    //Debug.Log(member.Name + " is an array.");
                    var array = (Array) Activator.CreateInstance((OptimizationContainer.GetType(member)), new object[] { member.Members.Count });

                    var i = 0;
                    foreach (var element in member.Members)
                    {
                        //Debug.Log(element.Name);

                        var elementType = OptimizationContainer.GetType(element);

                        if (element.IsNull)
                        {
                            //Debug.Log("IsNull");
                            array.SetValue(null, i++);
                            continue;
                        }

                        if (elementType.IsPrimitive || elementType.IsEnum || elementType == typeof(string))
                        {
                            //Debug.Log(element.Name + " is a primitive type or an enum.");
                            array.SetValue(Convert.ChangeType(OptimizationContainer.GetValue(element), OptimizationContainer.GetType(element)), i++);
                        }

                        else
                        {
                            // IS ASSET
                            if (!typeof (Component).IsAssignableFrom(elementType) && typeof (Object).IsAssignableFrom(elementType))
                            {
                                //Debug.LogWarning("IS AN ASSET");

                                string[] separator = {" ("};
                                var trimmedElementName = OptimizationContainer.GetValue(element).TrimEnd(')').Split(separator, 2, StringSplitOptions.None);

                                //Debug.Log(element.Value);
                                //Debug.Log(test[0]);
                                //Debug.Log(compName[1]);
                                //Debug.Log(test[1]);

                                var resource = Resources.FindObjectsOfTypeAll(elementType).FirstOrDefault(x => x.name == trimmedElementName[0]);

                                if (resource == null)
                                {
                                    resource = Resources.Load(trimmedElementName[0], elementType);

                                    if (resource == null)
                                    {
                                        Debug.LogWarning(
                                            String.Format(
                                                "UniSave: Asset [{0}] could not be found. Are you sure it's been added to a Resources folder?",
                                                trimmedElementName[0]));
                                        return;
                                    }
                                }

                                array.SetValue(resource, i++);
                            }

                            // IS COMPONENT REFERENCE
                            else if (typeof(Component).IsAssignableFrom(elementType))
                            {
                                string[] separator = { " (" };
                                var trimmedElementName = OptimizationContainer.GetValue(element).TrimEnd(')').Split(separator, 2, StringSplitOptions.None);
                                var componentName = trimmedElementName[1].Split('.');

                                //Debug.Log(element.Value);
                                //Debug.Log(test[0]);
                                //Debug.Log(compName[1]);

                                var myComponent = GameObject.Find(trimmedElementName[0]).GetComponent(componentName[1]);

                                if (myComponent == null)
                                {
                                    Debug.LogWarning(String.Format("UniSave: Component [{0}] on GameObject [{1}] could not be found in the scene.", componentName[1], trimmedElementName[0]));
                                    return;
                                }

                                array.SetValue(myComponent, i++);
                            }

                            else
                            {
                                //Debug.Log(element.Name + " is a class or struct.");
                                var obj = Activator.CreateInstance(OptimizationContainer.GetType(element));

                                foreach (var selement in element.Members)
                                {
                                    SetMembers(obj, selement);
                                }

                                array.SetValue(obj, i++);
                            }
                        }
                    }

                    SetValue(parentObject, memberInfo, array);
                }

                else if (typeof(IList).IsAssignableFrom(memberObjectType))
                {
                    //Debug.Log(member.Name + " is a list.");
                    var list = (IList)Activator.CreateInstance(OptimizationContainer.GetType(member), new object[] { member.Members.Count });

                    foreach (var element in member.Members)
                    {
                        //Debug.Log(element.Name);

                        var elementType = OptimizationContainer.GetType(element);

                        if (element.IsNull)
                        {
                            //Debug.Log("IsNull");
                            list.Add(null);
                            continue;
                        }

                        if (elementType.IsPrimitive || elementType.IsEnum || elementType == typeof(string))
                        {
                            //Debug.Log(element.Name + " is a primitive type or an enum.");
                            list.Add(Convert.ChangeType(OptimizationContainer.GetValue(element), OptimizationContainer.GetType(element)));
                        }

                        // IS ASSET
                        if (!typeof(Component).IsAssignableFrom(elementType) && typeof(Object).IsAssignableFrom(elementType))
                        {
                            //Debug.LogWarning("IS AN ASSET");

                            string[] separator = { " (" };
                            var trimmedElementName = OptimizationContainer.GetValue(element).TrimEnd(')').Split(separator, 2, StringSplitOptions.None);

                            //Debug.Log(element.Value);
                            //Debug.Log(test[0]);
                            //Debug.Log(compName[1]);
                            //Debug.Log(test[1]);

                            var resource = Resources.FindObjectsOfTypeAll(elementType).FirstOrDefault(x => x.name == trimmedElementName[0]);

                            if (resource == null)
                            {
                                resource = Resources.Load(trimmedElementName[0], elementType);

                                if (resource == null)
                                {
                                    Debug.LogWarning(
                                        String.Format(
                                            "UniSave: Asset [{0}] could not be found. Are you sure it's been added to a Resources folder?",
                                            trimmedElementName[0]));
                                    return;
                                }
                            }

                            list.Add(resource);
                        }

                        // IS COMPONENT REFERENCE
                        else if (typeof(Component).IsAssignableFrom(elementType))
                        {
                            string[] separator = { " (" };
                            var trimmedElementName = OptimizationContainer.GetValue(element).TrimEnd(')').Split(separator, 2, StringSplitOptions.None);
                            var componentName = trimmedElementName[1].Split('.');

                            //Debug.Log(element.Value);
                            //Debug.Log(test[0]);
                            //Debug.Log(compName[1]);

                            var myComponent = GameObject.Find(trimmedElementName[0]).GetComponent(componentName[1]);

                            if (myComponent == null)
                            {
                                Debug.LogWarning(String.Format("UniSave: Component [{0}] on GameObject [{1}] could not be found in the scene.", componentName[1], trimmedElementName[0]));
                                return;
                            }

                            list.Add(myComponent);
                        }

                        else
                        {
                            //Debug.Log(element.Name + " is a class or struct.");
                            var obj = Activator.CreateInstance(OptimizationContainer.GetType(element));

                            foreach (var selement in element.Members)
                            {
                                SetMembers(obj, selement);
                            }

                            list.Add(obj);
                        }
                    }

                    SetValue(parentObject, memberInfo, list);
                }

                else if (member.Members.Count > 0)
                {
                    //Debug.Log(member.Name + " has child members.");

                    foreach (var childMember in member.Members)
                    {
                        var obj = GetValue(parentObject, memberInfo);
                        SetMembers(obj, childMember);
                        SetValue(parentObject, memberInfo, obj);
                    }
                }

                // IS ASSET
                else if (!typeof(Component).IsAssignableFrom(memberObjectType) && typeof(Object).IsAssignableFrom(memberObjectType))
                {
                    //Debug.LogWarning("IS AN ASSET");

                    string[] separator = { " (" };
                    var trimmedMemberName = OptimizationContainer.GetValue(member).TrimEnd(')').Split(separator, 2, StringSplitOptions.None);

                    //Debug.Log(member.Value);
                    //Debug.Log(test[0]);
                    //Debug.Log(compName[1]);
                    //Debug.Log(test[1]);

                    var resource = Resources.FindObjectsOfTypeAll(memberObjectType).FirstOrDefault(x => x.name == trimmedMemberName[0]);

                    if (resource == null)
                    {
                        resource = Resources.Load(trimmedMemberName[0], memberObjectType);

                        if (resource == null)
                        {
                            Debug.LogWarning(String.Format("UniSave: Asset [{0}] could not be found. Are you sure it's been added to a Resources folder?", trimmedMemberName[0]));
                            return;
                        }
                    }

                    SetValue(parentObject, memberInfo, resource);
                }

                // IS COMPONENT REFERENCE
                else if (typeof(Component).IsAssignableFrom(memberObjectType))
                {
                    string[] separator = { " (" };
                    var trimmedMemberName = OptimizationContainer.GetValue(member).TrimEnd(')').Split(separator, 2, StringSplitOptions.None);
                    var componentName = trimmedMemberName[1].Split('.');

                    //Debug.Log(member.Value);
                    //Debug.Log(trimmedMemberName[0]);
                    //Debug.Log(componentName[1]);

                    var myComponent = GameObject.Find(trimmedMemberName[0]).GetComponent(componentName[1]);

                    if (myComponent == null)
                    {
                        Debug.LogWarning(String.Format("UniSave: Component [{0}] on GameObject [{1}] could not be found in the scene.", componentName[1], trimmedMemberName[0]));
                        return;
                    }

                    SetValue(parentObject, memberInfo, myComponent);
                }

                else
                {
                    // Check if data type of the member was changed after saving.
                    if (memberObjectType != OptimizationContainer.GetType(member))
                    {
                        Debug.LogWarning("UniSave: Member / Data Type mismatch!");

                    }

                    else
                    {
                        SetValue(parentObject, memberInfo, Convert.ChangeType(OptimizationContainer.GetValue(member), OptimizationContainer.GetType(member)));
                    }
                }

            }
        }

        private static void SetValue(object parentObject, MemberInfo member, object value)
        {
            if (member is FieldInfo)
            {
                var field = (FieldInfo)member;
                field.SetValue(parentObject, value);
            }

            if (member is PropertyInfo)
            {
                var property = (PropertyInfo)member;
                property.GetSetMethod(true).Invoke(parentObject, new[] { value });
            }
        }

        private static object GetValue(object parentObject, MemberInfo member)
        {
            if (member is FieldInfo)
            {
                var field = (FieldInfo) member;
                return field.GetValue(parentObject);
            }

            if (member is PropertyInfo)
            {
                var property = (PropertyInfo) member;
                return property.GetGetMethod(true).Invoke(parentObject, null);
            }

            return null;
        }
    }
}
