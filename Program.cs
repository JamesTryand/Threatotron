using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.Versioning;
using System.Security.Cryptography.X509Certificates;
using static Threatotron.Program;
//using static Threatotron.Program.ObstacleMaps;
//using static Threatotron.Program.ObstacleMap.Map;
namespace Threatotron
{
    class Program
    {
        public static void Main(string[] args)
        {
            List<Obstacle> ListofObstacles = new List<Obstacle>(); // stores the properties of obstacles
            List<Vector> BlockedPoints = new List<Vector>(); // stores the blocked points of the obstacles
            List<Camera> ListofCameras = new List<Camera>(); //camera has its own list because its special


            Console.WriteLine("Welcome to the Threat-o-tron 9000 Obstacle Avoidance System.\r\n\r\nValid commands are:\r\nadd guard <x> <y>: registers a guard obstacle\r\nadd fence <x> <y> <orientation> <length>: registers a fence obstacle. Orientation must be 'east' or 'north'.\r\nadd sensor <x> <y> <radius>: registers a sensor obstacle\r\nadd camera <x> <y> <direction>: registers a camera obstacle. Direction must be 'north', 'south', 'east' or 'west'.\r\ncheck <x> <y>: checks whether a location and its surroundings are safe\r\nmap <x> <y> <width> <height>: draws a text-based map of registered obstacles\r\npath <agent x> <agent y> <objective x> <objective y>: finds a path free of obstacles\r\nhelp: displays this help message\r\nexit: closes this program");

            int i = 0;
            while (i == 0)
            {

                Console.WriteLine("Enter command:");
                string userinput = Console.ReadLine();
                string[] userinput_split = userinput.Split(' ');
                int userinput_length = userinput_split.Length;
                switch (userinput_split[0])
                {
                    case "add":

                        switch (userinput_split[1])
                        {
                            case "guard":
                                if (userinput_length != 4)
                                {
                                    Console.WriteLine("Incorrect number of arguments.");
                                    break;
                                }

                                int Guardcoordx = int.Parse(userinput_split[2]);
                                int Guardcoordy = int.Parse(userinput_split[3]);
                                Guard guard = new Guard(Guardcoordx, Guardcoordy);
                                ListofObstacles.Add(new Guard(Guardcoordx, Guardcoordy));
                                BlockedPoints.AddRange(guard.ReturnPoints());
                                Console.WriteLine("Successfully added guard obstacle.");
                                break;

                            case "fence":
                                if (userinput_length != 6)
                                {
                                    Console.WriteLine("Incorrect number of arguments.");
                                    break;
                                }
                                int Fencecoordx = int.Parse(userinput_split[2]);
                                int Fencecoordy = int.Parse(userinput_split[3]);
                                string Fencedir = userinput_split[4];
                                int Fencelength = int.Parse(userinput_split[5]);
                                if (Fencedir != "east" && Fencedir != "north")
                                {
                                    Console.WriteLine("Orientation must be 'east' or 'north'.");
                                    break;
                                }
                                if (Fencelength <= 0)
                                {
                                    Console.WriteLine("Length must be a valid integer greater than 0.");
                                    break;
                                }
                                Fence fence = new Fence(Fencecoordx, Fencecoordy, Fencedir, Fencelength);
                                ListofObstacles.Add(new Fence(Fencecoordx, Fencecoordy, Fencedir, Fencelength));
                                BlockedPoints.AddRange(fence.ReturnPoints());
                                break;

                            case "sensor":
                                if (userinput_length != 5)
                                {
                                    Console.WriteLine("Incorrect number of arguments.");
                                    break;
                                }
                                int Sensorcoordx = int.Parse(userinput_split[2]);
                                int Sensorcoordy = int.Parse(userinput_split[3]);
                                Double Range = double.Parse(userinput_split[4]);
                                Sensor sensor = new Sensor(Sensorcoordx, Sensorcoordy, Range);
                                ListofObstacles.Add(new Sensor(Sensorcoordx, Sensorcoordy, Range));
                                BlockedPoints.AddRange(sensor.ReturnPoints());
                                Console.WriteLine("Successfully added sensor obstacle.");
                                break;

                            case "camera":
                                if (userinput_length != 5)
                                {
                                    Console.WriteLine("Incorrect number of arguments.");
                                    break;
                                }
                                int Camerax = int.Parse(userinput_split[2]);
                                int Cameray = int.Parse(userinput_split[3]);
                                string Dir = userinput_split[4];
                                Camera camera = new Camera(Camerax, Cameray, Dir);
                                ListofObstacles.Add(new Camera(Camerax, Cameray, Dir));
                                ListofCameras.Add(new Camera(Camerax, Cameray, Dir));
                                Console.WriteLine("Successfully added camera obstacle.");
                                break;

                            case "":
                                Console.WriteLine("You need to specify an obstacle type");
                                break;


                        }
                        break;
                    case "check":
                        if (userinput_length != 3)
                        {
                            Console.WriteLine("Incorrect number of arguments.");
                            break;
                        }
                        int Locationx = int.Parse(userinput_split[1]);
                        int Locationy = int.Parse(userinput_split[2]);
                        DirectionChecker(Locationy, Locationx, BlockedPoints, ListofCameras);
                        break;

                    case "map":
                        int Map_Startx = int.Parse(userinput_split[1]);
                        int Map_Starty = int.Parse(userinput_split[2]);
                        int map_height = int.Parse(userinput_split[3]);
                        int map_width = int.Parse(userinput_split[4]);
                        ObstacleMap m = new ObstacleMap(Map_Startx, Map_Starty, map_height, map_width, ListofObstacles);
                        break;

                    case "path":
                        break;

                    case "help":
                        Console.WriteLine("Valid commands are:\r\nadd guard <x> <y>: registers a guard obstacle\r\nadd fence <x> <y> <orientation> <length>: registers a fence obstacle. Orientation must be 'east' or 'north'.\r\nadd sensor <x> <y> <radius>: registers a sensor obstacle\r\nadd camera <x> <y> <direction>: registers a camera obstacle. Direction must be 'north', 'south', 'east' or 'west'.\r\ncheck <x> <y>: checks whether a location and its surroundings are safe\r\nmap <x> <y> <width> <height>: draws a text-based map of registered obstacles\r\npath <agent x> <agent y> <objective x> <objective y>: finds a path free of obstacles\r\nhelp: displays this help message\r\nexit: closes this program");
                        break;

                    case "exit":
                        Console.WriteLine("Thank you for using the Threat-o-tron 9000.");
                        i++;
                        break;

                    default:
                        Console.WriteLine("Invalid option: " + userinput_split[0]);
                        Console.WriteLine("Type 'help' to see a list of commands.");
                        break;


                }
            }
        }
        
