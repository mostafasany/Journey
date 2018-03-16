using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abstractions.Models;
using Abstractions.Services.Contracts;
using Journey.Resources;
using Prism.Commands;
using Prism.Navigation;
using Unity;

namespace Journey.ViewModels
{
    public class ChooseLocationPageViewModel : BaseViewModel, INavigationAware
    {
        private readonly IFacebookService _facebookService;
        private readonly ILocationService _locationService;
        private readonly ISettingsService _settingsService;
        private const string DefaultLocation = "DefaultLocation";

        public ChooseLocationPageViewModel(IUnityContainer container, ILocationService locationService,
            IFacebookService facebookService, ISettingsService settingsService) :
            base(container)
        {
            _locationService = locationService;
            _facebookService = facebookService;
            _settingsService = settingsService;
        }

        #region Methods

        public override async void Intialize(bool sync = false)
        {
            try
            {
                ShowProgress();
                await Task.Delay(1000);
                Location position = await _locationService.ObtainMyLocationAsync();
                if (position != null)
                    Locations = await _facebookService.GetLocationsAsync(Name, position.Lat, position.Lng);

                SelectedLocation = null;
                base.Intialize(sync);
            }
            catch (Exception e)
            {
                ExceptionService.HandleAndShowDialog(e);
            }
            finally
            {
                HideProgress();
            }
        }

        #endregion

        #region Events

        public void OnNavigatedFrom(NavigationParameters parameters)
        {
        }

        public void OnNavigatedTo(NavigationParameters parameters)
        {
            try
            {
                Intialize();
            }
            catch (Exception ex)
            {
                ExceptionService.HandleAndShowDialog(ex);
            }
        }

        public void OnNavigatingTo(NavigationParameters parameters)
        {
        }

        #endregion

        #region Properties

        private string _name="FitX";

        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                RaisePropertyChanged();
            }
        }

        private List<Location> _locations;

        public List<Location> Locations
        {
            get => _locations;
            set
            {
                _locations = value;
                RaisePropertyChanged();
            }
        }

        private Location _selectedLocation;

        public Location SelectedLocation
        {
            get => _selectedLocation;
            set
            {
                if (_selectedLocation == value)
                    return;
                _selectedLocation = value;
                RaisePropertyChanged();
                if (value != null)
                    OnSelectedLocationCommand.Execute(value);
            }
        }

        #endregion

        #region Commands

        #region OnSelectedLocationCommand

        public DelegateCommand<Location> OnSelectedLocationCommand => new DelegateCommand<Location>(OnSelectedLocation);

        private async void OnSelectedLocation(Location selectedLocation)
        {
            var dafaultLocationCommand = new DialogCommand
            {
                Label = AppResource.Yes,
                Invoked = async () =>
                {
                    string locationId = await _settingsService.Get(DefaultLocation);
                    if (locationId != selectedLocation.Id)
                        await _settingsService.Set(DefaultLocation, selectedLocation.Id);
                    NavigationService.GoBack(selectedLocation, "Location");
                }
            };

            var cancelCommand = new DialogCommand
            {
                Label = AppResource.No
            };

            var commands = new List<DialogCommand>
            {
                dafaultLocationCommand,
                cancelCommand
            };
            await DialogService.ShowMessageAsync("", AppResource.Location_DefaultLocationTitle, commands);
        }

        #endregion

        #region OnSearchCommand

        //public DelegateCommand OnSearchCommand => new DelegateCommand(OnSearch);

        //private async void OnSearch()
        //{
        //    try
        //    {
        //        IsBusy = true;
        //        // var position = await locationService.ObtainMyLocationAsync();
        //        if (string.IsNullOrEmpty(Name))
        //            Locations = originalLocations;
        //        else
        //            Locations = originalLocations.Where(a => a.Name.IndexOf(Name,0,System.StringComparison.OrdinalIgnoreCase)>0).ToList();
        //    }
        //    catch (System.Exception ex)
        //    {
        //        ExceptionService.Handle(ex);
        //    }
        //    finally
        //    {
        //        IsBusy = false;
        //    }
        //}

        #endregion

        #region OnCloseCommand

        public DelegateCommand OnCloseCommand => new DelegateCommand(OnClose);

        private void OnClose()
        {
            NavigationService.GoBack();
        }

        #endregion

        #endregion
    }
}