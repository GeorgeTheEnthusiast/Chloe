using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Flights.Client.Converters;
using Flights.Client.Domain;
using Flights.Client.Properties;

namespace Flights.Client
{
    public partial class SearchCriteriaForm : Form
    {
        public SearchCriteriaForm()
        {
            InitializeComponent();
        }

        private void buttonChangeGroups_Click(object sender, EventArgs e)
        {
            GroupsForm groupsForm = new GroupsForm();
            groupsForm.ShowDialog();

        }

        private void SearchCriteriaForm_Load(object sender, EventArgs e)
        {
            this.searchCriteriasTableAdapter.Fill(this.flightsDataSet.SearchCriterias);
            this.netTableAdapter.Fill(this.flightsDataSet.Net);
            this.flightWebsitesTableAdapter.Fill(this.flightsDataSet.FlightWebsites);
            this.citiesTableAdapter.Fill(this.flightsDataSet.Cities);
            this.receiverGroupsTableAdapter.Fill(this.flightsDataSet.ReceiverGroups);
            this.notificationReceiversTableAdapter.Fill(this.flightsDataSet.NotificationReceivers);

            checkedListBoxCitiesFrom.DataSource = flightsDataSet.Cities.Rows
                .Cast<FlightsDataSet.CitiesRow>()
                .OrderBy(x => x.Name)
                .ToList();
            checkedListBoxCitiesFrom.DisplayMember = "Name";

            checkedListBoxCitiesTo.DataSource = flightsDataSet.Cities.Rows
                .Cast<FlightsDataSet.CitiesRow>()
                .OrderBy(x => x.Name)
                .ToList();
            checkedListBoxCitiesTo.DisplayMember = "Name";

            checkedListBoxFlightWebsites.DataSource = flightsDataSet.FlightWebsites.Rows
                .Cast<FlightsDataSet.FlightWebsitesRow>()
                .OrderBy(x => x.Name)
                .ToList();
            checkedListBoxFlightWebsites.DisplayMember = "Name";

            dateTimePickerDepartureDate.MinDate = DateTime.Now.AddDays(1);
        }

