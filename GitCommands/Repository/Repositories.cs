﻿using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace GitCommands.Repository
{
    public static class Repositories
    {
        private static RepositoryHistory _repositoryHistory;
        private static BindingList<RepositoryCategory> _repositoryCategories;

        public static RepositoryHistory RepositoryHistory
        {
            get { return _repositoryHistory ?? (_repositoryHistory = new RepositoryHistory()); }
            set { _repositoryHistory = value; }
        }

        public static BindingList<RepositoryCategory> RepositoryCategories
        {
            get { return _repositoryCategories ?? (_repositoryCategories = new BindingList<RepositoryCategory>()); }
            set { _repositoryCategories = value; }
        }

        public static string SerializeRepositories()
        {
            try
            {
                var sw = new StringWriter();
                var serializer = new XmlSerializer(typeof (BindingList<RepositoryCategory>));
                serializer.Serialize(sw, RepositoryCategories);
                return sw.ToString();
            }
            catch
            {
                return null;
            }
        }

        public static void DeserializeRepositories(string xml)
        {
            try
            {
                var serializer = new XmlSerializer(typeof (BindingList<RepositoryCategory>));
                using (var stringReader = new StringReader(xml))
                using (var xmlReader = new XmlTextReader(stringReader))
                {
                    var obj = serializer.Deserialize(xmlReader) as BindingList<RepositoryCategory>;
                    if (obj != null)
                    {
                        RepositoryCategories = obj;

                        foreach (var repositoryCategory in RepositoryCategories)
                        {
                            repositoryCategory.SetIcon();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex.Message);
            }
        }

        public static string SerializeHistoryIntoXml()
        {
            try
            {
                var sw = new StringWriter();
                var serializer = new XmlSerializer(typeof (RepositoryHistory));
                serializer.Serialize(sw, RepositoryHistory);
                return sw.ToString();
            }
            catch
            {
                return null;
            }
        }

        public static void DeserializeHistoryFromXml(string xml)
        {
            try
            {
                var serializer = new XmlSerializer(typeof (RepositoryHistory));
                using (var stringReader = new StringReader(xml))
                using (var xmlReader = new XmlTextReader(stringReader))
                {
                    var obj = serializer.Deserialize(xmlReader) as RepositoryHistory;
                    if (obj != null)
                    {
                        RepositoryHistory = obj;
                        RepositoryHistory.SetIcon();
                    }
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex.Message);
            }
        }

        public static void AddCategory(string title)
        {
            RepositoryCategories.Add(new RepositoryCategory {Description = title});
        }
    }
}