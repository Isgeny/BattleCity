using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Collections.Generic;
using System.Windows.Forms;

namespace BattleCity
{
    public class GameForm : AbstractForm
    {
        private Field field;
        private PlayerTanksManager pTanksManager;
        private CompTanksManager cTanksManager;
        private Bonus bonus;

        public GameForm(GUIForm guiForm, GameManager gameManager, int players) : base(guiForm, gameManager)
        {
            field = new Field(guiForm, new RectangleF(64.0f, 64.0f, 832.0f, 832.0f));
            field.LoadStage(39);
        }

        public Field Field
        {
            get => default(Field);
        }

        private void OnPaint(object sender, PaintEventArgs e)
        {
            Bitmap bmap = GUIForm.Bitmap;
            Graphics g = Graphics.FromImage(bmap);
            g.FillRectangle(new SolidBrush(Color.FromArgb(102, 102, 102)), new Rectangle(0, 0, 1024, 960));
            g.FillRectangle(new SolidBrush(Color.Black), new Rectangle(64, 64, 832, 832));
        }

        public override void Subscribe()
        {
            GUIForm.Paint += OnPaint;
            field.SubscribeToForm();

            GUIForm.Invalidate();
        }

        public override void Unsubscribe()
        {
            throw new NotImplementedException();
        }
    }
}