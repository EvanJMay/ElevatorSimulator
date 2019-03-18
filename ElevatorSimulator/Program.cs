using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevatorSimulator
{
    class Program
    {
        static void Main(string[] args)
        {
            var elevatorList = new List<Elevator>();

            // Elevator initialisation.
            for (int i = 1; i <= Config.elevatorCount; i++)
            {
                elevatorList.Add(new Elevator(i));
            }

            Console.WriteLine("Welcome to Elevator AI.");
            Console.WriteLine("Type 'help' for a index of available" + " commands.");

            // Console Input Loop.
            do
            {
                string input = Console.ReadLine();
                string[] inputs = input.Split(' ');
                InputAnalyser.Start(inputs, elevatorList, Floor);
                Array.Clear(inputs, 0, inputs.Length);
            } while (true);
        }

        public static int Floor { get; set; } 
        public static bool SysMaintenance { get; set; } = false;

        public static void SetFloor(int floornum)
        {
            if (floornum < Config.floorsMin || floornum > Config.floorsMax)
            {
                Console.WriteLine("The requested floor does not exist in the current configuration.");
                Console.WriteLine("Floor Range: " + Config.floorsMin.ToString() + " to " + Config.floorsMax.ToString() + ".");
            }
            else
            {
                Floor = floornum;
            }
        }

        // Toggles System Maintenace mode. This disables all elevator movement when enabled.
        public static void ToggleSysMaintenance()
        {
            SysMaintenance = !SysMaintenance;
            if (SysMaintenance == true) { Console.WriteLine("System Maintenance Enabled."); }
            else if (SysMaintenance == false) { Console.WriteLine("System Maintenance Disabled.");}
        }

        // Method responsible for adding floor numbers to the stop queues of elevators chosen to handle the request.
        public static void AddToStopQueue(Elevator elevator, int floor)
        {
            if (floor < Config.floorsMin || floor > Config.floorsMax)
            {
                Console.WriteLine("The requested floor does not exist in the current configuration.");
                Console.WriteLine("Floor Range: " + Config.floorsMin.ToString() + " to " + Config.floorsMax.ToString() + ".");
            }
            else if (!elevator.stopQueue.Contains(floor))
            {
                elevator.stopQueue.Add(floor);
                Console.WriteLine("Floor " + floor.ToString() + " has been added to the stop queue for Elevator " + elevator.Id.ToString() + ".");
            }
            return;
        }
    }
}
