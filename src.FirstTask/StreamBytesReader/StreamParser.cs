using System.Text;

namespace StreamBytesReader
{
    public sealed class StreamParser : IStreamParser
    {
        private Stream _stream;
        private readonly StringBuilder _sb;

        private int _cursorPosition = 0;

        private string _msgEndSymb = Constants.DefaultMsgEndSymbol;
        private int _bufferSize = Constants.DefaultBufferSize;

        private readonly long _streamLength;

        public StreamParser(Stream stream)
        {
            if(stream is null) throw new ArgumentNullException(nameof(stream));
            if (!stream.CanRead) throw new ArgumentException("Переданный объект потока невозможно прочесть");

            _stream = stream;
            _sb = new();

            // вынесено в отдельную переменную, так как после освобождения потока невозможно будет определить длину потока
            _streamLength = _stream.Length;
        }

        public StreamParser(Stream stream, string msgEndSymb) : this(stream)
        {
            _msgEndSymb = msgEndSymb;
        }

        public StreamParser(Stream stream, int bufferSize) : this(stream)
        {
            if (bufferSize <= 0)
                throw new ArgumentOutOfRangeException(nameof(bufferSize));

            _bufferSize = bufferSize;
        }

        public StreamParser(Stream stream, string msgEndSymb, int bufferSize) : this(stream, bufferSize)
        {
            _msgEndSymb = msgEndSymb;
        }

        /// <summary>
        /// Полностью ли прочитано сообщение
        /// </summary>
        public bool IsMessageRead
        {
            get => _sb.Length > 0 && _cursorPosition == _streamLength;
        }

        /// <summary>
        /// Чтение всего сообщения из потока
        /// </summary>
        /// <returns>Полное сообщение, при этом после каждого сообщения ставится символ новой строки</returns>
        public string StreamMessage
        {
            get
             {
                if (!this.IsMessageRead)
                    FillStringBuilderWithMessage();

                return _sb.ToString().Replace(_msgEndSymb, "\n");
            }
            
        }

        /// <summary>
        /// Чтение всего сообщения из потока
        /// </summary>
        /// <returns>Полное сообщение в виде массива, сформированного по символу конца сообщения</returns>
        public IEnumerable<string> SplittedStreamMessage
        {
            get
            {
                if (!this.IsMessageRead)
                    FillStringBuilderWithMessage();

                return _sb.ToString().Split(_msgEndSymb);
            }
        }

        /// <summary>
        /// Обозначенный в задании метод для считывания байтового сообщения из потока
        /// Заполнение StringBuilder сообщением из потока
        /// </summary>
        /// <exception cref="NullReferenceException"></exception>
        private void FillStringBuilderWithMessage()
        {
            if (!_stream.CanRead)
                throw new InvalidOperationException("Невозможно прочесть объект Stream");

            // чтобы не создавать массив каждый цикл
            byte[] bufferSingleton = new byte[_bufferSize];

            while (_cursorPosition < _stream.Length)
            {
                var remainingSymbAmount = _stream.Length - _cursorPosition;
                byte[] bufferLocal = bufferSingleton;

                // чтобы реализовать возможность создания массива меньшего размера, если осташееся кол-во символов меньше утсановленного изначально размера буфера
                if (_bufferSize > remainingSymbAmount)
                {
                    bufferLocal = new byte[remainingSymbAmount];
                }

                _stream.Seek(_cursorPosition, SeekOrigin.Begin);

                var bytesRead = _stream.Read(bufferLocal, 0, bufferLocal.Length);
                var parsedMsg = Encoding.Default.GetString(bufferLocal);
                _sb.Append(parsedMsg);

                _cursorPosition += bytesRead;
            }
        }
    }
}