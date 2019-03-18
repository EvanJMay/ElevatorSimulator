using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevatorSimulator
{
    class InputAnalyser
    {
        // Translates the input from the command line and performs desired operations.
        public static void Start(string[] Inputs, List<Elevator> elevators, int floor)
        {
            if (Inputs[0] == "exit" || Inputs[0] == "quit")
            {
                Environment.Exit(0);
            }
            if (Inputs[0] == "floor")
            {
                if (Inputs.Length > 1)
                {
                    Int32.TryParse(Inputs[1], out int newfloor);
                    Program.SetFloor(newfloor);
                }
                Console.WriteLine("You are now on Floor " + Program.Floor.ToString());
                return;
            }
            if (Inputs[0] == "status")
            {
                foreach (var elevator in elevators)
                {
                    Status(elevator);
                }
                return;
            }
            if (Inputs[0] == "call")
            {
                if (Inputs[1] == "elevator")
                {
                    if (Inputs.Length >= 3)
                    {
                        if (Inputs[2] == "floor")
                        {
                            Int32.TryParse(Inputs[3], out int newfloor);
                            Program.SetFloor(newfloor);
                        }
                        else if (Inputs[2] == "up") { Elevator e = ElevatorDispatcher.Start(elevators, floor, 1); Program.AddToStopQueue(e, floor); DistributionEngine.Start(e);  return; }
                        else if (Inputs[2] == "down") { Elevator e = ElevatorDispatcher.Start(elevators, floor, -1); Program.AddToStopQueue(e, floor); DistributionEngine.Start(e); return; }
                    }
                    Elevator elevToSend = ElevatorDispatcher.Start(elevators, floor, 0);
                    Program.AddToStopQueue(elevToSend, floor);
                    DistributionEngine.Start(elevToSend);
                    return;
                }
            }
            if (Inputs[0] == null || Inputs[0] == "")
            {
                Console.WriteLine("No Input Detected. Type 'help' for available commands.");
                return;
            }
            if (Inputs[0] == "elevator" && Inputs[2] == "floor")
            {
                Int32.TryParse(Inputs[3], out floor);
                Int32.TryParse(Inputs[1], out int elevNum);
                Elevator elevToSend = elevators.SingleOrDefault(Elevator => Elevator.Id == elevNum);
                Program.AddToStopQueue(elevToSend, floor);
                DistributionEngine.Start(elevToSend);

            }
            else if (Inputs[0] == "toggle" && Inputs[1] == "system" && Inputs[2] == "maintenance")
            {
                Program.ToggleSysMaintenance();
            }
            else if (Inputs[0] == "elevator" && Inputs[2] == "maintenancemode")
            {
                Int32.TryParse(Inputs[1], out int elevNum);
                Elevator elevToToggle = elevators.SingleOrDefault(Elevator => Elevator.Id == elevNum);
                elevToToggle.MaintenanceMode = !elevToToggle.MaintenanceMode;
                Console.WriteLine("Elevator " + elevToToggle.Id.ToString() + " Maintenance Mode set to " + elevToToggle.MaintenanceMode.ToString() + ".");
            }
            else
            {
                if (Inputs[0] == "help")
                {
                    Console.WriteLine("Type 'Floor *' to specify what floor you are on. (* = num) \n" +
                        "Type 'Floor' to see what floor you are on. \n" +
                        "Type 'Call Elevator Up/Down' to request an elevator to your floor. \n" +
                        "Type 'Elevator * Maintenance Mode' to toggle maintenance mode. (* = num) \n" +
                        "Type 'Elevator * Floor *' to add a floor to a specific elevator stops. (* = num) \n" +
                        "Type 'Toggle System Maintenance' to toggle system maintenance mode. \n" +
                        "Type 'Status' to see where each elevator is.");
                    return;
                }
                else
                {
                    Console.WriteLine("Command not recognized. Type 'help' for available commands.");
                    return;
                }
            }
        }

        private static void Status(Elevator elevator)
        {
            Console.WriteLine("Elevator " + elevator.Id.ToString() + " : \n"
                + "-- Floor: " + elevator.Floor.ToString() + " \n"
                + "-- Current Weight: " + elevator.ElevatorWeight.ToString() + " \n"
                + "-- Maintenance Mode: " + elevator.MaintenanceMode.ToString() + " \n"
                + "-- Stop Queue Count: " + elevator.stopQueue.Count() + " \n"
                + "-- Door State: " + elevator.doorsOpen.ToString() + " (True = Open / False = Closed) \n"
                + "-- Direction: " + elevator.direction.ToString() + " (1 = Up / 0 = Stationary / -1 = Down) \n");
        }
    }
}
