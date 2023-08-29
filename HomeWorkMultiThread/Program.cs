namespace HomeWorkMultiThread
{
    internal class Program
    {
        static SemaphoreSlim barberRdy = new SemaphoreSlim(0);
        static SemaphoreSlim customerRdy = new SemaphoreSlim(0);
        static int waitingCustomers = 0;
        static readonly int maxSeats = 5;
        static int customer = 0;

        static void Main(string[] args)
        {
            Thread barberThread = new Thread(Barber);
            barberThread.Start();

            for(int i = 1; i <= maxSeats; i++)
            {
                Thread customerThread = new Thread(Customer);
                customerThread.Start(i);
                Thread.Sleep(3000);//пауза между приходом клиентов
            }

            Console.ReadLine();
        }

        static void Barber()
        {
            while (true)
            {
                Console.WriteLine("Барбер спит...");
                barberRdy.Wait();
                

                Console.WriteLine($"Барбер стрижет: {customer} клиента");
                Thread.Sleep(1000);//віделяем время на стрижку

                Console.WriteLine($"Барбер закончил стрижку {customer} клиенту");

                customerRdy.Release();//клиент понимает что стрижка закончилась
            }
        }

        static void Customer(object customerNumber)
        {
            customer = (int)customerNumber;

            if(waitingCustomers < maxSeats)
            {
                Console.WriteLine($"Клиент {customer} вошел в барбершоп");
                waitingCustomers++;

                barberRdy.Release();//будим барбера
                Console.WriteLine($"Барбера разбудил {customer} клиент");

                customerRdy.Wait();//ждем стрижку
                Console.WriteLine($"Клиент {customer} очень доволен стрижкой и уходит из барбершопа");
                waitingCustomers--;
            }
            else
            {
                Console.WriteLine($"Для клиента {customer} нет свободных кресел ");
            }
        }
    }
}