using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ProjectManagementTree
{
    [ContentProperty("InternalContent")]
    public partial class PannableCanvas : UserControl
    {
        public object InternalContent
        {
            get { return (object)GetValue(InternalContentProperty); }
            set { SetValue(InternalContentProperty, value); }
        }
        public static readonly DependencyProperty InternalContentProperty =
            DependencyProperty.Register("InternalContent", typeof(object), typeof(PannableCanvas));

        public PannableCanvas()
        {
            InitializeComponent();
        }

        #region DRAGGABILITY
        bool isDragged;
        private Point _last;
        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                CaptureMouse();
                _last = e.GetPosition(this);
                base.OnMouseDown(e);
                isDragged = true;
            }
            else if (e.MiddleButton == MouseButtonState.Pressed)
            {
                CenterCanvas();
            }
        }

        protected override void OnMouseUp(MouseButtonEventArgs e)
        {
            ReleaseMouseCapture();
            base.OnMouseUp(e);
            isDragged = false;
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (!isDragged)
                return;

            base.OnMouseMove(e);

            if (e.LeftButton == MouseButtonState.Pressed && IsMouseCaptured)
            {
                Point pos = e.GetPosition(this);
                
                Matrix matr = trans.Matrix;
                matr.Translate(pos.X - _last.X, pos.Y - _last.Y);
                trans.Matrix = matr;
                _last = pos;
            }
        }

        protected override void OnPreviewMouseWheel(MouseWheelEventArgs e)
        {
            base.OnPreviewMouseWheel(e);

            Matrix matr = trans.Matrix;
            matr.ScaleAt(1 + .001 * e.Delta, 1 + .001 * e.Delta, e.GetPosition(this).X, e.GetPosition(this).Y);
            trans.Matrix = matr;
        }

        public void CenterCanvas()
        {
            Point center = new Point(App.mainWindow.ActualWidth / 2, App.mainWindow.ActualHeight / 2);
            trans.Matrix = new Matrix(1, 0, 0, 1, center.X, center.Y);
        }
        #endregion
    }
}
