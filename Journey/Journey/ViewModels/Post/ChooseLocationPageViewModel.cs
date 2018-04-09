using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abstractions.Models;
using Abstractions.Services.Contracts;
using Prism.Commands;
using Prism.Navigation;
using Unity;

namespace Journey.ViewModels
{
    public class ChooseLocationPageViewModel : BaseViewModel, INavigationAware
    {
        private readonly IFacebookService _facebookService;
        private readonly ILocationService _locationService;


        public ChooseLocationPageViewModel(IUnityContainer container, ILocationService locationService,
            IFacebookService facebookService) :
            base(container)
        {
            _locationService = locationService;
            _facebookService = facebookService;
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
                    Locations = await _facebookService.GetLocationsAsync(SearchKeyword, position.Lat, position.Lng, null);

                RaisePropertyChanged(nameof(NoLocations));

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

        private string _searchKeyword;

        public string SearchKeyword
        {
            get => _searchKeyword;
            set => SetProperty(ref _searchKeyword, value);
        }

        private List<Location> _locations;

        public List<Location> Locations
        {
            get => _locations;
            set => SetProperty(ref _locations, value);
        }

        public bool NoLocations
        {
            get => Locations == null || Locations.Count == 0;
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
            NavigationService.GoBack(selectedLocation, "Location");
        }

        #endregion

        #region OnSearchCommand

        public DelegateCommand OnSearchCommand => new DelegateCommand(OnSearch);

        private async void OnSearch()
        {
            try
            {
                Intialize();
            }
            catch (Exception ex)
            {
                ExceptionService.Handle(ex);
            }
        }

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