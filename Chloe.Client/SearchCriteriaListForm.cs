using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Flights.Client.Properties;

namespace Flights.Client
{
    public partial class SearchCriteriaListForm : Form
    {
        public SearchCriteriaListForm()
        {
            InitializeComponent();

            //dataGridViewSearchCriterias.AutoGenerateColumns = true;
        }

        private void SearchCriteriaListForm_Load(object sender, EventArgs e)
        {
            this.searchCriteriasTableAdapter.Fill(this.flightsDataSet.SearchCriterias);
            this.flightsTableAdapter.Fill(this.flightsDataSet.Flights);
            this.searchCriteria_ViewTableAdapter.Fill(this.flightsDataSet.SearchCriteria_View);
        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            if (dataGridViewSearchCriterias.SelectedRows.Count == 0)
            {
                MessageBox.Show(Resources.SearchCriteriaListForm_YouHaveToSelectAtLeastOneRow);
                return;
            }

            if (
                MessageBox.Show(Resources.SearchCriteriaListForm_AreYouSureToDeleteSearchCriteriaAndFlights, Resources.SearchCriteriaListForm_AcceptChanges,
                    MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                foreach (var selectedRow in dataGridViewSearchCriterias.SelectedRows.Cast<DataGridViewRow>())
                {
                    var row = selectedRow.DataBoundItem as DataRowView;
                    var searchCriteriaRow = row.Row as FlightsDataSet.SearchCriteria_ViewRow;
                    int id = (int)searchCriteriaRow["Id"];
                    searchCriteriaRow.Delete();

                    var searchCriteriaRowsToDelete = flightsDataSet.Flights
                        .Where(x => x.SearchCriteria_Id == id);
                    foreach (var rowToDelete in searchCriteriaRowsToDelete)
                    {
                        rowToDelete.Delete();
                    }
                    flightsDataSet.SearchCriterias.FindById(id).Delete();

                    flightsTableAdapter.Update(flightsDataSet.Flights);
                    searchCriteriasTableAdapter.Update(flightsDataSet.SearchCriterias);
                }

                MessageBox.Show(Resources.SearchCriteriaListForm_DeleteRowsSuccessfull);
            }
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            SearchCriteriaAddForm form = new SearchCriteriaAddForm();
            form.ShowDialog();
        }
    }
}
