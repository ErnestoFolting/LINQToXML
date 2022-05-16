using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LINQToXML.Structure
{
    internal class Project
    {
        public string code { get; set; }
        public string name { get; set; }
        public double cost { get; set; }
        public DateTime startTime { get; set; }
        public DateTime endTime { get; set; }

        public List<Person> participants { get; set; } = new List<Person>();

        public Project(string code,string name,double cost,DateTime startTime,DateTime endTime,List<Person> participants)
        {
            if(startTime >= endTime)
            {
                Console.WriteLine("We can not create a project because of invalid date input");
            }
            else
            {
                this.code = code;
                this.name = name;
                this.cost = cost;
                this.startTime = startTime;
                this.endTime = endTime;
                this.participants = participants;
            }
        }
        public override string ToString()
        {
            return string.Format("Code:\n{0}\nName:\n{1}\nCost:\n{2}\nStartTime:\n{3}\nEndTime:\n{4}\nParticipants:\n\n{5}\n", code,name,cost,startTime,endTime,string.Join(" ",participants));
        }
    }
}
