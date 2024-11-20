using OrganizerApp1.Models;
using OrganizerApp1.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Maui.Controls;
using System.Text.Json;

namespace OrganizerApp1.Pages
{
    public partial class ExportReportPage : ContentPage
    {
        private readonly DatabaseService _databaseService;
        public List<Event> Events { get; set; }
        public List<EventReport> Reports { get; set; }

        public ExportReportPage()
        {
            InitializeComponent();
            _databaseService = new DatabaseService();
            Reports = new List<EventReport>();
            Events = new List<Event>();
            LoadReports();
            LoadPastEvents();
            MessagingCenter.Subscribe<ReportDetailsPage, int>(this, "ReportDeleted", (sender, reportId) =>
            {
                // ������� � ������� ����� �� ������
                var reportToRemove = Reports.FirstOrDefault(r => r.Id == reportId);
                if (reportToRemove != null)
                {
                    Reports.Remove(reportToRemove);
                    ReportsListView.ItemsSource = null;
                    ReportsListView.ItemsSource = Reports;
                }
            });
        }

        private async void LoadPastEvents()
        {
            try
            {
                Events = await _databaseService.GetPastEventsAsync() ?? new List<Event>();
            }
            catch (Exception ex)
            {
                await DisplayAlert("������", $"�� ������� ��������� ����������� �����������: {ex.Message}", "OK");
            }
        }

        private async void LoadReports()
        {
            try
            {
                Reports = await _databaseService.GetReportsAsync();
                ReportsListView.ItemsSource = Reports;
            }
            catch (Exception ex)
            {
                await DisplayAlert("������", $"�� ������� ��������� ������: {ex.Message}", "OK");
            }
        }

        private async void OnCreateReportClicked(object sender, EventArgs e)
        {
            try
            {
                var pastEvents = Events.Where(evt => evt.Date < DateTime.Now).ToList();

                if (!pastEvents.Any())
                {
                    await DisplayAlert("��������", "��� ����������� ����������� ��� �������� ������.", "OK");
                    return;
                }

                var report = new EventReport
                {
                    ReportName = $"����� �� {DateTime.Now:dd.MM.yyyy HH:mm}",
                    EventIds = string.Join(",", pastEvents.Select(evt => evt.Id)),
                    SerializedEvents = JsonSerializer.Serialize(pastEvents)
                };

                await _databaseService.SaveReportAsync(report);

                await DisplayAlert("�����", "����� ������� �������.", "OK");
                LoadReports();
            }
            catch (Exception ex)
            {
                await DisplayAlert("������", $"�� ������� ������� �����: {ex.Message}", "OK");
            }
        }

        private async void OnReportTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item is EventReport report)
            {
                try
                {
                    var events = JsonSerializer.Deserialize<List<Event>>(report.SerializedEvents);

                    if (events == null || !events.Any())
                    {
                        await DisplayAlert("������", "� ������ ����������� ������ � ������������.", "OK");
                        return;
                    }

                    await Navigation.PushAsync(new ReportDetailsPage(report.ReportName, events, report.Id));
                }
                catch (Exception ex)
                {
                    await DisplayAlert("������", $"�� ������� ������� �����: {ex.Message}", "OK");
                }
            }
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            MessagingCenter.Unsubscribe<ReportDetailsPage, int>(this, "ReportDeleted");
        }
    }
}