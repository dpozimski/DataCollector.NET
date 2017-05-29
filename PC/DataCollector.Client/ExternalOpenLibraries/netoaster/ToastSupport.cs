using netoaster.Enumes;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Threading;
using Application = System.Windows.Application;
using Matrix = System.Windows.Media.Matrix;
using Point = System.Windows.Point;
using Rectangle = System.Drawing.Rectangle;
using StoryBoard = System.Windows.Media.Animation.Storyboard;

namespace netoaster
{
    internal class ToastSupport
    {
        public static StoryBoard GetAnimation(UIElement toaster)
        {
            var story = new StoryBoard();

            toaster.RenderTransformOrigin = new Point(1, 0);
            toaster.RenderTransform = new TranslateTransform(300.0, 0);
            var slideinFromRightAnimation = new DoubleAnimationUsingKeyFrames
            {
                Duration = new Duration(TimeSpan.FromSeconds(6)),
                KeyFrames = new DoubleKeyFrameCollection
                        {
                            new EasingDoubleKeyFrame(300.0, KeyTime.FromPercent(0)),
                            new EasingDoubleKeyFrame(0.0, KeyTime.FromPercent(0.1), new ExponentialEase
                            {
                                EasingMode = EasingMode.EaseInOut
                            }),
                            new EasingDoubleKeyFrame(0.0, KeyTime.FromPercent(0.8)),
                            new EasingDoubleKeyFrame(300.0, KeyTime.FromPercent(0.9), new ExponentialEase
                            {
                                EasingMode = EasingMode.EaseOut
                            })
                        }
            };

            Storyboard.SetTargetProperty(slideinFromRightAnimation,
                new PropertyPath("RenderTransform.(TranslateTransform.X)"));
            story.Children.Add(slideinFromRightAnimation);

            return story;
        }

        public static Dictionary<string, double> GetTopandLeft(Window windowRef,
            double margin)
        {
            var retDict = new Dictionary<string, double>();
            Point bottomcorner;

            var workingArea = System.Windows.Forms.Screen.PrimaryScreen.WorkingArea;

            
            var currentAppWindow = Application.Current.MainWindow;
            //Get the Currently running applications screen.
            var screen = System.Windows.Forms.Screen.FromHandle(
                new System.Windows.Interop.WindowInteropHelper(currentAppWindow).Handle);

            var transform = GetTransform(windowRef);
            retDict.Add("Left", 0);
            retDict.Add("Top", 0);

            workingArea = screen.WorkingArea;
            bottomcorner = transform.Transform(new Point(workingArea.Right, workingArea.Bottom));
            retDict["Left"] = bottomcorner.X - windowRef.ActualWidth - margin;
            retDict["Top"] = bottomcorner.Y - windowRef.ActualHeight - margin;

            return retDict;
        }

        private static Matrix GetTransform(Visual visual)
        {
            var presentationSource = PresentationSource.FromVisual(visual);
            if (presentationSource.CompositionTarget != null)
            {
                return presentationSource.CompositionTarget.TransformFromDevice;
            }

            return new Matrix();
        }
    }
}