﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SchoolAdmin.UI.Config
{
  /// <summary>
  /// Interaction logic for ConfigWindow.xaml
  /// </summary>
  public partial class ConfigWindow : Window
  {
    public ConfigWindow()
    {
      InitializeComponent();

      Wisa.Load();
      Smartschool.Load();
    }

    protected override void OnClosing(CancelEventArgs e)
    {
      Wisa.Save();
      Smartschool.Save();
      Properties.Settings.Default.Save();
      base.OnClosing(e);
    }
  }
}
