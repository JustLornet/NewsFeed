using System.Reflection;

namespace NewsFeedEngine.Service
{
    internal static class Config
    {
        /// <summary>
        /// Извлечение строки подключения из файла appsettings
        /// </summary>
        /// <returns>Строка подключения к БД</returns>
        internal static string GetConnectionString(string connectionStringName = "DefaultConnection")
        {
            var builder = new ConfigurationBuilder();
            // установка пути к текущему каталогу
            var cuurentFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            builder.SetBasePath(cuurentFolder);
            // получаем конфигурацию из файла appsettings.json
            builder.AddJsonFile("appsettings.json");
            // создаем конфигурацию
            var config = builder.Build();

            if(config is null) throw new NullReferenceException($"Неудачное создание конфигурации. Не уадлось прочитать appsettings.json");

            // получаем строку подключения
            string connectionString = config.GetConnectionString(connectionStringName)!;

            if (connectionString is null) throw new KeyNotFoundException($"В файле конфигурации (appsettings.json) не была найдена строка подключения \"{connectionStringName}\"");

            return connectionString;
        }
    }
}
