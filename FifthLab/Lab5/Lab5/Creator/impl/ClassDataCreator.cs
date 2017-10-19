using Lab5.Creator.Manager;
using Lab5.Entity;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Lab5.Creator
{
    public class ClassDataCreator : ICreator
    {
        CreatorManager manager;
        const string APPROPRIATE_ATTR = "ExportXML";
        const BindingFlags BF  = 
            (BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.DeclaredOnly | BindingFlags.Instance);

        public ClassDataCreator(CreatorManager manager)
        {
            this.manager = manager;
        }

        public object Create(object obj)
        {
            Type type = (Type)obj;
            if (manager.WasMentioned(type)) return null;
            manager.MarkType(type);
            if (!CheckClassAttribute(type)) return null;
            return FillClassData(type);
        }

        private ClassData FillClassData(Type type)
        {
            ClassData classData = new ClassData();
            classData.Namespace = IdentifyNamespace(type);
            classData.ClassName = IdentifyClassName(type);
            classData.Inheritors = IdentifyInheritances(type);
            classData.Methods = IdentifyMethods(type);
            FillClassFields(classData, type);
            return classData;
        }

        bool IsClass(FieldInfo fi)
        {
            foreach(var type in manager.Types)
            {
                if (type.Name == fi.FieldType.Name) return true;
            }
            return false;
        }

        ClassData.SimpleField FillSimpleField(FieldInfo field)
        {
            ClassData.SimpleField sf = new ClassData.SimpleField();
            sf.Name = field.Name;
            sf.Type = field.FieldType;
            sf.AccessModifier = field.Attributes.ToString();
            return sf;
        }

        ClassData.LinkField FillLinkField(FieldInfo field, ClassData cd)
        {
            ClassData.LinkField lf = new ClassData.LinkField();
            lf.Name = field.Name;
            lf.AccessModifier = field.Attributes.ToString();
            lf.Type = cd;
            return lf;
        }

        void FillClassFields(ClassData classData, Type type)
        {
            List<ClassData.SimpleField> listSF = new List<ClassData.SimpleField>();
            List<ClassData.LinkField> listLF = new List<ClassData.LinkField>();
            foreach(var field in type.GetFields(BF))
            {
                if (IsClass(field))
                {
                    object cd = Create(field.FieldType); 
                    if (cd == null)
                    {
                        listSF.Add(FillSimpleField(field));
                    }
                    listLF.Add(FillLinkField(field, (ClassData)cd));
                }
                else
                {
                    listSF.Add(FillSimpleField(field));
                }
            }
            classData.SimpleFields = listSF;
            classData.LinkFields = listLF;
        }

        bool CheckClassAttribute(Type type)
        {
            foreach(var attr in type.GetCustomAttributes(false))
            {
                if (attr.GetType().Name == typeof(ExportXml).Name) return true;
            }
            return false;

        }

        string IdentifyNamespace(Type type)
        {
            return type.Namespace;
        }

        string IdentifyClassName(Type type)
        {
            return type.Name;
        }

        List<string> IdentifyInheritances(Type type)
        {
            List<string> list = new List<string>();
            foreach(var interf in type.GetInterfaces())
            {
                list.Add(interf.ToString());
            }
            // add classes
            return list;
        }

        ClassData.Var FillVar(ParameterInfo param)
        {
            ClassData.Var var = new ClassData.Var();
            var.Name = param.Name;
            var.Type = param.ParameterType;
            return var;
        }

        List<ClassData.Var> IdentifyMethodVars(MethodInfo method)
        {
            List<ClassData.Var> list = new List<ClassData.Var>();
            foreach(var var in method.GetParameters())
            {
                list.Add(FillVar(var));
            }
            return list;
        }

        string IdentifyAccessModifier(MethodInfo method)
        {
            const string PRIVATE = "Private";
            const string PUBLIC = "Public";
            const string PROTECTED = "Protected";

            if (method.IsPrivate) return PRIVATE;
            if (method.IsPublic) return PUBLIC;
            return PROTECTED;
        }

        ClassData.Method FillMethodClass(MethodInfo method)
        {
            ClassData.Method classMethod = new ClassData.Method();
            classMethod.Name = method.Name;
            classMethod.ReturnType = method.ReturnType;
            classMethod.Vars = IdentifyMethodVars(method);
            classMethod.AccessModifier = IdentifyAccessModifier(method);
            return classMethod;
        }

        List<ClassData.Method> IdentifyMethods(Type type)
        {
            List<ClassData.Method> list = new List<ClassData.Method>();
            MethodInfo[] methodsInfo =
                type.GetMethods(BF);
            foreach(var method in methodsInfo)
            {
                list.Add(FillMethodClass(method));
            }
            return list;
        }

        
    }
}
