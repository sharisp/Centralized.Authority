using Microsoft.Extensions.Configuration;

namespace Identity.Infrastructure
{

    public class AppHelper
    {
        private static IConfiguration _config;

    
        public static void Init(IConfiguration configuration)
        {
            _config = configuration;
        }

        public static string ReadAppSetting(string sessions)
        {
            try
            {
                return _config[sessions];

            }
            catch
            {
                return "";
            }
        }
        /// <summary>
        /// 读取指定节点的字符串
        /// </summary>
        /// <param name="sessions"></param>
        /// <returns></returns>
        public static string ReadAppSettings(params string[] sessions)
        {
            try
            {
                if (sessions.Any())
                {
                    return _config[string.Join(":", sessions)];
                }
            }
            catch
            {
                return "";
            }
            return "";
        }
        public static List<IConfigurationSection> ReadAppSettingsSection(params string[] sessions)
        {
            try
            {
                if (sessions.Any())
                {
                    return _config.GetSection(string.Join(":", sessions)).GetChildren().ToList();
                }
            }
            catch
            {
                return null;
            }
            return null;
        }

        //T可以传递对象，也可以传递List<string> string[]
        public static T ReadAppSettingsSection<T>(params string[] sessions)
        {
            try
            {
                if (sessions.Any())
                {
                    return _config.GetSection(string.Join(":", sessions)).Get<T>();
                }
            }
            catch
            {
                return default;
            }
            return default;
        }


    }
}
