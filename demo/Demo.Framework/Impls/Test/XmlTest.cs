using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Sean.Utility.Config;
using Sean.Utility.Contracts;

namespace Demo.Framework.Impls.Test
{
    public class XmlTest : ISimpleDo
    {
        private string _xmlFilePath;

        public void Execute()
        {
            _xmlFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"book.xml");

            //Console.WriteLine(XmlHelper.GetXmlAttributeInnerText(_xmlFilePath, "/books/book[@id='2']", "ISDN"));
            //Console.WriteLine(XmlHelper.GetXmlNodeInnerText(_xmlFilePath, "/books/book/author"));
        }

        private void Test1()
        {
            // 1.创建XML文档：
            bool isSuccess = XmlHelper.CreateXmlDocument(_xmlFilePath, "books", "1.0", "utf-8", null);
            Console.WriteLine("创建XML文档成功:" + isSuccess);
        }

        private void Test2()
        {
            // 2.向XML文档中添加一个新节点：
            string xpath = "/books";  //这是新节点的父节点路径
            string nodename = "book";　//这是新节点名称,在父节点下新增
            string nodetext = "这是新节点中的文本值";
            bool isSuccess = XmlHelper.CreateOrUpdateXmlNode(_xmlFilePath, xpath, nodename, nodetext);
            Console.WriteLine("XML节点添加或更新成功:" + isSuccess);
        }
        private void Test3()
        {
            // 3.向XML文档中的子节点中新增或修改（如果存在则修改）一个子节点,比如name,author,date节点等：
            string xpath = "/books/book";  //这是新子节点的父节点路径
            string nodename = "name";　//这是新子节点名称,在父节点下新增
            string nodetext = "我的世界我的梦";
            bool isSuccess = XmlHelper.CreateOrUpdateXmlNode(_xmlFilePath, xpath, nodename, nodetext);
            Console.WriteLine("XML节点添加或更新成功:" + isSuccess);
        }
        private void Test4()
        {
            // 4.向XML文档中的子节点中新增或修改（如果存在则修改）一个子节点属性,比如id,ISDN属性等：
            string xpath = "/books/book"; //要新增属性的节点
            string attributeName = "id";　//新属性名称,ISDN号也是这么新增的
            string attributeValue = "1";　//新属性值
            bool isSuccess = XmlHelper.CreateOrUpdateXmlAttribute(_xmlFilePath, xpath, attributeName, attributeValue);
            Console.WriteLine("XML属性添加或更新成功:" + isSuccess.ToString());
        }
        private void Test5()
        {
            // 5.删除XML文档中的子节点：
            string xpath = "/books/book[@id='1']";
            bool isSuccess = XmlHelper.DeleteXmlNode(_xmlFilePath, xpath);
            Console.WriteLine("XML节点删除成功:" + isSuccess.ToString());
        }
        private void Test6()
        {
            // 6.删除XML文档中子节点的属性：
            string xpath = "/books/book[@id='2']";
            string attributeName = "ISDN";
            bool isSuccess = XmlHelper.DeleteXmlAttribute(_xmlFilePath, xpath, attributeName);
            Console.WriteLine("XML属性删除成功:" + isSuccess.ToString());
        }
        private void Test7()
        {
            // 7.读取XML文档中的所有子节点：
            string xpath = "/books/book[@id='1']";
            XmlNodeList nodeList = XmlHelper.GetXmlNodeList(_xmlFilePath, xpath);
            string strAllNode = string.Empty;
            foreach (XmlNode node in nodeList)
            {
                strAllNode += "\n name:" + node.Name + " InnerText:" + node.InnerText;
            }
            Console.WriteLine("XML节点中所有子节点有:" + strAllNode);
        }
    }
}
