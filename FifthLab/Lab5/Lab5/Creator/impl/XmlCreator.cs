using Lab5.Entity;
using System.Collections.Generic;
using System.Xml;

namespace Lab5.Creator.impl
{
    public class XmlCreator : ICreator
    {
        public object Create(object obj)
        {
            List<ClassData> classDataList = (List<ClassData>)obj;

            XmlDocument xmlDoc = new XmlDocument();

            XmlNode rootNode = xmlDoc.CreateElement("Assembly");
            xmlDoc.AppendChild(rootNode);

            foreach(var classData in classDataList)
            {
                XmlNode node = CreateXmlNode(classData, xmlDoc);
                rootNode.AppendChild(node);                
            }

            //xmlDoc.Save("test-doc.xml");
            return xmlDoc;
        }

        XmlNode CreateXmlNode(ClassData classData, XmlDocument xmlDoc)
        {
            XmlNode mainNode = CreateNamespace(classData, xmlDoc);
            mainNode.AppendChild(CreateClassName(classData, xmlDoc));
            mainNode.AppendChild(CreateInheritances(classData, xmlDoc));
            mainNode.AppendChild(CreateClassFields(classData, xmlDoc));
            mainNode.AppendChild(CreateMethods(classData, xmlDoc));               
            return mainNode;
        }

        XmlNode CreateMethods(ClassData classData, XmlDocument xmlDoc)
        {
            XmlNode mainNode = xmlDoc.CreateElement("Methods");
            foreach (var method in classData.Methods)
            {
                XmlNode workNode = xmlDoc.CreateElement("Method");
                CreateAttribute("name", method.Name, workNode, xmlDoc);
                CreateAttribute("return_type", method.ReturnType.Name, workNode, xmlDoc);
                CreateAttribute("access_modifier", method.AccessModifier, workNode, xmlDoc);
                XmlNode varsNode = xmlDoc.CreateElement("Vars");
                workNode.AppendChild(varsNode);
                foreach(var param in method.Vars)
                {
                    XmlNode varNode = xmlDoc.CreateElement("Var");
                    varsNode.AppendChild(varNode);
                    CreateAttribute("name", param.Name, varNode, xmlDoc);
                    CreateAttribute("type", param.Type.Name, varNode, xmlDoc);
                }
                mainNode.AppendChild(workNode);
            }
            return mainNode;
        }

        void CreateAttribute(string attrName, string attrValue, XmlNode node, XmlDocument xmlDoc)
        {
            XmlAttribute attr = xmlDoc.CreateAttribute(attrName);
            attr.Value = attrValue;
            node.Attributes.Append(attr);
        }

        List<XmlNode> CreateSimpleFieldsNodes(ClassData classData, XmlDocument xmlDoc)
        {
            List<XmlNode> list = new List<XmlNode>();
            foreach (var simpleField in classData.SimpleFields)
            {
                XmlNode workNode = xmlDoc.CreateElement("Field");
                CreateAttribute("name", simpleField.Name, workNode, xmlDoc);
                CreateAttribute("type", simpleField.Type.Name, workNode, xmlDoc);
                CreateAttribute("access_modifier", simpleField.AccessModifier, workNode, xmlDoc);
                list.Add(workNode);
            }
            return list;
        }

        List<XmlNode> CreateLinkFieldsNodes(ClassData classData, XmlDocument xmlDoc)
        {
            List<XmlNode> list = new List<XmlNode>();
            foreach (var linkField in classData.LinkFields)
            {
                XmlNode mainNode = xmlDoc.CreateElement("Field");
                CreateAttribute("name", linkField.Name, mainNode, xmlDoc);
                CreateAttribute("access_modifier", linkField.AccessModifier, mainNode, xmlDoc);
                if (linkField.Type != null)
                {
                    XmlNode linkNode = CreateXmlNode(linkField.Type, xmlDoc);
                    mainNode.AppendChild(linkNode);
                }
                list.Add(mainNode);
            }
            return list;
        }

        XmlNode CreateClassFields(ClassData classData, XmlDocument xmlDoc)
        {
            XmlNode mainNode = xmlDoc.CreateElement("Fields");
            foreach(var node in CreateSimpleFieldsNodes(classData, xmlDoc))
            {
                mainNode.AppendChild(node);
            }
            foreach(var node in CreateLinkFieldsNodes(classData, xmlDoc))
            {
                mainNode.AppendChild(node);
            }
            return mainNode;
        }

        XmlNode CreateNamespace(ClassData classData, XmlDocument xmlDoc)
        {
            XmlNode mainNode = xmlDoc.CreateElement("Namespace");
            CreateAttribute("name", classData.Namespace, mainNode, xmlDoc);
            return mainNode;
        }

        XmlNode CreateClassName(ClassData classData, XmlDocument xmlDoc)
        {
            XmlNode workNode = xmlDoc.CreateElement("Class");
            workNode.InnerText = classData.ClassName;
            return workNode;
        }

        XmlNode CreateInheritances(ClassData classData, XmlDocument xmlDoc)
        {
            XmlNode mainNode = xmlDoc.CreateElement("Inheritances");
            foreach(var inheritanceName in classData.Inheritances)
            {
                XmlNode workNode = xmlDoc.CreateElement("Inheritance");
                workNode.InnerText = inheritanceName;
                mainNode.AppendChild(workNode);
            }          
            return mainNode;
        }
    }
}
