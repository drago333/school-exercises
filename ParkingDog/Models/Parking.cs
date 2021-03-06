using System;
using System.Collections.Generic;
using System.Text;
using ParkingDog.Models.Abstract;

namespace ParkingDog.Models
{
    public class Parking
    {
        public Parking(string name, int lightCars, int mediumCars, int heavyCars)
        {
            if (name.Length < 2 || name.Length > 50)
                throw new ApplicationException(
                    $"Невалидно име за паркинг.\nДължината трябва да бъде между {2} и {50} символа");

            Name = name;
            if (lightCars < 0 || mediumCars < 0 || heavyCars < 0 ||
                lightCars > 2500000 || mediumCars > 2500000 || heavyCars > 2500000)
                throw new ApplicationException("Невалидна стойност за капацитет на паркинг.\nДопустимите стойности са между 0 и 2500000");

            LightCars = new List<LightCar>(lightCars);
            MediumCars = new List<MediumCar>(mediumCars);
            HeavyCars = new List<HeavyCar>(heavyCars);
        }

        public string Name { get; set; }

        public List<LightCar> LightCars { get; }
        public List<MediumCar> MediumCars { get; }
        public List<HeavyCar> HeavyCars { get; }

        public bool HasEmptySpot(Car car)
        {
            return car switch
            {
                LightCar _ => LightCars.Count < LightCars.Capacity,
                MediumCar _ => MediumCars.Count < MediumCars.Capacity,
                HeavyCar _ => HeavyCars.Count < HeavyCars.Capacity,
                _ => throw new AggregateException()
            };
        }

        public Car AddCar(Car car)
        {
            if (!HasEmptySpot(car))
                throw new AggregateException();

            switch (car)
            {
                case LightCar lightCar:
                    LightCars.Add(lightCar);
                    break;
                case MediumCar mediumCar:
                    MediumCars.Add(mediumCar);
                    break;
                case HeavyCar heavyCar:
                    HeavyCars.Add(heavyCar);
                    break;
            }

            return car;
        }

        public override string ToString()
        {
            var sb = new StringBuilder();

            sb.AppendLine($"Паркирани автомобили в паркинг {Name}:");

            foreach (var car in LightCars)
            {
                sb.AppendLine(car.ToString());
            }

            foreach (var car in MediumCars)
            {
                sb.AppendLine(car.ToString());
            }

            foreach (var car in HeavyCars)
            {
                sb.AppendLine(car.ToString());
            }

            // TODO: Remove the last newline

            return sb.ToString();
        }

        public string CurrentLoadInformation()
        {
            var sb = new StringBuilder();

            sb.AppendLine($"Паркинг {Name} разполага със следните места:");

            sb.AppendLine($"Леки автомобили {LightCars.Capacity}, заети {LightCars.Count}");
            sb.AppendLine($"Лекотоварни автомобили {MediumCars.Capacity}, заети {MediumCars.Count}");
            // TODO: Use AppendLine and then remove the last \n
            sb.Append($"Тежкотоварни автомобили {HeavyCars.Capacity}, заети {HeavyCars.Count}");
            
            return sb.ToString();
        }
    }
}