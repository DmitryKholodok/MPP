using System.Reflection;
using System;

namespace Lab5.Loader.impl
{
    class AssemblyLoader : ILoader
    {
        public object Load(string filename)
        {
            Assembly ass;
            try
            {
                ass = Assembly.LoadFrom(filename);
            }
            catch(Exception)
            {
                return null; 
            }
            return ass;
        }
    }
}
