using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOp1
{
    internal class Animal
    {
        public String Name { get; set; }
     public Animal() { }
        public virtual void Speak() {
            Console.WriteLine("Animal is speaking.");
        }
    }
}
