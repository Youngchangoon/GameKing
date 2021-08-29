using CloudStructures.Converters;
using MessagePack;

namespace GameKing.Server
{
    public class MessagePackConverter : IValueConverter
    {
        private MessagePackSerializerOptions Options { get; }

        public MessagePackConverter(MessagePackSerializerOptions options)
            => Options = options;

        public byte[] Serialize<T>(T value)
            => MessagePackSerializer.Serialize(value, Options);

        public T Deserialize<T>(byte[] value)
            => MessagePackSerializer.Deserialize<T>(value, Options);
    }
}