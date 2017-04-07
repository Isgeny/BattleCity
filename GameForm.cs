using System;
using System.Drawing;
using System.Collections.Generic;
using System.Windows.Forms;

namespace BattleCity
{
    public class GameForm : AbstractForm
    {
        private Field _field;
        private PlayerTank _p1Tank;
        private PlayerTank _p2Tank;
        private List<Shell> _shells;
        private BonusManager _bonusManager;

        //private PlayerTanksManager pTanksManager;
        //private CompTanksManager cTanksManager;

        public GameForm(GUIForm guiForm, GameManager gameManager) : base(guiForm, gameManager)
        {
            _field = new Field(guiForm, new RectangleF(64.0f, 64.0f, 832.0f, 832.0f));
            _field.LoadStage(1);

            _p1Tank = new FirstPlayerTank(GUIForm, new RectangleF(320.0f, 832.0f, 64.0f, 64.0f));
            _p2Tank = new SecondPlayerTank(GUIForm, new RectangleF(576.0f, 832.0f, 64.0f, 64.0f));

            _shells = new List<Shell>();

            _bonusManager = new BonusManager(guiForm);
        }

        public Field Field
        {
            get { return _field; }
        }

        public PlayerTank P1Tank
        {
            get { return _p1Tank; }
        }

        public PlayerTank P2Tank
        {
            get { return _p2Tank; }
        }

        public override void Subscribe()
        {
            GUIForm.Paint += OnPaint;

            _field.Subscribe();
            _field.SubscribeToCheckPosition(_p1Tank);
            _field.SubscribeToCheckPosition(_p2Tank);
            _field.ObstacleDestroyed += OnObstacleDestroyed;

            _p1Tank.Subscribe();
            _p1Tank.SubscribeToCheckPosition(_p2Tank);
            _p1Tank.Shoot += OnShoot;

            _p2Tank.Subscribe();
            _p2Tank.SubscribeToCheckPosition(_p1Tank);
            _p2Tank.Shoot += OnShoot;
        }

        public override void Unsubscribe()
        {
            GUIForm.Paint -= OnPaint;

            _field.Unsubscribe();
            _field.UnsubscribeFromCheckPosition(_p1Tank);
            _field.UnsubscribeFromCheckPosition(_p2Tank);
            _field.ObstacleDestroyed -= OnObstacleDestroyed;

            _p1Tank.Unsubscribe();
            _p1Tank.UnsubscribeFromCheckPosition(_p2Tank);
            _p1Tank.Shoot -= OnShoot;

            _p2Tank.Unsubscribe();
            _p2Tank.UnsubscribeFromCheckPosition(_p1Tank);
            _p2Tank.Shoot -= OnShoot;
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
            g.DrawString(_p1Tank.Lives.ToString(), MyFont.GetFont(22), new SolidBrush(Color.Black), new PointF(940.0f, 626.0f));
            g.DrawString(name, MyFont.GetFont(22), new SolidBrush(Color.Black), new PointF(910.0f, 585.0f));

            g.DrawImageUnscaled(Properties.Resources.Player_Icon, new Point(910, 732));
            g.DrawString(_p2Tank.Lives.ToString(), MyFont.GetFont(22), new SolidBrush(Color.Black), new PointF(940.0f, 732.0f));
            name = "";
            name += Properties.Settings.Default.P2Name[0];
            name += Properties.Settings.Default.P2Name[1];
            g.DrawString(name, MyFont.GetFont(22), new SolidBrush(Color.Black), new PointF(910.0f, 690.0f));
        }

        private void OnShoot(object sender, ShellEventArgs e)
        {
            Shell s = e.Shell;
            _field.SubscribeToCheckPosition(s);
            s.Subscribe();
            s.SubscribeToCheckPosition(_p1Tank);
            s.SubscribeToCheckPosition(_p2Tank);
            s.Destroyed += OnShellDestroyed;
            SubscribeShellToShells(s);
            _shells.Add(s);
        }

        private void SubscribeShellToShells(Shell s)
        {
            foreach(Shell shell in _shells)
                s.SubscribeToCheckPosition(shell);
        }

        private void OnShellDestroyed(object sender, EventArgs e)
        {
            _shells.Remove((Shell)sender);
        }

        private void OnObstacleDestroyed(object sender, EventArgs e)
        {
            Object o = sender as Object;
            o.UnsubscribeFromCheckPosition(_p1Tank);
            o.UnsubscribeFromCheckPosition(_p2Tank);
        }
    }
}