using System;
using System.Collections.Generic;
using System.Data;
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

namespace DataVisualizer.Desktop.Controls
{
    /// <summary>
    /// Interaction logic for TableColumnSelectorControl.xaml
    /// </summary>
    public partial class TableColumnSelectorControl : UserControl
    {
        private bool[] _selectedColumns;
        private Dictionary<Rectangle, bool> _rectangles = new Dictionary<Rectangle, bool>();

        //TODO :: Move to resources
        private Brush _columnNormal = new SolidColorBrush() { Opacity = 0.0 };
        private Brush _columnHover = new SolidColorBrush(Colors.CadetBlue) { Opacity = 0.3 };
        private Brush _columnSelected = new SolidColorBrush(Colors.CadetBlue) { Opacity = 0.4 };

        #region Bindables
        public readonly DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register("ItemsSource", typeof(DataTable), typeof(TableColumnSelectorControl),
                new FrameworkPropertyMetadata(new DataTable(), OnItemsSourceChanged));

        public DataTable ItemsSource
        {
            get => (DataTable)GetValue(ItemsSourceProperty);
            set => SetValue(ItemsSourceProperty, value);
        }

        public readonly DependencyProperty AllowMultipleProperty =
            DependencyProperty.Register("AllowMultiple", typeof(bool), typeof(TableColumnSelectorControl),
                new FrameworkPropertyMetadata(false));

        public bool AllowMultiple
        {
            get => (bool)GetValue(AllowMultipleProperty);
            set => SetValue(AllowMultipleProperty, value);
        }
        #endregion

        public TableColumnSelectorControl()
        {
            InitializeComponent();
        }

        private void DrawGrid(DataTable table)
        {
            foreach (var column in table.Columns)
            {
                grdMain.ColumnDefinitions.Add(new ColumnDefinition());
            }
            int rowNumber = 0;
            foreach (DataRow row in table.Rows)
            {
                grdMain.RowDefinitions.Add(new RowDefinition());
                for (int columnNumber = 0; columnNumber < table.Columns.Count; columnNumber++)
                {
                    var cellText = new TextBlock()
                    {
                        Text = row[columnNumber].ToString(),
                    };
                    grdMain.Children.Add(cellText);
                    cellText.SetValue(Grid.RowProperty, rowNumber);
                    cellText.SetValue(Grid.ColumnProperty, columnNumber);
                }
                rowNumber++;
            }
            for (int colNumber = 0; colNumber < grdMain.ColumnDefinitions.Count; colNumber++)
            {
                var rect = GetColumnRectangle(colNumber, rowNumber);
                grdMain.Children.Add(rect);
                _rectangles.Add(rect, false);
            }
        }

        private Rectangle GetColumnRectangle(int colNumber, int rowsNumber)
        {
            Rectangle rect = new Rectangle();
            rect.Fill = _columnNormal;
            rect.SetValue(Grid.ColumnProperty, colNumber);
            rect.SetValue(Grid.RowSpanProperty, rowsNumber);
            rect.SetValue(Grid.ZIndexProperty, 10);


            //Subscribe to events
            rect.MouseEnter += OnColumnMouseEnter;
            rect.MouseLeave += OnColumnMouseLeave;
            rect.MouseDown += OnColumnSelected;
            return rect;
        }

        private static void OnItemsSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DataTable newTable = (DataTable)e.NewValue;
            TableColumnSelectorControl grdMain = (TableColumnSelectorControl)d;
            grdMain.DrawGrid(newTable);
        }

        private void OnColumnSelected(object sender, MouseButtonEventArgs args)
        {
            var clickedRectangle = ((Rectangle)sender);

            if (_rectangles[clickedRectangle])
            {
                _rectangles[clickedRectangle] = false;
                clickedRectangle.Fill = _columnNormal;
            }
            else
            {
                if (AllowMultiple)
                {
                    _rectangles[clickedRectangle] = true;
                    clickedRectangle.Fill = _columnSelected;
                }
                else
                {
                    var highlightedRectangles = _rectangles.Where(x => x.Value == true)
                        .Select(x => x.Key);
                    foreach (Rectangle rect in highlightedRectangles.ToList())
                    {
                        _rectangles[rect] = false;
                        rect.Fill = _columnNormal;
                    }
                    _rectangles[clickedRectangle] = true;
                    clickedRectangle.Fill = _columnSelected;
                }
            }
        }

        private void OnColumnMouseLeave(object sender, MouseEventArgs args)
        {
            if (!IsColumnSelected((Rectangle)sender)) 
                ((Rectangle)sender).Fill = _columnNormal;
        }

        private void OnColumnMouseEnter(object sender, MouseEventArgs args)
        {
            if (!IsColumnSelected((Rectangle)sender))
                ((Rectangle)sender).Fill = _columnHover;
        }

        private bool IsColumnSelected(Rectangle rect)
        {
            if (_rectangles.ContainsKey(rect))
            {
                if (_rectangles[rect])
                    return true;
            }
            return false;
        }
    }
}
