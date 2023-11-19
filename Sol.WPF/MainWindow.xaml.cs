using Sol.Domain.Commanding;
using Sol.Domain.Commands;
using Sol.Domain.Common;
using Sol.Domain.Common.Maybes;
using Sol.Domain.Models;
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
        private ISaveFile saveFile = new SaveFile();

        private readonly ICommand<SaveToFileCommandContext> saveToFileCommand;
        private readonly ICommand<LoadFromFileCommandContext, ISaveFile> loadFromFileCommand;
        private readonly ICommand<StartReadingBookCommandContext> startReadingBookCommand;
        private readonly ICommand<StopReadingBookCommandContext> stopReadingBookCommand;
        private readonly ICommand<FinishBookCommandContext> finishBookCommand;
        private readonly ICommand<CreateBookCommandContext> createBookCommand;
        private readonly ICommand<DoNotFinishBookCommandContext> doNotFinishBookCommand;
        private readonly ICommand<SwapBookOrderCommandContext> swapBookCommand;
        private readonly ICommand<ExportTbrToFileCommandContext> exportTbrCommand;

        private Profile SelectedProfile { get; set; } = Profile.Personal;

        public MainWindow(ICommand<SaveToFileCommandContext> saveToFileCommand, ICommand<LoadFromFileCommandContext, ISaveFile> loadFromFileCommand, ICommand<StartReadingBookCommandContext> startReadingBookCommand, ICommand<StopReadingBookCommandContext> stopReadingBookCommand, ICommand<FinishBookCommandContext> finishBookCommand, ICommand<CreateBookCommandContext> createBookCommand, ICommand<DoNotFinishBookCommandContext> doNotFinishBookCommand, ICommand<SwapBookOrderCommandContext> swapBookCommand, ICommand<ExportTbrToFileCommandContext> exportTbrCommand)
        {
            this.saveToFileCommand = saveToFileCommand;
            this.loadFromFileCommand = loadFromFileCommand;

            this.startReadingBookCommand = startReadingBookCommand;
            this.stopReadingBookCommand = stopReadingBookCommand;
            this.finishBookCommand = finishBookCommand;
            this.createBookCommand = createBookCommand;
            this.doNotFinishBookCommand = doNotFinishBookCommand;
            this.swapBookCommand = swapBookCommand;
            this.exportTbrCommand = exportTbrCommand;

            InitializeComponent();
        }

        private void MoveBookButton_Click(object sender, RoutedEventArgs e)
        {
            // Move from TBR to Currently Reading, vice versa.
            try
            {
                var book = (ToBeReadListBox.SelectedItem as Book).ToMaybe().GetOrElse(Book.Empty)!;

                if (book == Book.Empty)
                {
                    book = (CurrentlyReadingListBox.SelectedItem as Book).ToMaybe().GetOrThrow()!;
                    stopReadingBookCommand.Execute(new(book, saveFile));
                }
                else
                {
                    startReadingBookCommand.Execute(new(book, saveFile));
                }

                ReloadListBoxes();
            }
            catch (EmptyMaybeException)
            {
                MessageBox.Show("Select a book from your TBR or Currently Reading list to move it.", "Failure to move book");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"And error occurred performing this action: {ex.Message}", "An error has occurred");
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // Load from file.
            saveFile = loadFromFileCommand.Execute(new(Data.Directory, Data.FullName(SelectedProfile)));
            ProfilesListBox.ItemsSource = Enum.GetValues<Profile>();
            ReloadListBoxes();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            saveToFileCommand.Execute(new(Data.Directory, Data.FullName(SelectedProfile), saveFile));
        }

        private void ReloadListBoxes()
        {
            MoveBookButton.Content = "Select Book To Move";

            var tbr = saveFile.GetToBeRead();
            var finished = saveFile.GetFinished();
            LoadListBox(ToBeReadListBox, tbr);
            LoadListBox(CurrentlyReadingListBox, saveFile.GetCurrentlyReading());
            LoadListBox(FinishedListBox, finished);

            ToBeReadLabel.Content = $"To Be Read ({tbr.Count()}):";
            FinishedLabel.Content = $"Finished ({finished.Count()}):";
        }

        private static void LoadListBox<T>(ListBox listBox, IEnumerable<T> source)
        {
            listBox.Items.Clear();
            foreach (var item in source)
            {
                listBox.Items.Add(item);
            }
        }

        private void SaveFileButton_Click(object sender, RoutedEventArgs e)
        {
            saveToFileCommand.Execute(new(Data.Directory, Data.FullName(SelectedProfile), saveFile));
        }

        private void CurrentlyReadingListBox_GotFocus(object sender, RoutedEventArgs e)
        {
            ToBeReadListBox.SelectedIndex = -1;
            MoveBookButton.Content = "Pause Reading";
        }

        private void ToBeReadListBox_GotFocus(object sender, RoutedEventArgs e)
        {
            CurrentlyReadingListBox.SelectedIndex = -1;
            MoveBookButton.Content = "Start Reading";
        }

        private void FinishBookButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var book = (CurrentlyReadingListBox.SelectedItem as Book).ToMaybe().GetOrThrow()!;
                finishBookCommand.Execute(new(book, saveFile));
                ReloadListBoxes();
            }
            catch (EmptyMaybeException)
            {
                MessageBox.Show("Select a book you're currently reading to finish it.", "Failed to finish book");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred performing this action: {ex.Message}", "An error has occurred");
            }
        }

        private void AddBookButton_Click(object sender, RoutedEventArgs e)
        {
            var addBookWindow = new AddBookWindow(createBookCommand, saveFile);
            addBookWindow.ShowDialog();

            if (addBookWindow.DialogResult == true)
            {
                ReloadListBoxes();
            }
        }

        private void DoNotFinishBookButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var book = (CurrentlyReadingListBox.SelectedItem as Book).ToMaybe().GetOrThrow()!;
                doNotFinishBookCommand.Execute(new(book, saveFile));
                ReloadListBoxes();
            }
            catch (EmptyMaybeException)
            {
                MessageBox.Show("Select a book you're currently reading to DNF it.", "Failed to DNF book");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred performing this action: {ex.Message}", "An error has occurred");
            }
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
            try
            {
                var book1 = (ToBeReadListBox.SelectedItem as Book).ToMaybe().GetOrThrow()!;
                var index = ToBeReadListBox.Items.IndexOf(ToBeReadListBox.SelectedItem as Book);

                if (index == 0)
                {
                    throw new Exception("Top book cannot be bumped higher.");
                }

                var book2 = (ToBeReadListBox.Items[index - 1] as Book).ToMaybe().GetOrThrow("In order to bump a book up, you must first select which book to bump.")!;
                var context = new SwapBookOrderCommandContext(book1, book2, saveFile);

                swapBookCommand.Execute(context);
                ReloadListBoxes();

                ToBeReadListBox.SelectedIndex = index - 1;
                ToBeReadListBox.Focus();
            }
            catch (EmptyMaybeException ex)
            {
                MessageBox.Show(ex.Message, "An error has occurred");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred performing this action: {ex.Message}", "An error has occurred");
            }
        }

        private void BumpDownButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var book1 = (ToBeReadListBox.SelectedItem as Book).ToMaybe().GetOrThrow()!;
                var index = ToBeReadListBox.Items.IndexOf(ToBeReadListBox.SelectedItem as Book);

                if (index == ToBeReadListBox.Items.Count - 1)
                {
                    throw new Exception("Last book cannot be bumped lower.");
                }

                var book2 = (ToBeReadListBox.Items[index + 1] as Book).ToMaybe().GetOrThrow("In order to bump a book lower, you must first select which book to bump.")!;
                var context = new SwapBookOrderCommandContext(book1, book2, saveFile);

                swapBookCommand.Execute(context);
                ReloadListBoxes();

                ToBeReadListBox.SelectedIndex = index + 1;
                ToBeReadListBox.Focus();
            }
            catch (EmptyMaybeException ex)
            {
                MessageBox.Show(ex.Message, "An error has occurred");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred performing this action: {ex.Message}", "An error has occurred");
            }
        }

        private void ExportTbrListButton_Click(object sender, RoutedEventArgs e)
        {
            var titles = new List<string>();
            {
                foreach (var book in saveFile.GetToBeRead())
                {
                    titles.Add(book.Title);
                }
            }

            exportTbrCommand.Execute(new(Data.Directory, "tbr-list.txt", titles));

            MessageBox.Show("TBR exported as a .txt file. Open your save data directory for the resulting file.", "TBR Exported");
        }

        private void ProfilesListBox_GotFocus(object sender, RoutedEventArgs e)
        {
            ChangeProfileButton.IsEnabled = true;
        }

        private void ChangeProfileButton_Click(object sender, RoutedEventArgs e)
        {
            SelectedProfile = Enum.Parse<Profile>(ProfilesListBox.SelectedItem.ToString()!);
            Window_Loaded(sender, e);
            ChangeProfileButton.IsEnabled = false;
        }
    }
}