using Knox.Exceptions;
using Knox.Extensions;
using Knox.Mediation;
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
            var itemGift = (NotStartedListBox.SelectedItem as Item).Wrap();
            var newStatus = ItemStatus.InProgress;

            if (itemGift.IsEmpty)
            {
                itemGift = (InProgressListBox.SelectedItem as Item).Wrap();
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

            //// Move from TBR to Currently Reading, vice versa.
            //try
            //{
            //    var book = (ToBeReadListBox.SelectedItem as Book).ToMaybe().GetOrElse(Book.Empty)!;

            //    if (book == Book.Empty)
            //    {
            //        book = (CurrentlyReadingListBox.SelectedItem as Book).ToMaybe().GetOrThrow()!;
            //        stopReadingBookCommand.Execute(new(book, saveFile));
            //    }
            //    else
            //    {
            //        startReadingBookCommand.Execute(new(book, saveFile));
            //    }

            //    ReloadListBoxes();
            //}
            //catch (EmptyMaybeException)
            //{
            //    MessageBox.Show("Select a book from your TBR or Currently Reading list to move it.", "Failure to move book");
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show($"And error occurred performing this action: {ex.Message}", "An error has occurred");
            //}
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ChangeProfileButton.IsEnabled = false;

            // Load from file.
            saveFile = LoadFromFileCommand.Execute(new(Data.Directory, Data.FullName()));
            ProfilesListBox.ItemsSource = allHobbies;

            SetHobbyAndReload(HobbyType.Books);
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

            //var finished = saveFile.GetFinished();
            //LoadListBox(FinishedListBox, finished);

            //FinishedLabel.Content = $"Finished ({finished.Count()}):";
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

            //ToBeReadListBox.SelectedIndex = -1;
            //MoveBookButton.Content = "Pause Reading";
        }

        private void NotStartedListBox_GotFocus(object sender, RoutedEventArgs e)
        {
            NotStartedListBox.SelectedIndex = -1;
            MoveItemButton.Content = selectedHobby.StartText;

            //CurrentlyReadingListBox.SelectedIndex = -1;
            //MoveBookButton.Content = "Start Reading";
        }

        private void FinishItemButton_Click(object sender, RoutedEventArgs e)
        {
            //try
            //{
            //    var book = (CurrentlyReadingListBox.SelectedItem as Book).ToMaybe().GetOrThrow()!;
            //    finishBookCommand.Execute(new(book, saveFile));
            //    ReloadListBoxes();
            //}
            //catch (EmptyMaybeException)
            //{
            //    MessageBox.Show("Select a book you're currently reading to finish it.", "Failed to finish book");
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show($"An error occurred performing this action: {ex.Message}", "An error has occurred");
            //}
        }

        private void AddItemButton_Click(object sender, RoutedEventArgs e)
        {
            var addItemWindow = new AddItemWindow(saveFile, mediator, selectedHobby, saveFile.GetAllItems(selectedHobby.Type).Where(item => item.Status == ItemStatus.NotStarted).Count() + 1);
            addItemWindow.ShowDialog();

            if (addItemWindow.DialogResult == true)
            {
                ReloadListBoxes();
            }

            //var addBookWindow = new AddBookWindow(createBookCommand, saveFile);
            //addBookWindow.ShowDialog();

            //if (addBookWindow.DialogResult == true)
            //{
            //    ReloadListBoxes();
            //}
        }

        private void DoNotFinishBookButton_Click(object sender, RoutedEventArgs e)
        {
            //try
            //{
            //    var book = (CurrentlyReadingListBox.SelectedItem as Book).ToMaybe().GetOrThrow()!;
            //    doNotFinishBookCommand.Execute(new(book, saveFile));
            //    ReloadListBoxes();
            //}
            //catch (EmptyMaybeException)
            //{
            //    MessageBox.Show("Select a book you're currently reading to DNF it.", "Failed to DNF book");
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show($"An error occurred performing this action: {ex.Message}", "An error has occurred");
            //}
        }

        private void OpenSaveDirectoryButton_Click(object sender, RoutedEventArgs e)
        {
            if (!Directory.Exists(Data.Directory))
                Directory.CreateDirectory(Data.Directory);

            var t = Environment.GetEnvironmentVariable("WINDIR") + @"\explorer.exe";
            Process.Start(t, Data.Directory);
        }

        private void BumpUpButton_Click(object sender, RoutedEventArgs e)
        {
            //try
            //{
            //    var book1 = (ToBeReadListBox.SelectedItem as Book).ToMaybe().GetOrThrow()!;
            //    var index = ToBeReadListBox.Items.IndexOf(ToBeReadListBox.SelectedItem as Book);

            //    if (index == 0)
            //    {
            //        throw new Exception("Top book cannot be bumped higher.");
            //    }

            //    var book2 = (ToBeReadListBox.Items[index - 1] as Book).ToMaybe().GetOrThrow("In order to bump a book up, you must first select which book to bump.")!;
            //    var context = new SwapBookOrderCommandContext(book1, book2, saveFile);

            //    swapBookCommand.Execute(context);
            //    ReloadListBoxes();

            //    ToBeReadListBox.SelectedIndex = index - 1;
            //    ToBeReadListBox.Focus();
            //}
            //catch (EmptyMaybeException ex)
            //{
            //    MessageBox.Show(ex.Message, "An error has occurred");
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show($"An error occurred performing this action: {ex.Message}", "An error has occurred");
            //}
        }

        private void BumpDownButton_Click(object sender, RoutedEventArgs e)
        {
            //try
            //{
            //    var book1 = (ToBeReadListBox.SelectedItem as Book).ToMaybe().GetOrThrow()!;
            //    var index = ToBeReadListBox.Items.IndexOf(ToBeReadListBox.SelectedItem as Book);

            //    if (index == ToBeReadListBox.Items.Count - 1)
            //    {
            //        throw new Exception("Last book cannot be bumped lower.");
            //    }

            //    var book2 = (ToBeReadListBox.Items[index + 1] as Book).ToMaybe().GetOrThrow("In order to bump a book lower, you must first select which book to bump.")!;
            //    var context = new SwapBookOrderCommandContext(book1, book2, saveFile);

            //    swapBookCommand.Execute(context);
            //    ReloadListBoxes();

            //    ToBeReadListBox.SelectedIndex = index + 1;
            //    ToBeReadListBox.Focus();
            //}
            //catch (EmptyMaybeException ex)
            //{
            //    MessageBox.Show(ex.Message, "An error has occurred");
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show($"An error occurred performing this action: {ex.Message}", "An error has occurred");
            //}
        }

        private void ExportTbrListButton_Click(object sender, RoutedEventArgs e)
        {
            //var titles = new List<string>();
            //{
            //    foreach (var book in saveFile.GetToBeRead())
            //    {
            //        titles.Add(book.Title);
            //    }
            //}

            //exportTbrCommand.Execute(new(Data.Directory, "tbr-list.txt", titles));

            //MessageBox.Show("TBR exported as a .txt file. Open your save data directory for the resulting file.", "TBR Exported");
        }

        private void ProfilesListBox_GotFocus(object sender, RoutedEventArgs e)
        {
            ChangeProfileButton.IsEnabled = true;
        }

        private void ChangeProfileButton_Click(object sender, RoutedEventArgs e)
        {
            SetHobbyAndReload(Enum.Parse<HobbyType>(ProfilesListBox.SelectedItem.ToString()!.Replace(" ", string.Empty)));
            ChangeProfileButton.IsEnabled = false;

            //SelectedProfile = Enum.Parse<Profile>(ProfilesListBox.SelectedItem.ToString()!);
            //Window_Loaded(sender, e);
            //ChangeProfileButton.IsEnabled = false;
        }

        private void SetHobbyAndReload(HobbyType hobbyType)
        {
            selectedHobby = allHobbies.ToList().Find(hobby => hobby.Type == hobbyType)!;
            ReloadListBoxes();

            AddItemButton.Content = selectedHobby.AddText;
        }
    }
}