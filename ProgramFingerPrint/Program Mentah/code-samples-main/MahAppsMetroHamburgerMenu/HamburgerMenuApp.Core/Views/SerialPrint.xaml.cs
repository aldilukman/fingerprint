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
using System.Windows.Shapes;
using System.IO.Ports;
using System.Threading;
using System.Windows.Threading;

namespace HamburgerMenuApp.Core.Views
{
    /// <summary>
    /// Interaction logic for SerialPrint.xaml
    /// </summary>
    public partial class SerialPrint : Window
    {
        FlowDocument mcFlowDoc = new FlowDocument();
        Paragraph para = new Paragraph();
        //Serial 
        SerialPort serial = new SerialPort();
        string recieved_data;
        public SerialPrint()
        {
            InitializeComponent();
            var portNames = SerialPort.GetPortNames();

            foreach (var port in portNames)
            {
                //Try for every portName and break on the first working
                dataSerial.Items.Add(port);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                serial.PortName = dataSerial.Text;
                serial.BaudRate = 115200;
                serial.Handshake = System.IO.Ports.Handshake.None;
                serial.Parity = Parity.None;
                serial.DataBits = 8;
                serial.StopBits = StopBits.One;
                serial.ReadTimeout = 200;
                serial.WriteTimeout = 50;
                serial.Open();
                serial.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(Recieve);
                connect.IsEnabled = false;
                refresh.IsEnabled = false;
                dataSerial.IsEnabled = false;
            }
            catch(Exception ee)
            {
                MessageBox.Show("Select port");
            }
            
        }

        private delegate void UpdateUiTextDelegate(string text);
        private void Recieve(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            // Collecting the characters received to our 'buffer' (string).
            recieved_data = serial.ReadExisting();
            Dispatcher.Invoke(DispatcherPriority.Send, new UpdateUiTextDelegate(WriteData), recieved_data);
        }
        private void WriteData(string text)
        {
            // Assign the value of the recieved_data to the RichTextBox.
            //para = new Paragraph();
            //mcFlowDoc = new FlowDocument();
            para.Inlines.Add(text);
            mcFlowDoc.Blocks.Add(para);
            showSerial.Document = mcFlowDoc;
            showSerial.ScrollToEnd();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var portNames = SerialPort.GetPortNames();

            dataSerial.Items.Clear();
            foreach (var port in portNames)
            {
                //Try for every portName and break on the first working
                dataSerial.Items.Add(port);
            }
        }
    }
}
