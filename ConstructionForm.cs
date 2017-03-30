﻿using System;
using System.IO;
using System.Drawing;
using System.Resources;
using System.Windows.Forms;
using System.Collections.Generic;

namespace BattleCity
{
    public class ConstructionForm : AbstractForm
    {
        private const int BLOCK_SZ = 64;
        private const int BLOCKS_COUNT = 13;
        private const int OBST_SZ = 32;
        private const int OBST_COUNT = 26;
        private int[,] blocks;
        private int activeBlock;
        private int iPos;
        private int jPos;
   
        public ConstructionForm(GUIForm guiForm, GameManager gameManager) : base(guiForm, gameManager)
        {
            blocks = new int[BLOCKS_COUNT, BLOCKS_COUNT];
            activeBlock = 1;
            iPos = 0;
            jPos = 0;
        }

        public override void Subscribe()
        {
            GUIForm.Paint += OnPaint;
            GUIForm.MouseClick += OnMouseClick;
            GUIForm.MouseMove += OnMouseMove;
            GUIForm.MouseWheel += OnMouseWheel;
            GUIForm.KeyDown += OnKeyDown;

            CleanField();
            GUIForm.Invalidate();
        }

        public override void Unsubscribe()
        {
            GUIForm.Paint -= OnPaint;
            GUIForm.MouseClick -= OnMouseClick;
            GUIForm.MouseMove -= OnMouseMove;
            GUIForm.MouseWheel -= OnMouseWheel;
            GUIForm.KeyDown -= OnKeyDown;
        }

        private void OnPaint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.FillRectangle(new SolidBrush(Color.FromArgb(102, 102, 102)), new Rectangle(0, 0, 1024, 960));
            g.FillRectangle(new SolidBrush(Color.Black), new Rectangle(64, 64, 832, 832));
            g.DrawString("ESC-MAIN MENU", MyFont.GetFont(14), new SolidBrush(Color.Black), new PointF(5.0f, 10.0f));
            g.DrawString("LMB-PLACE    WHEEL⬆⬇-CHOOSE    RMB-REMOVE", MyFont.GetFont(14), new SolidBrush(Color.Black), new PointF(5.0f, 930.0f));

            for(int i = 0; i < BLOCKS_COUNT; i++)
                for(int j = 0; j < BLOCKS_COUNT; j++)
                    if(blocks[i, j] != 0)
                        g.DrawImageUnscaled(GetBlockImage(blocks[i, j]), new Point(j * BLOCK_SZ + BLOCK_SZ, i * BLOCK_SZ + BLOCK_SZ));

            g.DrawImageUnscaled(GetBlockImage(activeBlock), new Point(jPos * BLOCK_SZ + BLOCK_SZ, iPos * BLOCK_SZ + BLOCK_SZ));
        }

        private Bitmap GetBlockImage(int value)
        {
            ResourceManager rm = Properties.Resources.ResourceManager;
            string filename = "Block_" + value;
            return (Bitmap)rm.GetObject(filename);
        }

