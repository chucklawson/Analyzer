namespace Analyzer
{
    public class WebSocketSimpleMovingAverageMessage
    {
        private string messageType;
        private string date;
        private string dataPointForSimpleMovingAverage;

        public WebSocketSimpleMovingAverageMessage(string dataPointForSimpleMovingAverageIn, string messageTypeIn)
        {
            messageType = messageTypeIn;

            dataPointForSimpleMovingAverage = dataPointForSimpleMovingAverageIn;
        }

        public string getMessageType()
        {
            return messageType;
        }

        public void setDate(string dateIn)
        {
            date = dateIn;
        }

        public string getDate()
        {
            return date;
        }

        public void setMessageType(string messageTypeIn)
        {
            messageType = messageTypeIn;
        }

        public string getDataPointForSimpleMovingAverage()
        {
            return dataPointForSimpleMovingAverage;
        }

        public void setDataPointForSimpleMovingAverage(string dataPointForSimpleMovingAverageIn)
        {
            dataPointForSimpleMovingAverage = dataPointForSimpleMovingAverageIn;
        }

        /* the string Implementation */
    public string ToString()
        {
            return "WebSocketSimpleMovingAverageMessage: \n" + "messageType: " + this.getMessageType() + "date: "
                    + this.getDate() + ", dataPointForSimpleMovingAverage" + this.getDataPointForSimpleMovingAverage();
        }
    }
}
