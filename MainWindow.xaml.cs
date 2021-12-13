using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
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
using System.Globalization;
using Microsoft.Win32;
using System.ComponentModel;
using System.Runtime.CompilerServices;
//using System.c
namespace WeatherBook
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : INotifyPropertyChanged
    {
        bool contextChanged;

        double minTemp1;
        double MaxTemp1;

        double minTemp2;
        double MaxTemp2;

        double dobAveTemp01;
        double dobAveTemp02;

        
        double dob_TotAveTemp;

        //Data Binding Properties
        public double PropMinTemp1
        {
            get { return minTemp1; }
            set
            {
                if (minTemp1 != value)
                {
                    minTemp1 = value;
                    OnPropertyChanged();
                    
                }
            }
        }

        public double PropMaxTemp1
        {
            get { return MaxTemp1; }
            set
            {
                if (MaxTemp1 != value)
                {
                    MaxTemp1 = value;
                    OnPropertyChanged();
                }
            }
        }

        public double PropMinTemp2
        {
            get { return minTemp2; }
            set
            {
                if (minTemp2 != value)
                {
                    minTemp2 = value;
                    OnPropertyChanged();
                }
            }
        }

        public double PropMaxTemp2
        {
            get { return MaxTemp2; }
            set
            {
                if (MaxTemp2 != value)
                {
                    MaxTemp2 = value;
                    OnPropertyChanged();
                }
            }
        }

        public double PropAve1
        {
            get { return dobAveTemp01; }
            set
            {
                dobAveTemp01 = (minTemp1 + MaxTemp1) / 2;
                OnPropertyChanged();
            }
        }

        public double PropAve2
        {
            get { return dobAveTemp02; }
            set
            {
                dobAveTemp02 = (minTemp2 + MaxTemp2) / 2;
                OnPropertyChanged();
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public MainWindow()
        {
            InitializeComponent();

            contextChanged = false;

            DataContext = this;
            /*minTemp1 = 0;
            MaxTemp1 = 0;
            minTemp2 = 0;
            MaxTemp2 = 0;*/
            
            

            dobAveTemp01 = (minTemp1 + MaxTemp1) / 2;
            dobAveTemp02 = (minTemp2 + MaxTemp2) / 2;

            dob_TotAveTemp = (dobAveTemp01 + dobAveTemp02) / 2;
        }

        void save() 
        {
            //Replace the existed file
            File.Delete("C:\\Users\\ghait\\source\\repos\\WeatherBook\\WeatherBook\\WeatherBook2021.txt");

            //Write the file again
            StreamWriter textFile = new StreamWriter("C:\\Users\\ghait\\source\\repos\\WeatherBook\\WeatherBook\\WeatherBook2021.txt");

            textFile.WriteLine(PrName.Text);
            textFile.WriteLine(PrLoc.Text);
            textFile.WriteLine(PrLoc.Text);

            List<string> lineStrings01 = new List<string>();

            lineStrings01.Add(Date01.Text);
            lineStrings01.Add(mTemp01.Text);
            lineStrings01.Add(MaxTemp01.Text);
            lineStrings01.Add(AveTemp01.Text);

            string line01 = string.Join(" ", lineStrings01);
            textFile.WriteLine(line01);

            List<string> lineStrings02 = new List<string>();

            lineStrings02.Add(Date02.Text);
            lineStrings02.Add(mTemp02.Text);
            lineStrings02.Add(MaxTemp02.Text);
            lineStrings02.Add(AveTemp02.Text);

            string line02 = string.Join(" ", lineStrings02);
            textFile.WriteLine(line02);

            textFile.Close();
        }
        private double convertToFahrenheit(double tempInCelisius) 
        {
            double tempFahrenheit = tempInCelisius * 33.08;

            return tempFahrenheit;
        }

        private double convertToCelisius(double tempInFahrenheit)
        {
            double tempFahrenheit = tempInFahrenheit / 33.08;

            return tempFahrenheit;
        }

        private void stringTodoubleConvertor(string[] strValues, out List<double> doubleValues)
        {
            doubleValues = new List<double>();
            try
            {
                //creating an object of NumberFormatInfo
                NumberFormatInfo provider = new NumberFormatInfo();
                provider.NumberDecimalSeparator = ".";
                provider.NumberGroupSeparator = ",";
                
                for (int i = 0; i < strValues.Length; i++)
                {
                    //converting string to double
                    double doubleVal = Convert.ToDouble(strValues[i], provider);

                    doubleValues.Add(doubleVal);
                }
                
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
            
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //Read inputs from WeatherBook Project
            if (File.Exists("C:\\Users\\ghait\\source\\repos\\WeatherBook\\WeatherBook\\WeatherBook2021.txt"))
            {
                //create new instance from stream reader
                using (StreamReader textFile = new StreamReader("C:\\Users\\ghait\\source\\repos\\WeatherBook\\WeatherBook\\WeatherBook2021.txt")) 
                {
                    string line;
                    //clear all text boxes
                    List<string> textLines = new List<string>();
                    while (textFile.EndOfStream == false) 
                    {
                        line = textFile.ReadLine();
                        textLines.Add(line);

                    }
                    //filling textboxes with the project information
                    if (textLines.Count > 0) 
                    {
                        PrName.Text = textLines[0];
                        PrLoc.Text = textLines[1];
                        //Divide day inputs
                        string day01 = textLines[3];
                        string day02 = textLines[4];
                        string[] dayInformation01 = day01.Split(" ", 4, StringSplitOptions.None);
                        string[] dayInformation02 = day02.Split(" ", 4, StringSplitOptions.None);

                        Date01.Text = dayInformation01[0];
                        Date02.Text = dayInformation02[0];

                        mTemp01.Text = dayInformation01[1];
                        mTemp02.Text = dayInformation02[1];

                        MaxTemp01.Text = dayInformation01[2];
                        MaxTemp02.Text = dayInformation02[2];

                        AveTemp01.Text = dayInformation01[3];
                        AveTemp02.Text = dayInformation02[3];

                        //Filling Generative information
                        //converting string inputs into doubles
                        List<string> strValues = new List<string>();
                        strValues.Add(mTemp01.Text);
                        strValues.Add(mTemp02.Text);
                        strValues.Add(MaxTemp01.Text);
                        strValues.Add(MaxTemp02.Text);

                        List<double> doubleValues = new List<double>();
                        string[] strValuesAr = strValues.ToArray();
                        stringTodoubleConvertor(strValuesAr, out doubleValues);

                        //Assign Class varriables 
                        minTemp1 = doubleValues[0];
                        MaxTemp1 = doubleValues[2];
                        minTemp2 = doubleValues[1];
                        MaxTemp2 = doubleValues[3];

                        dobAveTemp01 = (doubleValues[0] + doubleValues[2]) / 2;
                        dobAveTemp02 = (doubleValues[1] + doubleValues[3]) / 2;


                        dob_TotAveTemp = (dobAveTemp01 + dobAveTemp02) / 2;

                        TotAveTemp.Text = dob_TotAveTemp.ToString();
                    }
                }
            }
        }

        private void Butt_Save_Click(object sender, RoutedEventArgs e)
        {
            save();
        }

        private void Tf_Checked(object sender, RoutedEventArgs e)
        {

            minTemp1 = convertToFahrenheit(minTemp1);
            MaxTemp1 = convertToFahrenheit(MaxTemp1);
            minTemp2 = convertToFahrenheit(minTemp2);
            MaxTemp2 = convertToFahrenheit(MaxTemp2);

            mTemp01.Text = minTemp1.ToString();
            MaxTemp01.Text = MaxTemp1.ToString();
            mTemp02.Text = minTemp2.ToString();
            MaxTemp02.Text = MaxTemp2.ToString();

            dobAveTemp01 = (minTemp1 + MaxTemp1) / 2;
            dobAveTemp02 = (minTemp2 + MaxTemp2) / 2;
            dob_TotAveTemp = (dobAveTemp01 + dobAveTemp02) / 2;


            AveTemp01.Text = dobAveTemp01.ToString();
            AveTemp02.Text = dobAveTemp02.ToString();

            TotAveTemp.Text = dob_TotAveTemp.ToString();
        }

        

        
        private void Window_Closing_1(object sender, System.ComponentModel.CancelEventArgs e)
        {
            
            //make sure to save new changes
            if (contextChanged == true)
            {
                switch (MessageBox.Show("Do you want to save the new changes ?", "Request", MessageBoxButton.YesNoCancel, MessageBoxImage.Question))
                {
                    case MessageBoxResult.Yes:
                        save();
                        break;
                    case MessageBoxResult.Cancel:
                        e.Cancel = true;
                        break;
                }

            }
            
        }

       

        private void TotAveTemp_TextChanged(object sender, TextChangedEventArgs e)
        {
            contextChanged = true;
        }

        private void PrName_TextChanged(object sender, TextChangedEventArgs e)
        {
            contextChanged = true;
        }

        private void PrLoc_TextChanged(object sender, TextChangedEventArgs e)
        {
            contextChanged = true;
        }

        private void Tcil_Checked(object sender, RoutedEventArgs e)
        {
            minTemp1 = convertToCelisius(minTemp1);
            MaxTemp1 = convertToCelisius(MaxTemp1);
            minTemp2 = convertToCelisius(minTemp2);
            MaxTemp2 = convertToCelisius(MaxTemp2);

            mTemp01.Text = minTemp1.ToString();
            MaxTemp01.Text = MaxTemp1.ToString();
            mTemp02.Text = minTemp2.ToString();
            MaxTemp02.Text = MaxTemp2.ToString();

            dobAveTemp01 = (minTemp1 + MaxTemp1) / 2;
            dobAveTemp02 = (minTemp2 + MaxTemp2) / 2;
            dob_TotAveTemp = (dobAveTemp01 + dobAveTemp02) / 2;


            AveTemp01.Text = dobAveTemp01.ToString();
            AveTemp02.Text = dobAveTemp02.ToString();

            TotAveTemp.Text = dob_TotAveTemp.ToString();
        }

        private void mTemp01_TextChanged(object sender, TextChangedEventArgs e)
        {
            AveTemp01.Text = ((minTemp1 + MaxTemp1) / 2).ToString();

            TotAveTemp.Text = ((dobAveTemp01 + dobAveTemp02) / 2).ToString();
        }

        private void MaxTemp01_TextChanged(object sender, TextChangedEventArgs e)
        {
            AveTemp01.Text = ((minTemp1 + MaxTemp1) / 2).ToString();

            TotAveTemp.Text = ((dobAveTemp01 + dobAveTemp02) / 2).ToString();
        }

        private void mTemp02_TextChanged(object sender, TextChangedEventArgs e)
        {
            AveTemp02.Text = ((minTemp2 + MaxTemp2) / 2).ToString();

            TotAveTemp.Text = ((dobAveTemp01 + dobAveTemp02) / 2).ToString();
        }

        private void MaxTemp02_TextChanged(object sender, TextChangedEventArgs e)
        {
            AveTemp02.Text = ((minTemp2 + MaxTemp2) / 2).ToString();

            TotAveTemp.Text = ((dobAveTemp01 + dobAveTemp02) / 2).ToString();

        }
    }
}
