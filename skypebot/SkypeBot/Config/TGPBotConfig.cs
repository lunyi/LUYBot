using UGS.Infrastructures.JsonConfig;

namespace SkypeBot.Config
{
    public interface ITGPBotConfig
    {
        string AppId { get; }
        string AppPassword { get; }
        string SkypeServiceUrl { get;  }
        string SystemApiUrl { get; }
    }

    public sealed class TGPBotConfig : BaseJsonConfig<TGPBotConfigData>, ITGPBotConfig
    {
        public TGPBotConfig() 
            : base("TGPBotService")
        {

        }

        string ITGPBotConfig.AppId => Config.app_id;
        string ITGPBotConfig.AppPassword => Config.app_password;
        string ITGPBotConfig.SkypeServiceUrl => Config.skype_service_url;
        string ITGPBotConfig.SystemApiUrl => Config.system_api_url;
    }

    public class TGPBotConfigData
    {
        public string app_id { get; set; }
        public string app_password { get; set; }
        public string skype_service_url { get; set; }
        public string system_api_url { get; set; }
    }
}
