using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Text;

namespace monster_clicker
{
    public class UpgradeItem
    {
        public Texture2D texture;
        public Vector2 position;
        public int index;

        public UpgradeItem (int _index)
        {
            index = _index;
            position = new Vector2(110 + 80 * index, 110);
        }

        public void SetTexture(Texture2D _texture)
        {
            texture = _texture;
        }
    }
}
