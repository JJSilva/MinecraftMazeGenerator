using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;

namespace MinecraftMazeGenerator
{
    class Program
    {
        public static void Main(string[] args)
        {
            StringBuilder output = new StringBuilder();

            
            List<Tunnel> tunnels = new List<Tunnel>();
            
            Tunnel firstTunnel = new Tunnel();

            Console.WriteLine("Speciify Starting Coordinates");

            String input = Console.ReadLine();

            
            firstTunnel.Origin.X = Int32.Parse(input.Split(' ')[0]);
            firstTunnel.Origin.Y = Int32.Parse(input.Split(' ')[1]);
            firstTunnel.Origin.Z = Int32.Parse(input.Split(' ')[2]);
            firstTunnel.Destination.X = firstTunnel.Origin.X + 1;
            firstTunnel.Destination.Y = firstTunnel.Origin.Y + 2;
            firstTunnel.Destination.Z = firstTunnel.Origin.Z + 10;

            Console.WriteLine("How many tunnels?");
            int TunnelCount = Int32.Parse(Console.ReadLine());
            Console.WriteLine(String.Format("Starting at {0} {1} {2}", firstTunnel.Origin.X, firstTunnel.Origin.Y, firstTunnel.Origin.Z));



            Console.WriteLine("Generating Tunnels");
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

            } while (i < TunnelCount);

            Console.WriteLine("Generating Glass Block");
            Block glassBlock = new Block();
            glassBlock = glassBlock.CreateGlassBlock(tunnels);
            glassBlock.Command = glassBlock.GenerateFillCommand(glassBlock, Block.Type.glass);
            Console.WriteLine(glassBlock.Command);   
            output.AppendLine(glassBlock.Command);
            

            //output.AppendLine(Tunnel.GenerateCommand(tunnels.tunnels));
            foreach (Tunnel tunnel in tunnels)
            {
                Block block = new Block();
                block.Origin = tunnel.Origin;
                block.Destination = tunnel.Destination;
                block.Command = block.GenerateFillCommand(block, Block.Type.air);
                output.AppendLine(block.Command);
                Console.WriteLine(block.Command);
            }



            output.AppendLine("gamemode creative");
            output.AppendLine("give @a minecraft:torch 64");
            output.AppendLine(String.Format("setblock {0} {1} {2} chest replace",lastTunnel.Origin.X, lastTunnel.Origin.Y,lastTunnel.Origin.Z));
            output.AppendLine(String.Format("tp @a {0} {1} {2}", firstTunnel.Origin.X, firstTunnel.Origin.Y, firstTunnel.Origin.Z));


            string fifoPath = File.ReadAllLines("options.txt")[0];


            FileStream fs = File.OpenWrite(fifoPath);

            using (StreamWriter sw = new StreamWriter(fs))
            {
                sw.WriteLine("say hello world");
                sw.WriteLine(output.ToString());
            }

