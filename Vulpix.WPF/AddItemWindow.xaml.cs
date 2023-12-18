using Knox.Mediation;
using Vulpix.Domain.Commands;
using Vulpix.Domain.Models;
using Vulpix.Domain.Repositories;
using System;
using System.Windows;

namespace Vulpix.WPF
{
    /// <summary>
    /// Interaction logic for AddBookWindow.xaml
    /// </summary>
    public partial class AddItemWindow : Window
    {
        private readonly IHobbyFile hobbyFile;
        private readonly IMediator mediator;
        private readonly Hobby selectedHobby;
        private readonly int index;

        public AddItemWindow(IHobbyFile hobbyFile, IMediator mediator, Hobby selectedHobby, int index)
        {
            this.hobbyFile = hobbyFile;
            this.mediator = mediator;
            this.selectedHobby = selectedHobby;
            this.index = index;

            InitializeComponent();
        }

        private async void AddItemButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                await mediator.ExecuteCommandAsync(new CreateItemCommand(hobbyFile, ItemNameTextBox.Text, selectedHobby.Type));
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred performing this action: {ex.Message}", "An error has occurred");
            }
            finally
            {
                DialogResult = true;
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            AddItem.Title = selectedHobby.AddText;
            AddItemButton.Content = selectedHobby.AddText;

            ItemNameTextBox.Focus();
        }
    }
}
