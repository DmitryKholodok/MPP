using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab5.Entity
{
    public class ClassData
    {
        public class SimpleField
        {
            public string Name { get; set; }
            public Type Type { get; set; }
            public string AccessModifier { get; set; }
        }

        public class LinkField
        {
            public string Name { get; set; }
            public ClassData Type { get; set; }
            public string AccessModifier { get; set; }
        }

        public class Var
        {
            public string Name { get; set; }
            public Type Type { get; set; }
        }

        public class Method
        {
            public string Name;
            public string AccessModifier;
            public Type ReturnType;
            public List<Var> Vars { get; set; }
        }

        public string Namespace { get; set; }
        public string ClassName { get; set; }
        public List<string> Inheritances { get; set; }
        public List<SimpleField> SimpleFields { get; set; }
        public List<LinkField> LinkFields { get; set; }
        public List<Method> Methods { get; set; }
    }
}
