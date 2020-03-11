using System;
using System.Threading;

namespace CallCenter.Employee
{
    /*
     * Лучше увеличить продолжительность звонка
     */
    public abstract class Employeer
    {
        public bool Busy = false;
        public string Name { get; set; }
        public int Level;
        public void Message(Call call)
        {
            Random rnd1 = new Random();
            Busy = true;
            Console.WriteLine(call.Name + " - " + Name);
            Thread.Sleep(call.TimeCall); // Продолжительность звонка
            Busy = false;
        }
    }
}
