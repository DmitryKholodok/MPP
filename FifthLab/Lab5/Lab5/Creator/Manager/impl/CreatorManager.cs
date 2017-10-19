using System;
using System.Collections.Generic;

namespace Lab5.Creator.Manager
{
    public class CreatorManager : IManager
    {
        Dictionary<Type, bool> mentionMap = new Dictionary<Type, bool>();
        public Type[] Types { get; private set; }

        void InitTypeBoolMap(Type[] types)
        {
            foreach (Type type in types)
            {
                mentionMap.Add(type, false);
            }
        }

        public CreatorManager(Type[] types)
        {
            Types = types;
            InitTypeBoolMap(types);
        }

        public bool WasMentioned(Type type) 
        {
            return (mentionMap[type] == true) ? true : false;  
        }

        public void MarkType(Type type)
        {
            mentionMap[type] = true;
        }
    }
}