        public class Obstacle
        {
            protected List<Vector> BlockedPoints { get; set; }


            public int X { get; set; }
            public int Y { get; set; }

            public virtual List<Vector> ReturnPoints()
            {
                return BlockedPoints;
            }



            public virtual List<Vector> ReturnPointsCamera(int Map_Startx, int Map_Starty, int map_width, int map_height)
            {
                return BlockedPoints;
            }
        }


        public class Vector
        {
            public int X { get; set; }
            public int Y { get; set; }

            public Vector(int x, int y)
            {
                X = x;
                Y = y;
            }

            public static bool TryParse(string? input, out Vector? result)
            {
                result = null;
                if (input == null) return false;
                string[] parts = input!.Split(' ');
                if (parts.Length != 2) return false;
                if (!int.TryParse(parts[0].Trim(), out int x)) return false;
                if (!int.TryParse(parts[1].Trim(), out int y)) return false;
                result = new Vector(x, y);
                return true;
            }
        }


        public class Area //looks at each tile on the map, and subsequently fetches its location and safety data
        {
            public int Row { get; set; }
            public int Column { get; set; }

            public char Safe { get; set; }


            public Area(int row, int column)
            {
                Row = row;
                Column = column;
                Safe = '.';
            }
        }
        internal class Guard : Obstacle
        {



            public Guard(int x, int y)
            {
                X = x;
                Y = y;
                BlockedPoints = new List<Vector>();
            }

            public override List<Vector> ReturnPoints()
            {

                BlockedPoints.Add(new Vector(Y, X));
                return BlockedPoints;
            }
        }

        internal class Fence : Obstacle
        {
            //private int x, y;


            //public Vector StartLocation { get; private set; }
            public int Length { get; private set; }
            public string Dir { get; private set; }

            public Fence(int Fencestartx, int Fencestarty, string dir, int length)
            {
                X = Fencestartx;
                Y = Fencestarty;
                Length = length;
                Dir = dir;
                BlockedPoints = new List<Vector>();
            }

            public override List<Vector> ReturnPoints()
            {

                if (Dir == "east")
                {

                    for (int i = X; i < X + Length; i++)
                    {
                        BlockedPoints.Add(new Vector(i, Y));
                    }

                }

                if (Dir == "north")
                {
                    for (int i = Y; i < Y + Length; i++)
                    {
                        BlockedPoints.Add(new Vector(X, i));
                    }
                }
                return BlockedPoints;
            }
        }

