using OrganizerApp1.Models;
using OrganizerApp1.Services;
using System.Collections.Generic;
using Microsoft.Maui.Controls;

namespace OrganizerApp1.Pages
{
    public partial class ReportDetailsPage : ContentPage
    {
        private readonly int _reportId;
        private readonly DatabaseService _databaseService;

        public ReportDetailsPage(string reportName, List<OrganizerApp1.Models.Event> events, int reportId)
        {
            InitializeComponent();
            Title = reportName;
            _reportId = reportId;
            _databaseService = new DatabaseService();
            BindingContext = new { Events = events };
        }

        private async void OnDeleteReportClicked(object sender, EventArgs e)
        {
            bool confirm = await DisplayAlert("�������������", "�� �������, ��� ������ ������� �����?", "��", "���");
            if (confirm)
            {
                try
                {
                    var report = await _databaseService.GetReportByIdAsync(_reportId);
                    if (report != null)
                    {
                        await _databaseService.DeleteReportAsync(report);

                        await DisplayAlert("�����", "����� ������� �����.", "OK");

                        // ������� �� ������� �������� (MainPage.xaml)
                        await Navigation.PopToRootAsync();  // ��� �������� � �������� �� ������� ��������
                    }
                }
                catch (Exception ex)
                {
                    await DisplayAlert("������", $"�� ������� ������� �����: {ex.Message}", "OK");
                }
            }
        }
    }
}
