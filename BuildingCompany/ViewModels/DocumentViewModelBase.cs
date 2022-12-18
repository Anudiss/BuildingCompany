using BuildingCompany.Utilities;
using System;

namespace BuildingCompany.ViewModels
{
    public abstract class DocumentViewModelBase : WindowViewModelBase
    {
        protected RelayCommand _conductDocumentCommand;

        public RelayCommand ConductDocumentCommand =>
            _conductDocumentCommand ?? (_conductDocumentCommand = new RelayCommand(arg => ConductDocument()));

        public abstract int ID { get; }
        public abstract DateTime Date { get; }
        public static string DocumentName { get; private set; }

        public DocumentViewModelBase(string documentName) =>
            DocumentName = documentName;

        public abstract void ConductDocument();
    }
}
