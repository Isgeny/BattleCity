using System;
using System.Linq;
using System.Drawing;
using System.Resources;
using System.Windows.Forms;

namespace BattleCity
{
    public abstract class Tank : DynamicObject
    {
        private int _hp;
        private int _lives;
        private int _stars;
        private bool _immortal;

        public bool Amphibian { get; set; }
        public bool Gun { get; set; }
        public int Ammo { get; set; }
        public int Points { get; set; }

        public Timer ShotDelayTimer { get; set; }

        private Timer _immortalTimer;
        public Timer ImmortalDelayTimer { get; protected set; }
        private int _immortalSpriteFrame;

        public Timer RespawnTimer { get; set; }
        private Timer _respawnDelayTimer;
        private int _respawnSpriteFrame;

        protected Timer ExplosionTimer { get; set; }
        protected int ExplosionFrame { get; set; }

        public event ShellEventHandler Shoot;

        public Tank(GUIForm guiForm, Rectangle rect, Direction direction) : base(guiForm, rect, direction)
        {
            ShotDelayTimer = new Timer();
            ShotDelayTimer.Interval = 150;
            ShotDelayTimer.Tick += OnShotDelayTimer;

            _immortalSpriteFrame = 0;
            _immortalTimer = new Timer();
            _immortalTimer.Interval = 30;
            _immortalTimer.Tick += OnImmortalTimer;
            _immortalTimer.Start();

            ImmortalDelayTimer = new Timer();
            ImmortalDelayTimer.Tick += OnImmortalDelayTimer;
            ImmortalDelayTimer.Interval = 4000;

            RespawnTimer = new Timer();
            RespawnTimer.Interval = 50;
            RespawnTimer.Tick += OnRespawnTimer;

            _respawnDelayTimer = new Timer();
            _respawnDelayTimer.Interval = 1500;
            _respawnDelayTimer.Tick += OnRespawnDelayTimer;
            _respawnSpriteFrame = 0;

            ExplosionTimer = new Timer();
            ExplosionTimer.Interval = 100;
            ExplosionTimer.Tick += OnExplosionTimer;
            ExplosionFrame = 0;
        }

        public override void Subscribe()
        {
            GUIForm.Paint += OnPaint;
            Destroyed += OnDestroyed;
        }

        public override void Unsubscribe()
        {
            GUIForm.Paint -= OnPaint;
            Destroyed -= OnDestroyed;
        }

        public virtual int HP
        {
            get { return _hp; }
            set
            {
                if(!Immortal)
                {
                    _hp = value;
                    if(_hp == 0)
                        Lives--;
                }
            }
        }

        public virtual int Lives
        {
            get { return _lives; }
            set
            {
                _lives = value;
                if(_lives < 0)
                    InvokeDestroyed();
            }
        }

        public virtual int Stars
        {
            get { return _stars; }
            set
            {
                _stars = (value == 4) ? 3 : value;
            }
        }

        public virtual bool Immortal
        {
            get { return _immortal; }
            set
            {
                _immortal = value;
                if(_immortal)
                {
                    ImmortalDelayTimer.Stop();
                    ImmortalDelayTimer.Start();
                    _immortalTimer.Start();
                }
                else
                {
                    ImmortalDelayTimer.Stop();
                    _immortalTimer.Stop();
                }
                GUIForm.Invalidate(Rect);
            }
        }

        protected virtual void OnPaint(object sender, PaintEventArgs e)
        {
            var clipRect = e.ClipRectangle;
            if(Rect.IntersectsWith(clipRect))
            {
                var g = e.Graphics;
                if(RespawnTimer.Enabled)
                    g.DrawImage(Properties.Resources.Tank_Respawn, Rect, new Rectangle(_respawnSpriteFrame, 0, Rect.Width, Rect.Height), GraphicsUnit.Pixel);
                else if(ExplosionTimer.Enabled)
                    g.DrawImage(Properties.Resources.Tank_Death, new Rectangle(Rect.X - 32, Rect.Y - 32, 128, 128), new Rectangle(ExplosionFrame, 0, 128, 128), GraphicsUnit.Pixel);
                else
                {
                    int currentFrame = GetCurrentSpriteFrame();
                    Bitmap bmp = GetCurrentSprite();

                    g.DrawImage(bmp, Rect, new Rectangle(currentFrame, 0, Rect.Width, Rect.Height), GraphicsUnit.Pixel);

                    if(Amphibian)
                        g.DrawImage(Properties.Resources.Ship_Shield, Rect.Location);
                    if(Immortal)
                        g.DrawImage(Properties.Resources.Shield, Rect, new Rectangle(_immortalSpriteFrame, 0, Rect.Width, Rect.Height), GraphicsUnit.Pixel);
                }
            }
        }