            Console.WriteLine(output.ToString());
            Console.ReadLine();
        }

    

    }

    public class Block
    {
        public Coordinate Origin;
        public Coordinate Destination;
        public string Command;

        public enum Type
        {
            glass,
            air
        }

        public Block()
        {
            Origin = new Coordinate();
            Destination = new Coordinate();
        }

        public Block(Coordinate Origin, Coordinate Destination)
        {
            this.Origin = Origin;
            this.Destination = Destination;
        }
                

        public String GenerateFillCommand(Block block, Type type)
        {
            block.Command = String.Format("fill {0} {1} {2} {3} {4} {5} {6}", block.Origin.X, block.Origin.Y, block.Origin.Z, block.Destination.X, block.Destination.Y, block.Destination.Z, type);
            return block.Command;
        }


        public Block CreateGlassBlock(List<Tunnel> tunnels)
        {
            Block block = new Block();

            
            block.Origin.X = tunnels.OrderBy(i => i.Origin.X).FirstOrDefault().Origin.X -1;
            block.Origin.Y = tunnels.OrderBy(i => i.Origin.Y).FirstOrDefault().Origin.Y -1;
            block.Origin.Z = tunnels.OrderBy(i => i.Origin.Z).FirstOrDefault().Origin.Z -1;

            block.Destination.X = tunnels.OrderByDescending(i => i.Destination.X).FirstOrDefault().Destination.X+1;
            block.Destination.Y = tunnels.OrderByDescending(i => i.Destination.Y).FirstOrDefault().Destination.Y+1;
            block.Destination.Z = tunnels.OrderByDescending(i => i.Destination.Z).FirstOrDefault().Destination.Z+1;

            //foreach (Tunnel tunnel in tunnels)
            //{
                
            //    if (tunnel.Origin.X > block.Origin.X)
            //    {
            //        block.Origin.X = tunnel.Origin.X;
            //    }

            //    if (tunnel.Origin.Z > block.Origin.Z)
            //    {
            //        block.Origin.Z = tunnel.Origin.Z;
            //    }


            //    if (tunnel.Destination.X > block.Destination.X)
            //    {
            //        block.Destination.X = tunnel.Destination.X;
            //    }

            //    if (tunnel.Destination.Z > block.Destination.Z)
            //    {
            //        block.Destination.Z = tunnel.Destination.Z;
            //    }

            //    block.Origin.Y = tunnel.Origin.Y - 1;
            //    block.Destination.Y = tunnel.Destination.Y + 1;

            //}
            return block;
        }

    }

    public class Coordinate
    {
        public int X;
        public int Y;
        public int Z;


       

    }
  
    public class Tunnel
    {
        public Coordinate Origin;
        public Coordinate Destination;

        public Tunnel()
        {
            Origin = new Coordinate();
            Destination = new Coordinate();
        }

       
        

        public Tunnel AddTunnel(Tunnel lastTunnel, int i)
        {
            Tunnel newTunnel = new Tunnel();

            Random random = new Random();

            int newTunnelDepth;
            newTunnelDepth = random.Next(5, 25);
            if (i % 2 == 0)
            {
                //Even
               
                newTunnel.Origin.X = lastTunnel.Origin.X - (newTunnelDepth / 2);
                newTunnel.Destination.X = lastTunnel.Origin.X + (newTunnelDepth / 2);

                newTunnel.Origin.Y = lastTunnel.Origin.Y;
                newTunnel.Destination.Y = lastTunnel.Destination.Y;

                newTunnel.Origin.Z = (lastTunnel.Destination.Z) - (newTunnelDepth / 2);
                newTunnel.Destination.Z = (newTunnel.Origin.Z + 1);
            }
            else
            {
                //Odd, Rotate
                newTunnel.Origin.X = lastTunnel.Origin.X;
                newTunnel.Destination.X = (newTunnel.Origin.X + 1);

                newTunnel.Origin.Y = lastTunnel.Origin.Y;
                newTunnel.Destination.Y = lastTunnel.Destination.Y;

                newTunnel.Origin.Z = lastTunnel.Destination.Z - (newTunnelDepth / 2);
                newTunnel.Destination.Z = lastTunnel.Destination.Z + (newTunnelDepth / 2);
            }
            return newTunnel;
        }

        //public static string GenerateCommand(IEnumerable<Tunnel> tunnels)
        //{
        //    StringBuilder output = new StringBuilder();
        //    foreach (Tunnel tunnel in tunnels)
        //    {
        //        //output.AppendLine(String.Format("fill {0} {1} {2} {3} {4} {5} minecraft:diamond_block",
        //        output.AppendLine(String.Format("fill {0} {1} {2} {3} {4} {5} air",
        //            tunnel.Origin.X,
        //            tunnel.Origin.Y,
        //            tunnel.Origin.Z,
        //            tunnel.Destination.X,
        //            tunnel.Destination.Y,
        //            tunnel.Destination.Z));
        //    }
        //    return output.ToString();
        //}
    }
}