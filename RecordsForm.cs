using System;
using System.IO;
using System.Text;
using System.Linq;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;

namespace BattleCity
{
    public class RecordsForm : AbstractForm
    {
        private SortedDictionary<int, string> records;
        private GUIObject btnMainMenu;

        public RecordsForm(GUIForm guiForm, GameManager gameManager) : base(guiForm, gameManager)
        {
            btnMainMenu = new SelectButton(GUIForm, new RectangleF(270.0f, 790.0f, 0.0f, 0.0f), "MAIN MENU", true);

            records = new SortedDictionary<int, string>(new ReverseComparer<int>(Comparer<int>.Default));
            LoadRecordsFromFile();
        }

        ~RecordsForm()
        {
            WriteRecordsToFile();
        }

        private void OnPaint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.FillRectangle(new SolidBrush(Color.Black), new Rectangle(new Point(), GUIForm.Size));
            g.DrawImageUnscaled(Properties.Resources.Records, new Point(80, 30));
            g.DrawString("POS", MyFont.GetFont(24), new SolidBrush(Color.Gray), new PointF(170.0f, 190.0f));
            g.DrawString("NAME", MyFont.GetFont(24), new SolidBrush(Color.Gray), new PointF(350.0f, 190.0f));
            g.DrawString("POINTS", MyFont.GetFont(24), new SolidBrush(Color.Gray), new PointF(650.0f, 190.0f));

            float y = 250.0f;
            int i = 1;
            foreach(KeyValuePair<int, string> rec in records)
            {
                g.DrawString(i.ToString() + '.', MyFont.GetFont(20), new SolidBrush(Color.White), new PointF(170.0f, y));
                g.DrawString(rec.Value, MyFont.GetFont(20), new SolidBrush(Color.White), new PointF(350.0f, y));
                g.DrawString(rec.Key.ToString(), MyFont.GetFont(20), new SolidBrush(Color.White), new PointF(650.0f, y));
                y += 50;
                i++;
            }
        }

        public override void Subscribe()
        {
            GUIForm.Paint += OnPaint;

            btnMainMenu.SubscribeToPaint();
            btnMainMenu.SubscribeToKeyDown();
            btnMainMenu.Clicked += OnBtnMainMenuClicked;

            GUIForm.Invalidate();
        }

        public override void Unsubscribe()
        {
            GUIForm.Paint -= OnPaint;

            btnMainMenu.UnsubscribeFromPaint();
            btnMainMenu.UnsubscribeFromKeyDown();
            btnMainMenu.Clicked -= OnBtnMainMenuClicked;
        }

        private void OnBtnMainMenuClicked(object sender, EventArgs e)
        {
            Unsubscribe();
            GameManager.ActiveForm = GameManager.MainMenu;
        }

        public void AddRecord(string name, int points)
        {
            records.Add(points, name);
            if(records.Count > 10)
            {
                records.Remove(records.Keys.Last());
            }
        }

        private void LoadRecordsFromFile()
        {
            using(StreamReader sr = new StreamReader("Records.txt", Encoding.Default))
            {
                while(!sr.EndOfStream)
                {
                    string currentString = sr.ReadLine();
                    string[] record = currentString.Split(new char[] { ' ' });
                    string name = record[0];
                    int points = Convert.ToInt32(record[1]);
                    records.Add(points, name);
                }
            }
        }

        private void WriteRecordsToFile()
        {
            using(StreamWriter sw = new StreamWriter("Records.txt"))
            {
                foreach(KeyValuePair<int, string> rec in records)
                {
                    sw.Write(rec.Value);
                    sw.Write(" ");
                    sw.Write(rec.Key);
                    sw.WriteLine("");
                }
            }
        }
    }
}