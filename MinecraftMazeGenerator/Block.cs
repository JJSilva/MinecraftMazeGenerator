using System;
using System.Collections.Generic;
using System.Linq;

namespace MinecraftMazeGenerator
{
    public class Block
    {
        public Coordinate Origin;
        public Coordinate Destination;
        public string Command;

        public enum Type
        {
            glass,
            dirt,
            diamond_block,
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

        public Block AddBlock(Block lastTunnel, int i)
        {
            Block newBlock = new Block();

            Random random = new Random();

            int newBlockDepth;
            newBlockDepth = random.Next(5, 25);
            if (i % 2 == 0)
            {
                //Even

                newBlock.Origin.X = lastTunnel.Origin.X - (newBlockDepth / 2);
                newBlock.Destination.X = lastTunnel.Origin.X + (newBlockDepth / 2);

                newBlock.Origin.Y = lastTunnel.Origin.Y;
                newBlock.Destination.Y = lastTunnel.Destination.Y;

                newBlock.Origin.Z = (lastTunnel.Destination.Z) - (newBlockDepth / 2);
                newBlock.Destination.Z = (newBlock.Origin.Z + 1);
            }
            else
            {
                //Odd, Rotate
                newBlock.Origin.X = lastTunnel.Origin.X;
                newBlock.Destination.X = (newBlock.Origin.X + 1);

                newBlock.Origin.Y = lastTunnel.Origin.Y;
                newBlock.Destination.Y = lastTunnel.Destination.Y;

                newBlock.Origin.Z = lastTunnel.Destination.Z - (newBlockDepth / 2);
                newBlock.Destination.Z = lastTunnel.Destination.Z + (newBlockDepth / 2);
            }
            return newBlock;
        }

        public String GenerateFillCommand(Block block, Type type)
        {
            block.Command = String.Format("fill {0} {1} {2} {3} {4} {5} {6}", block.Origin.X, block.Origin.Y, block.Origin.Z, block.Destination.X, block.Destination.Y, block.Destination.Z, type);
            return block.Command;
        }


        public Block CreateGlassBlock(List<Block> blocks)
        {
            Block block = new Block();


            block.Origin.X = blocks.OrderBy(i => i.Origin.X).FirstOrDefault().Origin.X - 1;
            block.Origin.Y = blocks.OrderBy(i => i.Origin.Y).FirstOrDefault().Origin.Y - 1;
            block.Origin.Z = blocks.OrderBy(i => i.Origin.Z).FirstOrDefault().Origin.Z - 1;

            block.Destination.X = blocks.OrderByDescending(i => i.Destination.X).FirstOrDefault().Destination.X + 1;
            block.Destination.Y = blocks.OrderByDescending(i => i.Destination.Y).FirstOrDefault().Destination.Y + 1;
            block.Destination.Z = blocks.OrderByDescending(i => i.Destination.Z).FirstOrDefault().Destination.Z + 1;

            return block;
        }

    }

    public class Coordinate
    {
        public int X;
        public int Y;
        public int Z;
    }
}

