using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Journey.Models.Post;

namespace Journey.Services.Buisness.Measurment
{
    public interface IAccountMeasurmentService
    {
        //event ScaleMeasurmentsChangedEventHandler ScaleMeasurmentsChangedHandler;

        Task<List<ScaleMeasurment>> GetMeasurmentsAsync(bool sync = false);

        Task<List<ScaleMeasurment>> UpdateScaleMeasurments(List<ScaleMeasurment> measurments);
    }

    //public delegate void ScaleMeasurmentsChangedEventHandler(object sender, ScaleMeasurmentsChangedArgs e);

    //public class ScaleMeasurmentsChangedArgs : EventArgs
    //{
    //    public List<ScaleMeasurment> Measuremnts { get; set; }
    //}
}