        protected override void OnDestroyed(object sender, EventArgs e)
        {
            base.OnDestroyed(sender, e);
            ExplosionFrame = 0;
            ExplosionTimer.Start();
            MoveTimer.Stop();
        }

        private void OnShotDelayTimer(object sender, EventArgs e)
        {
            ShotDelayTimer.Stop();
        }

        private void OnImmortalTimer(object sender, EventArgs e)
        {
            _immortalSpriteFrame = (_immortalSpriteFrame + 64) % 128;
            GUIForm.Invalidate(Rect);
        }

        private void OnImmortalDelayTimer(object sender, EventArgs e)
        {
            Immortal = false;
            ImmortalDelayTimer.Interval = 10000;
        }

        private void OnRespawnTimer(object sender, EventArgs e)
        {
            _respawnSpriteFrame = (_respawnSpriteFrame + 64) % 448;
            GUIForm.Invalidate(Rect);
        }

        protected virtual void OnRespawnDelayTimer(object sender, EventArgs e)
        {
            RespawnTimer.Stop();
            _respawnDelayTimer.Stop();
            Immortal = true;
        }

        private void OnExplosionTimer(object sender, EventArgs e)
        {
            ExplosionFrame = ExplosionFrame + 128;
            if(ExplosionFrame % 1152 == 0)
            {
                ExplosionTimer.Stop();
                if(this is CompTank)
                {
                    Unsubscribe();
                    GUIForm.Invalidate(Rectangle.Inflate(Rect, 128, 128));
                    Respawn();
                }
            }
            GUIForm.Invalidate(Rectangle.Inflate(Rect, 128, 128));
        }

        public abstract void InitializeTank();

        public virtual void Respawn()
        {
            HP = 1;
            GUIForm.Invalidate(Rect);
        }

        public void ShowRespawnAnimation()
        {
            RespawnTimer.Start();
            _respawnDelayTimer.Start();
        }

        protected Bitmap GetCurrentSprite()
        {
            ResourceManager rm = Properties.Resources.ResourceManager;
            string filename = GetType().Name + "_" + Stars + "_" + (int)Direction;
            Bitmap bmp = (Bitmap)rm.GetObject(filename);
            return bmp;
        }

        protected virtual int GetCurrentSpriteFrame()
        {
            return ((Rect.X + Rect.Y) % 8 < 4) ? 0 : 64;
        }

        protected void Turn()
        {
            //Округление координат перед поворотом
            if(Dx != 0 || Dy != 0)
            {
                Rectangle oldRect = Rect;
                int[] arr = new int[26];
                for(int i = 0; i < 26; i++)
                    arr[i] = i * 32 + 64;

                int targetNumber = 0;
                if(Dy != 0)
                {
                    targetNumber = Rect.X;
                    int closestX = arr.OrderBy(v => Math.Abs((long)v - targetNumber)).First();
                    Rect = new Rectangle(closestX, Rect.Y, Rect.Width, Rect.Height);
                }
                else if(Dx != 0)
                {
                    targetNumber = Rect.Y;
                    int closestY = arr.OrderBy(v => Math.Abs((long)v - targetNumber)).First();
                    Rect = new Rectangle(Rect.X, closestY, Rect.Width, Rect.Height);
                }
                GUIForm.Invalidate(oldRect);
                GUIForm.Invalidate(Rect);
            }
        }

        public bool IsAlive()
        {
            return (_lives >= 0) ? true : false;
        }

        protected void InvokeShoot(ShellEventArgs e)
        {
            Shoot?.Invoke(this, e);
        }
    }
}