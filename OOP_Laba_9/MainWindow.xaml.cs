using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;
using System.IO;
using System.Windows.Controls.Primitives;
using System.Globalization;

namespace OOP_Laba_9
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            App.LanguageChanged += LanguageChanged;
            try
            {
                ((RichTextBox)dcmc.SelectedContent).FontFamily = new FontFamily(font.Text);
            }
            catch { }
            CultureInfo currLang = App.Language;
            //Заполняем меню смены языка:
            menuLanguage.Items.Clear();
            foreach (var lang in App.Languages)
            {
                MenuItem menuLang = new MenuItem();
                menuLang.Header = lang.DisplayName;
                menuLang.Tag = lang;
                menuLang.IsChecked = lang.Equals(currLang);
                menuLang.Click += ChangeLanguageClick;
                menuLanguage.Items.Add(menuLang);
            }
        }
        private void LanguageChanged(Object sender, EventArgs e)
        {
            CultureInfo currLang = App.Language;

            //Отмечаем нужный пункт смены языка как выбранный язык
            foreach (MenuItem i in menuLanguage.Items)
            {
                CultureInfo ci = i.Tag as CultureInfo;
                i.IsChecked = ci != null && ci.Equals(currLang);
            }
        }

        private void ChangeLanguageClick(Object sender, EventArgs e)
        {
            MenuItem mi = sender as MenuItem;
            if (mi != null)
            {
                CultureInfo lang = mi.Tag as CultureInfo;
                if (lang != null)
                {
                    App.Language = lang;
                }
            }

        }
   
    private void New_Click(object sender, RoutedEventArgs e)
        {
            TabItem doc = new TabItem();
            StackPanel sp1= new StackPanel();
            sp1.Orientation = Orientation.Horizontal;
            TextBlock tb = new TextBlock();
            tb.Margin = new Thickness(3);
            tb.Text="Document "+(dcmc.Items.Count + 1).ToString();
            sp1.Children.Add(tb);
            Button bt = new Button();
            bt.Content = "X";
            bt.Background = Brushes.Red;
            bt.Foreground = Brushes.White;
            bt.BorderBrush= Brushes.White;
            bt.Width = 15;
            bt.Margin = new Thickness(0, 2, 2, 2);
            bt.Opacity = 0.8;
            bt.Click += close_Click;
            sp1.Children.Add(bt);
            doc.Header = sp1;
            RichTextBox rtbEditor = new RichTextBox();
            rtbEditor.Margin = new Thickness(10,10,10,10);
            rtbEditor.SpellCheck.IsEnabled = true;
            
            doc.Content = rtbEditor;
            dcmc.Items.Add(doc);
            dcmc.SelectedIndex = dcmc.Items.Count-1;
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.Filter = "Rich Text Format (*.rtf)|*.rtf|All files (*.*)|*.*";
            if (dlg.ShowDialog() == true)
            {
                FileStream fileStream = new FileStream(dlg.FileName, FileMode.Create);
                TextRange range = new TextRange(((RichTextBox)dcmc.SelectedContent).Document.ContentStart, ((RichTextBox)dcmc.SelectedContent).Document.ContentEnd);
                range.Save(fileStream, DataFormats.Rtf);
                ((TextBlock)((StackPanel)((TabItem)dcmc.SelectedItem).Header).Children[0]).Text = dlg.FileName;
            }
        }

        private void Open_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                OpenFileDialog dlg = new OpenFileDialog();
                dlg.Filter = "Rich Text Format (*.rtf)|*.rtf|All files (*.*)|*.*";
                if (dlg.ShowDialog() == true)
                {
                    FileStream fileStream = new FileStream(dlg.FileName, FileMode.Open);
                    New_Click(sender, e);
                    TextRange range = new TextRange(((RichTextBox)dcmc.SelectedContent).Document.ContentStart, ((RichTextBox)dcmc.SelectedContent).Document.ContentEnd);
                    range.Load(fileStream, DataFormats.Rtf);
                    ((TextBlock)((StackPanel)((TabItem)dcmc.SelectedItem).Header).Children[0]).Text = dlg.FileName;
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Error! \nОшибка открытия.");
            }

        }

        private void close_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result =MessageBox.Show("Вы хотите сохранить перед закрытием?","Закрыть",MessageBoxButton.YesNoCancel);
            if(result==MessageBoxResult.No)
                dcmc.Items.Remove(dcmc.SelectedItem);
            if (result == MessageBoxResult.Yes)
            {
                Save_Click(sender, e);
                dcmc.Items.Remove(dcmc.SelectedItem);
            }
        }

        private void Copy_Click(object sender, RoutedEventArgs e)
        {
            ((RichTextBox)dcmc.SelectedContent).Copy();
        }

        private void Paste_Click(object sender, RoutedEventArgs e)
        {
            ((RichTextBox)dcmc.SelectedContent).Paste();
        }

        private void Сut_Click(object sender, RoutedEventArgs e)
        {
            ((RichTextBox)dcmc.SelectedContent).Cut();
        }

        private void Undo_Click(object sender, RoutedEventArgs e)
        {
            ((RichTextBox)dcmc.SelectedContent).Undo();
        }

        private void Redo_Click(object sender, RoutedEventArgs e)
        {
            ((RichTextBox)dcmc.SelectedContent).Redo();
        }
        
        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try {
                if (((RichTextBox)dcmc.SelectedContent).Selection.IsEmpty)
                {
                    var newRun = new Run(string.Empty, ((RichTextBox)dcmc.SelectedContent).CaretPosition.GetInsertionPosition(LogicalDirection.Forward)) { FontFamily = new FontFamily(font.Text) };
                    ((RichTextBox)dcmc.SelectedContent).CaretPosition.Paragraph.Inlines.Add(newRun);
                    ((RichTextBox)dcmc.SelectedContent).CaretPosition = newRun.ContentEnd;
                    ((RichTextBox)dcmc.SelectedContent).Focus();

                }
                else
                    ((RichTextBox)dcmc.SelectedContent).Selection.ApplyPropertyValue(TextElement.FontFamilyProperty, font.Text);
            }
            catch { }
            }

        private void ComboBox_SelectionChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            try
            {
                if (((RichTextBox)dcmc.SelectedContent).Selection.IsEmpty)
                {
                    var newRun = new Run(string.Empty, ((RichTextBox)dcmc.SelectedContent).CaretPosition.GetInsertionPosition(LogicalDirection.Forward))
                    { FontFamily = new FontFamily(font.Text) };
                    ((RichTextBox)dcmc.SelectedContent).CaretPosition.Paragraph.Inlines.Add(newRun);
                    ((RichTextBox)dcmc.SelectedContent).CaretPosition = newRun.ContentEnd;
                    ((RichTextBox)dcmc.SelectedContent).Focus();

                }
                else
                    ((RichTextBox)dcmc.SelectedContent).Selection.ApplyPropertyValue(TextElement.FontFamilyProperty, font.Text);
            }
            catch { }
        }

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            
            try
            {
                
                if (((RichTextBox)dcmc.SelectedContent).Selection.IsEmpty)
                {
                    var newRun = new Run(string.Empty, ((RichTextBox)dcmc.SelectedContent).CaretPosition.GetInsertionPosition(LogicalDirection.Forward))
                    { FontSize = siz.Value };
                    ((RichTextBox)dcmc.SelectedContent).CaretPosition.Paragraph.Inlines.Add(newRun);
                    ((RichTextBox)dcmc.SelectedContent).CaretPosition = newRun.ContentEnd;
                    ((RichTextBox)dcmc.SelectedContent).Focus();

                }
                else
                    ((RichTextBox)dcmc.SelectedContent).Selection.ApplyPropertyValue(TextElement.FontSizeProperty, siz.Value);
            }
            catch { }
        }

        private void B_Unchecked(object sender, RoutedEventArgs e)
        {
            try
            {
                    ((RichTextBox)dcmc.SelectedContent).Selection.ApplyPropertyValue(TextElement.FontWeightProperty, "Normal");
            }
            catch { }
        }

        private void B_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                   ((RichTextBox)dcmc.SelectedContent).Selection.ApplyPropertyValue(TextElement.FontWeightProperty, "Bold");
            }
            catch { }
        }

        private void I_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                ((RichTextBox)dcmc.SelectedContent).Selection.ApplyPropertyValue(TextElement.FontStyleProperty, "Italic");
            }
            catch { }
        }

        private void I_Unchecked(object sender, RoutedEventArgs e)
        {
            try
            {
                ((RichTextBox)dcmc.SelectedContent).Selection.ApplyPropertyValue(TextElement.FontStyleProperty, "Normal");
            }
            catch { }
        }

        private void U_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                ((RichTextBox)dcmc.SelectedContent).Selection.ApplyPropertyValue(TextBlock.TextDecorationsProperty, "Underline");
            }
            catch { }
        }

        private void U_Unchecked(object sender, RoutedEventArgs e)
        {
            try
            {
                ((RichTextBox)dcmc.SelectedContent).Selection.ApplyPropertyValue(TextBlock.TextDecorationsProperty, "None");
            }
            catch { }
        }

        

        private void richText_KeyUp(object sender, KeyEventArgs e)
        {
            //RichTextBox r = sender as RichTextBox;
            words.Content = ((new TextRange(((RichTextBox)dcmc.SelectedContent).Document.ContentStart, ((RichTextBox)dcmc.SelectedContent).Document.ContentEnd).Text).Length - 2).ToString();
        }

        private void File_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
