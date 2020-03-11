using System;
using System.Configuration;
using System.Diagnostics;

namespace CallCenter
{
    public class Call
    {
        public string Name { get; set; } = "Звонок ";
        /// <summary>
        /// Время существования звонка
        /// </summary>
        public Stopwatch LifeTime = new Stopwatch();
        /// <summary>
        /// Продолжительность звонка
        /// По заданию определяются случайным образом от 1 до 2 секунд
        /// Но я вынес настройки в конфиг, чтобы можно было их менять
        /// </summary>
        public int TimeCall;
        public Call()
        {
            Random rnd1 = new Random();
            TimeCall = rnd1.Next(int.Parse(ConfigurationManager.AppSettings["Диапазон звонка (min)"]), 
                int.Parse(ConfigurationManager.AppSettings["Диапазон звонка (max)"]));
        }
    }
}
