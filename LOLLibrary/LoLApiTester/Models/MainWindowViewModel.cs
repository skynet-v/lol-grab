using System;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Input;
using LOLApiAdapter.ApiHandlers;
using LOLApiAdapter.CommonDefinitions;
using LOLApiAdapter.CommonDefinitions.Enums;
using LOLApiAdapter.CommonDefinitions.Interfaces;
using MVVMModelLib;
using Newtonsoft.Json.Linq;
using PronOperatorUtility;
using XbetUtils;

namespace LoLApiTester.Models
{
    public class MainWindowViewModel : ViewModelBase
    {
        private string _apiKey = "1e2d01ec-f595-4e0e-b2c2-09b74ce3cc71";
        public string ApiKey
        {
            get => _apiKey;
            set
            {
                if (_apiKey == value)
                    return;

                _apiKey = value;
                OnPropertyChanged(nameof(ApiKey));
            }
        }

        private string _argument = "202900012";
        public string Argument
        {
            get => _argument;
            set
            {
                if (_argument == value)
                    return;

                _argument = value;
                OnPropertyChanged(nameof(Argument));
            }
        }

        private LolApiServerRegion _selectedRegion;
        public LolApiServerRegion SelectedRegion
        {
            get => _selectedRegion;
            set
            {
                if (_selectedRegion == value)
                    return;

                _selectedRegion = value;
                OnPropertyChanged(nameof(SelectedRegion));
                OnPropertyChanged(nameof(CurApiMethods));
                OnPropertyChanged(nameof(SelectedApiType));
                UpdateApiHandler();
            }
        }

        private LoLApiDataHandlerBase _handler;
        public LoLApiDataHandlerBase Handler
        {
            get => _handler;
            set
            {
                if (_handler == value)
                    return;
                _handler = value;
                OnPropertyChanged(nameof(Handler));
                OnPropertyChanged(nameof(CurApiMethods));
            }
        }

        public MethodInfo[] CurApiMethods => Handler?.GetType().GetMethods();
        public MethodInfo SelectedApiMethod { get; set; }

        public string RegionString => SelectedRegion.ToString().ToLower();

        private JToken _json;
        public JToken Json
        {
            get => _json;
            set
            {
                if (_json == value)
                    return;

                _json = value;
                OnPropertyChanged(nameof(Json));
            }
        }

        public ICommand ExecuteRequestCommand { get; set; }

        private LoLApiType _selectedApiType;
        public LoLApiType SelectedApiType
        {
            get => _selectedApiType;
            set
            {
                if (_selectedApiType == value)
                    return;
                _selectedApiType = value;

                OnPropertyChanged(nameof(SelectedApiType));
                UpdateApiHandler();
            }
        }

        private void UpdateApiHandler()
        {
            Handler = GetApiHandler(SelectedApiType, ApiKey, RegionString);
        }

        private Type[] functors = new[]
        {
            typeof(Func<ILoLResponse>),
            typeof(Func<long, ILoLResponse>),
            typeof(Func<long, long, ILoLResponse>),
            typeof(Func<string, ILoLResponse>),
            typeof(Func<string, Task<ILoLResponse>>),
        };

        public decimal Min(decimal a, decimal b) => (a >= b) ? b : a;
        public decimal Max(decimal a, decimal b) => (a >= b) ? a : b;

        private LoLApiDataHandlerBase GetApiHandler(LoLApiType type, string apiKey, string region)
        {
            switch (type)
            {
                case LoLApiType.SUMMONER_V3:
                    return new SummonerDataHandler(apiKey, region);
                case LoLApiType.MATCH_V3:
                    return new MatchDetailsDataHandler(apiKey, region);
                case LoLApiType.SPECTATOR_V3:
                    return new SpectatorDataHandler(apiKey, region);
                case LoLApiType.CHAMPION_V3:
                    return new ChampionsDataHandler(apiKey, region);
                case LoLApiType.STATIC_DATA_V3:
                    return new StaticApiDataHandler(apiKey, region);
                case LoLApiType.STATUS_V3:
                    return new ApiStatusDataHandler(apiKey, region);
                default:
                    return null;
            }
        }

        public MainWindowViewModel()
        {
            ExecuteRequestCommand = new RelayCommand(_ =>
            {
                ExecuteRequest();
            });
        }

        private void ExecuteRequest()
        {
            var arguments = new object[]
            {
                Convert.ToInt64(string.IsNullOrEmpty(Argument) ? "0" : Argument),
            };

            foreach (var functor in functors)
            {
                try
                {
                    var del = Delegate.CreateDelegate(functor, Handler, SelectedApiMethod.Name);
                    ILoLResponse response;
                    if (del.Method.GetParameters().Length >= 1)
                        response = del.DynamicInvoke(arguments) as ILoLResponse;
                    else
                        response = del.DynamicInvoke() as ILoLResponse;
                    if (response != null)
                    {
                        Json = JToken.FromObject(response);
                    }

                    break;
                }
                catch (WebException wex)
                {

                }
                catch (Exception e)
                {

                }

                Json = null;
            }
        }
    }
}
