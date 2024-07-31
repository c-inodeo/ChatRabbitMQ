namespace ChatRabbitMQ.Model
{
    public class MessageModel
    {
        public string Sender { get; set; }
        public string Content { get; set; }
        public  DateTime TimeStamp { get; set; }

        public MessageModel() { }

        public MessageModel(string sender, string content) 
        { 
            Sender = sender;
            Content = content;
            TimeStamp = DateTime.Now;
        }
    }
}
