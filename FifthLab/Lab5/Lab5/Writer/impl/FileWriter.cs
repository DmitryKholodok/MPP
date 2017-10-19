using System.Xml;

namespace Lab5.Writer.impl
{
    public class FileWriter : IWriter
    {
        const string PATH = "doc.xml";

        public void Write(object obj)
        {
            ((XmlDocument)obj).Save(PATH);
        }
    }
}
