namespace HomeWorkMultiThread
{
    internal class Program
    {
        static SemaphoreSlim barberRdy = new SemaphoreSlim(0);
        static AutoResetEvent haircutFinish = new AutoResetEvent(false);
        static int waitingCustomers = 0;
        static readonly int maxSeats = 5;
        static int currentCustomer = 1;
        
        

        static void Main(string[] args)
        {
            Thread barberThread = new Thread(Barber);
            barberThread.Start();

            for(int i = 1; i <= 10; i++)
            {
                Thread customerThread = new Thread(Customer);
                customerThread.Start(i);
                Thread.Sleep(1000);//пауза между приходом клиентов
            }

            Console.ReadLine();
        }

        static void Barber()
        {
            while (true)
            {
                if(waitingCustomers == 0)
                {
                    Console.WriteLine("Барбер спит...");
                    Console.WriteLine($"Барбера разбудил клиент {currentCustomer}");
                }
                
                barberRdy.Wait();                

                Console.WriteLine($"Барбер стрижет клиента: {currentCustomer}\n");
                Thread.Sleep(2000);//віделяем время на стрижку
                
                Console.WriteLine($"Барбер закончил стрижку клиенту: {currentCustomer}\n");
                currentCustomer++;

                haircutFinish.Set();//клиент понимает что стрижка закончилась
            }
        }

        static void Customer(object customerNumber)
        {
            int customer = (int)customerNumber;

            if(waitingCustomers < maxSeats)
            {
                Console.WriteLine($"Клиент {customer} вошел в барбершоп\n");
                waitingCustomers++;

                barberRdy.Release();//будим барбера               

                haircutFinish.WaitOne();//ждем стрижку
                

                Console.WriteLine($"Клиент {customer} очень доволен стрижкой и уходит из барбершопа\n");
                waitingCustomers--;
            }
            else
            {
                Console.WriteLine($"Для клиента {customer} нет свободных кресел и он ушел(((\n");
            }
        }
    }
}