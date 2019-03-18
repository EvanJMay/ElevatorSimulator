﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevatorSimulator
{
    public class ElevatorDispatcher
    {
        // The elevator dispatcher is responsible for deciding which elevator is best suitable to handle the request.
        public List<Elevator> Elevators { get; set; }

        public static Elevator Start(List<Elevator> elevatorList, int floor, int requestDirection)
        {
            List<int> elevatorFloors = new List<int>();
            foreach (var elev in elevatorList)
            {
                elevatorFloors.Add(elev.Floor);
            }

            // Assess' which elevators are available to handle the request.

            List<Elevator> availableElevators = new List<Elevator>();

            foreach (var elev in elevatorList)
            {
                var pse = elev.isPaused == false && elev.MaintenanceMode == false;
                var stationary = elev.stopQueue.Count == 0 && elev.direction == 0;

                if (pse && stationary) availableElevators.Add(elev); // If it is stationary and is not paused or in maintenance mode it is available.
                if (!pse && !stationary)
                {
                    // If the elevator is going to pass the request floor it is available.
                    if (elev.direction == 1 && elev.stopQueue.Min() > floor && requestDirection == elev.direction)
                    {
                        availableElevators.Add(elev);
                    }
                    else if (elev.direction == -1 && elev.stopQueue.Max() < floor && requestDirection == elev.direction)
                    {
                        availableElevators.Add(elev);
                    }
                }
            }


            // Gets the distance between the available elevators and the request floor.
            foreach (var elev in availableElevators)
            {
                ElevatorDistances(elev, floor);
            }

            // Gets the closest elevator, and returns that elevator as a result.
            int closestDistance = ClosestDistance(availableElevators);
            Elevator elevToSend = (from elev in availableElevators where elev.distanceToFloor == closestDistance select elev).First();
            return elevToSend;


            void ElevatorDistances(Elevator elevator, int f)
            {
                elevator.distanceToFloor = Math.Abs(elevator.Floor - f);
            }

            int ClosestDistance(List<Elevator> elevators)
            {
                List<int> distances = new List<int>();
                foreach (Elevator elev in elevators)
                {
                    distances.Add(elev.distanceToFloor);
                }
                return distances.Min();
            }
        }
    }
}
