using Newtonsoft.Json;
using System;

namespace CrudBaseServices.CrudAPIModels.Output
{
    public class ErrorStatusMessage : BaseStatusMessage
    {
        public int Level { get; set; } 
        public string Code { get; set; }
        public object Data { get; set; } 
        public ErrorStatusMessage Inner { get; set; }

        public ErrorStatusMessage(int Level = 0, string Code = null, string Message = null, object Data = null, ErrorStatusMessage Inner = null)
        {
            this.Level = Level;
            this.Code = Code;
            this.Message = Message;
            this.Data = Data;
            this.Inner = Inner;
        }

        public ErrorStatusMessage(ErrorStatusMessage Msg, ErrorStatusMessage Inner = null, object Data = null)
        {
            this.Level = Msg.Level;
            this.Inner = Inner;
            this.Data = Data;
            this.Code = (Msg == null) ? null : Msg.Code;
            this.Message = (Msg == null) ? null : Msg.Message;
        }

        public ErrorStatusMessage(Exception ex)
        {
            this.Level = 0;
            this.Inner = (ex.InnerException == null) ? null : new ErrorStatusMessage(ex.InnerException);
            this.Data = (ex.Data != null && ex.Data.Count > 0) ? ex.Data : null;
            this.Code = "InternalException";
            this.Message = ex.Message;
        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
