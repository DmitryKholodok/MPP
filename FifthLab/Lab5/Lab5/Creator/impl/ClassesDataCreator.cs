using Lab5.Creator.Manager;
using Lab5.Entity;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Lab5.Creator.impl
{
    // in - assembly
    public class ClassesDataCreator : ICreator
    {
        CreatorManager manager;

        public object Create(object obj)
        {
            Assembly assembly = (Assembly)obj;
            
            Type[] types = assembly.GetTypes();
            manager = new CreatorManager(types);
            return createListOfClassData(types);
        }

        List<ClassData> createListOfClassData (Type[] types)
        {
            List<ClassData> classes = new List<ClassData>();
            ClassDataCreator creator = new ClassDataCreator(manager);
            foreach (var type in types)
            {
                if (!manager.WasMentioned(type))
                {
                    ClassData classData = (ClassData)creator.Create(type);
                    if (classData != null) // good attribute
                    {
                        classes.Add(classData);
                    }
                }
            }
            return classes;
        }

        
    }
}
