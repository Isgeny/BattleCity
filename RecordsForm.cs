using System;
using System.IO;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;

namespace BattleCity
{
    public class RecordsForm : MenuForm
    {
        private List<RecordNode> _records;
        private GUIObject _btnMainMenu;

        public RecordsForm(GUIForm guiForm, FormsManager formsManager) : base(guiForm, formsManager)
        {
            InitializeComponents();
        }

        ~RecordsForm()
        {
            WriteRecordsToFile();
        }

        public override void Subscribe()
        {
            base.Subscribe();
            GUIForm.Paint += OnPaint;

            _btnMainMenu.Subscribe();
            _btnMainMenu.Clicked += OnBtnMainMenuClicked;
        }

        public override void Unsubscribe()
        {
            base.Unsubscribe();
            GUIForm.Paint -= OnPaint;

            _btnMainMenu.Unsubscribe();
            _btnMainMenu.Clicked -= OnBtnMainMenuClicked;
        }

        private void OnPaint(object sender, PaintEventArgs e)
        {
            var g = e.Graphics;
            g.FillRectangle(Brushes.Black, new Rectangle(new Point(), GUIForm.Size));
            g.DrawImageUnscaled(Properties.Resources.Records, 80, 30);
            g.DrawString("POS",     MyFont.GetFont(19), Brushes.Gray, 170, 190);
            g.DrawString("NAME",    MyFont.GetFont(19), Brushes.Gray, 350, 190);
            g.DrawString("POINTS",  MyFont.GetFont(19), Brushes.Gray, 650, 190);

            int y = 250;
            int i = 1;
            foreach(RecordNode record in _records)
            {
                g.DrawString(i.ToString() + '.',       MyFont.GetFont(19), Brushes.White, 170, y);
                g.DrawString(record.name,              MyFont.GetFont(19), Brushes.White, 350, y);
                g.DrawString(record.points.ToString(), MyFont.GetFont(19), Brushes.White, 650, y);
                y += 50;
                i++;
            }
        }

        private void OnBtnMainMenuClicked(object sender, EventArgs e)
        {
            FormsManager.SetMainMenuForm();
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
            using(var sr = new StreamReader("Records.txt", Encoding.Default))
                while(!sr.EndOfStream)
                {
                    string currentString = sr.ReadLine();
                    string[] record = currentString.Split(new char[] { ' ' });
                    string name = record[0];
                    int points = Convert.ToInt32(record[1]);
                    AddRecord(name, points);
                }
        }

        private void WriteRecordsToFile()
        {
            using(var sw = new StreamWriter("Records.txt"))
                foreach(RecordNode record in _records)
                {
                    sw.Write(record.name);
                    sw.Write(" ");
                    sw.Write(record.points);
                    sw.WriteLine("");
                }
        }

        public int GetHighestRecord()
        {
            return _records[0].points;
        }

        private void InitializeComponents()
        {
            _btnMainMenu = new SelectButton(GUIForm, new Point(360, 840), "MAIN MENU", true);
            Components.AddLast(_btnMainMenu);
            CurrentComponent = Components.Last;
            _records = new List<RecordNode>();
            LoadRecordsFromFile();
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