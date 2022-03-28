using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Text;

namespace monster_clicker
{
    public class RainCan
    {
        public Texture2D texture;
        public Vector2 position;
        public float rotation;
        private float rotationSpeed;
        private float rotationDirection;
        private float velY;
        public RainCan(Texture2D _texture, int _x, float _vely, Random random)
        {
            texture = _texture;
            position.X = _x;
            velY = _vely;
            position.Y = 0 - texture.Height;
            rotation = 0f;
            rotationSpeed = (float)random.NextDouble() * 2.14f + 1f;
            int randomInt = random.Next(0, 2);
            rotationDirection = 2 * randomInt - 1;
        }

        public void Update(double deltaTime)
        {
            position.Y += velY * (float)deltaTime;
            rotation += rotationSpeed * (float)deltaTime * rotationDirection;
        }
    }
}
