using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Text;

namespace monster_clicker
{
    public class ShopItem
    {
        public int cost, amount, rainInterval, frameCount;
        public float mps, modifier;
        public Texture2D texture, canTexture;
        public Vector2 position, costPosition, mpsPosition, amountPosition;
        public ShopItem(int _cost, float _mps, float _modifier)
        {
            amount = 0;
            cost = _cost;
            mps = _mps;
            modifier = _modifier;

            costPosition = new Vector2(225, 55);
            mpsPosition = new Vector2(225, 110);
            amountPosition = new Vector2(390, 70);

            rainInterval = 0;
        }

        public void Update()
        {
            if (amount > 0)
            {
                if (frameCount == 300)
                {
                    frameCount = 0;
                }
                frameCount++;

            }
        }

        public void Purchased()
        {
            cost = (int)(cost * modifier);
            if (amount == 1)
            {
                rainInterval = 300;
            }
            else if (amount == 10)
            {
                rainInterval = 150;
            }
            else if (amount == 100)
            {
                rainInterval = 120;
            }
        }
    }
}
