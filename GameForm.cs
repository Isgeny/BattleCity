using System;
using System.Drawing;
using System.Collections.Generic;
using System.Windows.Forms;

namespace BattleCity
{
    public class GameForm : AbstractForm
    {
        private Field field;
        private PlayerTank p1Tank;
        private PlayerTank p2Tank;

        private PlayerTanksManager pTanksManager;
        private CompTanksManager cTanksManager;
        private Bonus bonus;

        public GameForm(GUIForm guiForm, GameManager gameManager, int players) : base(guiForm, gameManager)
        {
            field = new Field(guiForm, new RectangleF(64.0f, 64.0f, 832.0f, 832.0f));
            field.LoadStage(39);

            p1Tank = new FirstPlayerTank(GUIForm, new RectangleF(320.0f, 832.0f, 64.0f, 64.0f));
            p2Tank = new SecondPlayerTank(GUIForm, new RectangleF(576.0f, 832.0f, 64.0f, 64.0f));
        }

        public Field Field
        {
            get { return field; }
        }

        public PlayerTank P1Tank
        {
            get { return p1Tank; }
        }

        public PlayerTank P2Tank
        {
            get { return p2Tank; }
        }

        private void OnPaint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.FillRectangle(new SolidBrush(Color.FromArgb(102, 102, 102)), new Rectangle(0, 0, 1024, 960));
            g.FillRectangle(new SolidBrush(Color.Black), new Rectangle(64, 64, 832, 832));

            g.DrawImageUnscaled(Properties.Resources.Player_Icon, new Point(910, 626));
            string name = "";
            name += Properties.Settings.Default.P1Name[0];
            name += Properties.Settings.Default.P1Name[1];
            g.DrawString(p1Tank.Lives.ToString(), MyFont.GetFont(22), new SolidBrush(Color.Black), new PointF(940.0f, 626.0f));
            g.DrawString(name, MyFont.GetFont(22), new SolidBrush(Color.Black), new PointF(910.0f, 585.0f));

            g.DrawImageUnscaled(Properties.Resources.Player_Icon, new Point(910, 732));
            g.DrawString(p2Tank.Lives.ToString(), MyFont.GetFont(22), new SolidBrush(Color.Black), new PointF(940.0f, 732.0f));
            name = "";
            name += Properties.Settings.Default.P2Name[0];
            name += Properties.Settings.Default.P2Name[1];
            g.DrawString(name, MyFont.GetFont(22), new SolidBrush(Color.Black), new PointF(910.0f, 690.0f));
        }

        public override void Subscribe()
        {
            GUIForm.Paint += OnPaint;

            field.SubscribeToPaint();
            field.SubscribeToCheckPosition(p1Tank);
            field.SubscribeToCheckPosition(p2Tank);
            field.SubscribeToObstaclesDestroy();
            field.ObstacleDestroyed += OnObstacleDestroyed;

            p1Tank.SubscribeToPaint();
            p1Tank.SubscribeToCheckPosition(p2Tank);
            p1Tank.SubscribeToKeyDown();
            p1Tank.SubscribeToKeyUp();
            p1Tank.Shoot += OnShoot;

            p2Tank.SubscribeToPaint();
            p2Tank.SubscribeToCheckPosition(p1Tank);
            p2Tank.SubscribeToKeyDown();
            p2Tank.SubscribeToKeyUp();
            p2Tank.Shoot += OnShoot;

            GUIForm.Invalidate();
        }

        public override void Unsubscribe()
        {
            GUIForm.Paint -= OnPaint;

            field.UnsubscribeFromPaint();
            field.UnsubscribeFromCheckPosition(p1Tank);
            field.UnsubscribeFromCheckPosition(p2Tank);
            field.UnsubscribeFromObstaclesDestroy();
            field.ObstacleDestroyed -= OnObstacleDestroyed;

            p1Tank.UnsubscribeFromPaint();
            p1Tank.UnsubscribeFromCheckPosition(p2Tank);
            p1Tank.UnsubscribeFromKeyDown();
            p1Tank.UnsubscribeFromKeyUp();
            p1Tank.Shoot -= OnShoot;

            p2Tank.UnsubscribeFromPaint();
            p2Tank.UnsubscribeFromCheckPosition(p1Tank);
            p2Tank.UnsubscribeFromKeyDown();
            p2Tank.UnsubscribeFromKeyUp();
            p2Tank.Shoot -= OnShoot;
        }

        private void OnShoot(object sender, ShellEventArgs e)
        {
            Shell s = e.Shell;
            field.SubscribeToCheckPosition(s);
            s.SubscribeToCheckPosition(p1Tank);
            s.SubscribeToCheckPosition(p2Tank);
            s.SubscribeToPaint();
        }

        private void OnObstacleDestroyed(object sender, EventArgs e)
        {
            Object o = sender as Object;
            o.UnsubscribeFromCheckPosition(p1Tank);
            o.UnsubscribeFromCheckPosition(p2Tank);
        }
    }
}