using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Flights.Client.Domain;
using Flights.Client.Properties;

namespace Flights.Client
{
    public partial class GroupsForm : Form
    {
        public GroupsForm()
        {
            InitializeComponent();
        }

        private void GroupsForm_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'flightsDataSet.NotificationReceivers' table. You can move, or remove it, as needed.
            this.notificationReceiversTableAdapter.Fill(this.flightsDataSet.NotificationReceivers);
            // TODO: This line of code loads data into the 'flightsDataSet.ReceiverGroups' table. You can move, or remove it, as needed.
            this.receiverGroupsTableAdapter.Fill(this.flightsDataSet.ReceiverGroups);

            RefreshViews();
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {


            this.Close();
        }

        private void RefreshViews()
        {
            using (var entities = new FlightsEntities1())
            {
                var joinedGroupsEntities = (from j in entities.NotificationReceiversGroups
                    join g in entities.ReceiverGroups on j.ReceiverGroups_Id equals g.Id
                    join r in entities.NotificationReceivers on j.NotificationReceivers_Id equals r.Id
                    select new { Id = j.Id, GroupName = g.Name, ReceiverName = r.Email}
                    ).ToList();

                dataGridViewJoined.DataSource = joinedGroupsEntities;
            }
        }

        private void buttonAddEmailToGroup_Click(object sender, EventArgs e)
        {
            if (dataGridViewGroups.SelectedRows.Count == 0)
            {
                MessageBox.Show(Resources.GroupsForm_OneGroupMustBeSelected);
                return;
            }

            if (dataGridViewReceivers.SelectedRows.Count == 0)
            {
                MessageBox.Show(Resources.GroupsForm_AtLeastOneEmailIsRequired);
                return;
            }

            var dataRowView = (dataGridViewGroups.SelectedRows[0].DataBoundItem as DataRowView);
            var groupRow = dataRowView.Row as FlightsDataSet.ReceiverGroupsRow;

            dataRowView = dataGridViewReceivers.SelectedRows[0].DataBoundItem as DataRowView;
            var receiverRow = dataRowView.Row as FlightsDataSet.NotificationReceiversRow;
            
            using (var flightEntities = new FlightsEntities1())
            {
                flightEntities.NotificationReceiversGroups.Add(new NotificationReceiversGroup()
                {
                    ReceiverGroups_Id = groupRow.Id,
                    NotificationReceivers_Id = receiverRow.Id
                });
                flightEntities.SaveChanges();
            }
        }
    }
}
