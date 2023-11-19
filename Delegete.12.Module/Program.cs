using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Delegete._12.Module
{
    // Абстрактный класс "автомобиль"
    public abstract class Car
    {
        public string Model { get; set; }
        public float Speed { get; set; }
        public double ElapsedMinutes { get; private set; }

        public event EventHandler<string> FinishEvent; // Событие при финише

        public void StartRace()
        {
            Console.WriteLine($"{Model} начал гонку!");
        }

        public void Drive()
        {
            float distance = 0;
            int num = 1;
            double elapsedTime = 0;
            while (distance < 100)
            {
                int del = 12;
                if (Speed < 150)
                {
                    elapsedTime = 5 * num++;
                }
                else
                {
                    elapsedTime = 1 * num++;
                    del = 60;
                }
                ElapsedMinutes = elapsedTime;
                distance += Speed/del;

                if (distance >= 100)
                {
                    OnFinish();
                }else
                {
                    Console.WriteLine($"{Model} проехал {distance} км. Время: {elapsedTime:F2} мин.");
                }
                System.Threading.Thread.Sleep(200);
            }
        }

        protected virtual void OnFinish()
        {
            FinishEvent?.Invoke(this, $"{Model} пришел к финишу за {ElapsedMinutes:F2} минут со скоростью {Speed} км/ч!");
        }

        protected int GenerateRandomSpeed()
        {
            System.Threading.Thread.Sleep(1000);
            Random random = new Random(DateTime.Now.Millisecond);
            return random.Next(40, 300); 
        }
    }


    public class SportsCar : Car
    {
        public SportsCar(string model)
        {
            Model = model;
            Speed = GenerateRandomSpeed();
        }
    }

    public class SedanCar : Car
    {
        public SedanCar(string model)
        {
            Model = model;
            Speed = GenerateRandomSpeed();
        }
    }

    public class Truck : Car
    {
        public Truck(string model)
        {
            Model = model;
            Speed = GenerateRandomSpeed();
        }
    }

    public class Bus : Car
    {
        public Bus(string model)
        {
            Model = model;
            Speed = GenerateRandomSpeed();
        }
    }

    public class RacingGame
    {
        public delegate void RaceHandler();

        public event RaceHandler RaceStartEvent;

        public void StartRace(Car[] cars)
        {
            Console.WriteLine("Гонка начинается!");
            RaceStartEvent?.Invoke();

            List<double> finishTimes = new List<double>();

            foreach (var car in cars)
            {
                car.StartRace();
                car.FinishEvent += (sender, message) =>
                {
                    Console.WriteLine(message);
                    finishTimes.Add(((Car)sender).ElapsedMinutes);
                };
                car.Drive();
            }

            Console.WriteLine("Гонка завершена!");

            double minTime = finishTimes.Min();
            var winners = cars.Where(car => car.ElapsedMinutes == minTime);

            Console.WriteLine($"Победители:");

            foreach (var winner in winners)
            {
                Console.WriteLine($"{winner.Model} - Время: {minTime:F2} мин, Скорость: {winner.Speed} км/ч");
            }
        }
    }

    class Program
    {
        static void Main()
        {
            RacingGame racingGame = new RacingGame();

            SportsCar sportsCar = new SportsCar("Ferrari");
            SedanCar sedanCar = new SedanCar("Toyota");
            Truck truck = new Truck("Volvo");
            Bus bus = new Bus("Mercedes");

            Car[] cars = { sportsCar, sedanCar, truck, bus };

            racingGame.RaceStartEvent += () => Console.WriteLine("Гонка началась!");

            racingGame.StartRace(cars);
        }
    }
}
