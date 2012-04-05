/*******************************************
 * 
 * Written by Cathal McNally
 * Date 05/04/2012
 * Free to all to make changes, but a mention would be nice :)
 * Released under the The MIT License.
 * http://www.opensource.org/licenses/mit-license.php 
 * 
 * Basic Paint Program i put together after i couldnt figure out how to implement it as part of another project
 * http://github.com/thefoofighter/KinectTest
 * 
 * I understand that the code is certainly not efficient nor is it the best possible way of implementing a paint program
 * It was the easiest and fastest way i found that did for me what i needed
 * 
 * Optimization issues include the List of Vector2 Objects which is going to keep adding to everytime the mouse is moved, while being clicked
 * Another issue is that the reddot is being redrawn at all these positions on every frame so thats numerous instances of the same object being draw onscreen
 * 
 * Possible resolutions would involve the renderToTarget Method, setData in a texture2D Background etc
 * 
 * Like i said it is not effiecient and could use optimzation
 * as well as that if you move the mouse too fast it will leave gaps :/
 * A possible solution for this if i have time would be to get the positions either side of teh gaps and fill the gap as a straight line of red dots 
 *  
 * Anyway i hope it helps someone sometime
 * 
 * Kinds regards
 * 
 * Cathal
 * 
 *********************************************/



using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Paint
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Paint : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Texture2D brush;
        MouseState current_mouse, previous_mouse;
        int mouseX, mouseY, prevPosX, prevPosY;
        public List<Vector2> coordinates { get; set; }

        public Paint()
        {
            graphics = new GraphicsDeviceManager(this);
            // Set the screen to 640*480
            graphics.PreferredBackBufferWidth = 640;
            graphics.PreferredBackBufferHeight = 480;
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
            // load in the red dot image for our brush
            brush = Content.Load<Texture2D>("reddot");
            // Create a new List of Vector2 objects
            coordinates = new List<Vector2>();
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
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            // TODO: Add your update logic here
            UpdateMouse(); 
            
            // If the left mouse button is clicked
            if (current_mouse.LeftButton == ButtonState.Pressed)
            {
                // Add the last position of the Mouse to the end of the list of Vector2 objects
                coordinates.Add(new Vector2(prevPosX, prevPosY));
            }
            base.Update(gameTime);
        }

        // Update Mouse Method
        protected void UpdateMouse()
        {
            // Set the previous Mouse postion to what the current mouse is
            previous_mouse = current_mouse;
            // Set the current Mouse position to... well the current mouse position 
            current_mouse = Mouse.GetState();

            // set the mouseX and mouseY values to that of the current mouse states X and Y values
            // This is used to update the position of the brush itself
            mouseX = current_mouse.X;
            mouseY = current_mouse.Y;

            // set the prevPosX and prevPosY values to that of the previous Mouse X and Y positions
            // This is then added to the list and then used as the position for painting the stroke
            prevPosX = previous_mouse.X;
            prevPosY = previous_mouse.Y;
        }
        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);
            // Begin the Draw
            spriteBatch.Begin();
                // Draw the brush cursor (reddot) and set its position to update to the current mouseX and mouseY values
                spriteBatch.Draw(brush, new Rectangle(mouseX, mouseY, 10, 10), Color.White);
                // get each vector2 position that is in the list and for each one draw the reddot at the current vector2's position
                foreach (Vector2 vect in coordinates)
                {
                    // Vector2's values are that of type float, (int) is used to cast them as integers
                    spriteBatch.Draw(brush, new Rectangle((int)vect.X, (int)vect.Y, 10, 10), Color.White);
                }

                // This here is not needed at all
                // What is does is draw a reddot at the previous position that is currently held by prevPosX and prevPosY
                // So its draws a trailing redot that is almost directly behind the main brush
                // try commenting it out and when you move the brush (while not left clicking) you will only see the cursor when you move it
                // however if you uncomment it again look closely at the cursor and you will see a slight blur behind it as if another reddot is chasing it
                // which in fact is what is happening
                // stumbled accross it by accident and i left it in because it looks nice :D               
                if (mouseX != 0 && mouseY != 0)
                {
                    spriteBatch.Draw(brush, new Rectangle(prevPosX, prevPosY, 10, 10), Color.White);
                }
            // End the Draw
            spriteBatch.End();
            // TODO: Add your drawing code here
            base.Draw(gameTime);
        }
    }
}
