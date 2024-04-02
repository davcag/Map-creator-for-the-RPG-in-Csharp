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
    public enum Direction
    {
        UP =3 ,
        DOWN =2 ,
        LEFT =1 ,
        RIGHT =0
    };

    

    class Sprite
    {
        public string Name { get; private set; }
        public bool Friendly { get; private set; }
        public Color Clr { get; private set; }
        public int Health { get; private set; }
        public int Attack { get; private set; }
        public int Defense { get; private set; }
        public int Distance { get; set; }
        public string Text { get; set; }

        public Direction Dir;

        public Rectangle Position { get; set; }
        public Rectangle Destination { get; set; }
        public Texture2D Texture { get; set; }

        public Sprite(string name, bool friendly, int hp, int att, int def)
        {
            Name = name;
            Friendly = friendly;
            Clr = Color.White;
            Health = hp;
            Attack = att;
            Defense = def;
        }

        public Sprite(string name, bool friendly, Vector2 position, Direction dir, 
            int hp, int att, int def)
        {
            Name = name;
            Friendly = friendly;
            Clr = Color.White;
            Position = new Rectangle((int)position.X, (int)position.Y, 60,60);
            Destination = new Rectangle(0, 60 * (int)Dir, 60, 60);
            Dir = dir;
            Health = hp;
            Attack = att;
            Defense = def;
        }

        public Sprite(string name, bool friendly, Vector2 position, Direction dir, 
            int dist, Color clr, int hp, int att, int def)
        {
            Name = name;
            Friendly = friendly;
            Clr = clr;
            Position = new Rectangle((int)position.X, (int)position.Y, 60, 60);
            Destination = new Rectangle(0, 60*(int)Dir, 60, 60);
            Dir = dir;
            Distance = dist;
            Health = hp;
            Attack = att;
            Defense = def;
        }

        public Sprite(string name, bool friendly, Vector2 position, Direction dir, int dist, 
            string text, Color clr, int hp, int att, int def)
        {
            Name = name;
            Friendly = friendly;
            Clr = clr;
            Position = new Rectangle((int)position.X, (int)position.Y, 60, 60);
            Destination = new Rectangle(0, 60 * (int)Dir, 60, 60);
            Dir = dir;
            Distance = dist;
            Text = text;
            Health = hp;
            Attack = att;
            Defense = def;
        }

    }
}
