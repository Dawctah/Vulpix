using Sol.Domain.Commanding;
using Sol.Domain.Commands;
using Sol.Domain.Repositories;
using System;
using System.Windows;

namespace Sol.WPF
{
    /// <summary>
    /// Interaction logic for AddBookWindow.xaml
    /// </summary>
    public partial class AddBookWindow : Window
    {
        private readonly ICommand<CreateBookCommandContext> createBookCommand;
        private readonly ISaveFile saveFile;

        public AddBookWindow(ICommand<CreateBookCommandContext> createBookCommand, ISaveFile saveFile)
        {
            this.createBookCommand = createBookCommand;
            this.saveFile = saveFile;

            InitializeComponent();
        }

        private void AddBookButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var title = BookTitleTextBox.Text;
                var context = new CreateBookCommandContext(title, saveFile);
                createBookCommand.Execute(context);
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
            BookTitleTextBox.Focus();
        }
    }
}
