using CsvHelper;
using CsvHelper.Configuration;
using IBA_TestTask;
using Microsoft.Extensions.Options;
using System.Globalization;

namespace IBA_TestTask_API.Managers
{
    public class FileDataManager : ISourceDataManager
    {
        private readonly AppSettings _appSettings;
        private readonly CsvConfiguration _csvConfig;

        public FileDataManager(IOptionsSnapshot<AppSettings> options)
        {
            _appSettings = options.Value;
            if (!File.Exists(_appSettings.FilePath))
            {
                File.Create(_appSettings.FilePath).Close();
            }

            _csvConfig = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                Delimiter = ";",
            };
        }


        public void AddDataRange(IList<ControlSystemData> newData)
        {
            var isEditMode = IsEditMode();
            if (isEditMode)
            {
                _csvConfig.HasHeaderRecord = false;
            }

            using var writer = new StreamWriter(_appSettings.FilePath, true);
            using var csv = new CsvWriter(writer, _csvConfig);
            csv.Context.RegisterClassMap<ControlSystemDataMap>();
            csv.WriteRecords(newData);
            csv.Flush();
        }

        public IList<ControlSystemData> GetOutspeedData(DateTime date, double speed)
        {
            using var reader = new StreamReader(_appSettings.FilePath);
            using var csv = new CsvReader(reader, _csvConfig);
            var result = csv.GetRecords<ControlSystemData>();
            return result.Where(d => d.DateTime.Value.Date == date.Date && d.VehicleSpeed > speed).ToList();
        }

        public ControlSystemData GetMaxSpeedData(DateTime date)
        {
            using var reader = new StreamReader(_appSettings.FilePath);
            using var csv = new CsvReader(reader, _csvConfig);
            var result = csv.GetRecords<ControlSystemData>();
            return result.Where(p => p.DateTime.Value.Date == date.Date).OrderByDescending(p => p.VehicleSpeed).FirstOrDefault();
        }

        public ControlSystemData GetMinSpeedData(DateTime date)
        {
            using var reader = new StreamReader(_appSettings.FilePath);
            using var csv = new CsvReader(reader, _csvConfig);
            var result = csv.GetRecords<ControlSystemData>();
            return result.Where(p => p.DateTime.Value.Date == date.Date).OrderByDescending(p => p.VehicleSpeed).LastOrDefault();
        }

        public IList<ControlSystemData> GetAllData()
        {
            using var reader = new StreamReader(_appSettings.FilePath);
            using var csv = new CsvReader(reader, _csvConfig);
            csv.Context.RegisterClassMap<ControlSystemDataMap>();
            return csv.GetRecords<ControlSystemData>().ToList();
        }

        private bool IsEditMode()
        {
            using var reader = new StreamReader(_appSettings.FilePath);
            using var csv = new CsvReader(reader, _csvConfig);
            return csv.Read();
        }
    }

    public class ControlSystemDataMap : ClassMap<ControlSystemData>
    {
        public ControlSystemDataMap()
        {
            Map(m => m.DateTime).Index(0).Name("DateTime");
            Map(m => m.VehicleIDNumber).Index(1).Name("VehicleIDNumber");
            Map(m => m.VehicleSpeed).Index(2).Name("VehicleSpeed");
        }
    }
}
