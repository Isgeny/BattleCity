﻿using System;
using System.Windows.Forms;

namespace BattleCity
{
    static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            GUIForm guiForm = new GUIForm();
            GameManager gameManager = new GameManager(guiForm);

            Application.Run(guiForm);
        }
    }
}
