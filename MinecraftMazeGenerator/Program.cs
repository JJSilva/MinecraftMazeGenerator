using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace MinecraftMazeGenerator
{
    class Program
    {
        public static void Main(string[] args)
        {
            
           List<Tunnel> tunnels = new List<Tunnel>();
           Tunnel firstTunnel = new Tunnel();

           Console.WriteLine("Speciify Starting Coordinates");
            
            String input = Console.ReadLine();

            firstTunnel.OriginX = Int32.Parse(input.Split(' ')[0]);
            firstTunnel.OriginY = Int32.Parse(input.Split(' ')[1]);
            firstTunnel.OriginZ = Int32.Parse(input.Split(' ')[2]);
            firstTunnel.DestinationX = firstTunnel.OriginX + 1;
            firstTunnel.DestinationY = firstTunnel.OriginY + 1;
            firstTunnel.DestinationZ = firstTunnel.OriginZ + 10;


            Console.WriteLine(String.Format("Starting at {0} {1} {2}", firstTunnel.OriginX, firstTunnel.OriginY, firstTunnel.OriginZ));
            int i = 0;
            tunnels.Add(firstTunnel);


            Tunnel lastTunnel = new Tunnel();
            lastTunnel = firstTunnel;
            do
            {
                Tunnel newTunnel = new Tunnel();
                newTunnel = newTunnel.AddTunnel(lastTunnel, i);
                tunnels.Add(newTunnel);
                lastTunnel = newTunnel;
                i++;

            } while (i < 10);

            String output = Tunnel.GenerateCommand(tunnels);

            Console.WriteLine(output);
            Console.ReadLine();
        }

        
    }
        

    public class Tunnel
    {
        public int OriginX;
        public int OriginY;
        public int OriginZ;

        public int DestinationX;
        public int DestinationY;
        public int DestinationZ;
  

        public Tunnel AddTunnel(Tunnel lastTunnel, int i)
        {
            Tunnel newTunnel = new Tunnel();
           
            Random random = new Random();

            int newTunnelDepth = random.Next(5, 25);



            if (i % 2 == 0)
            {
                //Even
                newTunnel.OriginX = lastTunnel.OriginX - (newTunnelDepth / 2);
                newTunnel.DestinationX = lastTunnel.OriginX + (newTunnelDepth / 2);

                newTunnel.OriginY = lastTunnel.DestinationY;
                newTunnel.DestinationY = lastTunnel.DestinationY;

                newTunnel.OriginZ = lastTunnel.DestinationZ;
                newTunnel.DestinationZ = (lastTunnel.DestinationZ + 1);

            }
            else
            {
                //Odd, Rotate


                newTunnel.OriginX = lastTunnel.OriginX;
                newTunnel.DestinationX = (lastTunnel.OriginX +1);

                newTunnel.OriginY = lastTunnel.DestinationY;
                newTunnel.DestinationY = lastTunnel.DestinationY;

                //newTunnel.OriginZ = lastTunnel.DestinationZ;
                //newTunnel.DestinationZ = (lastTunnel.DestinationZ + newTunnelDepth);

                newTunnel.OriginZ = lastTunnel.DestinationZ - (newTunnelDepth / 2);
                newTunnel.DestinationZ = lastTunnel.DestinationZ + (newTunnelDepth / 2);
            }

            return newTunnel;
        }

        public static string GenerateCommand(IEnumerable<Tunnel> tunnels)
        {
            StringBuilder output = new StringBuilder();
            foreach (Tunnel tunnel in tunnels)
            {
                output.AppendLine(String.Format("fill {0} {1} {2} {3} {4} {5} minecraft:diamond_block",
                    tunnel.OriginX,
                    tunnel.OriginY,
                    tunnel.OriginZ,
                    tunnel.DestinationX,
                    tunnel.DestinationY,
                    tunnel.DestinationZ));
            }
            return output.ToString();
        }
    }
    //class Program
    //{
    //    static void Main(string[] args)
    //    {
    //        int i = 0;
    //        int startX = 0;
    //        int startY = 0;
    //        int startZ = 0;
    //        Program p = new Program();
    //        StringBuilder output = new StringBuilder();

    //        CommandOutput lastCommandOutput = new CommandOutput({ LastX = startX, LastY = startY, LastZ = startZ; }; });


    //        do {
    //            CommandOutput lastCommandOutput= new CommandOutput();
    //            lastCommandOutput.LastX = 1;
    //            lastCommandOutput.LastY = 2;
    //            lastCommandOutput.LastZ = 3;

    //            lastCommandOutput.AddCommand(lastCommandOutput);

    //            output.AppendLine(lastCommandOutput.output.ToString());
    //            i++;
    //        } while (i < 10);

    //        Console.WriteLine(output.ToString());
    //        Console.ReadLine();
    //    }
       

    //}
    //class CommandOutput
    //{
    //    public int LastX;
    //    public int LastY;
    //    public int LastZ;
    //    public StringBuilder output;

    //    public CommandOutput AddCommand(CommandOutput lastCommandOutput)
    //    {
                      

    //        Random random = new Random();
    //        int depth = random.Next(20, 50);

    //        lastCommandOutput.output.Append("/fill ");
    //        lastCommandOutput.output.Append(String.Format("{0} {1} {2} {3} {4} {5}", 1, 2, LastZ, 4, 5, depth));
    //        lastCommandOutput.output.Append(" air");

    //        return lastCommandOutput;
    //    }

}




//using System;
//using System.Text;

//class Program
//{
//    static Random random = new Random();
//    static int mazeSize = 10; // Adjust the maze size as needed
//    static char[][] maze;

//    static void Main()
//    {
//        maze = new char[mazeSize][];
//        InitializeMaze();
//        GenerateMaze();
//        PrintMazeCommands();
//    }

//    static void InitializeMaze()
//    {
//        for (int i = 0; i < mazeSize; i++)
//        {
//            maze[i] = new char[mazeSize];
//            for (int j = 0; j < mazeSize; j++)
//            {
//                // Fill the maze with walls initially
//                maze[i][j] = 'W'; // 'W' represents a wall in Minecraft
//            }
//        }
//    }

//    static void GenerateMaze()
//    {
//        // Implement your maze generation algorithm here.
//        // For simplicity, let's just create a random path.
//        int x = 1;
//        int y = 1;
//        maze[x][y] = 'A'; // 'A' represents air in Minecraft

//        while (x < mazeSize - 2 || y < mazeSize - 2)
//        {
//            int direction = random.Next(4); // 0: up, 1: right, 2: down, 3: left

//            if (direction == 0 && x > 1)
//            {
//                x--;
//            }
//            else if (direction == 1 && y < mazeSize - 2)
//            {
//                y++;
//            }
//            else if (direction == 2 && x < mazeSize - 2)
//            {
//                x++;
//            }
//            else if (direction == 3 && y > 1)
//            {
//                y--;
//            }

//            maze[x][y] = 'A';
//        }
//    }

//    static void PrintMazeCommands()
//    {
//        StringBuilder commands = new StringBuilder();

//        for (int i = 0; i < mazeSize; i++)
//        {
//            for (int j = 0; j < mazeSize; j++)
//            {
//                if (maze[i][j] == 'A')
//                {
//                    // Generate Fill Commands for air blocks
//                    string fillCommand = $"/fill {i} 50 {j} {i} 50 {j} minecraft:air\n";
//                    commands.Append(fillCommand);
//                }
//            }
//        }

//        Console.WriteLine(commands.ToString());
//    }
//}

