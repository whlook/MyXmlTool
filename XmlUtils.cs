﻿using System;
using System.IO;
using System.Xml;
using System.Linq;
using System.Text;
using System.Collections;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace XmlTool
{
    class XmlUtils
    {
        // xml操作对象
        private static XmlDocument xmlDoc = null;

        // 根节点
        private static XmlElement root = null; 

        #region 创建xml文档
        /// <summary>
        /// 创建一个新的xml文档，如果该文档已经存在，则创建失败
        /// </summary>
        /// <param name="filePath">xml文档创建的完整路径，包括文件名</param>
        /// <returns></returns>
        public static bool CreateXml(string filePath)
        {
    
            if(!File.Exists(filePath)) // 如果不存在该文件，则创建，并生成根节点
            {
                xmlDoc = new XmlDocument();

                root = xmlDoc.CreateElement("root");

                xmlDoc.AppendChild(root);

                xmlDoc.Save(filePath);

                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion

        #region 载入xml文档
        /// <summary>
        /// 载入xml文档，如果该文档不存在，则载入失败
        /// </summary>
        /// <param name="filePath">xml文档载入的完整路径，包括文件名</param>
        /// <returns></returns>
        public static bool LoadXml(string filePath)
        {
            if(!File.Exists(filePath)) // 如果不存在，则载入失败
            {
                return false;
            }
            else
            {
                if( xmlDoc == null )
                {
                    xmlDoc = new XmlDocument();

                    xmlDoc.Load(filePath);

                    root = (XmlElement)xmlDoc.SelectSingleNode("root"); // 读取根节点

                    if (root == null)
                        return false;

                    return true;
                }
                else
                {
                    xmlDoc.Load(filePath);

                    root =(XmlElement)xmlDoc.SelectSingleNode("root"); // 读取根节点

                    if (root == null)
                        return false;

                    return true;
                }
            }
        }
        #endregion

        #region 保存xml文档
        /// <summary>
        /// 保存xml文档到指定路径,如果未创建或未载入则保存失败
        /// </summary>
        /// <param name="filePath">xml文档保存的完整路径，包括文件名</param>
        public static bool SaveXml(string filePath)
        {
            if (xmlDoc == null)
                return false;
           
            xmlDoc.Save(filePath);

            return true;
        }
        #endregion

        #region 在xml文档中写入值
        /// <summary>
        /// 在指定行列位置修改值，如果位置不存在，则添加，在此之前应该先创建或载入xml，否则失败
        /// </summary>
        /// <param name="rowKeyName">行首关键字，不存在则添加</param>
        /// <param name="colKeyName">列首关键字，不存在则添加</param>
        /// <param name="value"></param>
        public static bool SetValue(string rowKey,string colKey,string value)
        {
            if(xmlDoc==null)
            {
                return false;
            }
            else
            {
                XmlElement row = (XmlElement)root.SelectSingleNode(rowKey); // 获得指定的行标签

                if(row == null) // 如果不存在则创建
                {
                   
                    XmlElement newRow = xmlDoc.CreateElement(rowKey);

                    XmlElement newCol = xmlDoc.CreateElement(colKey);

                    XmlElement val = xmlDoc.CreateElement("value");

                    val.InnerText = value;

                    newCol.AppendChild(val);

                    newRow.AppendChild(newCol);

                    root.AppendChild(newRow);

                    return true;
                }
                else
                {
                   
                    foreach(XmlElement col in row.ChildNodes) // 查找指定的列标签，不存在则创建
                    {
                       
                        if(col.Name == colKey)
                        {
                            col.FirstChild.InnerText = value;

                            return true;
                        }
                    }

                    XmlElement newCol = xmlDoc.CreateElement(colKey);

                    XmlElement val = xmlDoc.CreateElement("value");

                    val.InnerText = value;

                    newCol.AppendChild(val);

                    row.AppendChild(newCol);

                    return true;
                }
                
            }
        }
        #endregion

        #region 读取xml中的值
        /// <summary>
        /// 以字符串格式读取指定行列的值，如果该位置不存在，则读取失败
        /// </summary>
        /// <param name="rowKey">指定的行关键字</param>
        /// <param name="colKey">指定的列关键字</param>
        /// <param name="val">保存读取值</param>
        /// <returns></returns>
        public static bool ReadXml(string rowKey, string colKey, ref string val)
        {
            if (xmlDoc == null)
            {
                return false;
            }
            else
            {
                XmlElement row = (XmlElement)root.SelectSingleNode(rowKey); // 获得指定的行标签，不存在则读取失败

                if (row == null)
                {
                    return false;
                }
                else
                {

                    foreach (XmlElement col in row.ChildNodes) // 查找指定的列标签，不存在则读取失败
                    {

                        if (col.Name == colKey)
                        {
                            val = col.FirstChild.InnerText;

                            return true;
                        }
                    }

                    return false;
                }

            }
        }
        #endregion

        #region 删除xml中的某个值
        /// <summary>
        /// 删除指定位置的值（即列），位置不存在则删除失败
        /// </summary>
        /// <param name="rowKey"></param>
        /// <param name="colKey"></param>
        /// <returns></returns>
        public static bool DeleteValue(string rowKey, string colKey)
        {
            if (xmlDoc == null)
            {
                return false;
            }
            else
            {
                XmlElement row = (XmlElement)root.SelectSingleNode(rowKey); // 获得指定的行标签，不存在则删除失败

                if (row == null)
                {
                    return false;
                }
                else
                {

                    foreach (XmlElement col in row.ChildNodes) // 获得指定的列标签，不存在则删除失败
                    {

                        if (col.Name == colKey)
                        {
                            row.RemoveChild(col);

                            return true;
                        }
                    }

                    return false;
                }

            }
        }

        #endregion
    }
}