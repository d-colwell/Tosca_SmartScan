using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tricentis.CrowdIQ.Controls.ViewModels
{
    public class CustomisationViewModel : ViewModelBase

    {
        private Customisation _customisation;

        public CustomisationViewModel() { }
        public CustomisationViewModel(Customisation customisation)
        {
            _customisation = customisation;
        }

        public string Name { get { return _customisation.Name; } }
        public bool Download
        {
            get { return _customisation.Download; }
            set
            {
                if (_customisation.Download == value)
                    return;
                _customisation.Download = value;
                NotifyPropertyChanged(nameof(Download));
            }
        }
    }
}
