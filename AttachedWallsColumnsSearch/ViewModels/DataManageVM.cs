using Prism.Mvvm;
using Autodesk.Revit.DB;
using AttachedWallsColumnsSearch.Models;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;
using System.Windows.Data;
using System;

namespace AttachedWallsColumnsSearch.ViewModels
{
    public class DataManageVM : BindableBase
    {
        private Document _doc;
        private List<AttachedElement> _attachedElements = new List<AttachedElement>();

        public ICollectionView AttachedElements { get; set; }
        public string WallsTotalCountText { get; set; }
        public string ColumnsTotalCountText { get; set; }
        public string ElementsTotalCountText { get; set; }

        public Action CloseAction { get; set; }

        public DataManageVM(Document doc)
        {
            _doc = doc;

            _attachedElements = GetAllAttachedElements(_doc);
            AttachedElements = CollectionViewSource.GetDefaultView(_attachedElements);
        }

        #region METHODS
        private List<Element> GetAllAttachedWalls(Document doc)
        {
            return new FilteredElementCollector(doc)
                .OfCategory(BuiltInCategory.OST_Walls)
                .WhereElementIsNotElementType()
                .ToElements()
                .Where(element => element.get_Parameter(BuiltInParameter.WALL_TOP_IS_ATTACHED)?.AsInteger() == 1 ||
                                  element.get_Parameter(BuiltInParameter.WALL_BOTTOM_IS_ATTACHED)?.AsInteger() == 1)
                .ToList();
        }

        private List<Element> GetAllAttachedStructuralColumns(Document doc)
        {
            return new FilteredElementCollector(doc)
                .OfCategory(BuiltInCategory.OST_StructuralColumns)
                .WhereElementIsNotElementType()
                .ToElements()
                .Where(element => element.get_Parameter(BuiltInParameter.COLUMN_TOP_ATTACHED_PARAM)?.AsInteger() == 1 ||
                                  element.get_Parameter(BuiltInParameter.COLUMN_BASE_ATTACHED_PARAM)?.AsInteger() == 1)
                .ToList();
        }

        private List<AttachedElement> GetAllAttachedElements(Document doc)
        {
            var attachedElements = new List<AttachedElement>();
            var allAttachedWalls = GetAllAttachedWalls(doc);
            var allAttachedStructuralColumns = GetAllAttachedStructuralColumns(doc);

            foreach (var attachedWall in allAttachedWalls)
            {
                var id = attachedWall.Id.IntegerValue;
                var typeName = _doc.GetElement(attachedWall.GetTypeId()).Name;
                var isBottomAttached = (attachedWall as Wall).get_Parameter(BuiltInParameter.WALL_BOTTOM_IS_ATTACHED)?.AsInteger() == 0? false : true;
                var isTopAttached = (attachedWall as Wall).get_Parameter(BuiltInParameter.WALL_TOP_IS_ATTACHED)?.AsInteger() == 0 ? false : true;

                attachedElements.Add(new AttachedElement(id, typeName, isBottomAttached, isTopAttached));
            }

            foreach (var attachedStructuralColumns in allAttachedStructuralColumns)
            {
                var id = attachedStructuralColumns.Id.IntegerValue;
                var typeName = _doc.GetElement(attachedStructuralColumns.GetTypeId()).Name;
                var isBottomAttached = (attachedStructuralColumns).get_Parameter(BuiltInParameter.COLUMN_BASE_ATTACHED_PARAM)?.AsInteger() == 0 ? false : true;
                var isTopAttached = (attachedStructuralColumns).get_Parameter(BuiltInParameter.COLUMN_TOP_ATTACHED_PARAM)?.AsInteger() == 0 ? false : true;

                attachedElements.Add(new AttachedElement(id, typeName, isBottomAttached, isTopAttached));
            }

            WallsTotalCountText = $"Всего стен: {allAttachedWalls.Count}";
            ColumnsTotalCountText = $"Всего колонн: {allAttachedStructuralColumns.Count}";
            ElementsTotalCountText = $"Всего элементов: {attachedElements.Count}";

            return attachedElements;
        }

        public bool IsDataEmpty()
        {
            if (_attachedElements.Count == 0)
            {
                return true;
            }

            return false;
        }

        public void Close(object sender, EventArgs e)
        {
            AttachedWallsColumnsSearchApp.IsOpened = false;
            CloseAction();
        }
        #endregion
    }
}