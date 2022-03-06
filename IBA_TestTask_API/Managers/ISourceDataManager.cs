using IBA_TestTask;

namespace IBA_TestTask_API.Managers
{
    public interface ISourceDataManager
    {
        void AddDataRange(IList<ControlSystemData> data);

        IList<ControlSystemData> GetOutspeedData(DateTime date, double speed);

        ControlSystemData GetMaxSpeedData(DateTime date);

        ControlSystemData GetMinSpeedData(DateTime date);

        IList<ControlSystemData> GetAllData();
    }
}
