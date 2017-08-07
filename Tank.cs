using System;
using System.Linq;
using System.Drawing;
using System.Windows.Forms;

namespace BattleCity
{
    public abstract class Tank : MovingObject
    {
        public Timer RespawnTimer { get; set; }
        protected int RespawnFrame { get; set; }

        public Timer ImmortalTimer { get; set; }
        protected int ImmortalFrame { get; set; }
        protected int ImmortalDelay { get; set; }

        private int _hp;
        public virtual int HP
        {
            get { return _hp; }
            set
            {
                if(!Immortal && !RespawnTimer.Enabled)
                {
                    _hp = value;
                    if(_hp == 0)
                    {
                        MoveTimer.Stop();
                        ExplosionTimer.Start();
                    }
                }
            }
        }

        private int _lives;
        public virtual int Lives
        {
            get { return _lives; }
            set
            {
                _lives = value;
                if(_lives < 0)
                {
                    MoveTimer.Stop();
                    InvokeDestroyed();
                    Unsubscribe();
                }
            }
        }

        private int _stars;
        public virtual int Stars
        {
            get { return _stars; }
            set
            {
                _stars = (value >= 4) ? 3 : value;
            }
        }

        private bool _immortal;
        public virtual bool Immortal
        {
            get { return _immortal; }
            set
            {
                _immortal = value;
                ImmortalFrame = 0;
                if(Immortal)
                    ImmortalTimer.Start();
                else
                {
                    ImmortalTimer.Stop();
                    ImmortalDelay = 8000;
                }
            }
        }

        public bool Amphibian { get; set; }
        public bool Gun { get; set; }
        public virtual int Ammo { get; set; }
        public int Points { get; set; }

        public event ShellEventHandler Shot;

        public Tank(GUIForm guiForm) : base(guiForm)
        {
            RespawnTimer = new Timer();
            RespawnTimer.Interval = 50;
            RespawnTimer.Tick += OnRespawnTimerTick;
            RespawnFrame = 0;

            ExplosionTimer = new Timer();
            ExplosionTimer.Interval = 50;
            ExplosionTimer.Tick += OnExplosionTimerTick;
            ExplosionFrame = 0;

            ImmortalTimer = new Timer();
            ImmortalTimer.Interval = 40;
            ImmortalTimer.Tick += OnImmortalTimerTick;
            ImmortalFrame = 0;
            ImmortalDelay = 8000;
        }

        public override void Subscribe()
        {
            GUIForm.Paint += OnPaint;
        }

        public override void Unsubscribe()
        {
            GUIForm.Paint -= OnPaint;
        }

        protected virtual void OnPaint(object sender, PaintEventArgs e)
        {
            var g = e.Graphics;
            if(Rect.IntersectsWith(e.ClipRectangle))
            {
                if(RespawnTimer.Enabled)
                    g.DrawImage(Properties.Resources.Tank_Respawn, Rect, new Rectangle(RespawnFrame % 448, 0, Rect.Width, Rect.Height), GraphicsUnit.Pixel);
                else if(ExplosionTimer.Enabled)
                    g.DrawImage(Properties.Resources.Tank_Death, new Rectangle(Rect.X - 32, Rect.Y - 32, 128, 128), new Rectangle(ExplosionFrame, 0, 128, 128), GraphicsUnit.Pixel);
                else
                {
                    var rm = Properties.Resources.ResourceManager;
                    var filename = GetType().Name + "_" + Stars + "_" + (int)Direction;
                    var bmp = (Bitmap)rm.GetObject(filename);
                    g.DrawImage(bmp, Rect, new Rectangle(GetCurrentSpriteFrame(), 0, Rect.Width, Rect.Height), GraphicsUnit.Pixel);

                    if(Immortal)
                        g.DrawImage(Properties.Resources.Shield, Rect, new Rectangle(ImmortalFrame % 128, 0, Rect.Width, Rect.Height), GraphicsUnit.Pixel);

                    if(Amphibian)
                        g.DrawImageUnscaled(Properties.Resources.Ship_Shield, Rect);
                }
            }
        }

        protected virtual int GetCurrentSpriteFrame()
        {
            return ((Rect.X + Rect.Y) % 8 < 4) ? 0 : 64;
        }

        protected void InvokeShot(ShellEventArgs e)
        {
            Shot?.Invoke(this, e);
        }

        public void RemoveShotListeners()
        {
            Shot = null;
        }

        protected override void OnCheckPosition(object sender, RectEventArgs e)
        {
            if(!ExplosionTimer.Enabled)
            {
                if(sender is Tank && Rect.IntersectsWith(e.NewRect) && !Rect.IntersectsWith(e.CurrentRect))
                    TankCollision(sender as Tank);
                else if(sender is Shell && Rect.IntersectsWith(e.NewRect))
                    ShellCollision(sender as Shell);
            }
        }

        protected virtual void OnRespawnTimerTick(object sender, EventArgs e)
        {
            RespawnFrame += 64;
            if(RespawnFrame == 896)
            {
                RespawnFrame = 0;
                RespawnTimer.Stop();

                MoveTimer.Start();

                ImmortalDelay = 4000;
                Immortal = true;
            }
            GUIForm.Invalidate(Rect);
        }

        protected virtual void OnExplosionTimerTick(object sender, EventArgs e)
        {
            ExplosionFrame += 128;
            if(ExplosionFrame == 1152)
            {
                ExplosionFrame = 0;
                ExplosionTimer.Stop();

                Dx = 0;
                Dy = 0;
                HP = 1;
                Immortal = false;
                Amphibian = false;
                Gun = false;
                Ammo = 1;

                Lives--;
            }
            GUIForm.Invalidate(Rectangle.Inflate(Rect, 128, 128));
        }

        protected void OnImmortalTimerTick(object sender, EventArgs e)
        {
            ImmortalFrame += 64;
            if(ImmortalFrame == (ImmortalDelay / ImmortalTimer.Interval * 64))
                Immortal = false;
            GUIForm.Invalidate(Rect);
        }

        public void StopMoving()
        {
            Dx = 0;
            Dy = 0;
        }

        //Округление координат при повороте
        protected void RoundCoordsOnTurning()
        {
            if(Dx != 0 || Dy != 0)
            {
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
            }
        }

        protected abstract void MoveToStartPosition();
        public abstract void SetNewGameParameters();
        public abstract void SetNewStageParameters();
    }
}