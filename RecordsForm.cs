using System;
using System.IO;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;

namespace BattleCity
{
    public class RecordsForm : AbstractForm
    {
        private List<RecordNode> _records;

        public RecordsForm(GUIForm guiForm, GameManager gameManager) : base(guiForm, gameManager)
        {
            GUIObjs.AddLast(new SelectButton(GUIForm, new Rectangle(360, 840, 0, 0), "MAIN MENU", true));
            CurrentGUIObj = GUIObjs.Last;
            _records = new List<RecordNode>();
            LoadRecordsFromFile();
        }

        ~RecordsForm()
        {
            WriteRecordsToFile();
        }

        public override void Subscribe()
        {
            base.Subscribe();
            GUIForm.Paint += OnPaint;

            GUIObjs.Last.Value.Subscribe();
            GUIObjs.Last.Value.Clicked += OnBtnMainMenuClicked;
        }

        public override void Unsubscribe()
        {
            base.Unsubscribe();
            GUIForm.Paint -= OnPaint;

            GUIObjs.Last.Value.Unsubscribe();
            GUIObjs.Last.Value.Clicked -= OnBtnMainMenuClicked;
        }

        private void OnPaint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.FillRectangle(new SolidBrush(Color.Black), new Rectangle(new Point(), GUIForm.Size));
            g.DrawImageUnscaled(Properties.Resources.Records, 80, 30);
            g.DrawString("POS", MyFont.GetFont(19), new SolidBrush(Color.Gray), 170, 190);
            g.DrawString("NAME", MyFont.GetFont(19), new SolidBrush(Color.Gray), 350, 190);
            g.DrawString("POINTS", MyFont.GetFont(19), new SolidBrush(Color.Gray), 650, 190);

            int y = 250;
            int i = 1;
            foreach(RecordNode record in _records)
            {
                g.DrawString(i.ToString() + '.', MyFont.GetFont(19), new SolidBrush(Color.White), 170, y);
                g.DrawString(record.name, MyFont.GetFont(19), new SolidBrush(Color.White), 350, y);
                g.DrawString(record.points.ToString(), MyFont.GetFont(19), new SolidBrush(Color.White), 650, y);
                y += 50;
                i++;
            }
        }

        private void OnBtnMainMenuClicked(object sender, EventArgs e)
        {
            GameManager.SetMainMenuForm();
        }

        public void AddRecord(string name, int points)
        {
            _records.Add(new RecordNode(name, points));
            _records.Sort(new RecordsComparer());

            int records = _records.Count;
            if(records > 10)
                _records.RemoveAt(records - 1);
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
                    AddRecord(name, points);
                }
            }
        }

        private void WriteRecordsToFile()
        {
            using(StreamWriter sw = new StreamWriter("Records.txt"))
            {
                foreach(RecordNode record in _records)
                {
                    sw.Write(record.name);
                    sw.Write(" ");
                    sw.Write(record.points);
                    sw.WriteLine("");
                }
            }
        }

        public int GetHighestRecord()
        {
            return _records[0].points;
        }

        private struct RecordNode
        {
            public string name;
            public int points;

            public RecordNode(string name, int points)
            {
                this.name = name;
                this.points = points;
            }
        }

        private class RecordsComparer : IComparer<RecordNode>
        {
            public int Compare(RecordNode x, RecordNode y)
            {
                int compareRecord = 0;
                if(x.points < y.points)
                    compareRecord = 1;
                else if(x.points > y.points)
                    compareRecord = -1;
                return compareRecord;
            }
        }
    }
}