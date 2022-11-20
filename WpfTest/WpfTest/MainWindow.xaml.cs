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
using System.ComponentModel;
using System.Windows.Threading;
using ServerTest.Models;

namespace WpfTest
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private KeyObj keyObj;
        private RequestFromServer requestFromServer;
        
        public MainWindow()
        {
            InitializeComponent();
            keyObj = new KeyObj(1,DateTime.Now, DateTime.Now);
            RequestFromServer.ReturnRezult += DrawGraficInvoke;
            requestFromServer = new RequestFromServer();
            DataContext = keyObj;
            button1.DataContext = requestFromServer;
        }

        #region wrong for MVVM patern
        private void DrawGraficInvoke()
        {
            Dispatcher.Invoke(DrawGrafic);
        }

        private void DrawGrafic()
        {
            chart.ChartAreas.Clear();
            List<MyCurrency> list = requestFromServer.listCurrency;

            if (list?.Count > 1)
            {
                list = list.OrderBy(x => x.Date).ToList();
                //Все графики находятся в пределах области построения ChartArea, создадим ее
                chart.ChartAreas.Add(new ChartArea("Default"));
                chart.ChartAreas[0].AxisX.Title = "Дата";
                chart.ChartAreas[0].AxisX.Minimum = 1;
                chart.ChartAreas[0].AxisX.Maximum = list.Count;
                chart.ChartAreas[0].AxisX.Interval = 1;
                chart.ChartAreas[0].AxisY.Title = "Курс за" + list[0].Amount + list[0].Currency;
                decimal min = list.Min(x => x.Value),max=list.Max(x=>x.Value);
                chart.ChartAreas[0].AxisY.Minimum = Convert.ToDouble(min);
                chart.ChartAreas[0].AxisY.Maximum = Convert.ToDouble(max);
                chart.ChartAreas[0].AxisY.Interval = Convert.ToDouble((max - min) / 5);


                // Добавим линию, и назначим ее в ранее созданную область "Default"
                chart.Series.Add(new Series("Series1"));
                chart.Series["Series1"].ChartArea = "Default";
                chart.Series["Series1"].ChartType = SeriesChartType.Line;


                // добавим данные линии
                List<string> axisXData = list.Select(x=>x.Date.ToString("dd.MM")).ToList();
                List<decimal> axisYData = list.Select(x=>x.Value).ToList();
                chart.Series["Series1"].Points.DataBindXY(axisXData, axisYData);
            }
            else if (list?.Count == 1)
            {
                MessageBox.Show($"Курс запрошен только на 1 дату, курс = {list[0].Value}","Курс",MessageBoxButton.OK,MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show($"Нету информации от сервера, или завершон с ошибкой", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            button1.IsEnabled = true;

        }

        #endregion
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            button1.IsEnabled = false;
            requestFromServer.MakeRequest(keyObj);
        }




    }
}
