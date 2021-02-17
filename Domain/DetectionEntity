using System;
using System.Collections.Generic;

namespace Api_face_recognition.Domain
{
    public class DetectionEntity
    {
        public string id { get; set; }
        public string project { get; set; }
        public string iteration { get; set; }
        public string created { get; set; }
        public List<PredictionsEntity>  predictions { get; set; }
    }

    public class PredictionsEntity
    {
        public Double probability { get; set; }
        public string tagId { get; set; }
        public string tagName { get; set; }
    }
}
