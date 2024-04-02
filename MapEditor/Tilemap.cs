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
using System.Windows.Forms;
using System.Xml;

namespace MapEditor
{
    class Tilemap
    {
        public List<Tile> tiles { get; private set; }
        public List<Sprite> sprites { get; private set; }
        public static Texture2D TilemapTexture { get; set; }
        
        public Tilemap ()
        {
            tiles = new List<Tile>();
            sprites = new List<Sprite>();
        }


        /// <summary>
        /// Adds tiles to the tilemap
        /// </summary>
        public void Update(Tile nTile)
        {
           
            //go trough the tiles
            foreach (Tile tile in tiles)
            {
                //checks if tiles is already selected so it wouldnt add it again
                if (nTile.Position == tile.Position && nTile.TilemapPosition == tile.TilemapPosition)
                {
                    return;
                }
                //Checks if a diferent tile is in that position and removes the old one
                else if (nTile.Position == tile.Position)
                {
                    tiles.Remove(tile);
                    tiles.Add(nTile);
                    return;
                }
            }
            //in case a empty spot is selected
            tiles.Add(nTile);
        }


        /// <summary>
        /// Adds sprites to the tilemap
        /// </summary>
        public void Update(Sprite nSprite)
        {

            //go trough the tiles
            foreach (Sprite sprite in sprites)
            {
                //checks if tiles is already selected so it wouldnt add it again
                if (nSprite.Position == sprite.Position && nSprite.Name == sprite.Name)
                {
                    return;
                }
                //Checks if a diferent tile is in that position and removes the old one
                else if (nSprite.Position == sprite.Position)
                {
                    sprites.Remove(sprite);
                    sprites.Add(nSprite);
                    return;
                }
            }
            //in case a empty spot is selected
            sprites.Add(nSprite);
        }



        /// <summary>
        /// Saves the tilemap as an xml document
        /// </summary>
        public void Save()
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "XML|*.xml";
            if(sfd.ShowDialog() == DialogResult.OK)
            {
                XmlTextWriter xWriter = new XmlTextWriter(sfd.FileName, Encoding.UTF8);
                xWriter.Formatting = Formatting.Indented;

                xWriter.WriteStartElement("Tilemap");//<Tilemap>

                xWriter.WriteStartElement("Tiles");//<Tils>
                foreach(Tile tile in tiles)
                {
                    xWriter.WriteStartElement("Tile");//<Tile>

                    xWriter.WriteStartElement("TilemapPosition"); //<TilemapPosition>
                        xWriter.WriteStartElement("X"); //<X>
                        xWriter.WriteString(tile.TilemapPosition.X.ToString());
                        xWriter.WriteEndElement();//</X>
                        xWriter.WriteStartElement("Y"); //<Y>
                        xWriter.WriteString(tile.TilemapPosition.Y.ToString());
                        xWriter.WriteEndElement();//</Y>
                    xWriter.WriteEndElement();//</TilemapPosition>
                    xWriter.WriteStartElement("Passable"); //<Passable>
                    xWriter.WriteString(tile.Passable.ToString());
                    xWriter.WriteEndElement();//</Passable>
                    xWriter.WriteStartElement("Position"); //<Position>
                        xWriter.WriteStartElement("X"); //<X>
                        xWriter.WriteString(tile.Position.X.ToString());
                        xWriter.WriteEndElement();//</X>
                        xWriter.WriteStartElement("Y"); //<Y>
                        xWriter.WriteString(tile.Position.Y.ToString());
                        xWriter.WriteEndElement();//</Y>
                    xWriter.WriteEndElement();//</Position>
                    xWriter.WriteStartElement("Color"); //<Color>
                        xWriter.WriteStartElement("R"); //<R>
                        xWriter.WriteString(tile.Clr.R.ToString());
                        xWriter.WriteEndElement();//</R>
                        xWriter.WriteStartElement("G"); //<G>
                        xWriter.WriteString(tile.Clr.G.ToString());
                        xWriter.WriteEndElement();//</G>
                        xWriter.WriteStartElement("B"); //<B>
                        xWriter.WriteString(tile.Clr.B.ToString());
                        xWriter.WriteEndElement();//</B>
                        xWriter.WriteStartElement("A"); //<A>
                        xWriter.WriteString(tile.Clr.A.ToString());
                        xWriter.WriteEndElement();//</A>
                    xWriter.WriteEndElement();//</Color>

                    xWriter.WriteEndElement();//</Tile>
                }
                xWriter.WriteEndElement();//</Tiles>
                

                xWriter.WriteStartElement("Sprites");//<Sprites>
                foreach(Sprite sprite in sprites)
                {
                    xWriter.WriteStartElement("Sprite"); //<Sprite>
                        xWriter.WriteStartElement("Name"); //<Name>
                        xWriter.WriteString(sprite.Name);
                        xWriter.WriteEndElement();//</Name>
                        xWriter.WriteStartElement("Friendly"); //<Friendly>
                        xWriter.WriteString(sprite.Friendly.ToString());
                        xWriter.WriteEndElement();//</Friendly>
                        xWriter.WriteStartElement("Position"); //<Position>
                            xWriter.WriteStartElement("X"); //<X>
                            xWriter.WriteString(sprite.Position.X.ToString());
                            xWriter.WriteEndElement();//</X>
                            xWriter.WriteStartElement("Y"); //<Y>
                            xWriter.WriteString(sprite.Position.Y.ToString());
                            xWriter.WriteEndElement();//</Y>
                        xWriter.WriteEndElement();//</Position>
                        xWriter.WriteStartElement("Color"); //<Color>
                            xWriter.WriteStartElement("R"); //<R>
                            xWriter.WriteString(sprite.Clr.R.ToString());
                            xWriter.WriteEndElement();//</R>
                            xWriter.WriteStartElement("G"); //<G>
                            xWriter.WriteString(sprite.Clr.G.ToString());
                            xWriter.WriteEndElement();//</G>
                            xWriter.WriteStartElement("B"); //<B>
                            xWriter.WriteString(sprite.Clr.B.ToString());
                            xWriter.WriteEndElement();//</B>
                            xWriter.WriteStartElement("A"); //<A>
                            xWriter.WriteString(sprite.Clr.A.ToString());
                            xWriter.WriteEndElement();//</A>
                        xWriter.WriteEndElement();//</Color>
                        xWriter.WriteStartElement("Direction"); //<Direction>
                        xWriter.WriteString(sprite.Dir.ToString());
                        xWriter.WriteEndElement();//</Direction>
                        xWriter.WriteStartElement("Distance"); //<Distance>
                        xWriter.WriteString(sprite.Distance.ToString());
                        xWriter.WriteEndElement();//</Distance>
                    if(sprite.Friendly)
                    {
                        xWriter.WriteStartElement("Text"); //<Text>
                        xWriter.WriteString(sprite.Text);
                        xWriter.WriteEndElement();//</Text>
                    }
                    else if (!sprite.Friendly)
                    {
                        xWriter.WriteStartElement("Health"); //<Health>
                        xWriter.WriteString(sprite.Health.ToString());
                        xWriter.WriteEndElement();//</Health>
                        xWriter.WriteStartElement("Attack"); //<Attack>
                        xWriter.WriteString(sprite.Attack.ToString());
                        xWriter.WriteEndElement();//</Attack>
                        xWriter.WriteStartElement("Defense"); //<Defense>
                        xWriter.WriteString(sprite.Defense.ToString());
                        xWriter.WriteEndElement();//</Defense>
                    }
                    xWriter.WriteEndElement();//</Sprite>

                }
                xWriter.WriteEndElement();//</Sprites>
                xWriter.WriteEndElement();//</Tilemap>

                xWriter.Close();
            }

        }

