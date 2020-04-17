using HtmlAgilityPack;
using Syncfusion.Presentation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
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

namespace PPTGeneratorAssistant
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<string> luckyUrls;
        private bool JustChecked;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click_Search(object sender, RoutedEventArgs e)
        {

            //Get user input
            string title = TitleBox.Text;
            string text = TextBox.Text;

            //Get HTML code
            string html = GetHtmlCode(title, text);
            //Parse out the URLs from the HTML code
            List<string> urls = GetUrls(html);

            var rnd = new Random();
            this.luckyUrls = urls.OrderBy(x => rnd.Next()).Take(9).ToList();

            Img1.Source = SaveImage(luckyUrls[0]);
            Img2.Source = SaveImage(luckyUrls[1]);
            Img3.Source = SaveImage(luckyUrls[2]);
            Img4.Source = SaveImage(luckyUrls[3]);
            Img5.Source = SaveImage(luckyUrls[4]);
            Img6.Source = SaveImage(luckyUrls[5]);
            Img7.Source = SaveImage(luckyUrls[6]);
            Img8.Source = SaveImage(luckyUrls[7]);
            Img9.Source = SaveImage(luckyUrls[8]);

            //Set visibility
            SelectImage.Visibility = Visibility.Visible;
            btn1.Visibility = Visibility.Visible;
            btn2.Visibility = Visibility.Visible;
            btn3.Visibility = Visibility.Visible;
            btn4.Visibility = Visibility.Visible;
            btn5.Visibility = Visibility.Visible;
            btn6.Visibility = Visibility.Visible;
            btn7.Visibility = Visibility.Visible;
            btn8.Visibility = Visibility.Visible;
            btn9.Visibility = Visibility.Visible;
            Img1.Visibility = Visibility.Visible;
            Img2.Visibility = Visibility.Visible;
            Img3.Visibility = Visibility.Visible;
            Img4.Visibility = Visibility.Visible;
            Img5.Visibility = Visibility.Visible;
            Img6.Visibility = Visibility.Visible;
            Img7.Visibility = Visibility.Visible;
            Img8.Visibility = Visibility.Visible;
            Img9.Visibility = Visibility.Visible;
            PPTBtn.Visibility = Visibility.Visible;

        }

        // Export PPT slide
        private void Button_Click_Generate(object sender, RoutedEventArgs e)
        {
            //Create a new PowerPoint presentation
            IPresentation powerpointDoc = Presentation.Create();

            //Add a blank slide to the presentation
            ISlide slide = powerpointDoc.Slides.Add(SlideLayoutType.TitleOnly);

            IShape titleShape = slide.Shapes[0] as IShape;
            titleShape.TextBody.AddParagraph(TitleBox.Text).HorizontalAlignment = HorizontalAlignmentType.Center;
            //Add a textbox to the slide

            IShape descriptionShape = slide.AddTextBox(53.22, 141.73, 874.19, 77.70);
            descriptionShape.TextBody.Text = TextBox.Text;

            List<Stream> pictureStreams = new List<Stream>();
            if (btn1.IsChecked == true)
            {
                pictureStreams.Add(new MemoryStream(GetImage(luckyUrls[0])));
            }
            if (btn2.IsChecked == true)
            {
                pictureStreams.Add(new MemoryStream(GetImage(luckyUrls[1])));
            }
            if (btn3.IsChecked == true)
            {
                pictureStreams.Add(new MemoryStream(GetImage(luckyUrls[2])));
            }
            if (btn4.IsChecked == true)
            {
                pictureStreams.Add(new MemoryStream(GetImage(luckyUrls[3])));
            }
            if (btn5.IsChecked == true)
            {
                pictureStreams.Add(new MemoryStream(GetImage(luckyUrls[4])));
            }
            if (btn6.IsChecked == true)
            {
                pictureStreams.Add(new MemoryStream(GetImage(luckyUrls[5])));
            }
            if (btn7.IsChecked == true)
            {
                pictureStreams.Add(new MemoryStream(GetImage(luckyUrls[6])));
            }
            if (btn8.IsChecked == true)
            {
                pictureStreams.Add(new MemoryStream(GetImage(luckyUrls[7])));
            }
            if (btn9.IsChecked == true)
            {
                pictureStreams.Add(new MemoryStream(GetImage(luckyUrls[8])));
            }

            double top = 53.22;
            double left = 219.43;
            double width = 75;
            double height = 75;
            foreach (Stream pictureStream in pictureStreams)
            {
                //Adds the picture to a slide by specifying its size and position.
                slide.Shapes.AddPicture(pictureStream, top, left, width, height);
                top += width;
            }



            //Save the PowerPoint presentation
            powerpointDoc.Save("Sample.pptx");

            //Close the PowerPoint presentation
            powerpointDoc.Close();
            MessageBox.Show("PPT is generated at the repo's bin file!");
        }

        private List<string> GetUrls(string html)
        {
            var urls = new List<string>();

            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(html);
            foreach (HtmlNode img in doc.DocumentNode.SelectNodes("//img"))
            {
                urls.Add(img.GetAttributeValue("data-src", null));
            }
            urls.RemoveAll(string.IsNullOrWhiteSpace);
            return urls;
        }

        private string GetHtmlCode(string title, string text)
        {
            //Build search url
            string url = "https://www.google.com/search?q=" + title + text + "&tbm=isch";
            string html = "";

            //Get HTML Code
            var request = (HttpWebRequest)WebRequest.Create(url);
            //This allows to get full size of the images
            request.Accept = "text/html, application/xhtml+xml, */*";
            request.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64; Trident/7.0; rv:11.0) like Gecko";
            var response = (HttpWebResponse)request.GetResponse();

            using (Stream stream = response.GetResponseStream())
            {
                if (stream != null)
                {
                    using (var str = new StreamReader(stream))
                    {
                        html = str.ReadToEnd();
                    }
                }
            }
            return html;
        }

        private void RB_Checked(object sender, RoutedEventArgs e)
        {
            RadioButton s = sender as RadioButton;
            // Action on Check...
            JustChecked = true;
        }

        private void RB_Clicked(object sender, RoutedEventArgs e)
        {
            if (JustChecked)
            {
                JustChecked = false;
                e.Handled = true;
                return;
            }
            RadioButton s = sender as RadioButton;
            if (s.IsChecked == true)
            {
                s.IsChecked = false;
            }

        }
        private BitmapImage SaveImage(string url)
        {
            byte[] image = GetImage(url);
            using (var stream = new MemoryStream(image))
            {
                var bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.StreamSource = stream;
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.EndInit();
                bitmap.Freeze();
                return bitmap;
            }
        }
        private byte[] GetImage(string url)
        {
            var request = (HttpWebRequest)WebRequest.Create(url);
            var response = (HttpWebResponse)request.GetResponse();

            using (Stream stream = response.GetResponseStream())
            {
                if (stream != null)
                {
                    using (var str = new BinaryReader(stream))
                    {
                        byte[] bytes = str.ReadBytes(100000000);
                        return bytes;
                    }
                }
                return null;
            }
        }
    }
}
