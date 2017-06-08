using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using Tricentis.CrowdIQ.Controls.ViewModels;

namespace Tricentis.CrowdIQ.Controls
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly IList<Customisation> customisations;

        public MainWindow()
        {
            InitializeComponent();

            DialogResult = false;
            this.Activate();
            DataContext = this;
        }

        public MainWindow(WindowParameters parameters) : this()
        {
            this.customisations = parameters.Customisations;
            this.Customisations = new ObservableCollection<CustomisationViewModel>();
            foreach (var customisation in this.customisations)
            {
                Customisations.Add(new CustomisationViewModel(customisation));
            }
        }

        public ObservableCollection<CustomisationViewModel> Customisations
        {
            get { return (ObservableCollection<CustomisationViewModel>)GetValue(CustomisationsProperty); }
            set { SetValue(CustomisationsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Customisations.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CustomisationsProperty =
            DependencyProperty.Register("Customisations", typeof(ObservableCollection<CustomisationViewModel>), typeof(MainWindow), new PropertyMetadata(null));

        public IList<Customisation> GetCustomisations()
        {
            return customisations;
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            this.Close();
        }

        private void btnDownload_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            this.Close();
        }
    }
}
