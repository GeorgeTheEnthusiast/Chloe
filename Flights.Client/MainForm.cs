﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Flights.Client
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void buttonSearchCriterias_Click(object sender, EventArgs e)
        {
            SearchCriteriaListForm form = new SearchCriteriaListForm();
            form.ShowDialog();
        }
    }
}
