﻿using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace ImageViewer.Animations
{
    public static class AnimationLibrary
    {
        /// <summary>
        /// Use this method to make an animation for a control in X axis
        /// </summary>
        /// <param name="cntrl">The targhetting Control</param>
        /// <param name="XPos">Here the position to add</param>
        /// <param name="TimeSecond">The duration of the animation</param>
        /// <param name="TimeMillisecond">The delay of the animation</param>
        public static void MoveToTargetX(Control control, double From, double To, double TimeSecond, double TimeMillisecond = 0)
        {
            control.Margin = new Thickness(control.Margin.Left - To, control.Margin.Top, control.Margin.Right + To, control.Margin.Bottom);
            QuinticEase EP = new QuinticEase();
            EP.EasingMode = EasingMode.EaseOut;

            DoubleAnimation DirX = new DoubleAnimation
            {
                Duration = new Duration(TimeSpan.FromSeconds(TimeSecond)),
                From = From,
                To = To,
                BeginTime = TimeSpan.FromMilliseconds(TimeMillisecond),
                EasingFunction = EP,
                AutoReverse = false
            };
            control.RenderTransform = new TranslateTransform();
            control.RenderTransform.BeginAnimation(TranslateTransform.XProperty, DirX);
        }
        public static void OpacityControl(Control control, double From, double To, double TimeSecond, double TimeMillisecond = 0)
        {
            QuinticEase EP = new QuinticEase();
            EP.EasingMode = EasingMode.EaseOut;

            DoubleAnimation Dir = new DoubleAnimation
            {
                Duration = new Duration(TimeSpan.FromSeconds(TimeSecond)),
                From = From,
                To = To,
                BeginTime = TimeSpan.FromMilliseconds(TimeMillisecond),
                EasingFunction = EP,
                AutoReverse = false
            };
            control.BeginAnimation(UIElement.OpacityProperty, Dir);
        }
    }
}
