using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using LINQToXML.Structure;
using System.Globalization;
using System.Text.RegularExpressions;

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
            var format = new NumberFormatInfo() { NumberDecimalSeparator = ",", };
            foreach (XmlNode factory in doc.DocumentElement)
            {
                string factoryName = factory["name"].InnerText;
                Console.WriteLine("----------The {0} factory----------", factoryName);
                foreach (XmlNode project in factory["projects"])
                {
                    Console.WriteLine("-----Project-----");
                    string code = project["code"].InnerText;
                    string projectName = project["name"].InnerText;
                    double cost = Double.Parse(project["cost"].InnerText, format);
                    DateTime startTime = DateTime.Parse(project["startTime"].InnerText);
                    DateTime endTime = DateTime.Parse(project["endTime"].InnerText);
                    Console.WriteLine("Code:{0}\nName:{1}\nCost:{2}\nStartTime:{3}\nEndTime:{4}\n", code, projectName, cost, startTime, endTime);
                    Console.WriteLine("---Participants:---");
                    foreach (XmlNode person in project["participants"])
                    {
                        string surname = person["surname"].InnerText;
                        string name = person["name"].InnerText;
                        int age = int.Parse(person["age"].InnerText);

                        Console.WriteLine("Surname:{0}\nName:{1}\nAge:{2}\n", surname, name, age);
                    }
                }
                Console.WriteLine("\n\n");
            }
            XDocument xmlDoc = XDocument.Load("storage.xml");

            //1. All factories names
            var q1 = from el in xmlDoc.Root.Elements("factory")
                     select el.Element("name")?.Value;
            Console.WriteLine("*****1. All factories names");
            print(q1);

            //2. All projects on "2.Hanry Laundries" factory

            var q2 = from el in xmlDoc.Root.Elements("factory").Elements("projects")
                     where el.Parent.Element("name").Value == "2.Hanry Laundries"
                     select el.Element("project").Element("name").Value;
            Console.WriteLine("*****2. All projects on 2.Hanry Laundries factory");
            print(q2);

            //3. Project with cost more then 50000

            var q3 = from el in xmlDoc.Root.Elements("factory").Elements("projects").Elements("project")
                     where double.Parse(el.Element("cost").Value, format) > 50000
                     select el.Element("name").Value;
            Console.WriteLine("*****3. Project with cost more then 50000");
            print(q3);

            //4. Sorted projects by names ascending
            var q4 = from el in xmlDoc.Root.Elements("factory")
                     .Elements("projects").Elements("project")
                     orderby (el.Element("name").Value) 
                     select new
                     {
                         Name = el.Element("name").Value,
                         Code = el.Element("code").Value,
                         Cost = double.Parse(el.Element("cost").Value, format)
                     };
            Console.WriteLine("*****4. Sorted projects by names ascending");
            print(q4);

            //5. Average cost of projects 
            var q5 = (from el in xmlDoc.Root.Elements("factory")
                     .Elements("projects").Elements("project")
                      select double.Parse(el.Element("cost").Value, format)).Average();
            Console.WriteLine("*****5. Average cost of projects");
            Console.WriteLine(q5 + "\n");

            //6. startDates of projects that costs more than average
            var q6 = from el in xmlDoc.Root.Elements("factory")
                     .Elements("projects").Elements("project")
                     let tempCost = double.Parse(el.Element("cost").Value, format)
                     where (tempCost > q5)
                     select new
                     {
                         Data = el.Element("startTime").Value,
                         Cost = tempCost.ToString()
                     };
            Console.WriteLine("*****6. startDates of projects that costs more than average");
            print(q6);

            //7. Names of projects that factory's name starts with "1"
            var q7 = from el in xmlDoc.Root.Elements("factory")
                     .Elements("projects").Elements("project")
                     where el.Parent.Parent.Element("name").Value.StartsWith("1")
                     select el.Element("name").Value;
            Console.WriteLine("*****7. Names of projects that factory's name starts with 1");
            print(q7);

            //8. First two factories
            var q8 = xmlDoc.Root.Elements("factory")
                     .Take(2)
                     .Select(el => el.Element("name").Value);
            Console.WriteLine("*****8. First two factories");
            print(q8);

            //9. Sort people by their age 
            var q9 = (xmlDoc.Root.Descendants("person")
                .OrderBy(el => double.Parse(el.Element("age").Value, format))
                .Select(el => new {Surname = el.Element("surname").Value, Age = double.Parse(el.Element("age").Value, format) })).Distinct();
            Console.WriteLine("*****9. Sort people by their age");
            print(q9);

            //10. Projects grouped by factory they are execute on
            var q10 = xmlDoc.Root.Descendants("project")
                .GroupBy(el => el.Parent.Parent.Element("name").Value);
            Console.WriteLine("*****10. Projects grouped by factory they are execute on");
            foreach (var group in q10)
            {
                Console.WriteLine("Factory:" + group.Key );
                foreach(var el in group)
                {
                    Console.WriteLine("Project: " + el.Element("name").Value);
                }
                Console.WriteLine("\n");
            }

            //11. Factories what's project names match the template
            var q11 = xmlDoc.Root.Descendants("project")
                .Where(el => Regex.IsMatch(el.Element("name").Value, @"ear"))
                .Select(el => el.Parent.Parent.Element("name").Value).Distinct();
            Console.WriteLine("*****11. Factories what's project names match the template");
            print(q11);

            //12. Skip projects while their cost < 150000
            var q12 = xmlDoc.Root.Descendants("project")
                .SkipWhile(el => double.Parse(el.Element("cost").Value, format) < 150000)
                .Select(el => new
                {
                    Name = el.Element("name").Value,
                    Cost = double.Parse(el.Element("cost").Value,format)
                });
            Console.WriteLine("*****12. Skip projects while their cost < 150000");
            print(q12);

            //13. Join projects with cost >25000 with projects with startTime in September by name
            var q13 = from el in xmlDoc.Root.Descendants("project")
                      where (double.Parse(el.Element("cost").Value, format) > 25000)
                      join temp in xmlDoc.Root.Descendants("project")
                        on el.Element("name").Value equals temp.Element("name").Value
                      where (DateTime.Parse(temp.Element("startTime").Value).Month == 9)
                      select new { Name = el.Element("name").Value, Cost = double.Parse(el.Element("cost").Value, format), StartTime = temp.Element("startTime").Value };
            Console.WriteLine("*****13. Join projects with cost >25000 with projects with startTime in September by name");
            print(q13);

            //14. Count participants on projects  
            var q14 = xmlDoc.Root.Descendants("person")
                .GroupBy(el => el.Parent.Parent.Element("name").Value)
                .Select(el => new { Project = el.Key, Participants = el.Count() });
            Console.WriteLine("*****14. Count participants on projects");
            print(q14);
            //15. People that are under 25 and over 40 y.o.
            var q15 = xmlDoc.Root.Descendants("person")
                .Where(el => double.Parse(el.Element("age").Value, format) < 25)
                .Concat(
                    xmlDoc.Root.Descendants("person")
                .Where(el => double.Parse(el.Element("age").Value, format) > 40)
                )
                .Select(el => new { Surname = el.Element("surname").Value, Age = double.Parse(el.Element("age").Value, format) }).Distinct();
            Console.WriteLine("*****15. People that are under 25 and people that are over 40 y.o.");
            print(q15);
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