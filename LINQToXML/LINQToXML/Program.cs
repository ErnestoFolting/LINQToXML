using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using LINQToXML.Structure;
using System.Globalization;

namespace LINQToXML
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // CREATE PERSONS
            Person person1 = new Person("Pylypenko", "Nazar", 23);
            Person person2 = new Person("Ostapenko", "Taras", 27);
            Person person3 = new Person("Bagrych", "Ostap", 34);
            Person person4 = new Person("Hevlaha", "Stepan", 18);
            Person person5 = new Person("Gevnych", "Franklin", 45);

            // CREATE PROJECTS
            Project project1 = new Project("123-ASO", "1.Formula Prom", 23400,
                new DateTime(2016, 5, 28),
                new DateTime(2023, 3, 17),
                new List<Person>
                {
                    person1, person2, person3
                });

            Project project2 = new Project("823-HSO", "2.Boom Dear", 178000.5,
                new DateTime(2014, 7, 13),
                new DateTime(2022, 6, 7),
                new List<Person>
                {
                    person2, person3, person5
                });

            Project project3 = new Project("261-IHO", "3.Formula Pear", 1089000,
                new DateTime(2017, 9, 10),
                new DateTime(2024, 8, 23),
                new List<Person>
                {
                    person1, person4
                });
            Project project4 = new Project("098-HOG", "4.Update Sphere", 17000,
                new DateTime(2019, 8, 9),
                new DateTime(2020, 8, 24),
                new List<Person>
                {
                    person4, person5
                });

            // CREATE FACTORIES
            Factory factory1 = new Factory("1.Wool Products", new List<Project> { project1, project3 });
            Factory factory2 = new Factory("2.Hanry Laundries", new List<Project> { project2 });
            Factory factory3 = new Factory("3.Mekha Weapons", new List<Project> { project4 });
            List<Factory> factories = new List<Factory> { factory1, factory2, factory3 };
            List<Person> people = new List<Person> { person1, person2, person3, person4, person5 };
            List<Project> projects = new List<Project> { project1, project2, project3, project4 };

            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            using(XmlWriter writer = XmlWriter.Create("Storage.xml", settings))
            {
                writer.WriteStartElement("factories");
                foreach(var factory in factories)
                {
                    writer.WriteStartElement("factory");

                    writer.WriteElementString("name",factory.name);

                    writer.WriteStartElement("projects");
                    foreach (var project in factory.projects)
                    {
                        writer.WriteStartElement("project");

                        writer.WriteElementString("code", project.code);
                        writer.WriteElementString("name", project.name);
                        writer.WriteElementString("cost", project.cost.ToString());
                        writer.WriteElementString("startTime", project.startTime.ToString());
                        writer.WriteElementString("endTime", project.endTime.ToString());

                        writer.WriteStartElement("participants");
                        foreach (var person in project.participants)
                        {
                            writer.WriteStartElement("person");
                            writer.WriteElementString("surname", person.surname);
                            writer.WriteElementString("name", person.name);
                            writer.WriteElementString("age", person.age.ToString());
                            writer.WriteEndElement();
                        }
                        
                        writer.WriteEndElement();
                        

                        writer.WriteEndElement();
                    }
                    writer.WriteEndElement();

                    writer.WriteEndElement();
  
                }
                writer.WriteEndElement();
            }
            XmlDocument doc = new XmlDocument();
            doc.Load("storage.xml");
            var format = new NumberFormatInfo() { NumberDecimalSeparator = ".", };
            foreach (XmlNode factory in doc.DocumentElement)
            {
                string factoryName = factory["name"].InnerText;
                Console.WriteLine("----------The {0} factory----------",factoryName);
                foreach(XmlNode project in factory["projects"])
                {
                    Console.WriteLine("-----Project-----");
                    string code = project["code"].InnerText;
                    string projectName = project["name"].InnerText;
                    double cost = Double.Parse(project["cost"].InnerText,format);
                    DateTime startTime = DateTime.Parse(project["startTime"].InnerText);
                    DateTime endTime = DateTime.Parse(project["endTime"].InnerText);
                    Console.WriteLine("Code:{0}\nName:{1}\nCost:{2}\nStartTime:{3}\nEndTime:{4}\n", code, projectName, cost,startTime,endTime);
                    Console.WriteLine("---Participants:---");
                    foreach (XmlNode person in project["participants"])
                    {
                        string surname = person["surname"].InnerText;  
                        string name = person["name"].InnerText;  
                        int age = int.Parse(person["age"].InnerText);
                        
                        Console.WriteLine("Surname:{0}\nName:{1}\nAge:{2}\n",surname,name,age);
                    }
                }
                Console.WriteLine("\n\n");
            }
        }
        public static void print<T>(IEnumerable<T> lst)
        {
            foreach (T t in lst)
            {
                Console.WriteLine(t);
            }
            Console.WriteLine();
        }
    }
}