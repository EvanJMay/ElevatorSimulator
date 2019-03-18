using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevatorSimulator
{
    public class Elevator
    {
        // Variables pertaining to the elevator.
        public int Id { get; set; }
        public bool MaintenanceMode { get; set; } = false;
        public int Floor { get; set; } = 0;
        public bool isPaused { get; set; } = false; // Keeps the elevator from being reassigned while in use. (Doors open, just arrived).
        public int direction { get; set; } = 0; // 1 = Up, 0 = Stationary, -1 = Down.
        public bool doorsOpen { get; set; } = false;
        public List<int> stopQueue { get; set; } = new List<int>(); // List containing each floor number the elevator is scheduled to stop at.
        public int distanceToFloor = 0; // Calculation variable holding distance between this elevator and requested floor. - I dont think this should be here as simultaneous calculations would result invalid.
        static int weightLimit = 900; //KiloGrams (theoretical).
        static Random curWeight = new Random();
        public int ElevatorWeight { get; set; } = curWeight.Next(1, 1000); // 10% chance elevator will be over weight limit.

        // Sets the elevator identification number using the iteration number from the initialisation foreach elevator loop.
        public Elevator(int i)
        {
            Id = i;
        }

        // Close the elevator doors, check if it is over weight.
        public void CloseDoors()
        {
            doorsOpen = false;
            isPaused = false;
            Console.WriteLine("Elevator " + Id.ToString() + " doors are closed");
            if (ElevatorWeight > weightLimit && stopQueue.Count > 0)
            {
                WeightLimitTriggered();
            }
            return;
        }

        // The elevator is over the weight limit.  Open the doors, get a new weight reading.
        public void WeightLimitTriggered()
        {
            do
            {
                Console.WriteLine("Weight exceeds weight limit.");
                OpenDoors();
                int newWeight = curWeight.Next(1, 1000);
                ElevatorWeight = newWeight;
            } while (ElevatorWeight > weightLimit);
            CloseDoors();
            return;

        }

        public void OpenDoors()
        {
            doorsOpen = true;
            isPaused = true;
            Console.WriteLine("Elevator " + Id.ToString() + " doors are open.");
            return;
        }

    }
}
