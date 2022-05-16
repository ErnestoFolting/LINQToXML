using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LINQToXML.Structure
{
    internal class Person
    {
        public string surname { get; set; }
        public string name { get; set; }
        public int age { get; set; }
        public Person(string Surname,string Name, int age)
        {
            this.surname = Surname;
            this.name = Name;
            this.age = age;
        }
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(string.Format("Surname:\n{0}\nName:\n{1}\nAge:\n{2}\n", surname, name, age));
            return sb.ToString();
        }
    }
}
