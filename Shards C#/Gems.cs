using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Shards_CSharp
{
    class Gems
    {

        static readonly Random rnd = new Random(DateTime.Now.Millisecond);

        Vector2 position;
        Vector2 motion;
        float gemSpeed = 1f;

        Texture2D texture;
        Rectangle screenBounds;

        int dirX = 0;
        int dirY = 0;

        public Gems(Texture2D texture, Rectangle screenBounds)
        {

            this.texture = texture;
            this.screenBounds = screenBounds;
            SetInStartPosition();

        }

        public void Update()
        {

            motion = Vector2.Zero;

            if (dirX == 0)
            {
                motion.X = 3;
            }
            else
            {
                motion.X = -3;
            }

            if (dirY == 0)
            {
                motion.Y = 3;
            }
            else
            {
                motion.Y = -3;
            }

            motion.X *= gemSpeed;
            motion.Y *= gemSpeed;
            position += motion;

            LockGem();
        }

        private void LockGem()
        {
            if (position.X < 128)
            {
                position.X = 128;
                dirX = 0;
            }

            if (position.X > 848)
            {
                position.X = 848;
                dirX = 1;
            }
            

            if (position.Y < 128)
            {
                position.Y = 128;
                dirY = 0;
            }

            if (position.Y > 720)
            {
                position.Y = 720;
                dirY = 1;
            }

            if (position.X + texture.Width > screenBounds.Width)
            {
                position.X = screenBounds.Width - texture.Width;
            }
        }

        public void SetInStartPosition()
        {  
                position.X = (screenBounds.X + (int)(rnd.NextDouble() * screenBounds.Width));
                position.Y = 0;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, Color.White);
        }

        public Rectangle GetBounds()
        {
            return new Rectangle(
            (int)position.X,
            (int)position.Y,
            texture.Width,
            texture.Height);
        }

    }
}
