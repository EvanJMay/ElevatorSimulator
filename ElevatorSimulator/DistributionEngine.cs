using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevatorSimulator
{
    class DistributionEngine
    {
        public static void Start(Elevator elevator)
        {
            while (true)
            {
                if (elevator.stopQueue.Count > 0 && elevator.MaintenanceMode == false && Program.SysMaintenance == false)
                {
                    // Sets the elevator direction by comparing its current floor against its stop queue. 0 = Stationary / 1 = Ascending / -1 = Decending.
                    if (elevator.direction == 0)
                    {
                        int floor = 0;
                        if (elevator.Floor < elevator.stopQueue.Min())
                        {
                            elevator.direction = 1;
                            floor = elevator.stopQueue.Min();
                        }
                        else if (elevator.Floor > elevator.stopQueue.Max())
                        {
                            elevator.direction = -1;
                            floor = elevator.stopQueue.Max();
                        }
                    }
                    // If the elevator is supposed to be ascending do so untill it reaches the lowest floor in its stop queue. Remove it from the stop queue, open the doors.
                    if (elevator.direction == 1)
                    {
                        Console.WriteLine("Moving Elevator " + elevator.Id.ToString() + " to floor " + elevator.stopQueue.Min().ToString());
                        do
                        {
                            Console.WriteLine("Elevator " + elevator.Id.ToString() + " passing by floor " + elevator.Floor.ToString() + ".");
                            elevator.Floor++;
                        } while (elevator.Floor != elevator.stopQueue.Min());
                        elevator.stopQueue.Remove(elevator.Floor);
                        Console.WriteLine("Elevator " + elevator.Id + " at floor " + elevator.Floor.ToString() + ".");
                        elevator.OpenDoors();
                        Task.Delay(5000);
                        elevator.CloseDoors();
                    }
                    // If the elevator is supposed to be decending do so untill it reaches the highest floor in its stop queue. Remove the floor from the stop queue, open the doors.
                    else if (elevator.direction == -1)
                    {
                        Console.WriteLine("Moving Elevator " + elevator.Id.ToString() + " to floor " + elevator.stopQueue.Max().ToString());
                        do
                        {
                            Console.WriteLine("Elevator " + elevator.Id.ToString() + " passing by floor " + elevator.Floor.ToString() + ".");
                            elevator.Floor--;
                        } while (elevator.Floor != elevator.stopQueue.Max());
                        elevator.stopQueue.Remove(elevator.Floor);
                        Console.WriteLine("Elevator " + elevator.Id + " at floor " + elevator.Floor.ToString());
                        elevator.OpenDoors();
                        Task.Delay(5000);
                        elevator.CloseDoors();
                    }
                    // Are there any outstanding stops left in the queue, if so anounce journey end, set elevator direction to stationary and exit the loop.
                    if (elevator.stopQueue.Count == 0)
                    {
                        elevator.direction = 0;
                        Console.WriteLine("Elevator " + elevator.Id + " has finished its journey.");
                        break;
                    }
                }
                
            }
        }
    }
}
