using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
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

namespace ImageViewer.Controls
{
    /// <summary>
    /// Interaction logic for MovableControl.xaml
    /// </summary>
    public partial class MovableControl : UserControl
    {
        public bool IsMouseDown;
        public Point OldMousePosition;
        public Point MouseMoveDelta;

        public bool CanMove { get; set; }
        public bool CanZoom { get; set; }
        public int ZoomMultiplier { get; set; }
        public Action<MovableControl, Thickness> SetMarginCallback { get; set; }

        public MovableControl()
        {
            ZoomMultiplier = 1;
            CanMove = true;
            CanZoom = true;
            InitializeComponent();

            this.PreviewMouseLeftButtonDown += MovableContentParent_MouseLeftButtonDown;
            this.PreviewMouseLeftButtonUp += MovableContentParent_MouseLeftButtonUp;
            this.PreviewMouseMove += ControlMouseMove;
            this.PreviewMouseWheel += ControlMouseWheel;
        }

        private void ControlMouseMove(object sender, MouseEventArgs e)
        {
            if (this != null && CanMove)
            {
                IsMouseDown = (e.LeftButton == MouseButtonState.Pressed) ? true : false;
                if (IsMouseDown)
                {
                    Point mousePos = e.MouseDevice.GetPosition(null);
                    MouseMoveDelta = new Point(OldMousePosition.X - mousePos.X, OldMousePosition.Y - mousePos.Y);

                    // when X is -1, mouse moved right.
                    // when X is 1, mouse moved left
                    // when Y is -1, mouse moved down.
                    // when Y is 1, mouse moved up.

                    if (MouseMoveDelta.X <= -1) MoveContentRight(this, MouseMoveDelta.X);
                    if (MouseMoveDelta.X >= 1) MoveContentLeft(this, MouseMoveDelta.X);
                    if (MouseMoveDelta.Y <= -1) MoveContentDown(this, MouseMoveDelta.Y);
                    if (MouseMoveDelta.Y >= 1) MoveContentUp(this, MouseMoveDelta.Y);

                    OldMousePosition = mousePos;
                }
            }
        }

        public void MoveContentLeft(FrameworkElement control, double amount)
        {
            MoveControlHorizontally(control, -amount, amount);
        }
        public void MoveContentRight(FrameworkElement control, double amount)
        {
            MoveControlHorizontally(control, -amount, amount);
        }
        public void MoveContentUp(FrameworkElement control, double amount)
        {
            MoveControlVertically(control, -amount, amount);
        }
        public void MoveContentDown(FrameworkElement control, double amount)
        {
            MoveControlVertically(control, -amount, amount);
        }

        public void MoveControlHorizontally(FrameworkElement control, double amountLeft, double amountRight)
        {
            SetMargin(control, AddToMargin(control.Margin, amountLeft, 0, amountRight, 0));
        }
        public void MoveControlVertically(FrameworkElement control, double amountUp, double amountDown)
        {
            SetMargin(control, AddToMargin(control.Margin, 0, amountUp, 0, amountDown));
        }

        public void ControlZoomIn(FrameworkElement control, int amountToZoom)
        {
            Thickness newZoom = AddToMargin(control.Margin, amountToZoom, amountToZoom, amountToZoom, amountToZoom);
            SetMargin(control, newZoom);

        }
        public void ControlZoomOut(FrameworkElement control, int amountToZoom)
        {
            SetMargin(control, AddToMargin(control.Margin, -amountToZoom, -amountToZoom, -amountToZoom, -amountToZoom));
        }

        public Thickness AddToMargin(Thickness oldMargin, double left, double top, double right, double bottom)
        {
            Thickness realThick = new Thickness(
                oldMargin.Left + left,
                oldMargin.Top + top,
                oldMargin.Right + right,
                oldMargin.Bottom + bottom);
            return realThick;
        }

        //public Thickness MatchAspectRatio(Thickness margin, FrameworkElement control)
        //{
        //    double left = margin.Left, top = margin.Top, right = margin.Top, bottom = margin.Bottom;
        //    double width = ActualWidth - margin.Left - margin.Right, height = ActualHeight - margin.Top - margin.Bottom;
        //
        //    double hor = width / height;
        //    double ver = height / width;
        //
        //    double
        //       newLeft = left + hor,
        //       newTop = top + ver,
        //       newRight = right + hor,
        //       newBottom = bottom + ver;
        //    return new Thickness(newLeft, newTop, newRight, newBottom);
        //    //return new Thickness(newLeft, newTop, newRight, newBottom);
        //    //Thickness newMargin = new Thickness(left * (height / width), );
        //}

        public void SetMargin(FrameworkElement control, Thickness newMargin)
        {
            //SetMarginCallback?.Invoke(this, newMargin);
            control.Margin = newMargin;
        }

        public void ResetMargin()
        {
            this.Margin = new Thickness(5);
        }

        #region Rotation



        #endregion

        private void ControlMouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (this != null && CanMove)
            {
                ZoomMultiplier = Keyboard.IsKeyDown(Key.LeftCtrl) ? 5 : 1;
                // negative so zoom out
                if (e.Delta < 0)
                {
                    ControlZoomIn(this, 5 * ZoomMultiplier);
                }
                // positive so zoom in
                else if (e.Delta > 0)
                {
                    ControlZoomOut(this, 5 * ZoomMultiplier);
                }
            }
        }

        private void MovableContentParent_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            OldMousePosition = e.GetPosition(null);
        }

        private void MovableContentParent_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            OldMousePosition = e.GetPosition(null);
        }
    }
}