        internal class Sensor : Obstacle
        {
            public double Range { get; private set; }
            public Sensor(int sensorx, int sensory, double range)
            {
                X = sensorx;
                Y = sensory;
                Range = range;
                BlockedPoints = new List<Vector>();
            }

            public override List<Vector> ReturnPoints()
            {
                for (int y1 = Y - (int)Math.Ceiling(Range); y1 < (Y + Range); y1++)
                {
                    for (int x1 = (X - (int)Math.Ceiling(Range)); x1 < (X + Range); x1++)
                    {
                        double distance = Math.Sqrt((Math.Pow((x1 - X), 2)) + (Math.Pow((y1 - Y), 2)));
                        if (distance <= Range)
                        {
                            BlockedPoints.Add(new Vector(x1, y1));
                        }
                    }
                }
                return BlockedPoints;
            }
        }
        public class Camera : Obstacle
        {

            public int Camerax { get; private set; }
            public int Cameray { get; private set; }
            public string Direction { get; private set; }
            public Camera(int camerax, int cameray, string dir)
            {
                Camerax = camerax;
                Cameray = cameray;
                Direction = dir;
                BlockedPoints = new List<Vector>();
            }

            public override List<Vector> ReturnPointsCamera(int mapstartx, int mapstarty, int mapheight, int mapwidth)
            {
                double xChange;
                double yChange;
                switch (Direction)
                {
                    case "east":


                        for (int y = mapstarty; y <= mapstarty + mapwidth; y++)
                        {
                            for (int x = mapstartx; x <= mapstartx + mapheight; x++)
                            {
                                if (y == Cameray && x == Camerax)
                                {
                                    BlockedPoints.Add(new Vector(Cameray, Camerax));
                                }

                                yChange = Math.Abs(x - Cameray);
                                xChange = Math.Abs(y - Camerax);

                                if (xChange == 0)
                                {

                                    continue;
                                }
                                if (y < Camerax)
                                {
                                    continue;
                                }
                                double angle = Math.Atan(yChange / xChange);
                                angle = angle * (180.0 / Math.PI);
                                if (angle <= 45)
                                {
                                    BlockedPoints.Add(new Vector(x, y));
                                }



                            }

                        }
                        break;


                    case "west":

                        for (int x = mapstartx; x <= mapstartx + mapheight; x++)
                        {
                            for (int y = mapstarty; y <= mapstarty + mapwidth; y++)
                            {
                                if (y == Cameray && x == Camerax)
                                {
                                    BlockedPoints.Add(new Vector(Cameray, Camerax));
                                }

                                yChange = Math.Abs(x - Cameray);
                                xChange = Math.Abs(y - Camerax);

                                if (xChange == 0)
                                {

                                    continue;
                                }
                                if (y > Camerax)
                                {
                                    continue;
                                }
                                double angle = Math.Atan(yChange / xChange);
                                angle = angle * (180.0 / Math.PI);
                                if (angle <= 45)
                                {
                                    BlockedPoints.Add(new Vector(x, y));
                                }
                            }
                        }
                        return BlockedPoints;





                    case "north":
                        for (int x = mapstartx; x <= mapstartx + mapwidth; x++)
                        {
                            for (int y = mapstarty; y <= mapstarty + mapheight; y++)
                            {
                                if (y == Cameray && x == Camerax)
                                {
                                    BlockedPoints.Add(new Vector(Camerax, Cameray));
                                }

                                yChange = Math.Abs(y - Cameray);
                                xChange = Math.Abs(x - Camerax);

                                if (xChange == 0)
                                {

                                    continue;
                                }
                                if (x > Camerax)
                                {
                                    continue;
                                }
                                double angle = Math.Atan(yChange / xChange);
                                angle = angle * (180.0 / Math.PI);
                                if (angle <= 45)
                                {
                                    BlockedPoints.Add(new Vector(x, y));
                                }

                            }

                        }
                        return BlockedPoints;




                    case "south":

                        for (int x = mapstartx; x <= mapstartx + mapwidth; x++)
                        {
                            for (int y = mapstarty; y <= mapstarty + mapheight; y++)
                            {
                                if (y == Cameray && x == Camerax)
                                {
                                    BlockedPoints.Add(new Vector(Camerax, Cameray));
                                }

                                yChange = Math.Abs(y - Cameray);
                                xChange = Math.Abs(x - Camerax);

                                if (xChange == 0)
                                {

                                    continue;
                                }
                                if (x < Camerax)
                                {
                                    continue;
                                }
                                double angle = Math.Atan(yChange / xChange);
                                angle = angle * (180.0 / Math.PI);
                                if (angle <= 45)
                                {
                                    BlockedPoints.Add(new Vector(x, y));
                                }

                            }

                        }
                        return BlockedPoints;


                    default:
                        Console.WriteLine("Direction must be 'north', 'south', 'east' or 'west'.");
                        break;
                };
                return BlockedPoints;

            }
        }


