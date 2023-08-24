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