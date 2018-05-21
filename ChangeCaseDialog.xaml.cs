﻿using System.Windows;
using System.Windows.Controls;

namespace VietOCR
{
    /// <summary>
    /// Interaction logic for ChangeCaseDialog.xaml
    /// </summary>
    public partial class ChangeCaseDialog : Window
    {
        private string selectedCase;

        public event RoutedEventHandler CloseDlg;
        public event RoutedEventHandler ChangeCase;

        public string SelectedCase
        {
            set { selectedCase = value; }
            get { return selectedCase; }
        }

        public ChangeCaseDialog()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            foreach (RadioButton rb in this.radioButtonPanel.Children)
            {
                if (rb.Tag.ToString() == selectedCase)
                {
                    // Select Case last saved
                    rb.IsChecked = true;
                    break;
                }
            }
        }

        private void buttonChange_Click(object sender, RoutedEventArgs e)
        {
            foreach (RadioButton rb in this.radioButtonPanel.Children)
            {
                if (rb.IsChecked.Value)
                {
                    selectedCase = rb.Tag.ToString();
                    break;
                }
            }

            ChangeCase?.Invoke(this, e);
        }

        private void buttonClose_Click(object sender, RoutedEventArgs e)
        {
            CloseDlg?.Invoke(this, e);

            this.Close();
        }
    }
}
