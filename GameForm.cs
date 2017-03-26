﻿using System;
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

        /*private PlayerTanksManager pTanksManager;
        private CompTanksManager cTanksManager;
        private Bonus bonus;*/

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

        private void OnPaint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.FillRectangle(new SolidBrush(Color.FromArgb(102, 102, 102)), new Rectangle(0, 0, 1024, 960));
            g.FillRectangle(new SolidBrush(Color.Black), new Rectangle(64, 64, 832, 832));
        }

        public override void Subscribe()
        {
            GUIForm.Paint += OnPaint;

            field.SubscribeToPaint();
            field.SubscribeToCheckPosition(p1Tank);
            field.SubscribeToCheckPosition(p2Tank);

            p1Tank.SubscribeToPaint();

            p2Tank.SubscribeToPaint();

            field.SubscribeBushesToPaint();

            p1Tank.SubscribeToCheckPosition(p2Tank);
            p2Tank.SubscribeToCheckPosition(p1Tank);

            GUIForm.Invalidate();
        }

        public override void Unsubscribe()
        {
            GUIForm.Paint -= OnPaint;

            field.UnsubscribeFromPaint();
            field.UnsubscribeFromCheckPosition(p1Tank);
            field.UnsubscribeFromCheckPosition(p2Tank);

            p1Tank.UnsubscribeFromPaint();

            p2Tank.UnsubscribeFromPaint();

            field.UnsubscribeBushesFromPaint();

            p1Tank.UnsubscribeFromCheckPosition(p2Tank);
            p2Tank.UnsubscribeFromCheckPosition(p1Tank);
        }
    }
}