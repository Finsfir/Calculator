using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;


namespace Calculator
{
    public sealed partial class MainPage : Page
    {
        public enum Flags
        {
            Solved,
            Inputting
        }
        Flags flag = Flags.Inputting;
        string str = "";
        Expression exp = new Expression();

        public MainPage()
        {
            this.InitializeComponent();
            foreach (UIElement c in LayoutRoot.Children)
            {
                if (c is Button)
                {
                    ((Button)c).Click += Button_Click;
                }
            }
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // Получаем текст кнопки
            string s = (string)((Button)e.OriginalSource).Content;
            // Добавляем его в текстовое поле
            if(s == "=")
            {
                try
                {
                    textBlock.Text += "=" + exp.Result(str).ToString();
                    calcHistory.Items.Clear();
                    foreach(string a in exp.exprQueue)
                        calcHistory.Items.Add(a);
                }
                catch (Exception ex)
                {
                    Message("Exception", ex.Message);
                }
                flag = Flags.Solved;
            } else if(s == "CLEAR")
            {
                textBlock.Text = "";
                str = "";
            }else if (s == "CE")
            {
                if(str != null && str.Length > 0)
                    str = str.Remove(str.Length - 1, 1);
                textBlock.Text = str;
                flag = Flags.Inputting;
            }
            else
            {
                if (Flags.Solved == flag)
                {
                    str = "";
                    textBlock.Text = "";
                    flag = Flags.Inputting;
                }
                textBlock.Text += s;
                str += s;
            }
        }

        private async void Message(string title, string content)
        {
            ContentDialog deleteFileDialog = new ContentDialog()
            {
                Title = title,
                Content = content,
                PrimaryButtonText = "ОК"
            };

            ContentDialogResult result = await deleteFileDialog.ShowAsync();

            if (result == ContentDialogResult.Primary)
            {

            }
        }
    }
}
