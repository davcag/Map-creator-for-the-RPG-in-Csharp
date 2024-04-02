#region Using Statements
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.GamerServices;
#endregion

namespace MapEditor
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;


        public enum Menu
        {
            TILES,
            SPRITES
        };

        public enum SpritePhase
        {
            DEFAULT,
            DISTANCE,
            TEXT
        }

        //font
        SpriteFont font;

        //mouse stuff
        MouseState ms;
        int prevScrollValue=0;
        private bool mousePressed = false;
        SpritePhase sPhase;
        Texture2D mouseTexture;
        //screen size
        private int screenWidth = 800;
        private int screenHeight = 640;
        //menu type
        Menu menu;
        //tiles
        List<Tile> tiles;
        TileSheet tileSheet;
        Tile selectedTile;
        bool tilePassable;
        Texture2D bgTexture;
        //Tilemap
        Tilemap tileMap;

        //sprites
        Dictionary<string, Sprite> sprites;
        SpriteSheet spriteSheet;
        Sprite selectedSprite;
        Sprite nSprite;
        Direction curDir;
        string spriteText;

        //scrolling
        int scrollOffsetX = 0;
        int scrollOffsetY = 0;


        bool keyPressed = false;




        public Game1()
            : base()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferHeight = screenHeight;
            graphics.PreferredBackBufferWidth = screenWidth;
            graphics.ApplyChanges();
            Content.RootDirectory = "Content";
           
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            this.IsMouseVisible = true;

            menu = Menu.TILES;

            //create a list of tiles
            tiles = new List<Tile>();
            tilePassable = true;

            //create a dictionary and load sprites;
            sprites = new Dictionary<string, Sprite>();
            sprites.Add("Enemy1", new Sprite("Enemy1", false, 20,5,5));
            sprites.Add("NPC1", new Sprite("NPC1", true,0,0,0));
            spriteText = "";

            //create the tile sheet
            tileSheet = new TileSheet(screenHeight);

            //create the sprite sheet
            spriteSheet = new SpriteSheet(screenHeight);

            //create the tilemap
            tileMap = new Tilemap();

            //direction
            curDir = Direction.RIGHT;

            sPhase = SpritePhase.DEFAULT;
            

            base.Initialize();
            
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here

            //load white texture;
            bgTexture = Content.Load<Texture2D>("White");

            //font
            font = Content.Load<SpriteFont>("Font");

            //load the tiles
            Tilemap.TilemapTexture = Content.Load<Texture2D>("Tiles/Tilemap");
            LoadTiles(Tilemap.TilemapTexture);


            //pass the tiles to the tile sheet
            tileSheet.Load(tiles);

            //Load the sprites
            sprites["Enemy1"].Texture = Content.Load<Texture2D>("Sprites/EnemySheet");
            sprites["NPC1"].Texture = Content.Load<Texture2D>("Sprites/NpcSheet");

            //load the mouse skin
            mouseTexture = Content.Load<Texture2D>("Mouse/Target");

            //pass the sprites to the sprite sheet
            spriteSheet.Load(sprites);

        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            ms = Mouse.GetState();

            if (menu == Menu.TILES)
            {
                tileSheet.Update();
            }
            else if (menu == Menu.SPRITES)
            {
                spriteSheet.Update();
            }

            MouseUpdate();
            KeyboardUpdate();
            


            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code here
            spriteBatch.Begin();
            //Draws a grid
            DrawGrid();


            //draw the tiles in the tilemap
            if(tileMap.tiles.Count != 0)
            {
                foreach(Tile tile in tileMap.tiles)
                {
                    //have to add the offset for the x because of the tile sheet
                    // it does not save it in the map since there will be no tile sheet 
                    // in the real game
                    // also have to remove the offset for scrolling
                    int x = (int)tile.Position.X+80-scrollOffsetX;
                    int y = (int)tile.Position.Y-scrollOffsetY;

                    if(x >= 80 && y >= 0)
                    {
                        spriteBatch.Draw(Tilemap.TilemapTexture , new Rectangle(x,y,64,64),tile.TilemapPosition, tile.Clr);
                    }   
                }
            }

            //draw the sprites in the tilemap
            if (tileMap.sprites.Count != 0)
            {
                foreach (Sprite sprite in tileMap.sprites)
                {
                    //have to add the offset for the x because of the tile sheet
                    // it does not save it in the map since there will be no tile sheet 
                    // in the real game
                    // also have to remove the offset for scrolling
                    int x = (int)sprite.Position.X + 80 - scrollOffsetX;
                    int y = (int)sprite.Position.Y - scrollOffsetY;

                    if (x >= 80 && y >= 0)
                    {
                        spriteBatch.Draw(sprites[sprite.Name].Texture, new Rectangle(x, y,60,60),new Rectangle(0,(int)sprite.Dir*60,60,60), sprite.Clr);
                    }
                }
            }

            //draws the sprite that is waiting for his distance to be added
            if (nSprite != null)
            {
                int x = (int)nSprite.Position.X + 80 - scrollOffsetX;
                int y = (int)nSprite.Position.Y - scrollOffsetY;
                spriteBatch.Draw(sprites[nSprite.Name].Texture, new Rectangle(x, y, 60, 60), new Rectangle(0, (int)nSprite.Dir * 60, 60, 60), nSprite.Clr);
            }

            if (menu == Menu.TILES)
            {
                //Draw the background that shows if tiles are passable or nor
                if(tilePassable)
                {
                    spriteBatch.Draw(bgTexture, new Rectangle(0, 0, 80, 640), Color.White);
                }
                else if (!tilePassable)
                {
                    spriteBatch.Draw(bgTexture, new Rectangle(0, 0, 80, 640), Color.Red);
                }

                //Draws the tile sheet
                tileSheet.Draw(spriteBatch);

                //draw selected tile at mouse position if a tile is selected
                if (selectedTile != null)
                {
                    spriteBatch.Draw(Tilemap.TilemapTexture, new Rectangle(ms.X,ms.Y,64,64), 
                        selectedTile.TilemapPosition, Color.White);
                }
            }
            else if (menu == Menu.SPRITES)
            {
                //Draws the sprite sheet
                spriteSheet.Draw(spriteBatch);


                //draw selected sprite at mouse position if a sprite is selected
                if (selectedSprite != null)
                {
                    spriteBatch.Draw(selectedSprite.Texture, new Rectangle(ms.X, ms.Y, 60, 60), new Rectangle(0, (int)curDir * 60, 60, 60), Color.White);
                }

                if(sPhase == SpritePhase.DISTANCE)
                {
                    spriteBatch.Draw(mouseTexture,new Vector2(ms.X-32,ms.Y-32),Color.White);
                }
                else if (sPhase == SpritePhase.TEXT)
                {
                    if (spriteText != null)
                    {
                        spriteBatch.DrawString(font, spriteText, new Vector2(nSprite.Position.X + 80 - scrollOffsetX, nSprite.Position.Y - 20 - scrollOffsetY), Color.Black);
                    }
                }
            }


            spriteBatch.End();
            base.Draw(gameTime);
        }





        /// <summary>
        /// Will handle keyboad input
        /// </summary>
        public void KeyboardUpdate()
        {
            if (sPhase != SpritePhase.TEXT)
            {
                //scrolling
                if (Keyboard.GetState().IsKeyDown(Keys.W) && scrollOffsetY > 0 && keyPressed == false)
                {
                    scrollOffsetY -= 64;
                    keyPressed = true;
                }
                else if (Keyboard.GetState().IsKeyDown(Keys.S) && keyPressed == false)
                {
                    scrollOffsetY += 64;
                    keyPressed = true;
                }
                else if (Keyboard.GetState().IsKeyDown(Keys.A) && scrollOffsetX > 0 && keyPressed == false)
                {
                    scrollOffsetX -= 64;
                    keyPressed = true;
                }
                else if (Keyboard.GetState().IsKeyDown(Keys.D) && keyPressed == false)
                {
                    scrollOffsetX += 64;
                    keyPressed = true;
                }
                //menu change
                else if (Keyboard.GetState().IsKeyDown(Keys.C) && keyPressed == false)
                {
                    if (menu == Menu.TILES)
                    {
                        menu = Menu.SPRITES;
                    }
                    else if (menu == Menu.SPRITES)
                    {
                        menu = Menu.TILES;
                    }
                    keyPressed = true;
                }
                //direction change
                else if (Keyboard.GetState().IsKeyDown(Keys.Up) && keyPressed == false)
                {
                    curDir = Direction.UP;
                    keyPressed = true;
                }
                else if (Keyboard.GetState().IsKeyDown(Keys.Down) && keyPressed == false) 
                {
                    curDir = Direction.DOWN;
                    keyPressed = true;
                }
                else if (Keyboard.GetState().IsKeyDown(Keys.Left) && keyPressed == false) 
                {
                    curDir = Direction.LEFT;
                    keyPressed = true;
                }
                else if (Keyboard.GetState().IsKeyDown(Keys.Right) && keyPressed == false) 
                {
                    curDir = Direction.RIGHT;
                    keyPressed = true;
                }
                else if (Keyboard.GetState().IsKeyDown(Keys.P) && keyPressed == false) 
                {
                    if(tilePassable)
                    {
                        tilePassable = false;
                    }
                    else if (!tilePassable)
                    {
                        tilePassable = true;
                    }

                    keyPressed = true;
                }
                else if (Keyboard.GetState().IsKeyUp(Keys.W)
                    && Keyboard.GetState().IsKeyUp(Keys.S)
                    && Keyboard.GetState().IsKeyUp(Keys.A)
                    && Keyboard.GetState().IsKeyUp(Keys.D)
                    && Keyboard.GetState().IsKeyUp(Keys.C)
                    && Keyboard.GetState().IsKeyUp(Keys.Up)
                    && Keyboard.GetState().IsKeyUp(Keys.Down)
                    && Keyboard.GetState().IsKeyUp(Keys.Left)
                    && Keyboard.GetState().IsKeyUp(Keys.Right)
                    && Keyboard.GetState().IsKeyUp(Keys.Enter)
                    && Keyboard.GetState().IsKeyUp(Keys.P)
                    && keyPressed == true)
                {
                    keyPressed = false;
                }


                //save a tilemap
                if (Keyboard.GetState().IsKeyDown(Keys.Space))
                {
                    tileMap.Save();
                }
                //load a tilemap
                if (Keyboard.GetState().IsKeyDown(Keys.L))
                {
                    tileMap.Load();
                }
            }
            else if (sPhase == SpritePhase.TEXT)
            {
                if (!Keyboard.GetState().IsKeyDown(Keys.Enter))
                {

                    if (Keyboard.GetState().GetPressedKeys().Length > 0 && keyPressed == false)
                    {
                        KeyboardState kbState = Keyboard.GetState();
                        Keys[] pressedKeys = kbState.GetPressedKeys();

                        foreach (Keys key in pressedKeys)
                        {
                            if (key != Keys.Back)
                            {
                                string txt;
                                switch (key)
                                {
                                    case Keys.Space:
                                        txt = " ";
                                        break;
                                    case Keys.OemComma:
                                        txt = ",";
                                        break;
                                    case Keys.OemPeriod:
                                        txt = ".";
                                        break;
                                    case Keys.OemQuestion:
                                        txt = "?";
                                        break;
                                    default:
                                        txt = key.ToString();
                                        break;
                                }

                                spriteText += txt;
                            }
                            else
                            {
                                spriteText = spriteText.Remove(spriteText.Length-1);
                            }

                        }
                        keyPressed = true;
                    }
                    else if (keyPressed == true && Keyboard.GetState().GetPressedKeys().Length == 0)
                    {
                        keyPressed = false;
                    }
 
                }

                else if (Keyboard.GetState().IsKeyDown(Keys.Enter))
                {
                    nSprite.Text = spriteText;
                    tileMap.Update(nSprite);
                    keyPressed = true;
                    spriteText = "";
                    sPhase = SpritePhase.DEFAULT;
                }
            }


            
        }

        /// <summary>
        /// Will handle mouse inputs
        /// </summary>
        public void MouseUpdate ()
        {
            if (menu == Menu.TILES)
            {
                //mouse scrolling
                if(ms.ScrollWheelValue != prevScrollValue)
                {
                    if(ms.ScrollWheelValue>prevScrollValue)
                    {
                        if (tileSheet.ScrollY != 0) 
                        {
                            tileSheet.ScrollY += 64;
                        }
                    }
                    else if (ms.ScrollWheelValue<prevScrollValue)
                    {
                        tileSheet.ScrollY -= 64;
                    }
                    prevScrollValue = ms.ScrollWheelValue;
                }

                //selecting the tile
                if (ms.X < 80 && ms.LeftButton == ButtonState.Pressed && mousePressed == false)
                {
                    //need to return the tile that was cliked on
                    selectedTile = tileSheet.GiveTile(ms.Y);

                    mousePressed = true;
                }
                else if (mousePressed == true && ms.LeftButton == ButtonState.Released)
                {
                    mousePressed = false;
                }

                //Add tiles to the tilemap
                if (selectedTile != null && ms.X > 80 && ms.LeftButton == ButtonState.Pressed)
                {
                    int x = (ms.X - 80 + scrollOffsetX) / 64;
                    int y = (ms.Y + scrollOffsetY) / 64;
                    Tile nTile = new Tile(selectedTile.TilemapPosition, tilePassable, new Rectangle(x * 64, y * 64,64,64));
                    tileMap.Update(nTile);

                }
            }
            if (menu == Menu.SPRITES)
            {
                //selecting the sprite
                if (ms.X < 80 && ms.LeftButton == ButtonState.Pressed && mousePressed == false)
                {
                    //need to return the sprite that was cliked on
                    selectedSprite = spriteSheet.GiveSprite(ms.Y);

                    mousePressed = true;
                }
                else if (mousePressed == true && ms.LeftButton == ButtonState.Released)
                {
                    mousePressed = false;
                }

                //Add sprites to the tilemap
                
                if (sPhase == SpritePhase.DEFAULT)
                {
                    if(!this.IsMouseVisible)
                    {
                        this.IsMouseVisible = true;
                    }
                    if (selectedSprite != null && ms.X > 80 && ms.LeftButton == ButtonState.Pressed && mousePressed == false)
                    {
                        int x = (ms.X - 80 + scrollOffsetX) / 64;
                        int y = (ms.Y + scrollOffsetY) / 64;
                        nSprite = new Sprite(selectedSprite.Name, selectedSprite.Friendly, new Vector2(x * 64, y * 64), curDir,
                            selectedSprite.Health, selectedSprite.Attack, selectedSprite.Defense);
                        sPhase = SpritePhase.DISTANCE;
                        mousePressed = true;
                    }
                }
                else if (sPhase == SpritePhase.DISTANCE)
                {
                    if (this.IsMouseVisible)
                    {
                        this.IsMouseVisible = false;
                    }


                    if (ms.X > 80 && ms.LeftButton == ButtonState.Pressed && mousePressed == false)
                    {
                        int dist = 0;
                        int x = ((ms.X - 80 + scrollOffsetX) / 64)*64;
                        int y = ((ms.Y + scrollOffsetY) / 64)*64;
                        switch(nSprite.Dir)
                        {
                            case Direction.UP:
                                dist = nSprite.Position.Y - y;
                                break;
                            case Direction.DOWN:
                                dist = y - nSprite.Position.Y;
                                break;
                            case Direction.LEFT:
                                dist = nSprite.Position.X - x;
                                break;
                            case Direction.RIGHT:
                                dist = x - nSprite.Position.X;
                                break;
                        }


                        nSprite.Distance = dist;

                        if (nSprite.Friendly)
                        {
                            sPhase = SpritePhase.TEXT;
                        }
                        else
                        {
                            tileMap.Update(nSprite);
                            sPhase = SpritePhase.DEFAULT;
                            mousePressed = true;
                        }
                    }
                }
                if (mousePressed == true && ms.LeftButton == ButtonState.Released)
                {
                    mousePressed = false;
                }
                
            }
 
        }



        /// <summary>
        /// Draws a grid
        /// </summary>
        public void DrawGrid()
        {
            int nRows = screenHeight/64;
            if (screenHeight/64 != 0)
            {
                nRows++;
            }
            int nCol = (screenWidth-80)/64;
            if ((screenWidth - 80) / 64 != 0)
            {
                nCol++;
            }

            for (int i = 1; i < nRows; i++)
            {
                Primitives2D.DrawLine(spriteBatch,80,64*i,screenWidth,64*i,Color.Gray);
            }
            for (int i = 0; i < nCol; i++)
            {
                Primitives2D.DrawLine(spriteBatch, 80 + (64 * i), 0, 80 + (64 * i), screenHeight, Color.Gray);
            }
        }


        public void LoadTiles(Texture2D TilemapTexture)
        {
            int nRows=TilemapTexture.Height/64;
            int nCols=TilemapTexture.Width/64;

            for (int y = 0; y<nRows; y++)
            {
                for (int x=0; x<nCols; x++)
                {
                    tiles.Add(new Tile(new Rectangle(x * 64, y * 64,64,64)));
                }
            }

        }

    }
}
