using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;
using TRMDesktopUI.EventModels;

namespace TRMDesktopUI.ViewModels
{
    public class ShellViewModel : Conductor<object>,IHandle<LoggOnEvent>
    {

        private IEventAggregator _events;
        private SalesViewModel _salevVM;

        public ShellViewModel(IEventAggregator events,SalesViewModel salevVM,SimpleContainer container)
        {
            _salevVM = salevVM;
            _events = events;

            _events.Subscribe(this);

            ActivateItem(IoC.Get<LoginViewModel>());
        }

        public void Handle(LoggOnEvent message)
        {
            ActivateItem(_salevVM);
        }
    }
}
