using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;


namespace MinecraftMazeGenerator
{
    class Program
    {
        public static void Main(string[] args)
        {
            StringBuilder output = new StringBuilder();

            List<Block> blocks = new List<Block>();
            Block firstBlock = new Block();

            Console.WriteLine("Speciify Starting Coordinates");
            String input = Console.ReadLine();

            firstBlock.Origin.X = Int32.Parse(input.Split(' ')[0]);
            firstBlock.Origin.Y = Int32.Parse(input.Split(' ')[1]);
            firstBlock.Origin.Z = Int32.Parse(input.Split(' ')[2]);
            firstBlock.Destination.X = firstBlock.Origin.X + 1;
            firstBlock.Destination.Y = firstBlock.Origin.Y + 2;
            firstBlock.Destination.Z = firstBlock.Origin.Z + 10;

            Console.WriteLine("How many tunnels?");
            int TunnelCount = Int32.Parse(Console.ReadLine());
            Console.WriteLine(String.Format("Starting at {0} {1} {2}", firstBlock.Origin.X, firstBlock.Origin.Y, firstBlock.Origin.Z));

            Console.WriteLine("Generating Tunnels");
            int i = 0;
            blocks.Add(firstBlock);
            Block lastBlock = new Block();
            lastBlock = firstBlock;
            do
            {
                
                Block newBlock = new Block();
                newBlock = newBlock.AddBlock(lastBlock, i);
                blocks.Add(newBlock);
                lastBlock = newBlock;
                i++;

            } while (i < TunnelCount);

            Console.WriteLine("Generating Glass Block");
            Block glassBlock = new Block();
            glassBlock = glassBlock.CreateGlassBlock(blocks);
            glassBlock.Command = glassBlock.GenerateFillCommand(glassBlock, Block.Type.dirt);
            Console.WriteLine(glassBlock.Command);   
            output.AppendLine(glassBlock.Command);

            foreach (Block block in blocks)
            {
                block.Command = block.GenerateFillCommand(block, Block.Type.air);
                output.AppendLine(block.Command);
                Console.WriteLine(block.Command);
            }

            output.AppendLine("gamemode creative");
            output.AppendLine("give @a minecraft:torch 64");
            output.AppendLine(String.Format("setblock {0} {1} {2} chest replace",lastBlock.Origin.X, lastBlock.Origin.Y, lastBlock.Origin.Z));
            output.AppendLine(String.Format("tp @a {0} {1} {2}", firstBlock.Origin.X, firstBlock.Origin.Y, firstBlock.Origin.Z));
            output.AppendLine(String.Format("setblock {0} {1} {2} minecraft:torch keep", firstBlock.Origin.X, firstBlock.Origin.Y, firstBlock.Origin.Z));


            string fifoPath = File.ReadAllLines("options.txt")[0];
            fifoPath = fifoPath + "/fifo";
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

    
}