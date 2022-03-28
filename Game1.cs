using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace monster_clicker
{   
    public class Game1 : Game
    {
        /*      Declaring variabels */
        Texture2D background, canTexture, shopBackgroundTexture, upgradesBackground, upgradeIcon;
        Texture2D theDoctor, pacificPunch, ultraBlue, zeroUltra, monarch, lewisHamilton;
        SpriteFont moneyFont, mpsFont, shopFont, amountFont, debugFont;
        Vector2 moneyTextPosition, mainCanPosition, shopBackroundPosition, mpsTextPosition, upgradeIconPosition;
        float money, mps, mainCanScaling, mainCanScale;
        bool mouseToggle, debugMode, keyboardToggle;
        decimal roundedMoney, roundedMps;
        List<ShopItem> shopItems;
        List<RainCan> rainCans;
        List<UpgradeItem> upgradeItems;
        ShopItem shopItem1, shopItem2, shopItem3, shopItem4, shopItem5, shopItem6;
        Random random;
        int mainCanShrinkDelay, mainCanShrinkDelayCount;
        string currentMenu;

        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;


        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            /*      Graphics/window     */
            _graphics.PreferredBackBufferWidth = 1920;
            _graphics.PreferredBackBufferHeight = 1080;
            _graphics.IsFullScreen = true;
            _graphics.ApplyChanges();

            /*      Position can/text/background        */
            mainCanPosition = new Vector2(0, 0);
            moneyTextPosition = new Vector2(_graphics.PreferredBackBufferWidth / 2 - 300, 100);
            mpsTextPosition = new Vector2(_graphics.PreferredBackBufferHeight / 2 - 300, 225);
            shopBackroundPosition = new Vector2(1320, 0);
            upgradeIconPosition = new Vector2(10, 990);

            /*      Initialize variables        */
            money = 0f;
            mps = 0f;
            mouseToggle = false;
            random = new Random();
            rainCans = new List<RainCan>();
            debugMode = false;
            mainCanScaling = 0.9f;
            mainCanScale = 1f;
            mainCanShrinkDelay = 0;
            mainCanShrinkDelayCount = 10;
            currentMenu = "none";

            /*      Initialize shop and items       */
            shopItem1 = new ShopItem(15, 0.2f, 1.1f);
            shopItem1.position = new Vector2(1370, 10);
            shopItem2 = new ShopItem(100, 0.8f, 1.1f);
            shopItem2.position = new Vector2(1370, 180);
            shopItem3 = new ShopItem(500, 5f, 1.1f);
            shopItem3.position = new Vector2(1370, 350);
            shopItem4 = new ShopItem(2000, 12f, 1.1f);
            shopItem4.position = new Vector2(1370, 520);
            shopItem5 = new ShopItem(7000, 23f, 1.1f);
            shopItem5.position = new Vector2(1370, 690);
            shopItem6 = new ShopItem(50000, 104f, 1.1f);
            shopItem6.position = new Vector2(1370, 860);

            shopItems = new List<ShopItem>
            {
                shopItem1,
                shopItem2,
                shopItem3,
                shopItem4,
                shopItem5,
                shopItem6
            };

            /*      Upgrades        */
            List<int> upgradeCost = new List<int>()
            {
                1
            };

            upgradeItems = new List<UpgradeItem>();

            for (int i = 0; i < upgradeCost.Count; i++)
            {
                UpgradeItem item = new UpgradeItem(i);
                upgradeItems.Add(item);
            }


            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            theDoctor = Content.Load<Texture2D>("theDoctor");
            pacificPunch = Content.Load<Texture2D>("pacificPunch");
            ultraBlue = Content.Load<Texture2D>("ultraBlue");
            zeroUltra = Content.Load<Texture2D>("zeroUltra");
            monarch = Content.Load<Texture2D>("monarch");
            lewisHamilton = Content.Load<Texture2D>("lewisHamilton");

            background = Content.Load<Texture2D>("background1");
            upgradesBackground = Content.Load<Texture2D>("upgradesBackground");
            canTexture = Content.Load<Texture2D>("mainCan");
            upgradeIcon = Content.Load<Texture2D>("upgradeIcon");

            shopBackgroundTexture = Content.Load<Texture2D>("shopBackground");
            shopItem1.texture = Content.Load<Texture2D>("shopItem1");
            shopItem1.canTexture = theDoctor;
            shopItem2.texture = Content.Load<Texture2D>("shopitem2");
            shopItem2.canTexture = pacificPunch;
            shopItem3.texture = Content.Load<Texture2D>("shopitem3");
            shopItem3.canTexture = ultraBlue;
            shopItem4.texture = Content.Load<Texture2D>("shopitem4");
            shopItem4.canTexture = zeroUltra;
            shopItem5.texture = Content.Load<Texture2D>("shopitem5");
            shopItem5.canTexture = monarch;
            shopItem6.texture = Content.Load<Texture2D>("shopitem6");
            shopItem6.canTexture = lewisHamilton;

            moneyFont = Content.Load<SpriteFont>("moneyFont");
            mpsFont = Content.Load<SpriteFont>("mpsFont");
            shopFont = Content.Load<SpriteFont>("shopFont");
            amountFont = Content.Load<SpriteFont>("amountFont");
            debugFont = Content.Load<SpriteFont>("debugFont");

            for (int i = 0; i < upgradeItems.Count; i++)
            {
                upgradeItems[i].SetTexture(Content.Load<Texture2D>("upgradeItem1"));
            }
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            /*      Controlls       */
            var keyState = Keyboard.GetState();

            // cheat
            if (keyState.IsKeyDown(Keys.Space))
            {
                money += 1000;
            }

            // debug
            if (keyState.IsKeyDown(Keys.D) && keyboardToggle == false)
            {
                debugMode = !debugMode;
                keyboardToggle = true;
            } else if (keyState.IsKeyUp(Keys.D))
            {
                keyboardToggle = false;
            }

            /*      Can positon     */
            mainCanPosition.X = _graphics.PreferredBackBufferWidth / 2 - 300 - canTexture.Width / 2;
            mainCanPosition.Y = _graphics.PreferredBackBufferHeight / 2 - canTexture.Height / 2;

            /*      mainCan     */
            var mouseState = Mouse.GetState();

            if (mainCanShrinkDelay > 0 )
            {
                mainCanShrinkDelay--;
            } else
            {
                mainCanScale = 1f;
            }

            if (mouseState.LeftButton == ButtonState.Pressed)
            {
                // Check if main can is clicked
                if (mouseState.X >= mainCanPosition.X && mouseState.X <= mainCanPosition.X + canTexture.Width &&
                    mouseState.Y >= mainCanPosition.Y && mouseState.Y <= mainCanPosition.Y + canTexture.Height &&
                    mouseToggle != true)
                {
                    money += 1f;
                    mainCanScale = mainCanScaling;
                    mainCanShrinkDelay += mainCanShrinkDelayCount;
                }

                /*      Shop        */
                // check if a shop item is pressed
                for (int i = 0; i < shopItems.Count; i++)
                {
                    if (mouseState.X >= shopItems[i].position.X && 
                        mouseState.X <= shopItems[i].position.X + shopItems[i].texture.Width &&
                    mouseState.Y >= shopItems[i].position.Y &&
                    mouseState.Y <= shopItems[i].position.Y + shopItems[i].texture.Height &&
                    mouseToggle != true)
                    {
                        if (money >= shopItems[i].cost)
                        {
                            money -= shopItems[i].cost;
                            shopItems[i].amount += 1;
                            mps += shopItems[i].mps;
                            shopItems[i].Purchased();
                        }
                    }
                }

                // upgrades
                if (mouseState.X >= upgradeIconPosition.X && mouseState.X <= upgradeIconPosition.X + upgradeIcon.Width &&
                    mouseState.Y >= upgradeIconPosition.Y && mouseState.Y <= upgradeIconPosition.Y + upgradeIcon.Height &&
                    mouseToggle != true)
                {
                    if (currentMenu == "upgrade")
                    {
                        currentMenu = "none";
                    } else
                    {
                        currentMenu = "upgrade";
                    }
                }

                    mouseToggle = true;

            } else if (mouseState.LeftButton == ButtonState.Released)
            {
                mouseToggle = false;
            }

            /*      Money and mps       */
            // update money and mps
            money += mps * (float)gameTime.ElapsedGameTime.TotalSeconds;
            roundedMoney = Math.Round((decimal)money, 0);
            roundedMps = Math.Round((decimal)mps, 1);

            // Update money/mps text position
            moneyTextPosition.X = 
                _graphics.PreferredBackBufferWidth / 2 - moneyFont.MeasureString(roundedMoney + "$").Length() / 2 - 300;
            mpsTextPosition.X =
                _graphics.PreferredBackBufferWidth / 2 - mpsFont.MeasureString(roundedMps + "$/s").Length() / 2 - 300;


            /*      Raining cans        */
            // Framecount
            for (int i = 0; i < shopItems.Count; i++)
            {
                shopItems[i].Update();
            }

            // Adding new rainCans
            for (int i = 0; i < shopItems.Count; i++)
            {
                if (shopItems[i].rainInterval == 0)
                {
                    continue;
                }
                if (shopItems[i].frameCount % shopItems[i].rainInterval == 0)
                {
                    RainCan can = new RainCan(shopItems[i].canTexture, random.Next(0, 1320), (float)random.NextDouble() * 400 + 200, random);
                    rainCans.Add(can);
                }
            }

            // Updating rainCans
            for (int i = 0; i < rainCans.Count; i++)
            {
                rainCans[i].Update(gameTime.ElapsedGameTime.TotalSeconds);
                if (rainCans[i].position.Y >= _graphics.PreferredBackBufferHeight)
                {
                    rainCans.RemoveAt(i);
                    i--;
                }
            }
            

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // background
            _spriteBatch.Begin();
            _spriteBatch.Draw(background, new Vector2(0, 0), Color.White);

            // rainCans
            for (int i = 0; i < rainCans.Count; i++)
            {
                _spriteBatch.Draw(rainCans[i].texture,
                    new Rectangle((int)rainCans[i].position.X + rainCans[i].texture.Width / 2, (int)rainCans[i].position.Y + rainCans[i].texture.Height / 2, rainCans[i].texture.Width, rainCans[i].texture.Height),
                    null, Color.White, rainCans[i].rotation, new Vector2(rainCans[i].texture.Width / 2, rainCans[i].texture.Height / 2), SpriteEffects.None, 0f);
            }

            // mainCan/text
            _spriteBatch.Draw(
                canTexture,
                new Vector2(mainCanPosition.X + canTexture.Width / 2, mainCanPosition.Y + canTexture.Height / 2),
                null,
                Color.White,
                0f,
                new Vector2(canTexture.Width / 2, canTexture.Height / 2),
                new Vector2(mainCanScale, mainCanScale),
                SpriteEffects.None,
                0f); ;
            _spriteBatch.DrawString(moneyFont, roundedMoney + "$", moneyTextPosition, Color.White);
            _spriteBatch.DrawString(mpsFont, roundedMps + "$/s", mpsTextPosition, Color.White);

            //shop
            _spriteBatch.Draw(shopBackgroundTexture, shopBackroundPosition, Color.White);
            _spriteBatch.Draw(shopItem1.texture, shopItem1.position, Color.White);
            for (int i = 0; i < shopItems.Count; i++)
            {
                _spriteBatch.Draw(shopItems[i].texture, shopItems[i].position, Color.White);
                _spriteBatch.DrawString(shopFont, shopItems[i].cost + "$",
                    shopItems[i].position + shopItems[i].costPosition, Color.Black);
                _spriteBatch.DrawString(shopFont, shopItems[i].mps + "$/s",
                    shopItems[i].position + shopItems[i].mpsPosition, Color.Black);
                _spriteBatch.DrawString(amountFont, shopItems[i].amount + "",
                    shopItems[i].position + shopItems[i].amountPosition, Color.Black);
            }

            // upgrades
            _spriteBatch.Draw(upgradeIcon, upgradeIconPosition, Color.White);
            if (currentMenu == "upgrade")
            {
                _spriteBatch.Draw(upgradesBackground, new Vector2(100, 100), Color.White);
                for (int i = 0; i < upgradeItems.Count; i++)
                {
                    _spriteBatch.Draw(upgradeItems[i].texture, upgradeItems[i].position, Color.White);
                }
            }

            // debug
            if (debugMode)
            {
                _spriteBatch.DrawString(debugFont, "debugMode", new Vector2(1, 1), Color.White);
                _spriteBatch.DrawString(debugFont, "rainCans: " + rainCans.Count, new Vector2(1, 16), Color.White);
                _spriteBatch.DrawString(debugFont, "menu: " + currentMenu, new Vector2(1, 31), Color.White);
            }

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
