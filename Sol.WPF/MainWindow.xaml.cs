using Knox.Exceptions;
using Knox.Extensions;
using Knox.Mediation;
using Knox.Monads;
using Sol.Domain.Commands;
using Sol.Domain.Common;
using Sol.Domain.Models;
using Sol.Domain.Queries;
using Sol.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Sol.WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly IMediator mediator;
        private readonly IEnumerable<Hobby> allHobbies;
        private IHobbyFile saveFile;

        private Hobby selectedHobby;

        public MainWindow(IMediator mediator)
        {
            this.mediator = mediator;
            selectedHobby = new Reading();
            allHobbies = GetAllHobbiesQuery.GetHobbies();
            saveFile = new HobbyFile();

            InitializeComponent();
        }

        private async void MoveItemButton_Click(object sender, RoutedEventArgs e)
        {
            // Move from Not Started to In Progress and vice versa.
            var itemGift = GetSelectedItemWrapped(NotStartedListBox);
            var newStatus = ItemStatus.InProgress;

            if (itemGift.IsEmpty)
            {
                itemGift = GetSelectedItemWrapped(InProgressListBox);
                newStatus = ItemStatus.NotStarted;
            }
            try
            {
                await mediator.ExecuteCommandAsync(new ChangeItemStatusCommand(saveFile, itemGift.UnwrapOrTantrum()!, newStatus));

                ReloadListBoxes();
            }
            catch (EmptyGiftException)
            {
                MessageBox.Show($"Select an item from your {selectedHobby.NotStartedHeader} or {selectedHobby.InProgressHeader} list to move it.", "Failure to move item");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"And error occurred performing this action: {ex.Message}", "An error has occurred");
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // Load from file.
            saveFile = LoadFromFileCommand.Execute(new(Data.Directory, Data.FullName()));
            HobbiesListBox.ItemsSource = allHobbies;

            SetHobbyAndReload(Enum.Parse<HobbyType>(saveFile.LastSelectedHobbyTypeString));
        }

        private async void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            await mediator.ExecuteCommandAsync(new SaveHobbiesToFileCommand(saveFile, Data.Directory, Data.FullName()));
        }

        private void ReloadListBoxes()
        {
            var items = saveFile.GetAllItems(selectedHobby.Type);

            MoveItemButton.Content = "Select Item to Move";

            var notStarted = items.Where(item => item.Status == ItemStatus.NotStarted);
            LoadListBox(NotStartedListBox, notStarted);
            NotStartedLabel.Content = $"{selectedHobby.NotStartedHeader} ({notStarted.Count()})";

            var inProgress = items.Where(item => item.Status == ItemStatus.InProgress);
            LoadListBox(InProgressListBox, inProgress);
            InProgressLabel.Content = selectedHobby.InProgressHeader;

            var complete = items.Where(item => item.Status == ItemStatus.Complete);
            LoadListBox(CompletedListBox, complete);
            CompletedLabel.Content = $"{selectedHobby.CompleteHeader} ({complete.Count()})";
        }

        private static void LoadListBox<T>(ListBox listBox, IEnumerable<T> source)
        {
            listBox.Items.Clear();
            foreach (var item in source)
            {
                listBox.Items.Add(item);
            }
        }

        private async void SaveFileButton_Click(object sender, RoutedEventArgs e)
        {
            await mediator.ExecuteCommandAsync(new SaveHobbiesToFileCommand(saveFile, Data.Directory, Data.FullName()));
        }

        private void InProgressListBox_GotFocus(object sender, RoutedEventArgs e)
        {
            NotStartedListBox.SelectedIndex = -1;
            MoveItemButton.Content = selectedHobby.PauseText;
        }

        private void NotStartedListBox_GotFocus(object sender, RoutedEventArgs e)
        {
            NotStartedListBox.SelectedIndex = -1;
            MoveItemButton.Content = selectedHobby.StartText;
        }

        private async void CompleteItemButton_Click(object sender, RoutedEventArgs e)
        {
            var itemGift = GetSelectedItemWrapped(InProgressListBox);

            try
            {
                await mediator.ExecuteCommandAsync(new CompleteItemCommand(saveFile, itemGift.UnwrapOrTantrum()!));
                ReloadListBoxes();
            }
            catch (EmptyGiftException)
            {
                MessageBox.Show($"Select an item from your {selectedHobby.InProgressHeader} list to complete it.", "Failed to complete item");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred performing this action: {ex.Message}", "An error has occurred");
            }

        }

        private void AddItemButton_Click(object sender, RoutedEventArgs e)
        {
            var addItemWindow = new AddItemWindow(saveFile, mediator, selectedHobby, saveFile.GetAllItems(selectedHobby.Type).Where(item => item.Status == ItemStatus.NotStarted).Count() + 1);
            addItemWindow.ShowDialog();

            if (addItemWindow.DialogResult == true)
            {
                ReloadListBoxes();
            }
        }

        private async void DeleteItemButton_Click(object sender, RoutedEventArgs e)
        {
            // Find which item is selected.
            var itemGift = GetSelectedItemWrapped(NotStartedListBox);

            if (itemGift.IsEmpty)
            {
                itemGift = GetSelectedItemWrapped(InProgressListBox);

                if (itemGift.IsEmpty)
                {
                    itemGift = GetSelectedItemWrapped(CompletedListBox);
                }
            }

            try
            {
                await mediator.ExecuteCommandAsync(new DeleteItemCommand(saveFile, itemGift.UnwrapOrTantrum()));
            }
            catch (EmptyGiftException)
            {
                MessageBox.Show("Select an item to delete it.", "Failed to delete item");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred performing this action: {ex.Message}", "An error has occurred");
            }

            ReloadListBoxes();
        }

        private void OpenSaveDirectoryButton_Click(object sender, RoutedEventArgs e)
        {
            if (!Directory.Exists(Data.Directory))
                Directory.CreateDirectory(Data.Directory);

            var t = Environment.GetEnvironmentVariable("WINDIR") + @"\explorer.exe";
            Process.Start(t, Data.Directory);
        }

        private async void BumpUpButton_Click(object sender, RoutedEventArgs e)
        {
            await BumpInDirection(-1, 1);
        }

        private async void BumpDownButton_Click(object sender, RoutedEventArgs e)
        {
            // Get max number of items.
            var number = saveFile.GetAllItems(selectedHobby.Type).Where(item => item.Status == ItemStatus.NotStarted).Count();

            await BumpInDirection(1, number);
        }

        private async Task BumpInDirection(int direction, int invalidIndex)
        {
            var itemGift = GetSelectedItemWrapped(NotStartedListBox);

            try
            {
                var item = itemGift.UnwrapOrTantrum();

                if (item.Index == invalidIndex)
                {
                    throw new Exception("Book cannot be bumped in chosen direction.");
                }

                // Check direction, -1 for array starting at 0.
                var nextIndex = item.Index + direction - 1;
                await mediator.ExecuteCommandAsync(new BumpItemCommand(saveFile, item, direction));

                ReloadListBoxes();

                NotStartedListBox.Focus();
                NotStartedListBox.SelectedIndex = nextIndex;
            }
            catch (EmptyGiftException)
            {
                MessageBox.Show("Item to bump was not selected.", "An error has occurred");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred performing this action: {ex.Message}", "An error has occurred");
            }
        }

        private async void ExportNotStartedButton_Click(object sender, RoutedEventArgs e)
        {
            var items = saveFile
                .GetAllItems(selectedHobby.Type)
                .Where(item => item.Status == ItemStatus.NotStarted)
                .Select(item => item.Name)
                ;

            await mediator.ExecuteCommandAsync(new ExportNotStartedListCommand(Data.Directory, $"notstarted-{selectedHobby.Type}.txt", items));

            MessageBox.Show($"{selectedHobby.NotStartedHeader} exported as a .txt file. Open your save data directory for the resulting file.", $"{selectedHobby.NotStartedHeader} Exported");
        }

        private void SetHobbyAndReload(HobbyType hobbyType)
        {
            selectedHobby = allHobbies.ToList().Find(hobby => hobby.Type == hobbyType)!;
            saveFile.LastSelectedHobbyTypeString = selectedHobby.Type.ToString();
            ReloadListBoxes();

            AddItemButton.Content = selectedHobby.AddText;
            CompleteItemButton.Content = selectedHobby.FinishText;
            ExportNotStartedButton.Content = $"Export {selectedHobby.NotStartedHeader}";
        }

        private static Gift<Item> GetSelectedItemWrapped(ListBox listBox) => (listBox.SelectedItem as Item).Wrap()!;

        private void HobbiesListBox_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            SetHobbyAndReload(Enum.Parse<HobbyType>(HobbiesListBox.SelectedItem.ToString()!.Replace(" ", string.Empty)));
        }
    }
}