        /// <summary>
        /// Loads a tilemap from a xml document
        /// </summary>
        public void Load()
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "XML|*.xml";

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                //clear the old tiles and sprites to load the new ones
                tiles.Clear();
                sprites.Clear();
                XmlDocument xDoc = new XmlDocument();
                xDoc.Load(ofd.FileName);


                //load tiles
                foreach(XmlNode node in xDoc.SelectNodes("Tilemap/Tiles/Tile"))
                {
                    //read the xml stuff
                    Rectangle tilemapPos = new Rectangle (
                        int.Parse(node.SelectSingleNode("TilemapPosition/X").InnerText),
                        int.Parse(node.SelectSingleNode("TilemapPosition/Y").InnerText),64,64);

                    bool passable = bool.Parse(node.SelectSingleNode("Passable").InnerText);

                    Rectangle position = new Rectangle(int.Parse(node.SelectSingleNode("Position/X").InnerText), 
                        int.Parse(node.SelectSingleNode("Position/Y").InnerText),64,64);

                    Color clr = new Color(int.Parse(node.SelectSingleNode("Color/R").InnerText),
                        int.Parse(node.SelectSingleNode("Color/G").InnerText),
                        int.Parse(node.SelectSingleNode("Color/B").InnerText),
                        int.Parse(node.SelectSingleNode("Color/A").InnerText));

                    //add a new tile
                    tiles.Add(new Tile(tilemapPos, passable, position, clr));
                }

                //Load sprites
                foreach (XmlNode node in xDoc.SelectNodes("Tilemap/Sprites/Sprite"))
                {
                    //read the xml stuff
                    string name = node.SelectSingleNode("Name").InnerText;

                    bool friendly = bool.Parse(node.SelectSingleNode("Friendly").InnerText);

                    Vector2 position = new Vector2(float.Parse(node.SelectSingleNode("Position/X").InnerText), float.Parse(node.SelectSingleNode("Position/Y").InnerText));

                    Color clr = new Color(int.Parse(node.SelectSingleNode("Color/R").InnerText),
                        int.Parse(node.SelectSingleNode("Color/G").InnerText),
                        int.Parse(node.SelectSingleNode("Color/B").InnerText),
                        int.Parse(node.SelectSingleNode("Color/A").InnerText));

                    string direction = node.SelectSingleNode("Direction").InnerText;
                    Direction dir = Direction.RIGHT;

                    switch(direction)
                    {
                        case "UP":
                            dir = Direction.UP;
                            break;
                        case "DOWN":
                            dir = Direction.DOWN;
                            break;
                        case "LEFT":
                            dir = Direction.LEFT;
                            break;
                        case "RIGHT":
                            dir = Direction.RIGHT;
                            break;
                        default:
                            break;
                    }

                    int distance = int.Parse(node.SelectSingleNode("Distance").InnerText);

                    if (friendly)
                    {
                        string txt = node.SelectSingleNode("Text").InnerText;
                        //add a new sprite
                        sprites.Add(new Sprite(name, friendly, position, dir, distance, txt, clr,0,0,0));
                    }
                    else
                    {
                        int hp = int.Parse(node.SelectSingleNode("Health").InnerText);
                        int att = int.Parse(node.SelectSingleNode("Attack").InnerText);
                        int def = int.Parse(node.SelectSingleNode("Defense").InnerText);
                        //add a new sprite
                        sprites.Add(new Sprite(name, friendly, position, dir, distance, clr, hp, att, def));
                    }
                }

            }


        }



    }
}
