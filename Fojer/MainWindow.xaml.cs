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
using System.Diagnostics;
using System.Threading;

namespace Fojer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Constructor

        public MainWindow()
        {
            InitializeComponent();
        }
        #endregion

        #region Minimize
        /// <summary>
        /// Minimize the window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Minimize(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }
        #endregion

        #region Close
        /// <summary>
        /// Close the window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Close(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }
        #endregion

        #region Draw correct words

        /// <summary>
        /// Draw the words from dictionary
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>

        public int numOfWords { get; set; }
        public int numOfLetter { get; set; }
        public string fLetter { get; set; }
        public string lLetter { get; set; }
        private void AssignCriteria()
        {

            string temp;

            #region Num of words
            temp = NumOfWords.Text;
            int numofwords;
            try
            {
                numofwords = Int32.Parse(temp);
                numOfWords = numofwords;
            }
            catch
            {
                if (temp == "")
                    numOfWords = 5;
                else
                    ThrowError();
            }
            #endregion

            #region Num of letters
            temp = NumOfLetter.Text;
            int numofletter;
            try
            {
                numofletter = Int32.Parse(temp);
                numOfLetter = numofletter;
            }
            catch
            {
                if (temp == "")
                    numOfLetter = 0;
                else
                    ThrowError();
            }
            #endregion

            #region First Letter
            temp = firstLetter.Text;
            string fletter;
            try
            {
                fletter = temp;
                fLetter = fletter;
            }
            catch
            {
                ThrowError();
            }
            #endregion

            #region Last Letter
            temp = lastLetter.Text;
            string lletter;
            try
            {
                lletter = temp;
                lLetter = lletter;
            }
            catch
            {
                ThrowError();
            }
            #endregion

        }
        private void ThrowError()
        {
            //Trace.WriteLine("error");
        }

        private bool CheckWord(string word)
        {
            //number of letters
            if (word.Length != numOfLetter && numOfLetter != 0)
                return false;
            //first letter
            if (fLetter != "")
                if(word[0] != fLetter[0])
                    return false;
            //last letter
            if (lLetter != "")
                if(word[word.Length-1] != lLetter[0])
                    return false;
            //characters limit
            if (word.Length > 36)
                return false;

            return true;
        }
        private void Draw(object sender, RoutedEventArgs e)
        {
            // (1) Assigning criteria
            AssignCriteria();

            // (2) Check Words
            List<string> Pool = new List<string>();
            int count = 0;

            foreach (string line in System.IO.File.ReadLines("dictionary.txt", Encoding.UTF8))
            {
                byte[] bytes = Encoding.Default.GetBytes(line);
                string beef = Encoding.UTF8.GetString(bytes);
                //Trace.WriteLine(line);
                if (CheckWord(line) == true)
                {
                    Pool.Add(line);
                    //Trace.WriteLine(line);
                    count++;
                }
            }

            // (3) Drawing
            // if there are no words
            if (count == 0)
                return;

            Random r = new Random();
            List<string> Drawes = new List<string>();
            int quan = 0;
            for (int i = 1; i <= numOfWords; i++)
            {
                int n = r.Next(0, count - 1);
                if (Drawes.Contains(Pool[n]))
                    continue;
                Drawes.Add(Pool[n]);
                quan++;
            }

            // (4) Writing to window

            //Clear the labels
            for (int i = 1; i <= 7; i++)
            {
                var labelName = string.Format("Label{0}", i);
                var label = (TextBox)this.FindName(labelName);
                label.Text = "";
            }

            //write the labels
            for (int i = 1; i <= quan; i++)
            {
                var labelName = string.Format("Label{0}", i);
                var label = (TextBox)this.FindName(labelName);
                byte[] bytes = Encoding.Default.GetBytes(Drawes[i - 1]);
                Drawes[i - 1] = Encoding.UTF8.GetString(bytes);
                //Trace.WriteLine(Drawes[i - 1]);
                label.Text = Drawes[i-1];
                if(i-1 >= 6)
                    break;
            }
        }

        #endregion

    }
}