        private void OnMouseClick(object sender, MouseEventArgs e)
        {
            if(e.Button == MouseButtons.Left)
                PlaceBlock();
            else if(e.Button == MouseButtons.Right)
                RemoveBlock();
        }

        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            if(InsideField(e.Location))
            {
                ChangePosition(e.Location);
                if(e.Button == MouseButtons.Left)
                    PlaceBlock();
                else if(e.Button == MouseButtons.Right)
                    RemoveBlock();
                GUIForm.Invalidate();
            }
        }

        private void ChangePosition(Point p)
        {
            Point newP = XYtoIJ(RoundMousePos(p));
            iPos = newP.Y;
            jPos = newP.X;
        }

        private void PlaceBlock()
        {
            blocks[iPos, jPos] = activeBlock;
        }

        private void RemoveBlock()
        {
            blocks[iPos, jPos] = 0;
        }

        private void CleanField()
        {
            for(int i = 0; i < BLOCKS_COUNT; i++)
                for(int j = 0; j < BLOCKS_COUNT; j++)
                    blocks[i, j] = 0;
            blocks[12, 6] = 14;
            blocks[12, 5] = 5;
            blocks[11, 5] = 15;
            blocks[11, 6] = 4;
            blocks[11, 7] = 16;
            blocks[12, 7] = 3;
        }

        private static bool InsideField(Point p)
        {
            return new Rectangle(64, 64, 832, 832).Contains(p);
        }

        private void OnMouseWheel(object sender, MouseEventArgs e)
        {
            if(e.Delta < 1)
                activeBlock = (activeBlock < 13) ? ++activeBlock : 1;
            else
                activeBlock = (activeBlock != 1) ? --activeBlock : 13;
            GUIForm.Invalidate();
        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.S)
                SaveToFile();
            else if(e.KeyCode == Keys.Escape)
            {
                ConvertBlocksToList();
                Unsubscribe();
                GameManager.ActiveForm = GameManager.MainMenu;
            }
        }

        private List<Object> ConvertBlocksToList()
        {
            List<Object> list = new List<Object>();
            string[,] s = ConvertBlocksToStrings();
            for(int i = 0; i < OBST_COUNT; i++)
                for(int j = 0; j < OBST_COUNT; j++)
                {
                    RectangleF rect = new RectangleF(j * OBST_SZ, i * OBST_SZ, 32.0f, 32.0f);
                    switch(s[i,j])
                    {
                        case "b":
                            list.Add(new Brick(GUIForm, rect));
                            break;
                        case "c":
                            list.Add(new Concrete(GUIForm, rect));
                            break;
                        case "w":
                            list.Add(new Water(GUIForm, rect));
                            break;
                        case "s":
                            list.Add(new Bush(GUIForm, rect));
                            break;
                        case "i":
                            list.Add(new Ice(GUIForm, rect));
                            break;
                        default:
                            break;
                    }
                }
            return list;
        }

        private string[,] ConvertBlocksToStrings()
        {
            Dictionary<int, string> dict = new Dictionary<int, string>();
            dict[0] = "eeee";
            dict[1] = "bbbb";
            dict[2] = "bbee";
            dict[3] = "bebe";
            dict[4] = "eebb";
            dict[5] = "ebeb";
            dict[6] = "cccc";
            dict[7] = "ccee";
            dict[8] = "cece";
            dict[9] = "eecc";
            dict[10] = "ecec";
            dict[11] = "wwww";
            dict[12] = "ssss";
            dict[13] = "iiii";
            dict[14] = "eeee";
            dict[15] = "eeeb";
            dict[16] = "eebe";

            blocks[12, 6] = 14;
            string[,] obs = new string[OBST_COUNT, OBST_COUNT];
            for(int i = 0; i < BLOCKS_COUNT; i++)
                for(int j = 0; j < BLOCKS_COUNT; j++)
                {
                    obs[i * 2, j * 2]           = dict[ blocks[i, j] ][0].ToString();
                    obs[i * 2, j * 2 + 1]       = dict[ blocks[i, j] ][1].ToString();
                    obs[i * 2 + 1, j * 2]       = dict[ blocks[i, j] ][2].ToString();
                    obs[i * 2 + 1, j * 2 + 1]   = dict[ blocks[i, j] ][3].ToString();
                }
            return obs;
        }

        private void SaveToFile()
        {
            string[,] obstacles = ConvertBlocksToStrings();
            using(StreamWriter sw = new StreamWriter("Stage_" + DateTime.Now.ToFileTimeUtc().ToString() + ".txt"))
                foreach(string s in obstacles)
                    sw.Write(s);
        }

        public static Point RoundMousePos(Point p)
        { 
            return new Point(p.X / BLOCK_SZ * BLOCK_SZ, p.Y / BLOCK_SZ * BLOCK_SZ);
        }

        private static Point XYtoIJ(Point p)
        {
            return new Point((p.X - BLOCK_SZ) / BLOCK_SZ, (p.Y - BLOCK_SZ) / BLOCK_SZ);
        }
    }
}