        private void buttonAddSearchCriteria_Click(object sender, EventArgs e)
        {
            if (checkedListBoxCitiesFrom.CheckedItems.Count == 0)
            {
                MessageBox.Show(Resources.SearchCriteriaForm_AtLeastOneCityFromMustBeChecked);
                return;
            }

            if (checkedListBoxCitiesTo.CheckedItems.Count == 0)
            {
                MessageBox.Show(Resources.SearchCriteriaForm_AtLeastOneCityToMustBeChecked);
                return;
            }

            if (checkedListBoxFlightWebsites.CheckedItems.Count == 0)
            {
                MessageBox.Show(Resources.SearchCriteriaForm_buttonAddSearchCriteria_AtLeastOneFlightWebsiteMustBeChecked);
                return;
            }

            if (comboBoxReceiverGroup.SelectedItem == null)
            {
                MessageBox.Show(Resources.SearchCriteriaForm_ReceiverGroupMustBeSelected);
                return;
            }

            var flightWebsites =
                checkedListBoxFlightWebsites.CheckedItems
                .Cast<FlightsDataSet.FlightWebsitesRow>()
                .ToList();
            var citiesFrom =
                checkedListBoxCitiesFrom.CheckedItems
                .Cast<FlightsDataSet.CitiesRow>()
                .ToList();
            var citiesTo =
                checkedListBoxCitiesTo.CheckedItems
                .Cast<FlightsDataSet.CitiesRow>()
                .ToList();
            var receiverGroup = (comboBoxReceiverGroup.SelectedItem as DataRowView).Row as FlightsDataSet.ReceiverGroupsRow;

            foreach (var flightWebsite in flightWebsites)
            {
                var carrier = FlightWebsiteConverter.Convert(flightWebsite);

                foreach (var cityA in citiesFrom)
                {
                    var flightChangeA = from n in flightsDataSet.Net
                                        where n.Carrier_Id == carrier.Id
                                              && n.CityFrom_Id == cityA.Id
                                        select n;

                    if (flightChangeA.Any() == false)
                        continue;

                    foreach (var cityB in citiesTo)
                    {
                        var flightChangeB = from n in flightsDataSet.Net
                                            where n.Carrier_Id == carrier.Id
                                                  && n.CityTo_Id == cityB.Id
                                            select n;

                        if (flightChangeB.Any() == false)
                            continue;

                        var flightChangeCommon1 = flightChangeA
                            .Where(x => flightChangeB.Any(y => y.CityFrom_Id == x.CityTo_Id));
                        var flightChangeCommon2 = flightChangeB
                            .Where(x => flightChangeA.Any(y => y.CityTo_Id == x.CityFrom_Id));
                        var joinedFlightChanges = (from f1 in flightChangeCommon1
                            join f2 in flightChangeCommon2 on f1.CityTo_Id equals f2.CityFrom_Id
                            select new
                            {
                                CityA = f1.CitiesRowByFK_Net_CitiesFrom.Name,
                                CityB = f1.CitiesRowByFK_Net_CitiesTo.Name,
                                CityC = f2.CitiesRowByFK_Net_CitiesTo.Name
                            })
                            .ToList();
                    }


                }


//                var flightChangeA = from n in flightsDataSet.Net
//                    .Where(x => x.Carrier_Id == carrier.Id
//                                && citiesFrom.Any(y => y.Id == x.CityFrom_Id))
//                    select n;
//                var flightChangeB = from n in flightsDataSet.Net
//                    .Where(x => x.Carrier_Id == carrier.Id
//                                && citiesTo.Any(y => y.Id == x.CityTo_Id))
//                                           select n;
//
//                var flightChangeCommon = flightChangeA
//                    .Where(x => flightChangeB.Any(y => y.CityFrom_Id == x.CityTo_Id))
//                    .ToList();

               

//                        var availableNet = from n in flightsDataSet.Net
//                    .Where(x => x.Carrier_Id == carrier.Id
//                                && citiesFrom.Any(y => y.Id == x.CityFrom_Id)
//                                && citiesTo.Any(z => z.Id == x.CityTo_Id))
//                                join c1 in citiesFrom on n.CityFrom_Id equals c1.Id
//                                join c2 in citiesTo on n.CityTo_Id equals c2.Id
//                    select new {
//                        CityFrom_Id = n.CityFrom_Id,
//                        CityTo_Id = n.CityTo_Id,
//                        CityFrom = c1.Name,
//                        CityTo = c2.Name
//                        };
//
//                foreach (var searchCriteria in availableNet)
//                {
//                    bool existed = flightsDataSet.SearchCriterias
//                        .Any(x => x.FlightWebsite_Id == flightWebsite.Id
//                                && x.ReceiverGroups_Id == receiverGroup.Id
//                                && DateTime.Compare(x.DepartureDate.Date, dateTimePickerDepartureDate.Value.Date) == 0
//                                && x.CityFrom_Id == searchCriteria.CityFrom_Id
//                                && x.CityTo_Id == searchCriteria.CityTo_Id);
//
//                    if (existed == false)
//                    {
//                        searchCriteriasTableAdapter.Insert(
//                            searchCriteria.CityFrom_Id,
//                            searchCriteria.CityTo_Id,
//                            dateTimePickerDepartureDate.Value,
//                            flightWebsite.Id,
//                            null,
//                            receiverGroup.Id);
//
//                        MessageBox.Show(string.Format("Dodano połączenie {0} - {1}", searchCriteria.CityFrom, searchCriteria.CityTo));
//                    }
//                }
            }

            MessageBox.Show(Resources.SearchCriteriaForm_AddingNewSearchCriteriaCompleted);
        }

        private void buttonSetAllToDefaults_Click(object sender, EventArgs e)
        {
            UncheckAllItems(checkedListBoxCitiesFrom);
            UncheckAllItems(checkedListBoxCitiesTo);
            UncheckAllItems(checkedListBoxFlightWebsites);
            dateTimePickerDepartureDate.Value = DateTime.Now.AddDays(1);
        }

        private void UncheckAllItems(CheckedListBox checkedListBox)
        {
            foreach (int i in checkedListBox.CheckedIndices)
            {
                checkedListBox.SetItemCheckState(i, CheckState.Unchecked);
            }
        }
    }
}
