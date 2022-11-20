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
using System.Windows.Forms.DataVisualization.Charting;

namespace WpfTest
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private KeyObj keyObj;
        public MainWindow()
        {
            InitializeComponent();
            keyObj = new KeyObj(1,DateTime.Now, DateTime.Now);
            DataContext = keyObj;
            //comboBox1.ItemsSource = keyObj.CurrencyName;
        }

        private void WindowsFormsHost_Loaded(object sender, RoutedEventArgs e)
        {
            DrawGrafic();
        }

        private void DrawGrafic()
        {
            // Все графики находятся в пределах области построения ChartArea, создадим ее
            chart.ChartAreas.Add(new ChartArea("Default"));
            chart.ChartAreas[0].AxisX.Title = "Дата";

            chart.ChartAreas[0].AxisY.Title = "Курс";


            // Добавим линию, и назначим ее в ранее созданную область "Default"
            chart.Series.Add(new Series("Series1"));
            chart.Series["Series1"].ChartArea = "Default";
            chart.Series["Series1"].ChartType = SeriesChartType.Line;


            // добавим данные линии
            string[] axisXData = new string[] { "a", "b", "c" };
            double[] axisYData = new double[] { 0.1, 1.5, 1.9 };
            chart.Series["Series1"].Points.DataBindXY(axisXData, axisYData);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var z=keyObj.SelectCurrency;
        }
    }
}
