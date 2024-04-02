using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.GamerServices;

namespace MapEditor
{
    class Tile
    {
        public Rectangle TilemapPosition { get; private set; }
        public bool Passable { get; private set; }
        public Color Clr { get; private set; }

        public Rectangle Position { get; set; }
        public Texture2D Texture { get; set; }

        public Tile(Rectangle mapPos)
        {
            TilemapPosition = mapPos;
            Clr = Color.White;
        }

        public Tile(Rectangle mapPos, bool passable, Rectangle position)
        {
            TilemapPosition = mapPos;
            Passable = passable;
            Clr = Color.White;
            Position = position;
        }

        public Tile(Rectangle mapPos, bool passable, Rectangle position, Color clr)
        {
            TilemapPosition = mapPos;
            Passable = passable;
            Clr = clr;
            Position = position;
        }
    }
}
