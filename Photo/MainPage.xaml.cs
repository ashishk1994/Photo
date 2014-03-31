using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using Microsoft.Xna.Framework.Media;
using Microsoft.Phone.Tasks;
using AviarySDK;
using System.IO;
using System.Windows.Media.Imaging;
using System.IO.IsolatedStorage;
using Coding4Fun.Toolkit.Controls;
using System.Threading;

namespace Photo
{
    public partial class MainPage : PhoneApplicationPage
    {
        PhotoChooserTask photochooserTask;
        CameraCaptureTask cameracaptureTask;
        string str;
        List<AviaryFeature> features = new List<AviaryFeature>();
       
        public MainPage()
        {  
            InitializeComponent();
            Thread.Sleep(2500);
            Microsoft.Phone.Controls.TiltEffect.TiltableItems.Add(typeof(TiltableControl));
            photochooserTask = new PhotoChooserTask();
            photochooserTask.Completed += new EventHandler<PhotoResult>(photochooserTask_Completed);
            cameracaptureTask = new CameraCaptureTask();
            cameracaptureTask.Completed += new EventHandler<PhotoResult>(cameracaptureTask_Completed);
        }

        private void editingSession()
        {
            features.Add(AviaryFeature.Enhance);
            features.Add(AviaryFeature.Effects);
            features.Add(AviaryFeature.Orientation);
            features.Add(AviaryFeature.Blemish);
            features.Add(AviaryFeature.Brightness);
            features.Add(AviaryFeature.Contrast);
            features.Add(AviaryFeature.Crop);
            features.Add(AviaryFeature.Redeye);
            features.Add(AviaryFeature.Saturation);
            features.Add(AviaryFeature.Sharpness);
            features.Add(AviaryFeature.Stickers);
            features.Add(AviaryFeature.Text);
            features.Add(AviaryFeature.Whiten);
            features.Add(AviaryFeature.Draw);
        }
    
        void photochooserTask_Completed(object sender, PhotoResult e)
        {
            if (e.TaskResult == TaskResult.OK)
            {
                str = e.ChosenPhoto.Length.ToString();
                editingSession();
             
                Stream stream =e.ChosenPhoto;
                String originalFile= e.OriginalFileName;

                AviaryTask aviaryTask = new AviaryTask(stream, features, themeColor: "FF6A6A", originalFileName: originalFile);
                aviaryTask.Completed += new EventHandler<AviaryTaskResultArgs>(aviaryTask_Completed);
                aviaryTask.Show();
            }
        }

        

        void cameracaptureTask_Completed(object sender, PhotoResult e)
        {
            if (e.TaskResult == TaskResult.OK)
            {
                str = e.ChosenPhoto.Length.ToString();
                editingSession();

                Stream stream = e.ChosenPhoto;
                String originalFile = e.OriginalFileName;

                AviaryTask aviaryTask = new AviaryTask(stream, features, themeColor: "FF6A6A", originalFileName: originalFile);
                aviaryTask.Completed += new EventHandler<AviaryTaskResultArgs>(aviaryTask_Completed);
                aviaryTask.Show();
            }
        }


        void aviaryTask_Completed(object sender, AviaryTaskResultArgs e)
        {
            if (e.AviaryResult == AviaryResult.OK)
            {
                string fileName = "abc.jpg";
                fileName += ".jpg";
                var myStore = IsolatedStorageFile.GetUserStoreForApplication();
             
                if (myStore.FileExists(fileName))
                {
                    myStore.DeleteFile(fileName);
                }

                IsolatedStorageFileStream myFileStream = myStore.CreateFile(fileName);
                WriteableBitmap wr = e.PhotoResult;
                wr.SaveJpeg(myFileStream, wr.PixelWidth, wr.PixelHeight, 0, 85);
                myFileStream.Close();

                myFileStream = myStore.OpenFile(fileName, FileMode.Open, FileAccess.Read);
                MediaLibrary library = new MediaLibrary();
                library.SavePicture(fileName, myFileStream);

                System.Windows.Media.Imaging.BitmapImage bmp = new System.Windows.Media.Imaging.BitmapImage();
                bmp.SetSource(myFileStream);
                showImage.Source = bmp;

                ContentPanel.Visibility = System.Windows.Visibility.Collapsed;
                secondaryGrid.Visibility = System.Windows.Visibility.Visible;
               
                {
                   aviaryTask_Error(e.Exception);
                }
            }
        }
     
        void aviaryTask_Error(Exception ex)
        {
            if (ex == null)
                return;

            if (ex.Message == AviaryError.StreamNull)
            {
                // Input stream can't be null
            }
            else if (ex.Message == AviaryError.FeaturesEmpty)
            {
                // Features list determines which tools are exposed in the Aviary editor and cannot be null or empty
            }
            else if (ex.Message == AviaryError.ImageBig)
            {
                // The image cannot exceed 8 mega pixels
            }
            else if (ex.Message == AviaryError.AdjustmentsEmpty)
            {
                // The adjustment array passed into Photo Genius Apply is not valid.
                // The array must be of 4 float values and the array can't be empty or null
            }
            else
            {
                // This is to handle any error thrown by the system
            }
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
	        IDictionary<string, string> queryStrings = this.NavigationContext.QueryString;
	        
	        if (queryStrings.ContainsKey("token"))
	        {
 
		        MediaLibrary library = new MediaLibrary();
		       
		        Picture picture = library.GetPictureFromToken(queryStrings["token"]);
 
                Stream imageStream = picture.GetImage();
                editingSession();
                String originalFile = picture.Name;

                AviaryTask aviaryTask = new AviaryTask(imageStream, features, themeColor: "FF6A6A", originalFileName: originalFile);
                aviaryTask.Completed += new EventHandler<AviaryTaskResultArgs>(aviaryTask_Completed);
                aviaryTask.Show();

		        queryStrings.Remove("token");
 
	        }
	        base.OnNavigatedTo(e);
        }

        private void selectTile_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            photochooserTask.Show();
        }

        private void cameraTile_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            cameracaptureTask.Show();
        }

        private void RoundButton_Click(object sender, RoutedEventArgs e)
        {
            MarketplaceReviewTask objReviewTask = new MarketplaceReviewTask();
            objReviewTask.Show();
        }

        private void RoundButton_Click_1(object sender, RoutedEventArgs e)
        {
            EmailComposeTask emailComposeTask = new EmailComposeTask();
            emailComposeTask.To = "anaghsharma12@gmail.com";
            emailComposeTask.Subject = "CamTool - Feedback";
            emailComposeTask.Show();
        }

        private void RoundButton_Click_2(object sender, RoutedEventArgs e)
        {
            try
            {
                MarketplaceSearchTask mpSearchTask = new MarketplaceSearchTask();
                mpSearchTask.SearchTerms = "";
                mpSearchTask.Show();
            }
            catch (Exception)
            {
                //Do something or nothing
            }
        }

        private void RoundButton_Click_3(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Pages/About.xaml", UriKind.Relative));
        }

        private void RoundButton_Click_4(object sender, RoutedEventArgs e)
        {
            secondaryGrid.Visibility = System.Windows.Visibility.Collapsed;
            ContentPanel.Visibility = System.Windows.Visibility.Visible;
        }

    }
    public class TiltableControl : Grid
    {

    }
}