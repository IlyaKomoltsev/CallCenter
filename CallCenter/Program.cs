using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CallCenter.Employee;

namespace CallCenter
{
    class Program
    {
        //Порядковый номер звонка
        static int iCallCount = 0;
        // Очередь звонков
        static Queue<Call> Calls = new Queue<Call>();
        // Сотрудники
        static List<Employeer> Employees;      
      
        /// <summary>
        /// Создаём звонок и записываем его в очередь
        /// </summary>
        static void CreateAndAddCallInQueue(object obj)
        {
            iCallCount++;
            Call call = new Call();
            call.LifeTime.Start(); // Определяем сколько существует звонок
            call.Name += iCallCount.ToString();            
            Calls.Enqueue(call);    
        }
        /// <summary>
        /// Звонки создаются с определенным интервалом 
        /// Определен случайным образом, от 300 до 500 миллисекунд
        /// </summary>
        static void TimerCreateCall()
        {
            Random rnd1 = new Random();
            int num = 0;
            TimerCallback tm = new TimerCallback(CreateAndAddCallInQueue);
            Timer timer = new Timer(tm, num, 0, rnd1.Next(100, 200)); 
        }
        private static readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
        /// <summary>
        /// Проверяем наличие звонков в очереди       
        /// </summary>
        static void CheckCall()
        {
            while (!_cancellationTokenSource.Token.WaitHandle.WaitOne(TimeSpan.FromMilliseconds(300))) //проверка каждые 300 миллисекунд
            {
                Task.Run(() =>
                      {
                          var operatory = Employees.Where(x => x.Busy == false).OrderBy(x => x.Level).FirstOrDefault();
                          if (operatory != null)
                          {
                              Call call = Calls.Dequeue();
                              operatory.Message(call);
                          }
                          else
                          {
                              Call call = Calls.Peek();
                              //Т.к я использую Queue, всегда выбирается первый в очереди звонок
                              Console.WriteLine(call.Name + $" в ожидании {call.LifeTime.Elapsed:m\\:ss\\.ff}, 1й в очереди"); 
                          }
                      }
                      );
            }
        }
        
        static void Main(string[] args)
        {
            int oper = int.Parse(ConfigurationManager.AppSettings["Оператор"]); 
            int manager = int.Parse(ConfigurationManager.AppSettings["Менеджер"]);

            List<Task> tasks = new List<Task>();

            Employees = new List<Employeer>();
            for (int i = 1; i <= oper; i++)
                Employees.Add(new Operator(i));
            for (int i = 1; i <= manager; i++)
                Employees.Add(new Manager(i));
            Employees.Add(new Director());
            
            Task task1 = new Task(() => {
                TimerCreateCall();
            });
            task1.Start();

            Task _task = new Task(CheckCall, _cancellationTokenSource.Token);
            _task.Start();

            Console.Read();
        }
    }
}