        internal class ObstacleMap
        {

            public void UnsafePointReader(List<Vector> UnsafePoints, char symbol, Area[,] Obstaclemap, int mapStartx, int mapStarty, int mapwidth, int mapheight)
            {

                foreach (Vector point in UnsafePoints)
                {
                    Obstaclemap[point.X - mapStartx, point.Y - mapStarty].Safe = symbol;

                }
            }

            public int map_Startx { get; set; }
            public int map_Starty { get; set; }
            public int map_height { get; set; }
            public int map_width { get; set; }


            public ObstacleMap(int Map_Startx, int Map_Starty, int map_height, int map_width, List<Obstacle> ObstacleList)
            {
                Area[,] obstacleMap = new Area[map_width + 1, map_height + 1];
                if (map_width < 0 || map_height < 0)
                {
                    Console.WriteLine("Width and height must be valid positive integers.");
                    return;
                }


                Console.WriteLine("Here is a map of obstacles in the selected region:");

                for (int i = Map_Startx; i <= Map_Startx + map_width - 1; i++)
                {
                    for (int j = Map_Starty + map_height - 1; j >= Map_Starty; j--)
                    {
                        obstacleMap[i - Map_Startx, j - Map_Starty] = new Area(i, j);
                    }
                }

                {

                    foreach (Obstacle obstacle in ObstacleList)
                    {
                        List<Vector> unsafePoints;
                        Type type = obstacle.GetType();
                        if (type.Equals(typeof(Camera))) //if its a camera, runs a special return points method
                        {
                            unsafePoints = obstacle.ReturnPointsCamera(Map_Startx, Map_Starty, map_width, map_height);
                            UnsafePointReader(unsafePoints, 'C', obstacleMap, Map_Startx, Map_Starty, map_width, map_height);
                        }
                        else
                        {
                            unsafePoints = obstacle.ReturnPoints();
                        }
                        if (type.Equals(typeof(Guard)))
                        {

                            UnsafePointReader(unsafePoints, 'G', obstacleMap, Map_Startx, Map_Starty, map_width, map_height);

                        }
                        if (type.Equals(typeof(Fence)))
                        {

                            UnsafePointReader(unsafePoints, 'F', obstacleMap, Map_Startx, Map_Starty, map_width, map_height);
                        }
                        if (type.Equals(typeof(Sensor)))
                        {

                            UnsafePointReader(unsafePoints, 'S', obstacleMap, Map_Startx, Map_Starty, map_width, map_height);
                        }

                    }

                    for (int i = 0; i < obstacleMap.GetLength(0) - 1; i++)
                    {
                        for (int j = 0; j < obstacleMap.GetLength(1) - 1; j++)
                        {
                            Console.Write(obstacleMap[j, i].Safe);
                        }
                        Console.WriteLine("");
                    }
                }
            }
        }
        static void DirectionChecker(int Locationx, int Locationy, List<Vector> BlockedPointsList, List<Camera> ListOfCameras)
        {
            bool SafeNorth = true;
            bool SafeEast = true;
            bool SafeSouth = true;
            bool SafeWest = true;
            string SafeDirections = "";

            foreach (Camera camera in ListOfCameras)
            {
                BlockedPointsList.AddRange(camera.ReturnPointsCamera(Locationx - 1, Locationy - 1, Locationx + 1, Locationy + 1));

            }


            foreach (Vector point in BlockedPointsList)
            {
                if (point.X == Locationx && point.Y == Locationy)
                {
                    Console.WriteLine("Agent, your location is compromised. Abort mission.");
                    return;
                }
                if (point.X == Locationx + 1 && point.Y == Locationy)
                {
                    SafeEast = false;
                }
                if (point.X == Locationx - 1 && point.Y == Locationy)
                {
                    SafeWest = false;
                }
                if (point.Y == Locationy - 1 && point.X == Locationx)
                {
                    SafeNorth = false;
                }
                if (point.Y == Locationy + 1 && point.X == Locationx)
                {
                    SafeSouth = false;
                }
            }
            if (SafeNorth == true) { SafeDirections += "North\n"; }
            if (SafeSouth == true) { SafeDirections += "South\n"; }
            if (SafeEast == true) { SafeDirections += "West\n"; }
            if (SafeWest == true) { SafeDirections += "East\n"; }


            Console.WriteLine($"You can safely take any of the following directions:\n{SafeDirections}");
        }
    }
}