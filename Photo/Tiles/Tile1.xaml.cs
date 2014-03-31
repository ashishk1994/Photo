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

namespace Photo.Tiles
{
    public partial class Tile1 : UserControl
    {
        int faceSelected = 1;

        public Tile1()
        {
            InitializeComponent();
            CheckForAnimation();
        }

        private void liveTileAnimTop1_Part1_Completed(object sender, object e)
        {
            Storyboard anim = (Storyboard)FindName("liveTileAnimTop1_Part2");
            anim.Begin();
        }

        private void liveTileAnimTop2_Part1_Completed(object sender, object e)
        {
            Storyboard anim = (Storyboard)FindName("liveTileAnimTop2_Part2");
            anim.Begin();
        }

        private void liveTileAnimTop1_Part2_Completed(object sender, object e)
        {
            CheckForAnimation();
        }

        private void liveTileAnimTop2_Part2_Completed(object sender, object e)
        {
            CheckForAnimation();
        }

        private void CheckForAnimation()
        {
            if (faceSelected == 1)
            {
                faceSelected = 2;
                Storyboard anim = (Storyboard)FindName("liveTileAnimTop1_Part1");
                anim.Begin();
            }
            else if (faceSelected == 2)
            {
                faceSelected = 1;
                Storyboard anim = (Storyboard)FindName("liveTileAnimTop2_Part1");
                anim.Begin();
            }
        }
    }
}
