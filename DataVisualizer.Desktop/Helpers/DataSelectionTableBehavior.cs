using SciChart.Data.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Interactivity;
using System.Windows.Media;

namespace DataVisualizer.Desktop.Helpers
{
    public class DataSelectionTableBehavior : Behavior<UIElement>
    {
        private DataGrid dataGrid;
        private List<int> _selectedColumns = new List<int>();

        public bool AllowMultipleSelection
        {
            get { return (bool)GetValue(AllowMultipleSelectionProperty); }
            set { SetValue(AllowMultipleSelectionProperty, value); }
        }

        public static readonly DependencyProperty AllowMultipleSelectionProperty =
            DependencyProperty.Register("AllowMultipleSelection", typeof(bool), typeof(DataGrid));

        protected override void OnAttached()
        {
            base.OnAttached();

            dataGrid = (DataGrid)this.AssociatedObject;
            dataGrid.PreviewMouseLeftButtonDown += AssociatedObject_MouseLeftButtonDown;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            this.AssociatedObject.MouseLeftButtonDown -= AssociatedObject_MouseLeftButtonDown;
        }

        private void AssociatedObject_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            DependencyObject dep = (DependencyObject)e.OriginalSource;

            // iteratively traverse the visual tree
            while ((dep != null) &&
            !(dep is DataGridCell) &&
            !(dep is DataGridColumnHeader))
            {
                dep = VisualTreeHelper.GetParent(dep);
            }

            if (dep == null)
                return;

            if (dep is DataGridCell)
            {
                DataGridCell cell = (DataGridCell)dep;
                int clickedColumn = cell.Column.DisplayIndex;

                if (_selectedColumns.Contains(clickedColumn))
                    _selectedColumns.Remove(clickedColumn);
                else
                {
                    if (!AllowMultipleSelection)
                        _selectedColumns.Clear();
                    _selectedColumns.Add(clickedColumn);
                }
                SelectColumns();
            }
        }

        private void SelectColumns()
        {
            dataGrid.SelectedCells.Clear();
            
            foreach(DataRowView row in dataGrid.Items)
            {
                foreach (int column in _selectedColumns)
                {
                    //DataGridCellInfo a = row.Row.ItemArray[0];
                    //dataGrid.SelectedCells.Add(new DataGridCellInfo(GetCell(dataGrid, row, column)));
                }
            }
        }

        public static T FindVisualChild<T>(DependencyObject obj) where T : DependencyObject
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(obj, i);
                if (child != null && child is T)
                    return (T)child;
                else
                {
                    T childOfChild = FindVisualChild<T>(child);
                    if (childOfChild != null)
                        return childOfChild;
                }
            }
            return null;
        }

        public static DataGridCell GetCell(DataGrid dataGrid, DataGridRow rowContainer, int column)
        {
            if (rowContainer != null)
            {
                DataGridCellsPresenter presenter = FindVisualChild<DataGridCellsPresenter>(rowContainer);
                if (presenter == null)
                {
                    /* if the row has been virtualized away, call its ApplyTemplate() method
                     * to build its visual tree in order for the DataGridCellsPresenter
                     * and the DataGridCells to be created */
                    rowContainer.ApplyTemplate();
                    presenter = FindVisualChild<DataGridCellsPresenter>(rowContainer);
                }
                if (presenter != null)
                {
                    DataGridCell cell = presenter.ItemContainerGenerator.ContainerFromIndex(column) as DataGridCell;
                    if (cell == null)
                    {
                        /* bring the column into view
                         * in case it has been virtualized away */
                        dataGrid.ScrollIntoView(rowContainer, dataGrid.Columns[column]);
                        cell = presenter.ItemContainerGenerator.ContainerFromIndex(column) as DataGridCell;
                    }
                    return cell;
                }
            }
            return null;
        }

        public static void SelectCellsByIndexes(DataGrid dataGrid, IList<KeyValuePair<int, int>> cellIndexes)
        {
            if (!dataGrid.SelectionUnit.Equals(DataGridSelectionUnit.Cell))
                throw new ArgumentException("The SelectionUnit of the DataGrid must be set to Cell.");

            if (!dataGrid.SelectionMode.Equals(DataGridSelectionMode.Extended))
                throw new ArgumentException("The SelectionMode of the DataGrid must be set to Extended.");

            dataGrid.SelectedCells.Clear();
            foreach (KeyValuePair<int, int> cellIndex in cellIndexes)
            {
                int rowIndex = cellIndex.Key;
                int columnIndex = cellIndex.Value;

                if (rowIndex < 0 || rowIndex > (dataGrid.Items.Count - 1))
                    throw new ArgumentException(string.Format("{0} is an invalid row index.", rowIndex));

                if (columnIndex < 0 || columnIndex > (dataGrid.Columns.Count - 1))
                    throw new ArgumentException(string.Format("{0} is an invalid column index.", columnIndex));

                object item = dataGrid.Items[rowIndex]; //= Product X
                DataGridRow row = dataGrid.ItemContainerGenerator.ContainerFromIndex(rowIndex) as DataGridRow;
                if (row == null)
                {
                    dataGrid.ScrollIntoView(item);
                    row = dataGrid.ItemContainerGenerator.ContainerFromIndex(rowIndex) as DataGridRow;
                }
                if (row != null)
                {
                    DataGridCell cell = GetCell(dataGrid, row, columnIndex);
                    if (cell != null)
                    {
                        DataGridCellInfo dataGridCellInfo = new DataGridCellInfo(cell);
                        dataGrid.SelectedCells.Add(dataGridCellInfo);
                        cell.Focus();
                    }
                }
            }
        }
    }
}

